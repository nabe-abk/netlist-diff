using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Reflection;
using System.IO;
using System.Collections;
using System.Text.RegularExpressions;


namespace netlist_diff
{
    public partial class MainForm : Form
    {
        private Hashtable REPLACE_PINS;
        private bool   EnableAutoChangePin = false;
        private string AutoChangePinParts  = "(R|L|C)";

        private const bool TR_TO92MODE = false;
        private const bool REMOVE_ASTERISK    = true;
        private const bool IGNORE_NO_PIN_NAME = true;
        private const int  OPTIMIZE_LOOP      = 3;

        public MainForm()
        {
            InitializeComponent();
            MsgBox.LanguageOption = RichTextBoxLanguageOptions.UIFonts;
            TrReplaceSetting.SelectedIndex = 0;

            var assembly = Assembly.GetExecutingAssembly();
            var title     = assembly.GetCustomAttribute<AssemblyTitleAttribute>().Title;
            var copyright = assembly.GetCustomAttribute<AssemblyCopyrightAttribute>().Copyright;
            var name      = assembly.GetName();
            var version   = name.Version;
            this.Text = $"{title} - Ver{version.Major}.{version.Minor}{version.Build} (C){copyright}";
        }

        private void FileSelectBtn1_Click(object sender, EventArgs e)
        {
            openFileDialog( FileName1 );
        }
        private void FileSelectBtn2_Click(object sender, EventArgs e)
        {
            openFileDialog( FileName2 );
        }

        private void openFileDialog(System.Windows.Forms.TextBox tbox)
		{
            OpenFileDialog diag = new OpenFileDialog();

            diag.InitialDirectory = tbox.Text;
            diag.Filter = "Netlist File (*.net)|*.net|All files (*.*)|*.*";
            diag.FilterIndex = 1;
            diag.Title = "Select Telesis/BSch Netlist file";
            diag.RestoreDirectory = false;
            if (diag.ShowDialog(this) != DialogResult.OK) return;

            tbox.Text = diag.FileName;
        }

        //---------------------------------------------------------------------
        // init Parse Net List File
        //---------------------------------------------------------------------
        private bool initalize()
        {
            REPLACE_PINS = new Hashtable();
            string tr = TrReplaceSetting.Text.ToUpper();
            tr = Regex.Replace(tr, @"[^\w]", "");
            if (tr == "TO92") {
                REPLACE_PINS["E"] = "1";
                REPLACE_PINS["C"] = "2";
                REPLACE_PINS["B"] = "3";
            }
            else if (tr == "SMD")
            {
                REPLACE_PINS["E"] = "2";
                REPLACE_PINS["C"] = "3";
                REPLACE_PINS["B"] = "1";
            }

            string parts = AutoChangePinSetting.Text;
            if (parts == "")
            {
                addMsg("*** Disable auto pin changer ***");
                EnableAutoChangePin = false;
            }
            else if (Regex.IsMatch(parts, @"^\w+(\|\w+)*$"))
            {
                EnableAutoChangePin = true;
                AutoChangePinParts  = "(" + parts + ")";
            }
            else
            {
                addMsg("Auto change pin parts's format error!");
                return false;
            }

            return true;
        }

        //---------------------------------------------------------------------
        // Diff netlist
        //---------------------------------------------------------------------
        private void RunBtn_Click(object sender, EventArgs e)
		{
            string title1 = "[" + FileLabel1.Text + "] ";
            string title2 = "[" + FileLabel2.Text + "] ";

            clearMsg();
            if (!initalize()) return;

            Hashtable p1 = parseNetListFile(title1, FileName1.Text);
            Hashtable p2 = parseNetListFile(title2, FileName2.Text);

            if (p1 == null || p2 == null) return;

            Hashtable n1 = (Hashtable)p1["_net"];
            Hashtable n2 = (Hashtable)p2["_net"];
            p1.Remove("_net");
            p2.Remove("_net");

            check_ref(p1, p2);
            check_ref(p2, p1);
            output_not_pair(title1, p1);
            output_not_pair(title2, p2);
            netlist_remove_not_pair(title1, n1, p1);
            netlist_remove_not_pair(title2, n2, p2);

            long errors;
			{
                Hashtable chk = new Hashtable();
                check_netlist(chk, n1,  1);
                check_netlist(chk, n2, -1);
                errors = count_error_pairs(chk);
            }

            // pin exchange test
            var r_exchangeable = new Regex("^" + AutoChangePinParts + @"\d", RegexOptions.Compiled);
            Hashtable replace  = new Hashtable();

            for(int i=1; EnableAutoChangePin && i <= OPTIMIZE_LOOP; i++)
            {
                addMsg("*** Auto change pin optimize loop " + i + "/" + OPTIMIZE_LOOP + " ***");

                foreach (string pa in p1.Keys)
                {
                    if (!r_exchangeable.IsMatch(pa)) continue;

                    string x = pa + ".1";
                    string y = pa + ".2";

                    bool negative = replace.Contains(x);
                    if (negative)
                    {
                        replace.Remove(x);
                        replace.Remove(y);
                    }
                    else
                    {
                        replace[x] = y;
                        replace[y] = x;
                    }

                    var chk = new Hashtable();
                    check_netlist(chk, n1, 1, replace);
                    check_netlist(chk, n2, -1);
                    long err = count_error_pairs(chk);

                    if (err < errors)
                    {
                        // addMsg(title1 + "exchange: " + x + " <-> " + y + "  errors: " + errors + " to " + err);
                        errors = err;       // replace OK
                    }
                    else
                    {   // cancel replace
                        if (negative)
                        {
                            replace[x] = y;
                            replace[y] = x;
                        }
                        else
                        {
                            replace.Remove(x);
                            replace.Remove(y);
                        }
                    }
                }
            }

            Hashtable check = new Hashtable();
            check_netlist(check, n1,  1, replace);
            check_netlist(check, n2, -1);

            var n1only = new List<string>();
            var n2only = new List<string>();
            var same   = new List<string>();

            foreach (string k in check.Keys)
            {
                if ((int)check[k] == 0)
                {
                    same.Add(k);
                    continue;
                }
                if ((int)check[k] > 0)
                {
                    n1only.Add(k);
                }
                else
                {
                    n2only.Add(k);
                }
            }
            if (0 < n1only.Count)
            {
                addMsg(title1 + "only connect");
                output_error_pairs(n1only);
            }
            if (0 < n2only.Count)
            {
                addMsg(title2 + "only connect");
                output_error_pairs(n2only);
            }
            if (n1only.Count==0 && n2only.Count==0) {
                addMsg("\n*** Perfect match! ***");
            }
        }

        private void check_ref(Hashtable h1, Hashtable h2)
        {
            foreach (string k in h1.Keys)
            {
                if (h2.ContainsKey(k))
                {
                    ((Hashtable)h2[k])["_pair"] = true;
                }
            }
        }
        private void output_not_pair(string title, Hashtable h1)
        {
            var ary = new List<string>();
            foreach (string k in h1.Keys)
            {
                if (!((Hashtable)h1[k]).ContainsKey("_pair"))
                {
                    ary.Add(k);
                }
            }
            foreach (string k in ary)
			{
                h1.Remove(k);
            }

            if (0<ary.Count)
			{
                addMsg(title + "not pair: " + string.Join(" ", ary));
			}
        }
        private void netlist_remove_not_pair(string title, Hashtable nets, Hashtable parts)
		{
            var r_npin = new Regex(@"^([^\.]+)\.?(.*)$", RegexOptions.Compiled);

            foreach (string net in nets.Keys)
			{
                var nlist = (Hashtable)nets[net];
                var ary   = new List<string>();
                foreach (string pin in nlist.Keys)
				{
                    Match m = r_npin.Match(pin);
                    if (!m.Success) continue;
                    string name = m.Groups[1].Value;

                    if (!parts.Contains(name))
                        ary.Add(pin);
				}
                if (0<ary.Count)
				{
                    foreach (string k in ary)
                    {
                        nlist.Remove(k);
                    }
                    addMsg(title + "remove symbol: " + string.Join(" ", ary));
                }
            }
        }

        private void check_netlist(Hashtable check, Hashtable nlist, int add, Hashtable replace = null)
        {
            if (replace == null) { replace = new Hashtable(); }
            var r_exchangeable = new Regex("^" + AutoChangePinParts + @"\d+\.[12]$", RegexOptions.Compiled);

            foreach (string net in nlist.Keys)
			{
                var ary = ((Hashtable)nlist[net]).Keys.Cast<string>().ToList();

                for(int i=0; i<ary.Count; i++)
                {
                    for (int j=i+1; j<ary.Count; j++)
                    {
                        string x = ary[i];
                        string y = ary[j];
                        if (replace.Contains(x)) x = (string)replace[x];
                        if (replace.Contains(y)) y = (string)replace[y];

                        if (0<string.Compare(x,y))
						{
                            string t = x;
                            x = y;
                            y = t;
                        }
                        string pair = x + "--" + y;

                        if (check.Contains(pair))
						{
                            check[pair] = (int)(check[pair]) + add;
						}
                        else
						{
                            check[pair] = add;
                        }
                    }
                }
            }
        }
        private long count_error_pairs(Hashtable check)
        {
            long err = 0;
            foreach (string k in check.Keys)
            {
                int x = (int)check[k];
                err += x < 0 ? -x : x;
            }
            return err;
        }
        private void output_error_pairs(List<string> list)
        {
            var r_pair = new Regex(@"^([^\-]+)--+([^\-]+)$", RegexOptions.Compiled);

            Hashtable h = new Hashtable();

            foreach (string k in list)
            {
                var m = r_pair.Match(k);
                string x = m.Groups[1].Value;
                string y = m.Groups[2].Value;

                if (!h.Contains(x)) h[x] = new Hashtable();
                if (!h.Contains(y)) h[y] = new Hashtable();
                ((Hashtable)h[x])[y] = true;
                ((Hashtable)h[y])[x] = true;
            }

            List<string> ary = new List<string>();
            foreach(string k in h.Keys) {
                int    n = ((Hashtable)h[k]).Count;
                string m = "000000000000" + n.ToString();
                ary.Add(m.Substring(m.Length-12) + k);
			}
            ary.Sort();
            ary.Reverse();

            foreach (string k in ary)
            {
                string x = k.Substring(12);

                var keys   = ((Hashtable)h[x]).Keys;
                if (keys.Count<1) { continue; }

                string msg = "    " + x + " -->";
                foreach(string y in keys){
                    msg += " " + y;
                    ((Hashtable)h[y]).Remove(x);
                }
                addMsg( msg );
            }
        }

        //---------------------------------------------------------------------
        // parse Net List File
        //---------------------------------------------------------------------
        private Hashtable parseNetListFile(string title, string file)
		{
            Hashtable parts = new Hashtable();

            //-----------------------------------------------------------------
            // read lines
            //-----------------------------------------------------------------
            string[] lines;
            try {
                lines = File.ReadAllLines( file );
            } catch(Exception e) {
                addMsg(title + "can not read: " + file + "\n" + "Exit!\n");
                return null;
			}

            int i = 0;
            int length = lines.Length;
            while (i < length)
            {
                if (lines[i] == "$NETS") break;

                string x = lines[i++];
                if (x == "$PACKAGES") { break; }
                if (x == "$END")      { length = 0; break; }
            }

            //-----------------------------------------------------------------
            // $PACKAGES
            //-----------------------------------------------------------------
            var r_ast  = new Regex(@"\*0?$", RegexOptions.Compiled);
            var r_name = new Regex(@"; *([^\s]+)$", RegexOptions.Compiled);
            while (i < length)
            {
                string x = lines[i++];
                if (x == "$NETS") { break; }
                if (x == "$END")  { length = 0; break; }

                Match m = r_name.Match(x);
                if (m.Success)
				{
                    string name = m.Groups[1].Value;
                    if (REMOVE_ASTERISK) name = r_ast.Replace(name, "");
                    parts[name] = new Hashtable();
				}
            }

            //-----------------------------------------------------------------
            // $NET
            //-----------------------------------------------------------------
            var ary  = new List<string>();
            var r_sp = new Regex(@"^\s+", RegexOptions.Compiled);

            while (i < length)
            {
                string x = lines[i++];
                if (x == "$END") { break; }

                if (0<ary.Count && x.Substring(0,1) == " ")
				{
                    ary[ary.Count - 1] += r_sp.Replace(x, "");
                    continue;
				}
                ary.Add(x);
            }

            var r_net  = new Regex(@"^([^;]+); *(.*)$", RegexOptions.Compiled);
            var r_npin = new Regex(@"^([^\.]+)\.?(.*)$", RegexOptions.Compiled);

            var nlist  = new Hashtable();

            foreach (string l in ary)
			{
                Match m = r_net.Match(l);
                if (!m.Success) {
                    addMsg("[" + title + "] Unknown Line: " + l);
                    continue;
                }

                string net  = m.Groups[1].Value;
                string[] list = m.Groups[2].Value.Split(new string[] { ", " , " ," , " " }, StringSplitOptions.None);

                var conns = new List<string>();
                foreach (string p in list)
                {
                    Match x = r_npin.Match(p);
                    if (!x.Success)
                    {
                        addMsg("[" + title + "] Unknown Parts: " + p);
                        continue;
                    }
                    string name = x.Groups[1].Value;
                    string pin  = x.Groups[2].Value;
                    if (REMOVE_ASTERISK) name = r_ast.Replace(name, "");

                    if (IGNORE_NO_PIN_NAME && pin == "") continue;
                    if (REPLACE_PINS.Contains(pin)) pin = (string)REPLACE_PINS[pin];

                    conns.Add(name + "." + pin);
                 }

                var h = new Hashtable();
                foreach (string p in conns)
                {
                    h[p] = true;

                    Match x = r_npin.Match(p);
                    if (!x.Success) continue;

                    string name = x.Groups[1].Value;
                    if (!parts.Contains(name)) parts[name] = new Hashtable();
                }
                nlist[net] = h;
                // addMsg(title + net + ": " + string.Join(" ", h.Keys.Cast<string>()));
            }
            parts["_net"] = nlist;

            return parts;
        }

        //---------------------------------------------------------------------
        // Message box functions
        //---------------------------------------------------------------------
        private void clearMsg()
        {
            MsgBox.Text = "";
        }
        private void addMsg(string text)
        {
            MsgBox.Text  += text + "\n";
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace msrDesktop
{
    public partial class msrDesktop : Form
    {
        private const string MsrHome = "https://github.com/qualiu/msr";
        private readonly string MsrExePath = string.Empty;
        private readonly UsageParser MsrUsageParser = new UsageParser("msr.exe");

        private static readonly Dictionary<string, double> ArgNameToWidthRatioMap = new Dictionary<string, double>{
            { "-p", 1.0 }, { "-w", 0.4 },
            { "-t", 1.0 }, { "-x", 0.4 }, { "--nx", 0.4 }, { "--nt", 0.4 },  { "-e", 0.4 },
            { "-f", 1.0 }, { "--nf", 0.4 }, { "-d", 0.4 }, { "--nd", 0.4 }, { "--xp", 0.4 }, { "--pp", 0.4 }, { "--np", 0.4 },
            { "-o", 0.4 },
            { "-F", 0.3 }, { "-B", 0.3 }, { "-E", 0.3 },
            { "-s", 0.3 },
            { "-b", 0.3 }, { "-q", 0.3 }, { "-Q", 0.3 }
        };

        private static readonly HashSet<string> NewLineControlArgNames = new HashSet<string>
        {
            "-b", // "--w1", "--s1"
        };

        private static readonly Dictionary<string, object> ArgNameToDefaultValueMap = new Dictionary<string, object>
        {
            { "--w1", DateTime.Now.AddHours(-1).ToString("yyyy-MM-dd HH:mm:00") },
            { "--w2", DateTime.Now.ToString("yyyy-MM-dd HH:mm:00") },
            { "--s1", "1B" },
            { "--s2", "3.6MB" },
            { "-k", 16 },
            { "--timeout", 60 },
            { "-H", 100 },
            { "-T", 100 },
            { "-F", @"(\d{4}[-/]\d{1,2}[-/]\d{1,2}\D\d+:\d+:\d+[\.,]?\d*)" },
            { "-U", 5 },
            { "-D", 3 },
            { "--nd", @"^([\.\$]|(Release|Debug|objd?|bin|node_modules|static|dist|target|(Js)?Packages|\w+-packages?)$|__pycache__)" },
            { "-f", @"\.(cs(html)?|cpp|cxx|h|hpp|cc?|c\+{2}|\w+proj|sln|nuspec|config|conf|resx|props|java|scala|py|go|rs|php|vue|tsx?|jsx?|json|ya?ml|xml|ini|md|ipynb|rst|sh|bat|cmd|psm?1)$|^readme|^make\w+$" },
            { "-p", Path.GetDirectoryName(Application.StartupPath) },
            { "-V", "ne0" },
        };

        private readonly HashSet<string> EnabledArgNames = new HashSet<string>
        {
            "-r", "-p", "-t", "-f", "--nd"
            // "-k", "--timeout"
        };

        private static readonly Dictionary<string, Color> ArgNameToColorMap = new Dictionary<string, Color>
        {
            { "-t", Color.Blue },
            { "-x", Color.Blue },
            { "-i", Color.Blue },
            { "-R", Color.Red },
            { "-X", Color.Red },
            { "-o", Color.Magenta },
            { "-a", Color.Brown },
            { "-H", Color.Green },
            { "-J", Color.Green },
            { "-l", Color.Blue },
            { "--force", Color.Magenta },
        };

        private static readonly Regex ScalableRegexForDescription = new Regex(@"Regex|pattern|Path", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private readonly List<ArgAndControls> ArgControlList = new List<ArgAndControls>();

        private HashSet<string> HideArgNames = new HashSet<string>
        {
            "-h",
            "-z",
        };

        private HashSet<string> ForbidArgNames = new HashSet<string>();

        private Dictionary<string, ArgAndControls> ShortNameToArgControlsMap = new Dictionary<string, ArgAndControls>();

        private ArgAndControls LastVisibleArgControls = null;

        private string GetMsrCommandLineArgs() =>
            string.Join(" ", ArgControlList
                .Select(a => a.GetCommandLine(!checkBoxUseLongArgName.Checked))
                .Where(a => !string.IsNullOrEmpty(a)));

        public msrDesktop()
        {
            InitializeComponent();
            var pathValue = Environment.GetEnvironmentVariable("PATH") ?? string.Empty;
            var paths = Regex.Split(pathValue, @"\s*;\s*");
            var msrFolder = paths.FirstOrDefault(p => File.Exists(Path.Combine(p, "msr.exe")));
            if (string.IsNullOrEmpty(msrFolder))
            {
                var projectFolder = Regex.Replace(Application.StartupPath, @"\\bin\\.*$", "");
                var tmpMsrPath = Path.Join(projectFolder, "msr.exe");
                if (File.Exists(tmpMsrPath))
                {
                    msrFolder = projectFolder;
                    Environment.SetEnvironmentVariable("PATH", pathValue + ";" + projectFolder);
                }
            }

            MsrExePath = string.IsNullOrEmpty(msrFolder) ? string.Empty : Path.Combine(msrFolder, "msr.exe");
        }

        private void MsrDesktop_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(MsrExePath))
            {
                AppendMessage($"Not found msr in PATH. Please download it from {MsrHome}");
                return;
            }

            AppendMessage($"Found msr = {MsrExePath} , doc: {MsrHome}");
            MsrUsageParser.Parse();
            foreach (var arg in MsrUsageParser.AllArgs)
            {
                AddControl(arg);
            }

            AppendMessage($"Parsed {MsrUsageParser.AllArgs.Count} args.");
            AppendMessage($"This tool will be updated/published at github: https://github.com/qualiu/msrUI");

            richTextBoxInfo.LinkClicked += (s, t) =>
            {
                try
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = t.LinkText,
                        UseShellExecute = true,
                    });
                }
                catch
                {
                }
            };

            var forbidArgsTip = "Forbid some args like: -R -X --force (Free to use long/short names).";
            checkBoxForbidArgNamePattern.MouseHover += (s, t) => ShowTip(checkBoxForbidArgNamePattern, forbidArgsTip);
            checkBoxForbidArgNamePattern.MouseLeave += HideTipLabel;
            textBoxForbidArgNamePattern.MouseHover += (s, t) => ShowTip(textBoxForbidArgNamePattern, forbidArgsTip);
            textBoxForbidArgNamePattern.MouseLeave += HideTipLabel;
            textBoxForbidArgNamePattern.Enabled = checkBoxForbidArgNamePattern.Checked;

            var hideArgsTip = "Hide some args like: -z -h -R -X (Free to use long/short names).";
            checkBoxHideArgNamePattern.MouseHover += (s, t) => ShowTip(checkBoxHideArgNamePattern, hideArgsTip);
            checkBoxHideArgNamePattern.MouseLeave += HideTipLabel;
            textBoxHideArgNamePattern.MouseHover += (s, t) => ShowTip(textBoxHideArgNamePattern, hideArgsTip);
            textBoxHideArgNamePattern.MouseLeave += HideTipLabel;
            textBoxHideArgNamePattern.Enabled = checkBoxHideArgNamePattern.Checked;

            checkBoxShowExamples.MouseHover += (s, t) => ShowTip(checkBoxShowExamples, "Show examples when mouse hover.");
            checkBoxShowExamples.MouseLeave += HideTipLabel;

            checkBoxUseLongArgName.MouseHover += (s, t) => ShowTip(checkBoxUseLongArgName, "Output long arg names int command line.");
            checkBoxUseLongArgName.MouseLeave += HideTipLabel;

            buttonValidateArgs.MouseHover += (s, t) => ShowTip(buttonValidateArgs, "Basic checking + forbidden checking. (Hotkey: Alt+V)");

            checkBoxForbidArgNamePattern_CheckedChanged(checkBoxForbidArgNamePattern, null);
            // ReArrangeControls();
            checkBoxHideArgNamePattern_CheckedChanged(checkBoxHideArgNamePattern, null);
            this.CenterToScreen();
            this.BringToFront();
        }

        private void AddControl(OneArg arg)
        {
            var shortName = arg.GetName(preferShortName: true);
            var isHidden = HideArgNames.Contains(shortName) || HideArgNames.Contains(arg.LongName);

            var nameForControl = Regex.Replace(arg.GetName(), @"\W", "_");
            var panel = this.splitContainer.Panel1;
            var longName = arg.GetName(preferShortName: false);
            var checkBox = new CheckBox()
            {
                Name = $"checkBox_{nameForControl}",
                Text = longName,
                Checked = EnabledArgNames.Contains(shortName),
                AutoSize = true,
                Visible = !isHidden,
            };

            var textBox = !arg.HasValue ? null : new TextBox()
            {
                Name = $"textBox_{nameForControl}",
                Enabled = checkBox.Checked,
                Dock = DockStyle.None,
                Anchor = AnchorStyles.None,
                Text = ArgNameToDefaultValueMap.TryGetValue(shortName, out object defaultValue)
                    ? defaultValue.ToString()
                    : string.Empty,
                Visible = !isHidden,
            };

            var ac = new ArgAndControls(arg, checkBox, textBox);
            ArgControlList.Add(ac);
            ShortNameToArgControlsMap.Add(shortName, ac);
            checkBox.CheckedChanged += GenerateCommandLine;

            panel.Controls.Add(checkBox);
            checkBox.MouseHover += (sender, e) => ShowUsageExampleTip(checkBox, arg);
            checkBox.MouseLeave += (sender, e) => HideTipLabel(sender, e);
            if (ArgNameToColorMap.TryGetValue(shortName, out Color foreColor))
            {
                checkBox.ForeColor = foreColor;
            }

            if (textBox == null)
            {
                return;
            }

            if (arg.HasRegexValue)
            {
                textBox.TextChanged += (sender, e) => TryParseRegex(textBox, arg);
                textBox.MouseHover += (sender, e) => TryParseRegex(textBox, arg);
            }
            else
            {
                textBox.MouseHover += (sender, e) => ShowUsageExampleTip(textBox, arg);
            }

            textBox.MouseLeave += HideTipLabel;

            textBox.TextChanged += GenerateCommandLine;
            checkBox.Click += (sender, e) => textBox.Enabled = checkBox.Checked;
            panel.Controls.Add(textBox);
        }

        private void ReArrangeControls(int horizontalDistance = 5, int verticalDistance = 5)
        {
            var panel = this.splitContainer.Panel1;
            LastVisibleArgControls = null;
            for (int k = 0; k < ArgControlList.Count; k++)
            {
                var arg = ArgControlList[k].Arg;
                var checkBox = ArgControlList[k].CheckBox;
                var textBox = ArgControlList[k].TextBox;
                var shortName = arg.GetName(preferShortName: true);
                var isHidden = HideArgNames.Contains(shortName) || HideArgNames.Contains(arg.LongName);
                ArgControlList[k].SetVisibility(!isHidden);
                if (isHidden)
                {
                    continue;
                }

                Control lastControl = LastVisibleArgControls == null
                    ? checkBoxUseLongArgName
                    : (LastVisibleArgControls.TextBox ?? LastVisibleArgControls.CheckBox as Control);
                OneArg lastArg = LastVisibleArgControls == null ? null : LastVisibleArgControls.Arg;

                LastVisibleArgControls = ArgControlList[k];

                var position = new Point(
                    k == 0 ? lastControl.Left : lastControl.Right + horizontalDistance * 2,
                    k == 0 ? lastControl.Bottom + verticalDistance : lastControl.Top);
                var nextLeft = position.X + horizontalDistance + checkBox.Width;

                if (textBox != null)
                {
                    var desiredWidth = string.IsNullOrWhiteSpace(textBox.Text)
                        ? 100
                        : TextRenderer.MeasureText(textBox.Text, textBox.Font).Width + 10;

                    if (ArgNameToWidthRatioMap.TryGetValue(shortName, out double textBoxWidthRatio))
                    {
                        desiredWidth = (int)Math.Floor(panel.ClientSize.Width * textBoxWidthRatio);
                    }
                    textBox.Visible = !isHidden;
                    textBox.Width = desiredWidth;
                    nextLeft += horizontalDistance + textBox.Width + horizontalDistance;
                }

                if (nextLeft > panel.ClientSize.Width || NewLineControlArgNames.Contains(shortName))
                {
                    position.Y += verticalDistance + checkBoxUseLongArgName.Height;
                    position.X = checkBoxUseLongArgName.Left;
                    if (lastControl is TextBoxBase && lastArg != null && ScalableRegexForDescription.IsMatch(lastArg.Description))
                    {
                        lastControl.Width = panel.Width - lastControl.Left - horizontalDistance;
                    }
                }

                checkBox.Location = position;
                if (textBox != null)
                {
                    position.X = checkBox.Right + horizontalDistance * 2;
                    textBox.Location = position;
                }
            }
        }

        private delegate void AppendMessageDelegator(string text, bool appendNewLine = true, bool clearAtFirst = false);
        private void AppendMessage(string text, bool appendNewLine = true, bool clearAtFirst = false)
        {
            if (richTextBoxInfo.InvokeRequired)
            {
                richTextBoxInfo.BeginInvoke(new AppendMessageDelegator(_AppendMessage), text, appendNewLine, clearAtFirst);
            }
            else
            {
                _AppendMessage(text, appendNewLine, clearAtFirst);
            }
        }

        private void _AppendMessage(string text, bool appendNewLine = true, bool clearAtFirst = false)
        {
            if (clearAtFirst)
            {
                this.richTextBoxInfo.Clear();
            }

            this.richTextBoxInfo.AppendText(text);
            if (appendNewLine)
            {
                this.richTextBoxInfo.AppendText(Environment.NewLine);
            }
            this.richTextBoxInfo.ScrollToCaret();
        }

        private void TryParseRegex(TextBox textBox, OneArg arg)
        {
            labelUsage.ForeColor = Color.Blue; // Color.Green;
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                ShowUsageExampleTip(textBox, arg);
                return;
            }

            try
            {
                new Regex(textBox.Text);
                textBox.ForeColor = Color.Black;
                ShowUsageExampleTip(textBox, arg);
            }
            catch (Exception e)
            {
                ShowUsageExampleTip(textBox, arg, arg.HelpText + Environment.NewLine + e.Message);
                this.labelUsage.ForeColor = Color.Red; // Color.DarkRed;
                textBox.ForeColor = Color.Magenta;
            }
        }


        private void ShowUsageExampleTip(Control control, OneArg arg, string usage = null)
        {
            var panel = this.splitContainer.Panel1;
            int margin = 5;
            var explainedExamples = MsrUsageParser.ShortNameToExamplesMap.TryGetValue(arg.GetName(), out HashSet<ExplainedExample> examples)
                ? examples.Take((int)this.numericUpDownExampleCount.Value).Select(a => a.Explain + Environment.NewLine + a.Command).ToArray()
                : new string[0];
            var exampleText = string.Join(Environment.NewLine + Environment.NewLine, explainedExamples);

            labelUsage.Text = string.IsNullOrEmpty(usage) ? arg.HelpText : usage;
            labelUsage.BackColor = Color.LightGray;
            if (!this.checkBoxShowExamples.Checked || string.IsNullOrEmpty(exampleText))
            {
                labelExamples.Visible = false;
                ShowTip(control, labelUsage.Text);
                return;
            }

            labelUsage.AutoSize = false;
            labelUsage.Visible = !string.IsNullOrWhiteSpace(labelUsage.Text);
            var usageSize = TextRenderer.MeasureText(labelUsage.Text, labelUsage.Font, panel.ClientSize, TextFormatFlags.WordBreak);
            labelUsage.Width = Math.Min(usageSize.Width, panel.Width - margin);
            labelUsage.Height = usageSize.Height;

            labelExamples.Text = exampleText;
            labelExamples.AutoSize = false;
            var exampleSize = TextRenderer.MeasureText(labelExamples.Text, labelExamples.Font, panel.ClientSize, TextFormatFlags.WordBreak);
            labelExamples.Width = Math.Min(exampleSize.Width, panel.Width - margin);
            labelExamples.Height = exampleSize.Height;
            labelExamples.Visible = !string.IsNullOrWhiteSpace(labelExamples.Text);

            var usageLeftMove = Math.Max(0, control.Left + labelUsage.Width - labelUsage.Parent.Width);
            var exampleLeftMove = Math.Max(0, control.Left + labelExamples.Width - labelExamples.Parent.Width);

            var desiredTops = new List<int[]>
            {
                new int[] { 
                    // // above + below control
                    control.Top - labelUsage.Height - margin,
                    control.Bottom + margin
                },
                new int[] { 
                    // above control
                    control.Top - labelUsage.Height - labelExamples.Height - 2 * margin,
                    control.Top - labelExamples.Height - margin
                },
                new int[]
                {
                    // below control
                    control.Bottom + margin,
                    control.Bottom + labelUsage.Height + margin
                }
            };

            var usageTop = 0;
            var exampleTop = 0;
            for (int k = 0; k < desiredTops.Count; k++)
            {
                usageTop = desiredTops[k][0];
                exampleTop = desiredTops[k][1];
                if (usageTop >= 0 && exampleTop + labelExamples.Height < panel.Height)
                {
                    break;
                }
            }

            labelUsage.Top = usageTop;
            labelExamples.Top = exampleTop;
            labelUsage.Left = Math.Max(0, control.Left - usageLeftMove);
            labelUsage.BringToFront();
            labelExamples.BackColor = Color.LightGray;
            labelExamples.Left = Math.Max(0, control.Left - exampleLeftMove);
            labelExamples.BringToFront();
        }

        private void ShowTip(Control control, string tip)
        {
            int verticalMargin = 5;
            var panel = this.splitContainer.Panel1;

            labelUsage.Text = tip;
            labelUsage.AutoSize = true;
            labelUsage.Visible = !string.IsNullOrWhiteSpace(labelUsage.Text);
            var usageSize = TextRenderer.MeasureText(labelUsage.Text, labelUsage.Font, panel.ClientSize, TextFormatFlags.WordBreak);
            labelUsage.Top = control.Top - usageSize.Height - verticalMargin > 0
                ? control.Top - usageSize.Height - verticalMargin
                : control.Bottom + verticalMargin;
            var usageLeftMove = Math.Max(0, control.Left + labelUsage.Width - labelUsage.Parent.Width);
            labelUsage.Left = Math.Max(0, control.Left - usageLeftMove);
            labelUsage.BringToFront();
        }

        private void HideTipLabel(object sender, EventArgs e)
        {
            labelUsage.ForeColor = Color.Blue; // Color.Green;
            labelUsage.Visible = false;
            labelExamples.Visible = false;
        }

        private void GenerateCommandLine(object sender, EventArgs e)
        {
            AppendMessage($"msr {GetMsrCommandLineArgs()}", clearAtFirst: true);
        }

        private void ButtonValidateArgs_Click(object sender, EventArgs e)
        {
            foreach (var ac in ArgControlList)
            {
                if (ac.Arg.HasRegexValue && ac.CheckBox.Checked && string.IsNullOrEmpty(ac.TextBox.Text))
                {
                    AppendMessage($"No value set for {ac.Arg.CombinedNames}", clearAtFirst: true);
                    return;
                }
            }

            var commandArgs = GetMsrCommandLineArgs();
            AppendMessage($"msr {commandArgs}", clearAtFirst: true);
            var verboseArg = ShortNameToArgControlsMap.TryGetValue("--verbose", out ArgAndControls verbose) && verbose.CheckBox.Checked
                ? string.Empty : "--verbose";
            var noColorArg = ShortNameToArgControlsMap.TryGetValue("-C", out ArgAndControls noColor) && noColor.CheckBox.Checked
                ? string.Empty : "-C";
            var args = $"{commandArgs} -z test {verboseArg} {noColorArg}";
            AppendMessage($"msr {args}", clearAtFirst: true);
            var getErrorRegex = new Regex(@"^\d+-\d+.*?\s+ERROR\s+");
            var forbiddenShortArgNames = ForbidArgNames.Select(a => MsrUsageParser.ShortNameToLongNameMap.TryGetValue(a, out string longName) ? longName : a).ToList();
            var getForbiddenArgRegex = ForbidArgNames.Count < 1 ? null : new Regex(@"^\s*(" + string.Join("|", forbiddenShortArgNames) + @")\s");
            var foundForbiddenArgShortNames = new HashSet<string>();
            var hasFoundError = false;
            DataReceivedEventHandler stderrHandler = (sd, r) =>
            {
                if (string.IsNullOrEmpty(r.Data))
                {
                    return;
                }

                if (hasFoundError)
                {
                    // AppendMessage(r.Data);
                    return;
                }

                if (getForbiddenArgRegex != null)
                {
                    var matchForbiddenArg = getForbiddenArgRegex.Match(r.Data);
                    if (matchForbiddenArg.Success)
                    {
                        AppendMessage($"Should not use forbidden arg: {r.Data}");
                        foundForbiddenArgShortNames.Add(matchForbiddenArg.Groups[1].Value);
                        return;
                    }
                }

                var matchError = getErrorRegex.Match(r.Data);
                if (matchError.Success)
                {
                    hasFoundError = true;
                    AppendMessage(r.Data);
                }
            };

            var (output, error, exitCode) = CommandUtils.RunCommand("msr", args, errorHander: stderrHandler);

            if (hasFoundError || !string.IsNullOrEmpty(error))
            {
                AppendMessage($"ExitCode = {exitCode},  Validated arguments and found error: {error}");
            }
            else if (foundForbiddenArgShortNames.Count > 0)
            {
                AppendMessage($"Should not use forbidden args[{foundForbiddenArgShortNames.Count}]: {string.Join(", ", foundForbiddenArgShortNames)}");
            }
            else
            {
                AppendMessage($"Arguments validated successfully.");
            }
        }

        private void CheckBoxUseLongArgName_CheckedChanged(object sender, EventArgs e)
        {
            GenerateCommandLine(sender, e);
        }

        private void MsrDesktop_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                return;
            }

            ReArrangeControls();
            this.splitContainer.Panel1.Invalidate(true);
            this.splitContainer.Panel1.Update();
        }

        private void checkBoxHideArgNamePattern_CheckedChanged(object sender, EventArgs e)
        {
            UpdateHiddenArgSet();
        }

        private void textBoxHideArgNamePattern_TextChanged(object sender, EventArgs e)
        {
            UpdateHiddenArgSet();
        }

        private void UpdateHiddenArgSet()
        {
            var oldHiddenNames = HideArgNames.ToArray();
            HideArgNames.Clear();
            textBoxHideArgNamePattern.Enabled = checkBoxHideArgNamePattern.Checked;
            if (this.checkBoxHideArgNamePattern.Checked)
            {
                var names = Regex.Split(textBoxHideArgNamePattern.Text, @"\s+")
                .Where(a => !string.IsNullOrEmpty(a));
                HideArgNames.AddCollection(names);
            }

            if (string.Join(",", oldHiddenNames) != string.Join(",", HideArgNames))
            {
                ReArrangeControls();
            }
        }

        private void checkBoxForbidArgNamePattern_CheckedChanged(object sender, EventArgs e)
        {
            UpdateForbiddenArgSet();
        }

        private void textBoxForbidArgNamePattern_TextChanged(object sender, EventArgs e)
        {
            UpdateForbiddenArgSet();
        }

        private void UpdateForbiddenArgSet()
        {
            var oldForbiddenNames = ForbidArgNames.ToList();
            ForbidArgNames.Clear();
            textBoxForbidArgNamePattern.Enabled = checkBoxForbidArgNamePattern.Checked;
            if (checkBoxForbidArgNamePattern.Checked)
            {
                var names = Regex.Split(textBoxForbidArgNamePattern.Text, @"\s+").Where(a => !string.IsNullOrEmpty(a));
                ForbidArgNames.AddCollection(names);
            }

            var defaultBackColor = checkBoxUseLongArgName.BackColor;
            foreach (var freedName in oldForbiddenNames.Except(ForbidArgNames))
            {
                var shortName = MsrUsageParser.LongNameToShortNameMap.TryGetValue(freedName, out string name) ? name : freedName;
                if (ShortNameToArgControlsMap.TryGetValue(shortName, out ArgAndControls ac))
                {
                    ac.CheckBox.BackColor = defaultBackColor;
                    ac.CheckBox.Enabled = true;
                }
            }

            foreach (var forbiddenName in ForbidArgNames.Except(oldForbiddenNames))
            {
                var shortName = MsrUsageParser.LongNameToShortNameMap.TryGetValue(forbiddenName, out string name) ? name : forbiddenName;
                if (ShortNameToArgControlsMap.TryGetValue(shortName, out ArgAndControls ac))
                {
                    ac.CheckBox.BackColor = Color.Pink;
                    ac.CheckBox.Enabled = false;
                    ac.CheckBox.Checked = false;
                }
            }
        }

        private void checkBoxShowExamples_CheckedChanged(object sender, EventArgs e)
        {
            this.numericUpDownExampleCount.Enabled = this.checkBoxShowExamples.Checked;
        }
    }
}

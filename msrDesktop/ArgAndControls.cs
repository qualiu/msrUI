using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace msrDesktop
{
    public class ArgAndControls
    {
        public readonly OneArg Arg;

        public readonly CheckBox CheckBox;

        public readonly TextBoxBase TextBox;

        private static readonly Regex SkipQuoteRegex = new Regex(@"^[\w\.\\,=+-]*$", RegexOptions.Compiled);

        public ArgAndControls(OneArg arg, CheckBox checkBox, TextBoxBase textBox = null)
        {
            this.Arg = arg;
            this.CheckBox = checkBox;
            this.TextBox = textBox;
        }

        public Rectangle Rectangle
        {
            get
            {
                return TextBox == null
                    ? new Rectangle(CheckBox.Location, CheckBox.Size)
                    : new Rectangle(TextBox.Location, TextBox.Size);
            }
        }

        public void SetVisibility(bool visible)
        {
            this.CheckBox.Visible = visible;
            if (this.TextBox != null)
            {
                this.TextBox.Visible = visible;
            }
        }

        public string GetCommandLine(bool preferShortName)
        {
            if (!CheckBox.Checked)
            {
                return string.Empty;
            }

            var name = Arg.GetName(preferShortName);

            if (TextBox == null)
            {
                return name;
            }

            if (string.IsNullOrEmpty(TextBox.Text) && Arg.ShortName == "-o")
            {
                return $@"{name} """"";
            }

            if (SkipQuoteRegex.IsMatch(TextBox.Text))
            {
                return $@"{name} {TextBox.Text}";
            }

            return $@"{name} ""{TextBox.Text}""";
        }
    }
}

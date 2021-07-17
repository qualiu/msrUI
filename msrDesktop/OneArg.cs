namespace msrDesktop
{
    public class OneArg
    {
        public string ShortName { get; set; } = string.Empty;

        public string LongName { get; set; } = string.Empty;

        public bool HasValue { get; set; } = false;

        public string Description { get; set; } = string.Empty;

        public bool HasRegexValue => !string.IsNullOrEmpty(Description) && Description.Contains("Regex pattern");

        public string CombinedNames => $"{ShortName}/{LongName}".Trim('/');

        public string HelpText
        {
            get
            {
                if (string.IsNullOrEmpty(ShortName) || string.IsNullOrEmpty(LongName))
                {
                    return Description;
                }

                return $"{ShortName}({LongName}): {Description}";
            }
        }

        public string GetName(bool preferShortName = true)
        {
            if (preferShortName && !string.IsNullOrEmpty(ShortName))
            {
                return ShortName;
            }
            else
            {
                return string.IsNullOrEmpty(LongName) ? ShortName : LongName;
            }
        }

        public override string ToString()
        {
            return $"Name = {GetName()}, ShortName = {ShortName}, LongName = {LongName}, HasValue = {HasValue}, Description = {Description}";
        }
    }
}

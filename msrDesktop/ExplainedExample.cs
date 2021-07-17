using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace msrDesktop
{
    public class ExplainedExample
    {
        public string Explain { get; set; }

        public string Command { get; set; }

        public HashSet<string> ArgShortNames { get; set; } = new HashSet<string>();

        private static readonly Regex GetArgNameRegex = new Regex(@"\s+(--?[a-zA-Z][\w-]*)");

        public ExplainedExample(string explain, string command)
        {
            this.Explain = explain;
            this.Command = command;
        }

        public ExplainedExample ParseArgNames(Dictionary<string, string> longArgNameToShortNameMap, HashSet<string> shortNameSet)
        {
            ArgShortNames.Clear();
            var matches = GetArgNameRegex.Matches(Command);
            foreach (Match match in matches)
            {
                var argName = match.Groups[1].Value;
                var shortName = longArgNameToShortNameMap.TryGetValue(argName, out string name) ? name : argName;
                if (!shortNameSet.Contains(shortName))
                {
                    shortName = argName.Substring(0, 2);
                }

                if (shortNameSet.Contains(shortName))
                {
                    ArgShortNames.Add(shortName);
                }
            }

            return this;
        }
    }
}

using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace msrDesktop
{
    public class UsageParser
    {
        private readonly string ExeName;
        private readonly string HelpArgs;
        private readonly Regex QuitPattern;

        /// <summary>
        /// Example1: -n [ --sort-as-number ]     If has used -s :
        /// Example2: --xf                        Exclude/Skip link files.
        /// Example3: --xp arg                    Exclude/Skip full paths or sub-paths by plain text matching.
        /// </summary>
        private static readonly Regex ArgDescriptionRegex = new Regex(
            @"^\s*(?<ShortName>-[A-Za-z])?\s+"
            + @"(\[?\s*(?<LongName>--[A-Za-z0-9-]+)\s*\]?\s+)?"
            + @"(?<HasValue>arg\s+)?"
            + @"\s*(?<Description>.+?)\s*$",
            RegexOptions.Compiled);

        private static readonly Regex GetExampleRegex = new Regex(
            @"[\r\n]\s*Example-\d+\s*:\s*(?<Explain>[^\r\n]+?)\s*?" + @"[\r\n]\s*((\S+[/\\])?msr(\S+)?\s+(?<Args>-[^\r\n]+))"
                + "|"
            + @"[\r\n]\s*?\([1-9A-C]\)\s*(?<Explain>[^\r\n]+?)\s*?" + @"[\r\n]\s*msr\s+(?<Args>[^\r\n]+)"
                + "|"
            + @"[\r\n]\s*?\([1-9A-C]\)\s*(?<Explain>[^:]+?)\s*:\s*?" + @"msr\s+(?<Args>[^\r\n]+)",
            RegexOptions.Compiled);

        public List<OneArg> AllArgs { get; set; } = new List<OneArg>();

        public readonly Dictionary<string, string> ShortNameToLongNameMap = new Dictionary<string, string>();
        public readonly Dictionary<string, string> LongNameToShortNameMap = new Dictionary<string, string>();

        public Dictionary<string, HashSet<ExplainedExample>> ShortNameToExamplesMap = new Dictionary<string, HashSet<ExplainedExample>>();

        public UsageParser(string exeName, string helpArgs = "--help --no-color", string quitPattern = @"^\s*(-h|--help)")
        {
            this.ExeName = exeName;
            this.HelpArgs = helpArgs;
            this.QuitPattern = new Regex(quitPattern, RegexOptions.Compiled);
        }

        public void Parse()
        {
            var shouldQuit = false;
            var afterUsageTextBuffer = new StringBuilder();
            DataReceivedEventHandler outputHander = (sender, e) =>
            {
                var line = e.Data;
                if (shouldQuit || string.IsNullOrEmpty(line))
                {
                    afterUsageTextBuffer.AppendLine(line);
                    return;
                }

                var match = ArgDescriptionRegex.Match(line);
                if (match.Success)
                {
                    var arg = new OneArg
                    {
                        ShortName = match.Groups["ShortName"].Value,
                        LongName = match.Groups["LongName"].Value,
                        HasValue = !string.IsNullOrEmpty(match.Groups["HasValue"].Value),
                        Description = match.Groups["Description"].Value
                    };

                    ShortNameToLongNameMap.Add(arg.GetName(), arg.LongName);
                    LongNameToShortNameMap.Add(arg.GetName(false), string.IsNullOrEmpty(arg.ShortName) ? arg.LongName : arg.ShortName);
                    AllArgs.Add(arg);
                }

                if (this.QuitPattern.IsMatch(line))
                {
                    shouldQuit = true;
                    return;
                }

            };

            CommandUtils.RunCommand(ExeName, HelpArgs, outputHander);

            var textAfterUsage = afterUsageTextBuffer.ToString();
            var shortNameSet = new HashSet<string>(ShortNameToLongNameMap.Keys);
            var exampleMatches = GetExampleRegex.Matches(textAfterUsage);
            foreach (Match match in exampleMatches)
            {
                var explain = Regex.Replace(match.Groups["Explain"].Value, @"[:,;\.\s-]*$", "") + ":";
                var example = $"msr {match.Groups["Args"].Value}";
                var explainedExample = new ExplainedExample(explain, example).ParseArgNames(LongNameToShortNameMap, shortNameSet);
                foreach (var shortName in explainedExample.ArgShortNames)
                {
                    var exampleSet = ShortNameToExamplesMap.TryGetValue(shortName, out HashSet<ExplainedExample> examples)
                      ? examples
                      : new HashSet<ExplainedExample>();
                    exampleSet.Add(explainedExample);
                    ShortNameToExamplesMap[shortName] = exampleSet;
                }
            }
        }
    }
}

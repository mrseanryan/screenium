//original license: MIT
//
//See the file license.txt for copying permission.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace screenium
{
    class ArgsProcessor
    {
        //example cmd line:
        //screenium <testDetails.csv> <testName>|ALL <reportOutput.html> <OPTIONS>

        internal enum Args
        {
            CSV_FILE_PATH,
            TEST_NAME,
            OUTPUT_FILE_PATH,
            IMAGES_DIR_PATH,
            TEMPLATES_DIR_PATH,
            OPTIONS
        };

        internal enum Options
        {
            Run,
            Save,
            TestSelf,
            AcceptChanges,
            Verbose,
            QuietNotShowResults
        }

        Dictionary<Args, int> dictArgsToPos = new Dictionary<Args, int>();

        Dictionary<Options, bool> optionsOnOrOff = new Dictionary<Options, bool>();

        string[] args;

        internal ArgsProcessor(string[] args)
        {
            this.args = args;

            int iPos = 0;
            foreach (var opt in Enum.GetValues(typeof(Args)))
            {
                dictArgsToPos[(Args)opt] = iPos++;
            }
        }

        internal string GetArg(Args opt)
        {
            return args[dictArgsToPos[opt]];
        }

        internal int GetArgAsInt(Args opt)
        {
            return Int32.Parse(GetArg(opt));
        }

        internal double GetArgAsDouble(Args opt)
        {
            return double.Parse(GetArg(opt));
        }

        internal string GetUsage()
        {
            StringBuilder sb = new StringBuilder();

            AssemblyName asmName = this.GetType().Assembly.GetName();
            sb.Append(asmName.Name + " " + asmName.Version.ToString() + "\n");

            sb.Append("Usage: ");

            foreach (var arg in dictArgsToPos.Keys)
            {
                sb.Append("<" + arg + "> ");
            }

            sb.AppendLine("");
            sb.AppendLine("Options: ");
            var chToOption = BuildOptionMap();
            foreach (var ch in chToOption.Keys)
            {
                sb.AppendLine("-" + ch + " = " + chToOption[ch].ToString());
            }

            return sb.ToString();
        }

        internal bool Validate(out string message)
        {
            message = "OK";
            if (args.Length != dictArgsToPos.Keys.Count)
            {
                message = "Unexpected number of arguments";
                return false;
            }
            if(!AreOptionsValid())
            {
                message = "The options were not recognised. Options should be preceded by - or /";
                return false;
            }
            if (!AreOptionsCompatible())
            {
                message = "Incompatible combination of options";
                return false;
            }
            return true;
        }

        private bool AreOptionsCompatible()
        {
            List<Options> combinable = new List<Options>
            {
                Options.Verbose,
                Options.QuietNotShowResults
            };
            var optionsOnOnly = optionsOnOrOff.Where(pair => pair.Value);

            if(optionsOnOnly.Count() > 1 && optionsOnOnly.Count(opt => !combinable.Contains(opt.Key)) > 1)
            {
                return false;
            }
            return true;
        }

        private bool AreOptionsValid()
        {
            var opts = GetArg(Args.OPTIONS);
            if (!IsHyphen(opts[0]))
            {
                return false;
            }

            Dictionary<char, Options> chToOption;
                
            chToOption = BuildOptionMap();

            foreach (char ch in opts)
            {
                if (IsHyphen(ch))
                {
                    continue;
                }

                if (!chToOption.ContainsKey(ch))
                {
                    return false;
                }
                else
                {
                    optionsOnOrOff[chToOption[ch]] = true;
                }
            }

            return true;
        }

        private Dictionary<char, Options> BuildOptionMap()
        {
            Dictionary<char, Options> chToOption;
            chToOption = new Dictionary<char, Options>();

            foreach (Options opt in Enum.GetValues(typeof(Options)))
            {
                var ch = opt.ToString().ToLower()[0];
                if (chToOption.ContainsKey(ch))
                {
                    throw new InvalidOperationException("more than 1 option has same initial letter!");
                }

                chToOption[ch] = opt;
                optionsOnOrOff[opt] = false;
            }
            return chToOption;
        }

        private static bool IsHyphen(char opts)
        {
            return opts == '-' || opts == '/';
        }

        internal bool IsOptionOn(Options opt)
        {
            return optionsOnOrOff[opt];
        }
    }
}

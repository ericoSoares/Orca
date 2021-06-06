using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace webapp
{
    public class RuleResultDto
    {
        public string RuleName { get; set; }
        public string RuleDescription { get; set; }
        public string FilePath { get; set; }
        public int LineNumber { get; set; }
        public int SeverityLevel { get; set; }
    }
}

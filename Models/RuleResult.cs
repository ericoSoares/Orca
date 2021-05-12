using System;
using System.Collections.Generic;
using System.Text;

namespace tcc.Models
{
    public class RuleResult
    {
        public Rule Rule { get; set; }
        public string FilePath { get; set; }
        public int LineNumber { get; set; }

        public override string ToString()
        {
            return "Rule: " + Rule.Name + " | File: " + FilePath + " | Line: " + LineNumber;
        } 
    }
}

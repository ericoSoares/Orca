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

        public RuleResult(string filepath, int lineNumber, Rule rule)
        {
            this.FilePath = filepath;
            this.LineNumber = lineNumber;
            this.Rule = rule;
        }

        public override string ToString()
        {
            return "Rule: " + Rule.Name + " | File: " + FilePath + " | Line: " + LineNumber;
        } 
    }
}

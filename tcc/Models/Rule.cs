using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace tcc.Models
{
    public abstract class Rule
    {
        public string Name { get; set; } = "A rule name";
        public string Description { get; set; } = "A rule description";
        public ESeverityLevel SeverityLevel { get; set; } = ESeverityLevel.BLOCKER;
        public DesignPattern DesignPattern { get; set; }
        public DesignPatternRepository DesignPatternRepository => new DesignPatternRepository();

        public virtual IList<RuleResult> Execute(Repository repository)
        {
            return new List<RuleResult>();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace tcc.Models
{
    abstract class Rule
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DesignPattern DesignPattern { get; set; }
        public Repository Repository { get; set; }

        public virtual IList<RuleResult> Execute()
        {
            return new List<RuleResult>();
        }
    }
}

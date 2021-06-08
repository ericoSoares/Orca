using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tcc.Models;

namespace tcc
{
    public class RuleDriver
    {
        public IList<RuleResult> ExecuteRules(Repository repository)
        {
            var ruleClasses = ReflectiveEnumerator.GetEnumerableOfType<Rule>();
            List<RuleResult> results = new List<RuleResult>();

            foreach (var ruleClass in ruleClasses)
            {
                results.AddRange(ruleClass.Execute(repository));
            }

            return results;
        }

        public List<string> GetDPNames()
        {
            var ruleClasses = ReflectiveEnumerator.GetEnumerableOfType<Rule>();
            return ruleClasses.Select(r => r.DesignPattern.Name).Distinct().ToList();
        }

        public List<string> GetRuleNames()
        {
            var ruleClasses = ReflectiveEnumerator.GetEnumerableOfType<Rule>();
            return ruleClasses.Select(r => r.Name).Distinct().ToList();
        }
    }
}

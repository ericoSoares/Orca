using System;
using System.Collections.Generic;
using System.Text;
using tcc.Models;
using System.Linq;

namespace tcc
{
    public class FactoryRule1 : Rule
    {
        public FactoryRule1(Repository repository)
        {
            this.Repository = repository;
            this.Name = "ClassesWithMoreThan3Instantiations";
        }

        public override IList<RuleResult> Execute()
        {
            var results = new List<RuleResult>();

            var classesWithTooManyInstantiations = this.Repository
                .Entities
                .Where(r => r.SourceRelationships
                    .Where(r => r.Type == ERelationshipType.INSTANTIATION_IN_CONSTRUCTOR 
                        || r.Type == ERelationshipType.INSTANTIATION_IN_CLASS
                        || r.Type == ERelationshipType.INSTANTIATION_IN_METHOD)
                    .Count() >= 3)
                .ToList();

            results = classesWithTooManyInstantiations
                .Select(r => new RuleResult()
                {
                    FilePath = r.FilePath,
                    LineNumber = r.LineNumber,
                    Rule = this
                })
                .ToList();

            return results;
        }

    }
}

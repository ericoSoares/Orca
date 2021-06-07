using System;
using System.Collections.Generic;
using System.Text;
using tcc.Models;
using System.Linq;

namespace tcc
{
    public class FactoryRule1 : Rule
    {
        public FactoryRule1()
        {
            this.Name = "FatoryRule1";
            this.Description = "Uma classe A não pode instanciar uma classe B mais de 3 vezes";
            this.DesignPattern = this.DesignPatternRepository.Factory;
        }

        public override IList<RuleResult> Execute(Repository repository)
        {
            var results = new List<RuleResult>();

            var classesWithTooManyInstantiations = repository
                .Entities
                .Where(r => r.SourceRelationships
                    .Where(r => r.Type == ERelationshipType.INSTANTIATION_IN_CONSTRUCTOR
                        || r.Type == ERelationshipType.INSTANTIATION_IN_CLASS
                        || r.Type == ERelationshipType.INSTANTIATION_IN_METHOD)
                    .Count() >= 3)
                .Select(r => new RuleResult()
                {
                    FilePath = r.FilePath,
                    LineNumber = r.LineNumber,
                    Rule = this
                })
                .ToList();

            return classesWithTooManyInstantiations;
        }
    }

    public class FactoryRule2 : Rule
    {
        public FactoryRule2()
        {
            this.Name = "FatoryRule2";
            this.Description = "Um método não pode instanciar uma classe mais do que 3 vezes";
            this.DesignPattern = this.DesignPatternRepository.Factory;
            this.SeverityLevel = ESeverityLevel.BLOCKER;
        }

        public override IList<RuleResult> Execute(Repository repository)
        {
            var results = new List<RuleResult>();

            var classesWithTooManyInstantiations = repository
                .Entities
                .Where(r => r.SourceRelationships
                    .Where(r => r.Type == ERelationshipType.INSTANTIATION_IN_CONSTRUCTOR
                        || r.Type == ERelationshipType.INSTANTIATION_IN_CLASS
                        || r.Type == ERelationshipType.INSTANTIATION_IN_METHOD)
                    .Count() >= 3)
                .Select(r => new RuleResult()
                {
                    FilePath = r.FilePath,
                    LineNumber = r.LineNumber,
                    Rule = this
                })
                .ToList();

            return classesWithTooManyInstantiations;
        }
    }

    public class FactoryRule3 : Rule
    {
        public FactoryRule3()
        {
            this.Name = "FatoryRule3";
            this.Description = "Uma classe instancia classes da 'mesma familia' mais de uma vez";
            this.DesignPattern = this.DesignPatternRepository.Factory;
        }

        public override IList<RuleResult> Execute(Repository repository)
        {
            var results = new List<RuleResult>();

            var classesWithTooManyInstantiations = repository
                .Entities
                .Where(r => r.SourceRelationships
                    .Where(r => r.Type == ERelationshipType.INSTANTIATION_IN_CONSTRUCTOR
                        || r.Type == ERelationshipType.INSTANTIATION_IN_CLASS
                        || r.Type == ERelationshipType.INSTANTIATION_IN_METHOD)
                    .Count() >= 3)
                .Select(r => new RuleResult()
                {
                    FilePath = r.FilePath,
                    LineNumber = r.LineNumber,
                    Rule = this
                })
                .ToList();

            return classesWithTooManyInstantiations;
        }
    }

    public class BuilderRule1 : Rule
    {
        public BuilderRule1()
        {
            this.Name = "Builder";
            this.Description = "Se uma classe tiver mais que 5 dependencias, pode ser um caso de builder";
            this.DesignPattern = this.DesignPatternRepository.Builder;
        }

        public override IList<RuleResult> Execute(Repository repository)
        {
            return new List<RuleResult>();
        }
    }

    public class SingletonRule1 : Rule
    {
        public SingletonRule1()
        {
            this.Name = "SingletonRule1";
            this.Description = "Classes sem dependencias podem ser Singletons";
            this.DesignPattern = this.DesignPatternRepository.Singleton;
        }

        public override IList<RuleResult> Execute(Repository repository)
        {
            return new List<RuleResult>();
        }
    }

    public class CompositeRule1 : Rule
    {
        public CompositeRule1()
        {
            this.Name = "CompositeRule1";
            this.Description = "Herança em profundidade maior que 3 não é permitida";
            this.DesignPattern = this.DesignPatternRepository.Composite;
        }

        public override IList<RuleResult> Execute(Repository repository)
        {
            return new List<RuleResult>();
        }
    }
}

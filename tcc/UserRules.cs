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
            this.SeverityLevel = ESeverityLevel.CRITICAL;
        }

        public override IList<RuleResult> Execute(Repository repository)
        {
            return repository.Entities
                .Where(r => r.SourceRelationships
                    .Where(x => x.Type == ERelationshipType.INSTANTIATION_IN_CONSTRUCTOR
                        || x.Type == ERelationshipType.INSTANTIATION_IN_CLASS
                        || x.Type == ERelationshipType.INSTANTIATION_IN_METHOD)
                    .GroupBy(y => y.Target.SemanticType)
                    .Count() >= 3)
                .Select(r => new RuleResult()
                {
                    FilePath = r.FilePath,
                    LineNumber = r.LineNumber,
                    Rule = this
                })
                .ToList();
        }
    }

    public class FactoryRule2 : Rule
    {
        public FactoryRule2()
        {
            this.Name = "FatoryRule2";
            this.Description = "Um método não construtor não pode instanciar classes mais do que 3 vezes";
            this.DesignPattern = this.DesignPatternRepository.Factory;
            this.SeverityLevel = ESeverityLevel.BLOCKER;
        }

        public override IList<RuleResult> Execute(Repository repository)
        {
            return repository.Entities
                .Where(r => r.SourceRelationships
                    .Where(x => x.Type == ERelationshipType.INSTANTIATION_IN_METHOD)
                    .Count() >= 3)
                .Select(r => new RuleResult()
                {
                    FilePath = r.FilePath,
                    LineNumber = r.LineNumber,
                    Rule = this
                })
                .ToList();
        }
    }

    public class BuilderRule1 : Rule
    {
        public BuilderRule1()
        {
            this.Name = "BuilderRule1";
            this.Description = "Se uma classe tiver mais que 5 dependencias, pode ser um caso de builder";
            this.DesignPattern = this.DesignPatternRepository.Builder;
        }

        public override IList<RuleResult> Execute(Repository repository)
        {
            return repository.Entities
                .Where(r => r.SourceRelationships
                    .Where(x => x.Type == ERelationshipType.DEPENDENCY)
                    .Count() >= 5)
                .Select(r => new RuleResult()
                {
                    FilePath = r.FilePath,
                    LineNumber = r.LineNumber,
                    Rule = this
                })
                .ToList();
        }
    }

    public class PrototypeRule1 : Rule
    {
        public PrototypeRule1()
        {
            this.Name = "PrototypeRule1";
            this.Description = "Classe possui mais de 3 dependencias publicas e não possui recepções nem instanciações em construtor";
            this.DesignPattern = this.DesignPatternRepository.Singleton;
        }

        public override IList<RuleResult> Execute(Repository repository)
        {
            var classesWithMoreThan3PublicDependencies = repository.Entities
                .Where(r => r.SourceRelationships
                    .Where(x => x.Type == ERelationshipType.DEPENDENCY && x.AccessModifiers.Contains("public"))
                    .Count() > 3);
            return repository.Entities
                .Where(r => r.SourceRelationships
                    .Where(x => x.Type == ERelationshipType.DEPENDENCY && x.AccessModifiers.Contains("public"))
                    .Count() > 3)
                .Where(r => r.SourceRelationships
                    .Where(x => x.Type == ERelationshipType.RECEPTION_IN_CONSTRUCTOR 
                        || x.Type == ERelationshipType.INSTANTIATION_IN_CONSTRUCTOR)
                    .Count() == 0)
                .Select(r => new RuleResult()
                {
                    FilePath = r.FilePath,
                    LineNumber = r.LineNumber,
                    Rule = this
                })
                .ToList();
        }
    }

    public class SingletonRule1 : Rule
    {
        public SingletonRule1()
        {
            this.Name = "SingletonRule1";
            this.Description = "Classes estáticas sem dependencias podem ser Singletons";
            this.DesignPattern = this.DesignPatternRepository.Singleton;
        }

        public override IList<RuleResult> Execute(Repository repository)
        {
            return repository.Entities
                .Where(r => r.AccessModifier.Contains("static"))
                .Where(r => r.SourceRelationships
                    .Where(r => r.Type == ERelationshipType.DEPENDENCY)
                    .Count() == 0)
                .Select(r => new RuleResult()
                {
                    FilePath = r.FilePath,
                    LineNumber = r.LineNumber,
                    Rule = this
                })
                .ToList();
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

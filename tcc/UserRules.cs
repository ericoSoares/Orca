using System;
using System.Collections.Generic;
using System.Text;
using tcc.Models;
using System.Linq;

namespace tcc
{
    // Creational
    public class FactoryRule1 : Rule
    {
        public FactoryRule1()
        {
            this.Name = "FactoryRule1";
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
                    .Select(group => new
                    {
                        SemanticType = group.Key,
                        Count = group.Count()
                    })
                    .Where(r => r.Count > 3)
                    .Count() > 0)
                .Select(r => new RuleResult(r.FilePath, r.LineNumber, this))
                .ToList();
        }
    }

    public class FactoryRule2 : Rule
    {
        public FactoryRule2()
        {
            this.Name = "FactoryRule2";
            this.Description = "Uma classe não pode ter mais que 3 instanciações em métodos";
            this.DesignPattern = this.DesignPatternRepository.Factory;
            this.SeverityLevel = ESeverityLevel.MINOR;
        }

        public override IList<RuleResult> Execute(Repository repository)
        {
            return repository.Entities
                .Where(r => r.SourceRelationships
                    .Where(x => x.Type == ERelationshipType.INSTANTIATION_IN_METHOD)
                    .Count() >= 3)
                .Select(r => new RuleResult(r.FilePath, r.LineNumber, this))
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
            this.SeverityLevel = ESeverityLevel.MINOR;
        }

        public override IList<RuleResult> Execute(Repository repository)
        {
            return repository.Entities
                .Where(r => r.SourceRelationships
                    .Where(x => x.Type == ERelationshipType.DEPENDENCY)
                    .Count() >= 5)
                .Select(r => new RuleResult(r.FilePath, r.LineNumber, this))
                .ToList();
        }
    }

    public class BuilderRule2 : Rule
    {
        public BuilderRule2()
        {
            this.Name = "PrototypeRule1";
            this.Description = "Classe possui dependencias publicas e não possui recepções nem instanciações em construtor";
            this.DesignPattern = this.DesignPatternRepository.Builder;
            this.SeverityLevel = ESeverityLevel.BLOCKER;
        }

        public override IList<RuleResult> Execute(Repository repository)
        {
            var classesWithPublicDependencies = repository.Entities
                .Where(r => r.SourceRelationships
                    .Where(x => x.Type == ERelationshipType.DEPENDENCY && x.AccessModifiers.Contains("public"))
                    .Count() > 0)
                .ToList();

            var classesWithNoConstructorRelationships = classesWithPublicDependencies
                .Where(r => r.SourceRelationships
                    .Where(x => x.Type == ERelationshipType.INSTANTIATION_IN_CONSTRUCTOR
                        || x.Type == ERelationshipType.RECEPTION_IN_CONSTRUCTOR)
                    .Count() == 0)
                .ToList();

            return classesWithNoConstructorRelationships
                .Select(r => new RuleResult(r.FilePath, r.LineNumber, this))
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
            this.SeverityLevel = ESeverityLevel.INFO;
        }

        public override IList<RuleResult> Execute(Repository repository)
        {
            return repository.Entities
                .Where(r => r.AccessModifier.Contains("static"))
                .Where(r => r.SourceRelationships
                    .Where(r => r.Type == ERelationshipType.DEPENDENCY)
                    .Count() == 0)
                .Select(r => new RuleResult(r.FilePath, r.LineNumber, this))
                .ToList();
        }
    }

    // Structural
    public class CompositeRule1 : Rule
    {
        public CompositeRule1()
        {
            this.Name = "CompositeRule1";
            this.Description = "Herança em profundidade maior que 3 não é permitida";
            this.DesignPattern = this.DesignPatternRepository.Composite;
            this.SeverityLevel = ESeverityLevel.BLOCKER;
        }

        public override IList<RuleResult> Execute(Repository repository)
        {
            return repository.Entities
                .Where(r => r.SourceRelationships
                    .Where(x => x.Type == ERelationshipType.INHERITANCE)
                    .Count() > 2)
                .Select(r => new RuleResult(r.FilePath, r.LineNumber, this))
                .ToList();
        }
    }

    public class BridgeRule1 : Rule
    {
        public BridgeRule1()
        {
            this.Name = "BridgeRule1";
            this.Description = "Classe contem mais de 6 subclasses";
            this.DesignPattern = this.DesignPatternRepository.Bridge;
            this.SeverityLevel = ESeverityLevel.INFO;
        }

        public override IList<RuleResult> Execute(Repository repository)
        {
            return repository.Entities
                .Where(r => r.TargetRelationships
                    .Where(x => x.Type == ERelationshipType.INHERITANCE)
                    .Count() > 6)
                .Select(r => new RuleResult(r.FilePath, r.LineNumber, this))
                .ToList();
        }
    }

    public class FacadeRule1 : Rule
    {
        public FacadeRule1()
        {
            this.Name = "FacadeRule1";
            this.Description = "Uma classe está instanciando classes de outro projeto muitas vezes";
            this.DesignPattern = this.DesignPatternRepository.Facade;
            this.SeverityLevel = ESeverityLevel.BLOCKER;
        }

        public override IList<RuleResult> Execute(Repository repository)
        {
            return repository.Entities
                .Where(r => r.SourceRelationships
                    .Where(x => x.Type == ERelationshipType.INSTANTIATION_IN_CLASS
                        || x.Type == ERelationshipType.INSTANTIATION_IN_CONSTRUCTOR
                        || x.Type == ERelationshipType.INSTANTIATION_IN_METHOD)
                    .Where(x => x.Target.ProjectName != r.ProjectName)
                    .Count() > 3)
                .Select(r => new RuleResult(r.FilePath, r.LineNumber, this))
                .ToList();
        }
    }

    public class FlyweightRule1 : Rule
    {
        public FlyweightRule1()
        {
            this.Name = "FlyweightRule1";
            this.Description = "Muitas classes de um mesmo projeto dependem de uma classe de outro projeto";
            this.DesignPattern = this.DesignPatternRepository.Flyweight;
            this.SeverityLevel = ESeverityLevel.MINOR;
        }

        public override IList<RuleResult> Execute(Repository repository)
        {
            return repository.Entities
                .Where(r => r.TargetRelationships
                    .Where(x => x.Type == ERelationshipType.DEPENDENCY)
                    .Where(x => x.Source.ProjectName != r.ProjectName)
                    .Count() > 3)
                .Select(r => new RuleResult(r.FilePath, r.LineNumber, this))
                .ToList();
        }
    }

    public class MediatorRule1 : Rule
    {
        public MediatorRule1()
        {
            this.Name = "MediatorRule1";
            this.Description = "Classes de projetos distintos não devem se comunicar diretamente";
            this.DesignPattern = this.DesignPatternRepository.Mediator;
            this.SeverityLevel = ESeverityLevel.CRITICAL;
        }

        public override IList<RuleResult> Execute(Repository repository)
        {
            return repository.Entities
                .Where(r => r.SourceRelationships
                    .Where(x => x.Type == ERelationshipType.INSTANTIATION_IN_CLASS
                        || x.Type == ERelationshipType.INSTANTIATION_IN_CONSTRUCTOR
                        || x.Type == ERelationshipType.INSTANTIATION_IN_METHOD
                        || x.Type == ERelationshipType.RECEPTION_IN_CONSTRUCTOR
                        || x.Type == ERelationshipType.RECEPTION_IN_METHOD
                        || x.Type == ERelationshipType.DEPENDENCY)
                    .Where(x => x.Target.ProjectName != r.ProjectName)
                    .Count() > 0)
                .Select(r => new RuleResult(r.FilePath, r.LineNumber, this))
                .ToList();
        }
    }
}

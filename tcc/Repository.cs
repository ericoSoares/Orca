using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Text;
using tcc.Models;
using System.Linq;

namespace tcc
{
    public class Repository
    {
        public IList<Entity> Entities { get; set; }
        public IList<Relationship> Relationships { get; set; }

        public Repository()
        {
            this.Entities = new List<Entity>();
            this.Relationships = new List<Relationship>();
        }

        public bool AddRelationship(
            ERelationshipType type, string sourceType, string targetType, int lineNumber, string methodName = "", bool isMethodConstructor = false)
        {
            var foundSource = this.Entities.FirstOrDefault(r => r.SemanticType == sourceType);
            var foundTarget = this.Entities.FirstOrDefault(r => r.SemanticType == targetType);
            if (foundTarget == null || foundSource == null) return false;

            var relationship = new Relationship()
            {
                LineNumber = lineNumber,
                Source = foundSource,
                Target = foundTarget,
                Type = type,
                IsMethodConstructor = isMethodConstructor,
                MethodName = methodName
            };

            this.Relationships.Add(relationship);
            foundSource.SourceRelationships.Add(relationship);
            foundTarget.TargetRelationships.Add(relationship);

            return true;
        }
        
        public void PrintStatus()
        {
            var totalClasses = this.Entities.Where(r => r.Type == EEntityType.CLASS).Count();
            var totalInterfaces = this.Entities.Where(r => r.Type == EEntityType.INTERFACE).Count();
            var totalInheritances = this.Relationships.Where(r => r.Type == ERelationshipType.INHERITANCE).Count();
            var totalImplementations = this.Relationships.Where(r => r.Type == ERelationshipType.IMPLEMENTATION).Count();
            var totalReceptionsInMethod = this.Relationships.Where(r => r.Type == ERelationshipType.RECEPTION_IN_METHOD).Count();
            var totalReceptionsInCtor = this.Relationships.Where(r => r.Type == ERelationshipType.RECEPTION_IN_CONSTRUCTOR).Count();
            var totalInstantiationsInClass = this.Relationships.Where(r => r.Type == ERelationshipType.INSTANTIATION_IN_CLASS).Count();
            var totalInstantiationsInMethod = this.Relationships.Where(r => r.Type == ERelationshipType.INSTANTIATION_IN_METHOD).Count();
            var totalInstantiationsInCtor = this.Relationships.Where(r => r.Type == ERelationshipType.INSTANTIATION_IN_CONSTRUCTOR).Count();
            var totalDependencies = this.Relationships.Where(r => r.Type == ERelationshipType.DEPENDENCY).Count();

            Console.WriteLine("Total classes: " + totalClasses);
            Console.WriteLine("Total interfaces: " + totalInterfaces);
            Console.WriteLine("Total inheritances: " + totalInheritances);
            Console.WriteLine("Total implementations: " + totalImplementations);
            Console.WriteLine("Total recep on method: " + totalReceptionsInMethod);
            Console.WriteLine("Total recep on ctor: " + totalReceptionsInCtor);
            Console.WriteLine("Total inst in class: " + totalInstantiationsInClass);
            Console.WriteLine("Total inst in method: " + totalInstantiationsInMethod);
            Console.WriteLine("Total inst in ctor: " + totalInstantiationsInCtor);
            Console.WriteLine("Total dependencies: " + totalDependencies);

            Console.WriteLine("Total entities: " + this.Entities.Count);
            Console.WriteLine("Total relationships: " + this.Relationships.Count);
        }
    }
}

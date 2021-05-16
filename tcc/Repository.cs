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
        public IList<DesignPattern> DesignPatterns { get; set; } 

        public Repository()
        {
            this.Entities = new List<Entity>();
            this.Relationships = new List<Relationship>();
            this.DesignPatterns = new List<DesignPattern>()
            {
                new DesignPattern() { Name = "Factory", Description = "Factory design pattern", MoreInfoUrl = "www.google.com" },
                new DesignPattern() { Name = "Composite", Description = "Composite design pattern", MoreInfoUrl = "www.google.com" },
                new DesignPattern() { Name = "Visitor", Description = "Visitor design pattern", MoreInfoUrl = "www.google.com" },
            };
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
            Console.WriteLine("Total entities: " + this.Entities.Count);
            Console.WriteLine("Total relationships: " + this.Relationships.Count);
        }
    }
}

using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Text;
using tcc.Models;
using System.Linq;

namespace tcc
{
    class Repository
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

            this.Relationships.Add(new Relationship()
            {
                LineNumber = lineNumber,
                Source = foundSource,
                Target = foundTarget,
                Type = type,
                IsMethodConstructor = isMethodConstructor,
                MethodName = methodName
            });

            return true;
        }

        public void PrintStatus()
        {
            Console.WriteLine("Total entities: " + this.Entities.Count);
            Console.WriteLine("Total relationships: " + this.Relationships.Count);
        }
    }
}

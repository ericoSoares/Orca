using System;
using System.Collections.Generic;
using System.Text;

namespace tcc.Models
{
    class Relationship
    {
        public int Id { get; set; }
        public Entity Source { get; set; }
        public Entity Target { get; set; }
        public ERelationshipType Type { get; set; }
        public int LineNumber { get; set; }
        public string MethodName { get; set; }
        public bool IsMethodConstructor { get; set; }
        public string FileName
        {
            get
            {
                if (Source.SyntaxTree == null) return "";
                return Source.SyntaxTree.FilePath;
            }
        }
        public override string ToString()
        {
            var baseString = Enum.GetName(typeof(ERelationshipType), Type) + ": " + Source.SemanticType + " -> " + Target.SemanticType + " LINE: " + LineNumber;
            return baseString;
        }
    }
}

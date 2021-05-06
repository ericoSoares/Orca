using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace tcc.Models
{
    class Entity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public EEntityType Type { get; set; }
        public string AccessModifier { get; set; }
        public string SemanticType { get; set; }
        public int LineNumber { get; set; }
        public TypeDeclarationSyntax TypeDeclaration { get; set; }
        public SyntaxTree SyntaxTree { get; set; }
        public string FileName
        {
            get
            {
                if (SyntaxTree == null) return "";
                return SyntaxTree.FilePath;
            }
        }
    }
}

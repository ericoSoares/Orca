using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.MSBuild;
using tcc;

namespace SemanticQuickStart
{
    class Program
    {
        static void Main(string[] args)
        {

            var project1 = @"C:\Users\erico\source\repos\clean-architecture-manga\Clean-Architecture-Manga.sln";
            var project2 = @"C:\Users\erico\source\repos\TestProject\TestProject.sln";
            var extractor = new Extractor(project2);
            extractor.ReadSolution();
            var aaa = extractor.Repository.Entities.ToList();
            var sss = extractor.Repository.Relationships.ToList();
            var xx = aaa.Where(r => r.Type == tcc.Models.EEntityType.CLASS).ToList();
            var xxx = aaa.Where(r => r.Type == tcc.Models.EEntityType.INTERFACE).ToList();
            var x = 1;
        }
    }
}
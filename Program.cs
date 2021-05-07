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
            extractor.Run();
            extractor.Repository.PrintStatus();

            var ruleResults = new FactoryRule1(extractor.Repository).Execute();
            foreach(var ruleResult in ruleResults)
            {
                Console.WriteLine(ruleResult.ToString());
            }

            //foreach(var relationship in extractor.Repository.Relationships)
            //{
            //    Console.WriteLine(relationship.ToString());
            //}
        }
    }
}
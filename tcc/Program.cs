using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.MSBuild;
using tcc;
using tcc.Models;

namespace SemanticQuickStart
{
    public class Program
    {
        static void Main(string[] args)
        {

            var project1 = @"C:\Users\erico\source\repos\clean-architecture-manga\Clean-Architecture-Manga.sln";
            var project2 = @"C:\Users\erico\source\repos\TestProject\TestProject.sln";
            var project3 = @"C:\Users\erico\source\repos\eShopOnWeb\eShopOnWeb.sln";
            var project4 = @"C:\Users\erico\source\repos\DesignPatterns\DesignPatternsDotNetCore.sln";
            var project5 = @"C:\Users\erico\Source\Repos\sample-dotnet-core-cqrs-api\src\SampleProject.API.sln";
            var extractor = new Extractor(project1, new List<string>());
            extractor.Run();
            extractor.Repository.PrintStatus();
            var porra4 = extractor.Repository.Relationships.Where(r => r.Type == tcc.Models.ERelationshipType.IMPLEMENTATION).ToList();
            var porra5 = extractor.Repository.Relationships.Where(r => r.Type == tcc.Models.ERelationshipType.DEPENDENCY).ToList();
            var porra = extractor.Repository.Relationships.Where(r => r.Type == tcc.Models.ERelationshipType.INSTANTIATION_IN_CLASS).ToList();
            var porra2 = extractor.Repository.Relationships.Where(r => r.Type == tcc.Models.ERelationshipType.INSTANTIATION_IN_METHOD).ToList();
            var porra3 = extractor.Repository.Relationships.Where(r => r.Type == tcc.Models.ERelationshipType.INSTANTIATION_IN_CONSTRUCTOR).ToList();

            var ruleResults = new RuleDriver().ExecuteRules(extractor.Repository);
            foreach(var ruleResult in ruleResults)
            {
                Console.WriteLine(ruleResult.ToString());
            }
        }
    }
}
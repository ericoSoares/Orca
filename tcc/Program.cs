using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.MSBuild;
using tcc;

namespace SemanticQuickStart
{
    public class Program
    {
        static void Main(string[] args)
        {

            var project1 = @"C:\Users\erico\source\repos\clean-architecture-manga\Clean-Architecture-Manga.sln";
            var project2 = @"C:\Users\erico\source\repos\TestProject\TestProject.sln";
            var project3 = @"C:\Users\erico\source\repos\machinelearning\Microsoft.ML.sln";
            var project4 = @"C:\Users\erico\source\repos\eShopOnContainers\src\eShopOnContainers-ServicesAndWebApps.sln";
            var project5 = @"C:\Users\erico\source\repos\eShopOnWeb\eShopOnWeb.sln";
            var extractor = new Extractor(project5, new List<string>());
            extractor.Run();
            extractor.Repository.PrintStatus();

            var ruleResults = new FactoryRule1().Execute(extractor.Repository);

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
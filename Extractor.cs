using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.MSBuild;
using System.Collections.Immutable;
using Microsoft.Build.Locator;
using Newtonsoft.Json;

namespace tcc
{
	public class ConstructorSyntaxWalker : CSharpSyntaxWalker
	{
		public List<ISymbol> Parameters { get; set; }
		public int IfConditions { get; set; }

		SemanticModel sm;


		public ConstructorSyntaxWalker(SemanticModel sm)
		{
			this.sm = sm;
			Parameters = new List<ISymbol>();
		}

		public override void VisitIfStatement(IfStatementSyntax node)
		{
			Parameters.AddRange(sm.AnalyzeDataFlow(node).DataFlowsIn); // .AnalyzeDataFlow() is one of the most commonly used parts of the platform: it requires a compilation to work off and allows tracking dependencies. We could then check if these parameters are supplied to constructor and make a call whether this is allowed 
			IfConditions++; // just count for now, nothing fancy
			base.VisitIfStatement(node);
		}
	}

	class Extractor
    {
        private string SlnPath { get; set; }
		public Repository Repository { get; set; }

		public Extractor(string slnPath)
        {
            this.SlnPath = slnPath;
			this.Repository = new Repository();
        }

		public void ExtractEntities(SyntaxTree tree)
		{
			var root = tree.GetRoot();
			var interfaceDeclarations = root.DescendantNodes().OfType<InterfaceDeclarationSyntax>();
			var classDeclarations = root.DescendantNodes().OfType<ClassDeclarationSyntax>();

			var compilation = CSharpCompilation.Create("compilation")
				.AddReferences(
					MetadataReference.CreateFromFile(
					typeof(object).Assembly.Location))
				.AddSyntaxTrees(tree);

			var semanticModel = compilation.GetSemanticModel(tree);

			foreach (var interfaceDeclaration in interfaceDeclarations)
			{
				var typeSymbol = semanticModel.GetDeclaredSymbol(interfaceDeclaration);
				this.Repository.Entities.Add(new Models.Entity()
				{
					Name = interfaceDeclaration.Identifier.ValueText,
					SemanticType = typeSymbol.ToString(),
					LineNumber = interfaceDeclaration.SpanStart,
					AccessModifier = interfaceDeclaration.Modifiers.ToString(),
					Type = Models.EEntityType.INTERFACE
				});
			}

			foreach (var classDeclaration in classDeclarations)
			{
				var typeSymbol = semanticModel.GetDeclaredSymbol(classDeclaration);
				this.Repository.Entities.Add(new Models.Entity()
				{
					Name = classDeclaration.Identifier.ValueText,
					SemanticType = typeSymbol.ToString(),
					LineNumber = classDeclaration.SpanStart,
					AccessModifier = classDeclaration.Modifiers.ToString(),
					Type = Models.EEntityType.CLASS
				});
			}
		}


		public void ReadSolution()
        {
			MSBuildLocator.RegisterMSBuildPath("C:\\Program Files\\dotnet\\sdk\\3.1.101");
			using (var workspace = MSBuildWorkspace.Create())
			{
				// Read solution from specified path
				var solution = workspace.OpenSolutionAsync(SlnPath).Result;

				// For each .cs file in each project, generate syntax tree and extract information 
				foreach (var proj in solution.Projects)
				{
					Console.WriteLine("Project: " + proj.Name);
					foreach(var doc in proj.Documents)
					{
						this.ExtractEntities(doc.GetSyntaxTreeAsync().Result);
					}
				}
			}
		}

    }
}

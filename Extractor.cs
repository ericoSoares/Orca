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

        public Extractor(string slnPath)
        {
            this.SlnPath = slnPath;
        }

        public void ReadSolution()
        {
			MSBuildLocator.RegisterMSBuildPath("C:\\Program Files\\dotnet\\sdk\\3.1.101");
			using (var workspace = MSBuildWorkspace.Create())
			{
				var solution = workspace.OpenSolutionAsync(SlnPath).Result;
				foreach (var proj in solution.Projects)
				{
					Console.WriteLine(proj.Name);
					foreach(var doc in proj.Documents)
					{
						var tree = doc.GetSyntaxTreeAsync().Result;
						var root = doc.GetSyntaxRootAsync().Result;
						var compilation = CSharpCompilation.Create("teste")
							.AddReferences(
								MetadataReference.CreateFromFile(
								typeof(object).Assembly.Location))
							.AddSyntaxTrees(tree);

						var model = compilation.GetSemanticModel(tree);

						var porra = root.DescendantNodes().OfType<ClassDeclarationSyntax>().FirstOrDefault();
						if (porra != null)
						{
							var porra2 = model.GetDeclaredSymbol(porra);
							Console.WriteLine(porra.Identifier);
							Console.WriteLine("---" + porra2);
						}

					}
				}
			}
		}

    }
}

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

				var baseList = classDeclaration.BaseList?.DescendantNodes().OfType<SimpleBaseTypeSyntax>().ToList();
				if (baseList == null) continue;

				foreach(var baseItem in baseList) {
					Console.WriteLine("INHERITANCE: " + classDeclaration.Identifier + " -> " + baseItem.Type.ToString());
				}

				this.ExtractInstantiations(classDeclaration, semanticModel);
				this.ExtractAssociationsViaParameter(classDeclaration, semanticModel);
				this.ExtractCompositions(classDeclaration, semanticModel);
			}
		}

		public void ExtractInstantiations(ClassDeclarationSyntax classDeclaration, SemanticModel semanticModel)
		{
			var objCreations = classDeclaration
				.DescendantNodes().OfType<ObjectCreationExpressionSyntax>().ToList();
			
			foreach(var objCreation in objCreations)
            {
				var typeInfo = semanticModel.GetTypeInfo(objCreation);

                Console.WriteLine(
					"INSTANTIATION: " + classDeclaration.Identifier + " -> " + typeInfo.Type);
            }
		}

		public void ExtractAssociationsViaParameter(ClassDeclarationSyntax classDeclaration, SemanticModel semanticModel)
		{
			var methods = classDeclaration.DescendantNodes().OfType<MethodDeclarationSyntax>().ToList();
			foreach(var method in methods)
			{
				var parameters = method.ParameterList.Parameters.ToList();
				foreach(var param in parameters)
				{
					Console.WriteLine(
						"ASSOCIATION: " + classDeclaration.Identifier + " -> " + param.Type + " ON METHOD: " + method.Identifier.Text);
				}
			}
		}

		public void ExtractCompositions(ClassDeclarationSyntax classDeclaration, SemanticModel semanticModel)
		{
			var constructors = classDeclaration.DescendantNodes().OfType<ConstructorDeclarationSyntax>().ToList();
			var fieldDeclarations = classDeclaration.DescendantNodes().OfType<FieldDeclarationSyntax>().ToList();
			var propertyDeclarations = classDeclaration.DescendantNodes().OfType<PropertyDeclarationSyntax>().ToList();

			foreach(var constructor in constructors) 
			{
				var instantiations = constructor.Body.DescendantNodes().OfType<ObjectCreationExpressionSyntax>().ToList();
				foreach(var instantiation in instantiations)
				{
					var typeInfo = semanticModel.GetTypeInfo(instantiation);
					Console.WriteLine("COMPOSITION: " + classDeclaration.Identifier + " -> " + typeInfo.Type);
				}
			}

			foreach(var field in fieldDeclarations)
			{
				var objCreation = field.DescendantNodes().OfType<ObjectCreationExpressionSyntax>().FirstOrDefault();
				if (objCreation != null)
				{
					var typeInfo = semanticModel.GetTypeInfo(objCreation);
					Console.WriteLine("COMPOSITION: " + classDeclaration.Identifier + " -> " + typeInfo.Type);
				}
			}

			foreach (var prop in propertyDeclarations)
			{
				var objCreation = prop.DescendantNodes().OfType<ObjectCreationExpressionSyntax>().FirstOrDefault();
				if (objCreation != null)
				{
					var typeInfo = semanticModel.GetTypeInfo(objCreation);
					Console.WriteLine("COMPOSITION: " + classDeclaration.Identifier + " -> " + typeInfo.Type);
				}
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

					foreach (var doc in proj.Documents)
					{
						this.ExtractEntities(doc.GetSyntaxTreeAsync().Result);
					}
				}
			}
		}

    }
}

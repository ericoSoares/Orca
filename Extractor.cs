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

				this.ExtractInheritances(classDeclaration, typeSymbol, semanticModel);
				this.ExtractInstantiations(classDeclaration, typeSymbol, semanticModel);
				this.ExtractAssociationsViaParameter(classDeclaration, typeSymbol, semanticModel);
				this.ExtractCompositions(classDeclaration, typeSymbol, semanticModel);
			}
		}

		public void ExtractInheritances(
			ClassDeclarationSyntax classDeclaration, INamedTypeSymbol curClassTypeSymbol, SemanticModel semanticModel)
		{
			//var baseList = classDeclaration.BaseList?.DescendantNodes().OfType<SimpleBaseTypeSyntax>().ToList();

			//if (baseList == null) return;

			//foreach (var baseItem in baseList)
			//{
			//	var baseItemNode = baseItem.DescendantNodes().FirstOrDefault();
			//	if (baseItemNode == null) continue;
			//	var typeInfo = semanticModel.GetTypeInfo(baseItemNode);
				
			//}
			var baseType = curClassTypeSymbol.BaseType;
			while(baseType != null)
			{
				if (baseType.Name != "Object")
				{
					Console.WriteLine("INHERITANCE: " + curClassTypeSymbol.ToDisplayString() + " -> " + baseType.Name);
				} 
				baseType = baseType.BaseType;
			}
		}

		public void ExtractInstantiations(
			ClassDeclarationSyntax classDeclaration, INamedTypeSymbol curClassTypeSymbol, SemanticModel semanticModel)
		{
			var objCreations = classDeclaration
				.DescendantNodes().OfType<ObjectCreationExpressionSyntax>().ToList();
			
			foreach(var objCreation in objCreations)
            {
				var typeInfo = semanticModel.GetTypeInfo(objCreation);

                Console.WriteLine(
					"INSTANTIATION: " + curClassTypeSymbol.ToDisplayString() + " -> " + typeInfo.Type);
            }
		}

		public void ExtractAssociationsViaParameter(
			ClassDeclarationSyntax classDeclaration, INamedTypeSymbol curClassTypeSymbol, SemanticModel semanticModel)
		{
			var methods = classDeclaration.DescendantNodes().OfType<MethodDeclarationSyntax>().ToList();
			foreach(var method in methods)
			{
				var parameters = method.ParameterList.Parameters.ToList();
				foreach(var param in parameters)
				{
					var porra = param.DescendantNodes().ToList();
					var typeInfo = semanticModel.GetTypeInfo(param);
					Console.WriteLine(
						"ASSOCIATION: " + curClassTypeSymbol.ToDisplayString() + " -> " + param.Type + " ON METHOD: " + method.Identifier.Text);
				}
			}
		}

		public void ExtractCompositions(
			ClassDeclarationSyntax classDeclaration, INamedTypeSymbol curClassTypeSymbol, SemanticModel semanticModel)
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
					Console.WriteLine("COMPOSITION: " + curClassTypeSymbol.ToDisplayString() + " -> " + typeInfo.Type);
				}
			}

			foreach(var field in fieldDeclarations)
			{
				var objCreation = field.DescendantNodes().OfType<ObjectCreationExpressionSyntax>().FirstOrDefault();
				if (objCreation != null)
				{
					var typeInfo = semanticModel.GetTypeInfo(objCreation);
					Console.WriteLine("COMPOSITION: " + curClassTypeSymbol.ToDisplayString() + " -> " + typeInfo.Type);
				}
			}

			foreach (var prop in propertyDeclarations)
			{
				var objCreation = prop.DescendantNodes().OfType<ObjectCreationExpressionSyntax>().FirstOrDefault();
				if (objCreation != null)
				{
					var typeInfo = semanticModel.GetTypeInfo(objCreation);
					Console.WriteLine("COMPOSITION: " + curClassTypeSymbol.ToDisplayString() + " -> " + typeInfo.Type);
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

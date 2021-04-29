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

		public void ExtractEntities(Compilation compilation)
		{
			foreach (var syntaxTree in compilation.SyntaxTrees)
			{
				var root = syntaxTree.GetRoot();
				var interfaceDeclarations = root.DescendantNodes().OfType<InterfaceDeclarationSyntax>().ToList();
				var classDeclarations = root.DescendantNodes().OfType<ClassDeclarationSyntax>().ToList();

				var semanticModel = compilation.GetSemanticModel(syntaxTree);

				foreach (var interfaceDeclaration in interfaceDeclarations)
				{
					var typeSymbol = semanticModel.GetDeclaredSymbol(interfaceDeclaration);
					this.Repository.Entities.Add(new Models.Entity()
					{
						Name = interfaceDeclaration.Identifier.ValueText,
						SemanticType = typeSymbol.ToString(),
						LineNumber = interfaceDeclaration.SpanStart,
						AccessModifier = interfaceDeclaration.Modifiers.ToString(),
						Type = Models.EEntityType.INTERFACE,
						SyntaxTree = syntaxTree,
						TypeDeclaration = interfaceDeclaration
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
						Type = Models.EEntityType.CLASS,
						SyntaxTree = syntaxTree,
						TypeDeclaration = classDeclaration
					});
				}
			}
		}

		public void ExtractRelationships(Compilation compilation)
		{
			foreach (var entity in this.Repository.Entities.Where(r => r.Type == Models.EEntityType.CLASS))
			{
				var semanticModel = compilation.GetSemanticModel(entity.SyntaxTree);
				var typeSymbol = semanticModel.GetDeclaredSymbol(entity.TypeDeclaration);

				//this.ExtractInheritances(entity.TypeDeclaration, typeSymbol, semanticModel);
				//this.ExtractImplementations(entity.TypeDeclaration, typeSymbol, semanticModel);
				this.ExtractInstantiations(entity.TypeDeclaration, typeSymbol, semanticModel);
				//this.ExtractReceptionsViaParameter(entity.TypeDeclaration, typeSymbol, semanticModel);
				//this.ExtractCompositions(entity.TypeDeclaration, typeSymbol, semanticModel);
				//this.ExtractInheritances(entity.TypeDeclaration, typeSymbol, semanticModel);
				//this.ExtractImplementations(entity.TypeDeclaration, typeSymbol, semanticModel);
				//this.ExtractInstantiaions(entity.TypeDeclaration, typeSymbol, semanticModel);
				//this.ExtractReceptionsOnMethod(entity.TypeDeclaration, typeSymbol, semanticModel);
				//this.ExtractReceptionsOnConstructor(entity.TypeDeclaration, typeSymbol, semanticModel);
			}
		}

		public void ExtractInheritances(
			TypeDeclarationSyntax classDeclaration, INamedTypeSymbol curClassTypeSymbol, SemanticModel semanticModel)
		{
			var baseType = curClassTypeSymbol.BaseType;
			while(baseType != null)
			{
				if (baseType.Name != "Object")
				{
					this.Repository.AddRelationship(
						Models.ERelationshipType.INHERITANCE, curClassTypeSymbol.ToString(), baseType.ToString(), classDeclaration.SpanStart);

					Console.WriteLine("INHERITANCE: " + curClassTypeSymbol.ToDisplayString() + " -> " + baseType.ToString());
				} 
				baseType = baseType.BaseType;
			}
		}

		public void ExtractImplementations(
			TypeDeclarationSyntax classDeclaration, INamedTypeSymbol curClassTypeSymbol, SemanticModel semanticModel)
		{
			var interfaces = curClassTypeSymbol.AllInterfaces;
			foreach(var curInterface in interfaces)
			{
				this.Repository.AddRelationship(
					Models.ERelationshipType.IMPLEMENTATION, curClassTypeSymbol.ToString(), curInterface.ToString(), classDeclaration.SpanStart);

				Console.WriteLine("IMPLEMENTATION: " + curClassTypeSymbol.ToDisplayString() + " -> " + curInterface.ToString());
			}
		}

		public void ExtractInstantiations(
			TypeDeclarationSyntax classDeclaration, INamedTypeSymbol curClassTypeSymbol, SemanticModel semanticModel)
		{
			var objCreations = classDeclaration
				.DescendantNodes().OfType<ObjectCreationExpressionSyntax>().ToList();
			
			foreach(var objCreation in objCreations)
            {
				var typeInfo = semanticModel.GetTypeInfo(objCreation);

				// Busca nodos pais do nodo atual
				var ancestors = objCreation.Ancestors().ToList();

				// Busca nodo de declaração de construtor ou metodo
				var ancestorConstructor = ancestors.OfType<ConstructorDeclarationSyntax>().FirstOrDefault();
				var ancestorMethod = ancestors.OfType<MethodDeclarationSyntax>().FirstOrDefault();

				// Define tipo e nome do método caso a instanciação seja dentro de um construtor ou metodo
				var type = Models.ERelationshipType.INSTANTIATION_IN_CLASS;
				var methodName = "";
				if (ancestorConstructor != null)
				{
					type = Models.ERelationshipType.INSTANTIATION_IN_CONSTRUCTOR;
					methodName = "ctor";
				}

				if (ancestorMethod != null)
				{
					type = Models.ERelationshipType.INSTANTIATION_IN_METHOD;
					methodName = ancestorMethod.Identifier.ToString();
				}

				this.Repository.AddRelationship(
					type, 
					curClassTypeSymbol.ToString(), 
					typeInfo.Type.ToString(), 
					classDeclaration.SpanStart,
					methodName,
					(type == Models.ERelationshipType.INSTANTIATION_IN_CONSTRUCTOR));

				Console.WriteLine(
					"INSTANTIATION: " + curClassTypeSymbol.ToDisplayString() + " -> " + typeInfo.Type);
            }
		}

		public void ExtractReceptionsViaParameter(
			TypeDeclarationSyntax classDeclaration, INamedTypeSymbol curClassTypeSymbol, SemanticModel semanticModel)
		{
			var methods = classDeclaration.DescendantNodes().OfType<MethodDeclarationSyntax>().ToList();
			foreach(var method in methods)
			{
				var parameters = method.ParameterList.Parameters.ToList();
				foreach(var param in parameters)
				{
					var porra = param.DescendantNodes().ToList();
					var typeInfo = semanticModel.GetTypeInfo(param);
					this.Repository.Relationships.Add(new Models.Relationship()
					{
						LineNumber = classDeclaration.SpanStart,
						Source = this.Repository.Entities.FirstOrDefault(r => r.SemanticType == curClassTypeSymbol.ToString()),
						Target = this.Repository.Entities.FirstOrDefault(r => r.SemanticType == typeInfo.Type.ToString()),
						Type = Models.ERelationshipType.RECEPTION_ON_METHOD
					});
					Console.WriteLine(
						"ASSOCIATION: " + curClassTypeSymbol.ToDisplayString() + " -> " + param.Type + " ON METHOD: " + method.Identifier.Text);
				}
			}
		}

		public void ExtractCompositions(
			TypeDeclarationSyntax classDeclaration, INamedTypeSymbol curClassTypeSymbol, SemanticModel semanticModel)
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
			var partialCompilation = CSharpCompilation.Create("compilation")
				.AddReferences(
					MetadataReference.CreateFromFile(
					typeof(object).Assembly.Location));

			List<SyntaxTree> trees = new List<SyntaxTree>();
			using (var workspace = MSBuildWorkspace.Create())
			{
				// Read solution from specified path
				var solution = workspace.OpenSolutionAsync(SlnPath).Result;

				// For each .cs file in each project, generate syntax tree and extract information 
				foreach (var proj in solution.Projects)
				{
					Console.WriteLine("Project: " + proj.Name);
					var excluded = new List<string>() { "ComponentTests", "IntegrationTests", "UnitTests" };

					if (excluded.Contains(proj.Name)) continue;

					foreach (var doc in proj.Documents)
					{
						trees.Add(doc.GetSyntaxTreeAsync().Result);
					}
				}
			}

			var compilation = (Compilation)partialCompilation.AddSyntaxTrees(trees);

			this.ExtractEntities(compilation);
			this.ExtractRelationships(compilation);
		}

    }
}

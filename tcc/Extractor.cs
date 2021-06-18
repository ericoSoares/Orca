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
using tcc.Models;

namespace tcc
{
	public class Extractor
    {
        private string SlnPath { get; set; }
		private IList<string> Excluded { get; set; }
		public Repository Repository { get; set; }

		public Extractor(string slnPath, IList<string> excluded)
        {
            this.SlnPath = slnPath;
			this.Excluded = excluded;
			this.Repository = new Repository();
        }

		public void ExtractEntities(Compilation compilation, IList<Project> projects)
		{
			foreach (var syntaxTree in compilation.SyntaxTrees)
			{
				var root = syntaxTree.GetRoot();
				var interfaceDeclarations = root.DescendantNodes().OfType<InterfaceDeclarationSyntax>().ToList();
				var classDeclarations = root.DescendantNodes().OfType<ClassDeclarationSyntax>().ToList();

				var semanticModel = compilation.GetSemanticModel(syntaxTree);

				var currentProject = projects
					.Where(r => r.Documents.Select(x => x.FilePath.ToString()).Contains(syntaxTree.FilePath)).FirstOrDefault();

				foreach (var interfaceDeclaration in interfaceDeclarations)
				{
					var typeSymbol = semanticModel.GetDeclaredSymbol(interfaceDeclaration);
					this.Repository.Entities.Add(new Models.Entity()
					{
						Name = interfaceDeclaration.Identifier.ValueText,
						SemanticType = typeSymbol.ToString(),
						AccessModifier = interfaceDeclaration.Modifiers.ToString(),
						Type = EEntityType.INTERFACE,
						SyntaxTree = syntaxTree,
						TypeDeclaration = interfaceDeclaration,
						SourceRelationships = new List<Relationship>(),
						TargetRelationships = new List<Relationship>(),
						ProjectName = currentProject.Name.ToString()
					});
				}

				foreach (var classDeclaration in classDeclarations)
				{
					var typeSymbol = semanticModel.GetDeclaredSymbol(classDeclaration);
					this.Repository.Entities.Add(new Models.Entity()
					{
						Name = classDeclaration.Identifier.ValueText,
						SemanticType = typeSymbol.ToString(),
						AccessModifier = classDeclaration.Modifiers.ToString(),
						Type = EEntityType.CLASS,
						SyntaxTree = syntaxTree,
						TypeDeclaration = classDeclaration,
						SourceRelationships = new List<Relationship>(),
						TargetRelationships = new List<Relationship>(),
						ProjectName = currentProject.Name.ToString()
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

				this.ExtractInheritances(entity.TypeDeclaration, typeSymbol, semanticModel);
				this.ExtractImplementations(entity.TypeDeclaration, typeSymbol, semanticModel);
				this.ExtractInstantiations(entity.TypeDeclaration, typeSymbol, semanticModel);
				this.ExtractDependencies(entity.TypeDeclaration, typeSymbol, semanticModel);
				this.ExtractReceptions(entity.TypeDeclaration, typeSymbol, semanticModel);
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
						Models.ERelationshipType.INHERITANCE, curClassTypeSymbol.ToString(), baseType.ToString(),
						classDeclaration.GetLocation().GetMappedLineSpan().StartLinePosition.Line);
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
					Models.ERelationshipType.IMPLEMENTATION, curClassTypeSymbol.ToString(), curInterface.ToString(), 
					classDeclaration.GetLocation().GetMappedLineSpan().StartLinePosition.Line);
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
					objCreation.GetLocation().GetMappedLineSpan().StartLinePosition.Line,
					methodName,
					(type == Models.ERelationshipType.INSTANTIATION_IN_CONSTRUCTOR));

            }
		}

		// TODO: Tratar caso de object initializers
		public void ExtractReceptions(
			TypeDeclarationSyntax classDeclaration, INamedTypeSymbol curClassTypeSymbol, SemanticModel semanticModel)
		{
			var methods = classDeclaration.DescendantNodes().OfType<MethodDeclarationSyntax>().ToList();
			var ctors = classDeclaration.DescendantNodes().OfType<ConstructorDeclarationSyntax>().ToList();
			foreach(var method in methods)
			{
				var parameters = method.ParameterList.Parameters.ToList();
				foreach(var param in parameters)
				{
					var declaredSymbol = semanticModel.GetDeclaredSymbol(param);
					
					this.Repository.AddRelationship(
						Models.ERelationshipType.RECEPTION_IN_METHOD,
						curClassTypeSymbol.ToString(),
						declaredSymbol.Type.ToString(),
						param.GetLocation().GetMappedLineSpan().StartLinePosition.Line,
						method.Identifier.ToString());
					
				}
			}

			foreach(var ctor in ctors)
			{
				var parameters = ctor.ParameterList.Parameters.ToList();
				foreach(var param in parameters)
				{
					var declaredSymbol = semanticModel.GetDeclaredSymbol(param);

					this.Repository.AddRelationship(
						Models.ERelationshipType.RECEPTION_IN_CONSTRUCTOR,
						curClassTypeSymbol.ToString(),
						declaredSymbol.Type.ToString(),
						classDeclaration.SpanStart,
						"ctor", true);
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
				}
			}

			foreach(var field in fieldDeclarations)
			{
				var objCreation = field.DescendantNodes().OfType<ObjectCreationExpressionSyntax>().FirstOrDefault();
				if (objCreation != null)
				{
					var typeInfo = semanticModel.GetTypeInfo(objCreation);
				}
			}

			foreach (var prop in propertyDeclarations)
			{
				var objCreation = prop.DescendantNodes().OfType<ObjectCreationExpressionSyntax>().FirstOrDefault();
				if (objCreation != null)
				{
					var typeInfo = semanticModel.GetTypeInfo(objCreation);
				}
			}
		}

		public void ExtractDependencies(
			TypeDeclarationSyntax classDeclaration, INamedTypeSymbol curClassTypeSymbol, SemanticModel semanticModel)
		{
			var props = classDeclaration.DescendantNodes().OfType<PropertyDeclarationSyntax>().ToList();
			var fields = classDeclaration.DescendantNodes().OfType<FieldDeclarationSyntax>().ToList();
			foreach(var prop in props)
			{
				var declaredSymbol = semanticModel.GetDeclaredSymbol(prop);
				var type = declaredSymbol.Type;
				var accessModifiers = prop.Modifiers.ToString();
				this.Repository.AddRelationship(
					Models.ERelationshipType.DEPENDENCY,
					curClassTypeSymbol.ToString(),
					type.ToString(),
					prop.GetLocation().GetMappedLineSpan().StartLinePosition.Line,
					"", false, accessModifiers);

			}

			foreach(var field in fields)
			{
				var typeSymbol = semanticModel.GetTypeInfo(field.Declaration.Type);
				var type = typeSymbol.Type.ToString();
				var accessModifiers = field.Modifiers.ToString();
				this.Repository.AddRelationship(
					Models.ERelationshipType.DEPENDENCY,
					curClassTypeSymbol.ToString(),
					type,
					field.GetLocation().GetMappedLineSpan().StartLinePosition.Line,
					"", false, accessModifiers);
			}

		}

		public void Run()
        {
			if(MSBuildLocator.CanRegister)
				MSBuildLocator.RegisterMSBuildPath("C:\\Program Files\\dotnet\\sdk\\3.1.101");
			
			var partialCompilation = CSharpCompilation.Create("compilation")
				.AddReferences(
					MetadataReference.CreateFromFile(
					typeof(object).Assembly.Location));

			List<SyntaxTree> trees = new List<SyntaxTree>();
			List<Project> projects = new List<Project>();
			using (var workspace = MSBuildWorkspace.Create())
			{
				// Read solution from specified path
				var solution = workspace.OpenSolutionAsync(SlnPath).Result;

				// For each .cs file in each project, generate syntax tree and extract information 
				foreach (var proj in solution.Projects)
				{
					Console.WriteLine("Project: " + proj.Name);

					if (this.Excluded != null && this.Excluded.Contains(proj.Name)) continue;

					projects.Add(proj);

					foreach (var doc in proj.Documents)
					{
						Console.WriteLine(doc.Name.ToString());
						var syntaxTree = doc.GetSyntaxTreeAsync().Result;
						trees.Add(syntaxTree);
					}
				}
			}

			var compilation = (Compilation)partialCompilation.AddSyntaxTrees(trees);

			this.ExtractEntities(compilation, projects);
			this.ExtractRelationships(compilation);
		}
    }
}

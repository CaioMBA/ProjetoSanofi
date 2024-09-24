using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using System.Reflection;
using System.Text;

namespace Service.Builders
{
    internal class EntitiesAndTypesGeneratorService
    {
        public static Assembly Generate(List<TableInformationModel> tableInformationModels)
        {
            var codeBuilder = new StringBuilder();

            codeBuilder.AppendLine("using System;");
            codeBuilder.AppendLine("using System.Collections.Generic;");
            codeBuilder.AppendLine("using GraphQL.Types;");
            codeBuilder.AppendLine("namespace GeneratedEntitiesAndTypes");
            codeBuilder.AppendLine("{");

            foreach (TableInformationModel? table in tableInformationModels)
            {
                if (table == null)
                {
                    continue;
                }
                if (table.COLUMNS == null || table.COLUMNS.Count == 0)
                {
                    continue;
                }

                // Generate entity class
                codeBuilder.AppendLine($"public class {table.TABLE_NAME}Entity");
                codeBuilder.AppendLine("{");
                foreach (Table_Column column in table.COLUMNS)
                {
                    string nullableModifier = (bool)column.Nullable! ? "?" : "";
                    codeBuilder.AppendLine($"    public {MapType(column.Type)}{nullableModifier} {column.Name} {{ get; set; }}");
                }
                codeBuilder.AppendLine("}");

                // Generate GraphQL type class
                codeBuilder.AppendLine($"public class {table.TABLE_NAME}Type : ObjectGraphType<{table.TABLE_NAME}Entity>");
                codeBuilder.AppendLine("{");
                codeBuilder.AppendLine($"public {table.TABLE_NAME}Type()");
                codeBuilder.AppendLine("{");
                foreach (Table_Column column in table.COLUMNS)
                {
                    codeBuilder.AppendLine($"Field(x => x.{column.Name}).Description(\"The column: {column.Name} nullable: {column.Nullable}, filterable: {column.IsFilterable}, retrievable: {column.IsRetrievable}\");");
                }
                codeBuilder.AppendLine("}");
                codeBuilder.AppendLine("}");
            }

            codeBuilder.AppendLine("}");

            return CompileCode(codeBuilder.ToString());
        }

        private static string MapType(string dbType)
        {
            return dbType.ToLower() switch
            {
                "int" => "int",
                "tinyint" => "byte",
                "varchar" => "string",
                "datetime" => "DateTime",
                "bit" => "bool",
                _ => "string" // Default to string for unsupported types
            };
        }
        private static Assembly CompileCode(string code)
        {
            var syntaxTree = CSharpSyntaxTree.ParseText(code);
            var references = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => !a.IsDynamic)
                .Select(a => MetadataReference.CreateFromFile(a.Location))
                .Cast<MetadataReference>()
                .ToList();

            var compilation = CSharpCompilation.Create(
                "GeneratedsAssembly",
                new[] { syntaxTree },
                references,
                new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            using var ms = new MemoryStream();
            EmitResult result = compilation.Emit(ms);

            if (!result.Success)
            {
                var failures = result.Diagnostics.Where(diagnostic =>
                    diagnostic.IsWarningAsError ||
                    diagnostic.Severity == DiagnosticSeverity.Error);

                foreach (var diagnostic in failures)
                {
                    Console.Error.WriteLine($"{diagnostic.Id}: {diagnostic.GetMessage()}");
                }

                throw new InvalidOperationException("Entity generation failed");
            }

            ms.Seek(0, SeekOrigin.Begin);
            return Assembly.Load(ms.ToArray());
        }
    }
}

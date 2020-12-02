using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace NotifyPropertyChangedGenerator
{
  [Generator]
  public class NotifyPropertyChangedGenerator : ISourceGenerator
  {
    public void Initialize(GeneratorInitializationContext context)
    {
    }

    public void Execute(GeneratorExecutionContext context)
    {
      // uncomment to debug the actual build of the target project
      // Debugger.Launch();
      var compilation = context.Compilation;
      var notifyInterface = compilation.GetTypeByMetadataName("System.ComponentModel.INotifyPropertyChanged");

      foreach (var syntaxTree in compilation.SyntaxTrees)
      {
        var semanticModel = compilation.GetSemanticModel(syntaxTree);
        var immutableHashSet = syntaxTree.GetRoot()
          .DescendantNodesAndSelf()
          .OfType<ClassDeclarationSyntax>()
          .Select(x => semanticModel.GetDeclaredSymbol(x))
          .OfType<ITypeSymbol>()
          .Where(x => x.Interfaces.Contains(notifyInterface))
          .ToImmutableHashSet();

        foreach (var typeSymbol in immutableHashSet)
        {
          var source = GeneratePropertyChanged(typeSymbol);
          context.AddSource($"{typeSymbol.Name}.Notify.cs", source);
        }
      }
    }

    private string GeneratePropertyChanged(ITypeSymbol typeSymbol)
    {
      return $@"
using System.ComponentModel;

namespace {typeSymbol.ContainingNamespace}
{{
  partial class {typeSymbol.Name}
  {{
    {GenerateProperties(typeSymbol)}
    public event PropertyChangedEventHandler? PropertyChanged;
  }}
}}";
    }

    private static string GenerateProperties(ITypeSymbol typeSymbol)
    {
      var sb = new StringBuilder();
      var suffix = "BackingField";

      foreach (var fieldSymbol in typeSymbol.GetMembers().OfType<IFieldSymbol>()
        .Where(x=>x.Name.EndsWith(suffix)))
      {
        var propertyName = fieldSymbol.Name[..^suffix.Length];
        sb.AppendLine($@"
    public {fieldSymbol.Type} {propertyName}
    {{
      get => {fieldSymbol.Name};
      set
      {{
        {fieldSymbol.Name} = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof({propertyName})));
      }}
    }}");
      }

      return sb.ToString();
    }
  }
}

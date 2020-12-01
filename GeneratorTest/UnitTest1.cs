using System.Collections.Immutable;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using NUnit.Framework;
using PropertyChangedGenerator = NotifyPropertyChangedGenerator.NotifyPropertyChangedGenerator;

namespace GeneratorTest
{
  [TestFixture]
  public class GeneratorTest
  {
    [Test]
    public void SimpleGeneratorTest()
    {
      string userSource = @"
using System;
using System.ComponentModel;

namespace NotifyPropertyChangedDemo
{
  public class Test : INotifyPropertyChanged
  {
    private int regularField;
    private int IndexBackingField;
  }
}
";
      var comp = CreateCompilation(userSource);
      var newComp = RunGenerators(comp, out _, new PropertyChangedGenerator());

      var newFile = newComp.SyntaxTrees
        .Single(x => Path.GetFileName(x.FilePath).EndsWith(".Notify.cs"));

      Assert.NotNull(newFile);
      Assert.IsTrue(newFile.FilePath.EndsWith("Test.Notify.cs"));

      var generatedText = newFile.GetText().ToString();

      var expectedOutput = @"
using System.ComponentModel;

namespace NotifyPropertyChangedDemo
{
  partial class Test
  {
    
    public int Index
    {
      get => IndexBackingField;
      set
      {
        IndexBackingField = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Index)));
      }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
  }
}";

      Assert.AreEqual(expectedOutput, generatedText);
    }



    private static Compilation CreateCompilation(string source)
      => CSharpCompilation.Create("compilation",
        new[] { CSharpSyntaxTree.ParseText(source, new CSharpParseOptions(LanguageVersion.CSharp9)) },
        new[] { MetadataReference.CreateFromFile(typeof(Binder).GetTypeInfo().Assembly.Location), MetadataReference.CreateFromFile(typeof(INotifyPropertyChanged).GetTypeInfo().Assembly.Location) },
        new CSharpCompilationOptions(OutputKind.ConsoleApplication));

    private static GeneratorDriver CreateDriver(params ISourceGenerator[] generators)
      => CSharpGeneratorDriver.Create(generators);

    private static Compilation RunGenerators(Compilation compilation, out ImmutableArray<Diagnostic> diagnostics, params ISourceGenerator[] generators)
    {
      CreateDriver(generators).RunGeneratorsAndUpdateCompilation(compilation, out var newCompilation, out diagnostics);
      return newCompilation;
    }
  }
}
﻿using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Threading.Tasks;

namespace Csla.Analyzers.Tests
{
  [TestClass]
  public sealed class IsBusinessObjectSerializableAnalyzerTests
  {
    [TestMethod]
    public void VerifySupportedDiagnostics()
    {
      var analyzer = new IsBusinessObjectSerializableAnalyzer();
      var diagnostics = analyzer.SupportedDiagnostics;
      Assert.AreEqual(1, diagnostics.Length);

      var diagnostic = diagnostics[0];
      Assert.AreEqual(diagnostic.Id, IsBusinessObjectSerializableConstants.DiagnosticId,
        nameof(DiagnosticDescriptor.Id));
      Assert.AreEqual(diagnostic.Title.ToString(), IsBusinessObjectSerializableConstants.Title,
        nameof(DiagnosticDescriptor.Title));
      Assert.AreEqual(diagnostic.MessageFormat.ToString(), IsBusinessObjectSerializableConstants.Message,
        nameof(DiagnosticDescriptor.MessageFormat));
      Assert.AreEqual(diagnostic.Category, IsBusinessObjectSerializableConstants.Category,
        nameof(DiagnosticDescriptor.Category));
      Assert.AreEqual(diagnostic.DefaultSeverity, DiagnosticSeverity.Error,
        nameof(DiagnosticDescriptor.DefaultSeverity));
    }

    private static async Task RunAnalysisAsync(string path, int expectedDiagnosticCount)
    {
      var code = File.ReadAllText(path);
      var diagnostics = await TestHelpers.GetDiagnosticsAsync(
        code, new IsBusinessObjectSerializableAnalyzer());
      Assert.AreEqual(expectedDiagnosticCount, diagnostics.Count, nameof(diagnostics.Count));
    }

    [TestMethod]
    public async Task AnalyzeWhenClassIsNotStereotype()
    {
      await IsBusinessObjectSerializableAnalyzerTests.RunAnalysisAsync(
        $@"Targets\{nameof(IsBusinessObjectSerializableAnalyzerTests)}\{(nameof(this.AnalyzeWhenClassIsNotStereotype))}.cs",
        0);
    }

    [TestMethod]
    public async Task AnalyzeWhenClassIsStereotypeAndIsSerializable()
    {
      await IsBusinessObjectSerializableAnalyzerTests.RunAnalysisAsync(
        $@"Targets\{nameof(IsBusinessObjectSerializableAnalyzerTests)}\{(nameof(this.AnalyzeWhenClassIsStereotypeAndIsSerializable))}.cs",
        0);
    }

    [TestMethod]
    public async Task AnalyzeWhenClassIsStereotypeAndIsNotSerializable()
    {
      await IsBusinessObjectSerializableAnalyzerTests.RunAnalysisAsync(
        $@"Targets\{nameof(IsBusinessObjectSerializableAnalyzerTests)}\{(nameof(this.AnalyzeWhenClassIsStereotypeAndIsNotSerializable))}.cs",
        1);
    }
  }
}

using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Hypercube.Utilities.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class DependencySuppressor : DiagnosticSuppressor
{
    private static readonly LocalizableString Justification = "[Dependency] fields are initialized by container";

    private static readonly SuppressionDescriptor RuleCs8618 = new(
        "HUS0001",
        suppressedDiagnosticId: "CS8618",
        justification: Justification
    );
    
    private static readonly SuppressionDescriptor RuleCs0649 = new(
        "HUS0002",
        suppressedDiagnosticId: "CS0649",
        justification: Justification
    );

    public override ImmutableArray<SuppressionDescriptor> SupportedSuppressions => [RuleCs8618, RuleCs0649];

    public override void ReportSuppressions(SuppressionAnalysisContext context)
    {
        foreach (var diagnostic in context.ReportedDiagnostics)
        {
            var root = diagnostic.Location.SourceTree?.GetRoot(context.CancellationToken);
            var node = root?.FindNode(diagnostic.Location.SourceSpan);
            
            if (node is not VariableDeclaratorSyntax variableDeclarator)
                continue;
            
            if (variableDeclarator.Parent?.Parent is not FieldDeclarationSyntax fieldDeclaration)
                continue;
            
            if (!HasDependencyAttribute(fieldDeclaration))
                continue;
                
            switch (diagnostic.Id)
            {
                case "CS8618":
                    context.ReportSuppression(Suppression.Create(RuleCs8618, diagnostic));
                    break;
                
                case "CS0649":
                    context.ReportSuppression(Suppression.Create(RuleCs0649, diagnostic));
                    break;
            }
        }
    }
    
    private static bool HasDependencyAttribute(FieldDeclarationSyntax fieldDeclaration)
    {
        return fieldDeclaration.AttributeLists
            .SelectMany(list => list.Attributes)
            .Any(attribute => attribute.Name.ToString() is "DependencyAttribute" or "Dependency");
    }
}

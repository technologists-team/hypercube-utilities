using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Hypercube.Utilities.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class DependencyAnalyzer : DiagnosticAnalyzer
{
    private const string Id = "HUA0001";
    private static readonly LocalizableString Title = "Assignment in the [Dependency] field is prohibited";
    private static readonly LocalizableString MessageFormat = "The field {0} with the [Dependency] attribute cannot be assigned a value";
    private static readonly LocalizableString Description = "Fields marked with the Dependency attribute are not allowed to be assigned values in any way.";
    private const string Category = "Usage";

    private static readonly DiagnosticDescriptor Rule = new(
        Id,
        Title,
        MessageFormat,
        Category,
        DiagnosticSeverity.Error,
        isEnabledByDefault: true,
        description: Description);

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => [Rule];

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        
        context.RegisterSyntaxNodeAction(AnalyzeAssignmentExpression, SyntaxKind.SimpleAssignmentExpression,
                                                             SyntaxKind.AddAssignmentExpression,
                                                             SyntaxKind.SubtractAssignmentExpression,
                                                             SyntaxKind.MultiplyAssignmentExpression,
                                                             SyntaxKind.DivideAssignmentExpression,
                                                             SyntaxKind.ModuloAssignmentExpression,
                                                             SyntaxKind.AndAssignmentExpression,
                                                             SyntaxKind.OrAssignmentExpression,
                                                             SyntaxKind.ExclusiveOrAssignmentExpression,
                                                             SyntaxKind.LeftShiftAssignmentExpression,
                                                             SyntaxKind.RightShiftAssignmentExpression);
        context.RegisterSyntaxNodeAction(AnalyzePrefixUnary,SyntaxKind.PreIncrementExpression, SyntaxKind.PreDecrementExpression);
        context.RegisterSyntaxNodeAction(AnalyzePostfixUnary, SyntaxKind.PostIncrementExpression, SyntaxKind.PostDecrementExpression);
        context.RegisterSyntaxNodeAction(AnalyzeVariableDeclarator, SyntaxKind.VariableDeclarator);
        context.RegisterSyntaxNodeAction(AnalyzeArgument, SyntaxKind.Argument);
    }

    private static void AnalyzeAssignmentExpression(SyntaxNodeAnalysisContext context)
    {
        var assign = (AssignmentExpressionSyntax) context.Node;
        var left = assign.Left;

        if (ModelExtensions.GetSymbolInfo(context.SemanticModel, left, context.CancellationToken).Symbol is not IFieldSymbol symbol)
            return;

        if (!IsDependencyField(symbol))
            return;

        context.ReportDiagnostic(Diagnostic.Create(Rule, left.GetLocation(), symbol.Name));
    }

    private static void AnalyzePrefixUnary(SyntaxNodeAnalysisContext context)
    {
        var unary = (PrefixUnaryExpressionSyntax) context.Node;
        var operand = unary.Operand;
        
        if (ModelExtensions.GetSymbolInfo(context.SemanticModel, operand, context.CancellationToken).Symbol is not IFieldSymbol symbol)
            return;

        if (!IsDependencyField(symbol))
            return;
        
        context.ReportDiagnostic(Diagnostic.Create(Rule, operand.GetLocation(), symbol.Name));
    }

    private static void AnalyzePostfixUnary(SyntaxNodeAnalysisContext context)
    {
        var unary = (PostfixUnaryExpressionSyntax) context.Node;
        var operand = unary.Operand;
  
        if (ModelExtensions.GetSymbolInfo(context.SemanticModel, operand, context.CancellationToken).Symbol is not IFieldSymbol symbol)
            return;
        
        if (!IsDependencyField(symbol))
            return;
        
        context.ReportDiagnostic(Diagnostic.Create(Rule, operand.GetLocation(), symbol.Name));
    }

    private static void AnalyzeVariableDeclarator(SyntaxNodeAnalysisContext context)
    {
        var variable = (VariableDeclaratorSyntax) context.Node;
        if (variable.Initializer is null)
            return;
        
        if (variable.Parent is not VariableDeclarationSyntax variableDecl)
            return;

        if (variableDecl.Parent is not FieldDeclarationSyntax)
            return;

        if (ModelExtensions.GetDeclaredSymbol(context.SemanticModel, variable, context.CancellationToken) is not IFieldSymbol symbol)
            return;

        if (!HasDependencyAttribute(symbol))
            return;
        
        context.ReportDiagnostic(Diagnostic.Create(Rule, variable.Initializer.Value.GetLocation(), symbol.Name));
    }

    private static void AnalyzeArgument(SyntaxNodeAnalysisContext context)
    {
        var arg = (ArgumentSyntax) context.Node;
        
        var refKind = arg.RefKindKeyword.Kind();
        if (refKind != SyntaxKind.RefKeyword && refKind != SyntaxKind.OutKeyword && refKind != SyntaxKind.InKeyword)
            return;

        var expr = arg.Expression;
        if (ModelExtensions.GetSymbolInfo(context.SemanticModel, expr, context.CancellationToken).Symbol is not IFieldSymbol symbol)
            return;
        
        if (!HasDependencyAttribute(symbol))
            return;
        
        context.ReportDiagnostic(Diagnostic.Create(Rule, expr.GetLocation(), symbol.Name));
    }

    private static bool IsDependencyField(ISymbol? symbol)
    {
        if (symbol is IFieldSymbol field)
            return HasDependencyAttribute(field);

        return false;
    }

    private static bool HasDependencyAttribute(IFieldSymbol field)
    {
        foreach (var attr in field.GetAttributes())
        {
            var @class = attr.AttributeClass;
            if (@class is null)
                continue;
            
            if (@class.Name is "Dependency" or "DependencyAttribute")
                return true;
        }

        return false;
    }
}
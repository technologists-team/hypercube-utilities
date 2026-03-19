using System.Collections.Immutable;
using System.Composition;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;

namespace Hypercube.Utilities.Analyzers.CodeFix;

[Shared, ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(DependencyCodeFixProvider))]
public sealed class DependencyCodeFixProvider : CodeFixProvider
{
    private const string Title = "Remove assignment from [Dependency] field";

    public override ImmutableArray<string> FixableDiagnosticIds => ["HUA0001"];
    public override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

    public override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
        if (root is null)
            return;

        var diagnostic = context.Diagnostics[0];
        var diagnosticSpan = diagnostic.Location.SourceSpan;

        var node = root.FindNode(diagnosticSpan);

        context.RegisterCodeFix(
            Microsoft.CodeAnalysis.CodeActions.CodeAction.Create(
                title: Title,
                createChangedDocument: c => RemoveAssignmentAsync(context.Document, node, c),
                equivalenceKey: Title),
            diagnostic);
    }

    private static async Task<Document> RemoveAssignmentAsync(Document document, SyntaxNode node, CancellationToken cancellationToken)
    {
        var editor = await DocumentEditor.CreateAsync(document, cancellationToken).ConfigureAwait(false);

        switch (node)
        {
            case AssignmentExpressionSyntax assignment:
                editor.ReplaceNode(assignment, assignment.Left.WithoutTrivia());
                break;

            case VariableDeclaratorSyntax { Initializer: not null } declarator:
                editor.RemoveNode(declarator.Initializer, SyntaxRemoveOptions.KeepNoTrivia);
                break;

            case PrefixUnaryExpressionSyntax prefix:
                // ++field/--field => field
                editor.ReplaceNode(prefix, prefix.Operand.WithoutTrivia());
                break;

            case PostfixUnaryExpressionSyntax postfix:
                // field++/field-- => field
                editor.ReplaceNode(postfix, postfix.Operand.WithoutTrivia());
                break;

            case ArgumentSyntax argument:
                // ref/out/in field => field
                var cleanArg = argument.WithRefKindKeyword(default);
                editor.ReplaceNode(argument, cleanArg);
                break;
        }

        return editor.GetChangedDocument();
    }
}

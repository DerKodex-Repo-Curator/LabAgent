using Microsoft.Agents.AI;
using Microsoft.Agents.AI.Workflows;

namespace LabResultsAgent;

/// <summary>
/// Workflow executor that uses the checker agent to validate
/// whether the incoming text is a valid lab-results question.
/// </summary>
internal sealed partial class QuestionValidationExecutor(AIAgent checkerAgent)
    : Executor("QuestionValidationExecutor")
{
    private readonly AIAgent _checkerAgent = checkerAgent;

    [MessageHandler]
    private async ValueTask<QuestionValidationResult> HandleAsync(string message, IWorkflowContext context)
    {
        var response = await _checkerAgent.RunAsync(message);
        var verdict = (response.Text ?? string.Empty).Trim().ToUpperInvariant();
        var isValid = verdict.StartsWith("VALID", StringComparison.Ordinal);

        return new QuestionValidationResult
        {
            UserInput = message,
            IsValid = isValid,
            Reason = verdict
        };
    }
}


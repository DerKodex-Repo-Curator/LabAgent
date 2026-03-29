using Microsoft.Agents.AI.Workflows;

namespace LabResultsAgent;

/// <summary>
/// Workflow executor that handles invalid questions by returning
/// a friendly guidance message.
/// </summary>
internal sealed partial class InvalidQuestionExecutor()
    : Executor("InvalidQuestionExecutor")
{
    [MessageHandler]
    private ValueTask<string> HandleAsync(QuestionValidationResult message, IWorkflowContext context)
    {
        const string text =
            "Please ask a clear question about understanding your lab test results (what the tests or numbers mean). " +
            "I cannot answer unrelated questions, give diagnoses, or suggest treatments. " +
            "Always discuss your results with your doctor or healthcare provider.";

        return ValueTask.FromResult(text);
    }
}


using Microsoft.Agents.AI;
using Microsoft.Agents.AI.Workflows;

namespace LabResultsAgent;

/// <summary>
/// Workflow executor that calls the explainer agent for valid questions.
/// </summary>
internal sealed partial class ExplainLabResultsExecutor(AIAgent explainerAgent)
    : Executor("ExplainLabResultsExecutor")
{
    private readonly AIAgent _explainerAgent = explainerAgent;

    [MessageHandler]
    private async ValueTask<string> HandleAsync(QuestionValidationResult message, IWorkflowContext context)
    {
        var response = await _explainerAgent.RunAsync(message.UserInput);
        return response.Text;
    }
}


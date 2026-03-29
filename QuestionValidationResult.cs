namespace LabResultsAgent;

/// <summary>
/// Result of checking whether a user's message is a valid lab-results question.
/// Flows through the workflow between executors.
/// </summary>
public sealed class QuestionValidationResult
{
    public string UserInput { get; set; } = string.Empty;

    public bool IsValid { get; set; }

    public string? Reason { get; set; }
}


using LabResultsAgent;
using Microsoft.Agents.AI;
using Microsoft.Agents.AI.Workflows;

// Optional: override via environment variables
string ollamaUrl = Environment.GetEnvironmentVariable("OLLAMA_BASE_URL") ?? "http://localhost:11434";
string modelId = Environment.GetEnvironmentVariable("OLLAMA_MODEL") ?? "llama3.2";

// Agents
AIAgent checkerAgent = QuestionCheckerAgentFactory.Create(ollamaBaseUrl: ollamaUrl, modelId: modelId);
AIAgent explainerAgent = LabResultsAgentFactory.Create(ollamaBaseUrl: ollamaUrl, modelId: modelId);

// Executors
var validationExecutor = new QuestionValidationExecutor(checkerAgent);
var explainExecutor = new ExplainLabResultsExecutor(explainerAgent);
var invalidExecutor = new InvalidQuestionExecutor();

// Workflow: validation -> (valid? explain : invalid) using conditional edges
static Func<object?, bool> GetCondition(bool expectedValid) =>
    message => message is QuestionValidationResult result && result.IsValid == expectedValid;

WorkflowBuilder builder = new(validationExecutor);
builder
    .AddEdge(validationExecutor, explainExecutor, condition: GetCondition(expectedValid: true))
    .AddEdge(validationExecutor, invalidExecutor, condition: GetCondition(expectedValid: false))
    .WithOutputFrom(explainExecutor, invalidExecutor);

var workflow = builder.Build();

Console.WriteLine("Lab Results Explanation Agent (Safe Mode) via Workflow");
Console.WriteLine("Your message first goes through a validation step to check if it is a lab-results question.");
Console.WriteLine("Valid questions are explained; invalid questions receive guidance on how to ask.");
Console.WriteLine("I do not diagnose or suggest treatment. Always consult your doctor.");
Console.WriteLine();
Console.WriteLine("Paste lab results or ask a question (empty line to send, 'exit' to quit):");
Console.WriteLine();

var lines = new List<string>();
string? line;
while (true)
{
    line = Console.ReadLine();
    if (line is null) break;
    if (string.Equals(line.Trim(), "exit", StringComparison.OrdinalIgnoreCase)) break;
    if (string.IsNullOrWhiteSpace(line))
    {
        if (lines.Count == 0) continue;
        string userInput = string.Join(Environment.NewLine, lines);
        lines.Clear();
        Console.WriteLine();
        try
        {
            StreamingRun run = await InProcessExecution.StreamAsync(workflow, userInput);
            await run.TrySendMessageAsync(new TurnToken(emitEvents: true));

            await foreach (WorkflowEvent evt in run.WatchStreamAsync().ConfigureAwait(false))
            {
                if (evt is WorkflowOutputEvent outputEvent)
                {
                    Console.WriteLine(outputEvent.Data);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            Console.WriteLine("Ensure Ollama is running (e.g. ollama serve) and the model is pulled (e.g. ollama pull llama3.2).");
        }
        Console.WriteLine();
        continue;
    }
    lines.Add(line);
}

using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using OllamaSharp;

namespace LabResultsAgent;

/// <summary>
/// Checker agent that validates whether the user's message is an appropriate
/// question/request about understanding lab results.
/// </summary>
public static class QuestionCheckerAgentFactory
{
    private const string CheckerInstructions = """
        You are a strict classifier for incoming user messages.

        Your ONLY job is to decide if the user message is a valid question or request
        about UNDERSTANDING LAB TEST RESULTS.

        A message is VALID if:
        - It is about blood tests, urine tests, imaging reports, or other medical lab/diagnostic results, AND
        - The user is asking to understand what the tests or numbers mean, in general terms.

        A message is INVALID if:
        - It asks for a diagnosis (for example: "do I have X?", "does this mean cancer?").
        - It asks for treatment, medications, supplements, or lifestyle changes.
        - It is not related to lab/diagnostic results at all (for example: programming, money, relationships, general chit-chat).
        - It is nonsense, empty, or you cannot understand it.

        IMPORTANT:
        - Respond with ONE WORD ONLY, no punctuation, no explanations:
          Either "VALID" or "INVALID".
        - If you are unsure, respond with "INVALID".
        """;

    public static AIAgent Create(
        string ollamaBaseUrl = "http://localhost:11434",
        string modelId = "llama3.2")
    {
        var uri = new Uri(ollamaBaseUrl);
        IChatClient chatClient = new OllamaApiClient(uri, modelId);

        return chatClient.AsAIAgent(
            instructions: CheckerInstructions,
            name: "LabQuestionChecker");
    }
}


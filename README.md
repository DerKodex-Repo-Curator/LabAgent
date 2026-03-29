# Lab Results Explanation Agent (Safe Mode)

A .NET 8 agent that explains lab results in simple language using the **Microsoft Agent Framework** and **Ollama** as the LLM. It runs in **Safe Mode**: no diagnosis, no treatment suggestions, and it always advises consulting a doctor.

## Rules (enforced in agent instructions)

- **No diagnosis** – Does not interpret results as indicating any disease or condition.
- **No treatment suggestions** – Does not recommend medications, supplements, or lifestyle changes.
- **Always advises doctor consultation** – Every response ends with a reminder to see a doctor for personal medical advice.

## Prerequisites

1. **.NET 8 SDK** – [Download](https://dotnet.microsoft.com/download/dotnet/8.0)
2. **Ollama** – [Install Ollama](https://ollama.com), then run:
   ```bash
   ollama serve
   ollama pull llama3.2
   ```
   You can use another model (e.g. `mistral`, `phi3`) by setting `OLLAMA_MODEL`.

## Build and run

```bash
cd LabResultsAgent
dotnet restore
dotnet run
```

## Usage

1. Run the app; it will connect to Ollama at `http://localhost:11434` by default.
2. Paste lab results (e.g. CBC, metabolic panel text) or ask a question in plain language.
3. Press Enter on an empty line to send.
4. Type `exit` and Enter to quit.

### Environment variables (optional)

| Variable           | Default               | Description              |
|--------------------|-----------------------|--------------------------|
| `OLLAMA_BASE_URL`  | `http://localhost:11434` | Ollama server URL     |
| `OLLAMA_MODEL`     | `llama3.2`            | Ollama model name        |

Example:

```bash
set OLLAMA_MODEL=mistral
dotnet run
```

## Tech stack

- **C# / .NET 8**
- **Microsoft Agent Framework** (`Microsoft.Agents.AI`) – agent abstraction and `AsAIAgent`
- **OllamaSharp** – Ollama API client implementing `Microsoft.Extensions.AI.IChatClient` for use with the agent

## Disclaimer

This agent is for **educational use only**. It explains lab tests in simple language and does not provide medical diagnosis or treatment. Users must consult a doctor or healthcare provider for interpretation of their own results and any medical decisions.

namespace Kwaliteitsprotocol_7B.Services;

#pragma warning disable CA1822
public sealed class ScenarioService
{
    public async Task<IReadOnlyDictionary<int, bool>> GetScenarioCompletionStates()
    {
        var states = await Task.WhenAll(Enumerable.Range(1, 4).Select(IsScenarioCompleted));
        return states.Select((c, i) => (Completed: c, Index: i)).ToDictionary(e => e.Index + 1, e => e.Completed);
    }

    public async Task MarkScenario(int scenario, bool completed)
    {
        if (completed)
        {
            await SecureStorage.Default.SetAsync($"app_scenario_{scenario}", "completed");
        }
        else
        {
            SecureStorage.Default.Remove($"app_scenario_{scenario}");
        }
    }

    public async Task<bool> IsScenarioCompleted(int scenario)
        => await SecureStorage.Default.GetAsync($"app_scenario_{scenario}") is { Length: > 0 };
}

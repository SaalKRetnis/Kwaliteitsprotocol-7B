namespace Kwaliteitsprotocol_7B.Services;

public sealed class ScenarioService
{
    public async Task<bool> AreAllScenariosCompleted()
    {
        var states = await Task.WhenAll(
        [
            IsScenarioCompleted(1),
            IsScenarioCompleted(2),
            IsScenarioCompleted(3),
            IsScenarioCompleted(4),
        ]);

        return states.Any(state => state);
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

namespace Kwaliteitsprotocol_7B.Services;

public sealed partial class AudioService(DialogService dialog)
{
    public partial double GetMediaVolume();

    public Task Speak(string id, string text, int duration) => Task.WhenAll(
        dialog.Toast(text, duration),
        Play(id),
        Task.Delay(duration));

    public partial Task<IDisposable> Play(string id, double volume = 1, bool loop = false);

    public async Task PlayOnce(string id, double volume = 1, bool loop = false)
    {
        if (await SecureStorage.Default.GetAsync($"app_played_{id}") is not { Length: > 0 })
        {
            await SecureStorage.Default.SetAsync($"app_played_{id}", "true");
            await Play(id, volume, loop);
        }
    }
}

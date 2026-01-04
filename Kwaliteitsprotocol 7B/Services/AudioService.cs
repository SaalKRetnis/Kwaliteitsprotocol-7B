namespace Kwaliteitsprotocol_7B.Services;

public sealed partial class AudioService(DialogService dialog)
{
    public Task Speak(string id, string text, int duration) => Task.WhenAll(
        dialog.Toast(text, duration),
        Play(id),
        Task.Delay(duration));

    public partial Task Play(string id, double volume = 1, bool loop = false);
}

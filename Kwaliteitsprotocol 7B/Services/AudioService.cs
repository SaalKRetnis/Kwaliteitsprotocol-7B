namespace Kwaliteitsprotocol_7B.Services;

public sealed partial class AudioService
{
    public async Task Speak(string id, string text, int duration)
    {

        await Task.Delay(duration);
    }

    public partial Task Play(string id, double volume = 1, bool loop = false);
}

using System.Collections.Concurrent;
using Windows.Media.Core;
using Windows.Media.Playback;

namespace Kwaliteitsprotocol_7B.Services;

public sealed partial class AudioService
{
    readonly ConcurrentDictionary<string, MediaPlayer> Players = new(StringComparer.OrdinalIgnoreCase);

    public async partial Task Play(string id, double volume, bool loop)
    {
        var stream = await FileSystem.OpenAppPackageFileAsync($"audio/{id}.mp3");
        var player = Players.GetOrAdd(id, _ => new()
        {
            IsLoopingEnabled = loop,
            Source = MediaSource.CreateFromStream(stream.AsRandomAccessStream(), "audio/mpeg")
        });

        player.Position = TimeSpan.Zero;
        player.Volume = volume;
        player.Play();
    }
}

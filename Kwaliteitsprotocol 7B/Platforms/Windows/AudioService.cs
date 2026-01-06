using NAudio.CoreAudioApi;
using System.Collections.Concurrent;
using Windows.Media.Core;
using Windows.Media.Playback;

namespace Kwaliteitsprotocol_7B.Services;

#pragma warning disable CA1822
public sealed partial class AudioService
{
    readonly ConcurrentDictionary<string, MediaPlayer> Players = new(StringComparer.OrdinalIgnoreCase);

    public partial double GetMediaVolume()
    {
        using var deviceEnumerator = new MMDeviceEnumerator();
        var device = deviceEnumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
        return device.AudioEndpointVolume.MasterVolumeLevelScalar;
    }

    public async partial Task<IDisposable> Play(string id, double volume, bool loop)
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

        return new Disposable(() => Stop(id));
    }

    void Stop(string id)
    {
        if (Players.TryRemove(id, out var player))
        {
            player.Pause();
            player.Dispose();
        }
    }
}

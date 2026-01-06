using Android.Content;
using Android.Media;
using System.Collections.Concurrent;
using Application = Android.App.Application;
using Stream = Android.Media.Stream;

namespace Kwaliteitsprotocol_7B.Services;

#pragma warning disable CA1822
public sealed partial class AudioService
{
    readonly ConcurrentDictionary<string, MediaPlayer> Players = new(StringComparer.OrdinalIgnoreCase);
    readonly ConcurrentDictionary<MediaPlayer, double> Volumes = [];

    public partial double GetMediaVolume()
    {
        var audioManager = (AudioManager)Application.Context.GetSystemService(Context.AudioService)!;
        var currentVolume = audioManager.GetStreamVolume(Stream.Music);
        var maxVolume = audioManager.GetStreamMaxVolume(Stream.Music);
        return currentVolume / (maxVolume * 1d);
    }

    public async partial Task<IDisposable> Play(string id, double volume, bool loop)
    {
        try
        {
            var player = Players.GetOrAdd(id, _ =>
            {
                var player = new MediaPlayer { Looping = loop };

                var context = Platform.CurrentActivity;
                var fd = context!.Assets!.OpenFd($"audio/{id}.mp3");
                player.SetDataSource(fd.FileDescriptor, fd.StartOffset, fd.Length);
                player.Prepare();
                return player;
            });

            player.SetVolume((float)volume, (float)volume);
            Volumes[player] = volume;
            player.SeekTo(0);
            player.Start();
        }
        catch { }

        return Task.FromResult<IDisposable>(new Disposable(() => Stop(id)));
    }

    void Stop(string id)
    {
        if (Players.TryRemove(id, out var player))
        {
            player.Stop();
            player.Release();
            Volumes.TryRemove(player, out _);
        }
    }
}

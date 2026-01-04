using Android.Media;
using System.Collections.Concurrent;

namespace Kwaliteitsprotocol_7B.Services;

public sealed partial class AudioService
{
    readonly ConcurrentDictionary<string, MediaPlayer> Players = new(StringComparer.OrdinalIgnoreCase);

    public async partial Task Play(string id, double volume, bool loop)
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
            player.SeekTo(0);
            player.Start();
        }
        catch { }
    }
}

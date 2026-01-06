namespace Kwaliteitsprotocol_7B;

sealed class Disposable(Action dispose) : IDisposable
{
    public static Disposable Empty { get; } = new(() => { });
    public void Dispose() => dispose();
}

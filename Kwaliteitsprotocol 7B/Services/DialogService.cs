using Microsoft.JSInterop;

namespace Kwaliteitsprotocol_7B.Services;

public sealed class DialogService(IJSRuntime js)
{
    public async Task Toast(string text, int duration = 5000) => await js.InvokeVoidAsync("dialog.toast", text, duration);
}

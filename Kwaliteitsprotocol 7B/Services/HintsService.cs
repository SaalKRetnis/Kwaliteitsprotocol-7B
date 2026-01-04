using Kwaliteitsprotocol_7B.Models;

namespace Kwaliteitsprotocol_7B.Services;

#pragma warning disable CA1822
public sealed class HintsService
{
    const string GeneratedHintKey = "app_generated_hint";

    readonly Hint[] Hints =
    [
        new("Tseard is ooit eens voor een huisfeest van Ellen, in Enschede, via Utrecht gereden. Hij kwam dan ook wat later dan verwacht aan...", "an1-speech", "img/an1.png", "Er zit <strong>geen 5</strong> in de cijfercombinatie."),
        new("Tseard heeft ooit op een zonnige middag een gin-tonic besteld. Hij drinkt geen alcohol...", "an2-speech", "img/an2.png", "Het tweede cijfer is een <strong>1</strong>."),
        new("Koudummer Rummikub is raar en volstrekt onnodig.", "an3-speech", "img/an3.png", "Alle cijfers zijn <strong>numeriek</strong>."),
        new("Tseard heeft onbedoeld bij ons het Koudummer kwartiertje geïntroduceerd.", "an4-speech", "img/an4.png", "Het eerste cijfer is <strong>vandaag</strong>."),
        new("Wie kan dit moment nou vergeten...", "an5-speech", "img/an5.png", "De laatste twee cijfers slaan op het <strong>jaartal</strong>."),
        new("In Engeland vergat Tseard even dat ze daar links rijden...", "an6-speech", "img/an6.png", "Alleen het derde cijfer is een <strong>priemgetal</strong>."),
    ];

    public async Task<Hint> GenerateRandomHint()
    {
        await SecureStorage.Default.SetAsync(GeneratedHintKey, "true");
        return Hints[Random.Shared.Next(Hints.Length)];
    }

    public async Task<bool> HasGeneratedHint()
        => await SecureStorage.Default.GetAsync(GeneratedHintKey) is { Length: > 0 };
}

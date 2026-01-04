namespace Kwaliteitsprotocol_7B.Models;

public sealed record Question(string Text, Answer[] Answers, int ResultValue);

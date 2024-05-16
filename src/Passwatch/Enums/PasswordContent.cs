namespace Passwatch.Enums;

[Flags]
public enum PasswordContent
{
    None = 0,
    Lowercase = 1,
    Uppercase = 2,
    Digit = 4,
    Punctuation = 8
}

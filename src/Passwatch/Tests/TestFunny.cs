using Easy_Password_Validator.Interfaces;
using Passwatch.Enums;
using System.Runtime.CompilerServices;

namespace Passwatch.Tests;
public class TestFunny : IPasswordTest
{
    private readonly IEnumerable<string> _words;
    private readonly IEnumerable<string> _replies;
    private readonly BadWordsCheckType _checkType;

    /// <summary>
    /// Reads a file containing bad passwords and loads them into the badlist
    /// </summary>
    /// <param name="passwordRequirements">Object containing current settings</param>
    /// <param name="fileName">The full filename containing the bad password list to use</param>
    /// <exception cref="ArgumentException"></exception>
    public TestFunny(IPasswordRequirements passwordRequirements, string fileName, string fileNameReply, BadWordsCheckType checkType = BadWordsCheckType.StartsWith)
    {
        Settings = passwordRequirements;

        if (File.Exists(fileName) == false)
            throw new ArgumentException("Specified file does not exist", nameof(fileName));

        if (File.Exists(fileName) == false)
            throw new ArgumentException("Specified file does not exist", nameof(fileNameReply));

        //TODO Read as CSVs and parse
        _words = File.ReadAllLines(fileName, System.Text.Encoding.UTF8);
        _replies = File.ReadAllLines(fileNameReply, System.Text.Encoding.UTF8);
        _checkType = checkType;
    }



    /// <inheritdoc/>
    public int ScoreModifier { get; set; }

    /// <inheritdoc/>
    public string FailureMessage { get; set; } = null!;

    /// <inheritdoc/>
    public IPasswordRequirements Settings { get; set; }

    public bool TestAndScore(string password)
    {
        // Reset
        FailureMessage = null;
        ScoreModifier = 0;

        // Check for match
        var match = _words.Any(x => CheckContains(password, x));

        // Adjust score
        ScoreModifier = match ? -50 : 0;

        // Return result
        var pass = match == false;

        if (pass == false)
            FailureMessage = GetFailureMessage();

        return pass;
    }

    private string GetFailureMessage()
    {
        return _replies.ElementAt(new Random().Next(_replies.Count()));
    }

    private bool CheckContains(string password, string word)
    {
        return _checkType switch
        {
            BadWordsCheckType.StartsWith => password.StartsWith(word, StringComparison.OrdinalIgnoreCase),
            BadWordsCheckType.Contains => password.Contains(word, StringComparison.OrdinalIgnoreCase),
            BadWordsCheckType.Equals => password.Equals(word, StringComparison.OrdinalIgnoreCase),
            _ => false,
        };
    }

}

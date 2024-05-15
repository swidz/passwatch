
using Easy_Password_Validator.Models;
using Easy_Password_Validator;
using System.Security;
using Passwatch.Tests;

while (true)
{
    var password = GetPasswordAsString("Podaj hasło:");

    Console.WriteLine();
    //Console.WriteLine($"Twoje hasło to: {password}");

    var result = CheckPassword(password);
    Console.ReadKey();
    Console.Clear();
}

static bool CheckPassword(string password)
{

    var requirements = new PasswordRequirements()
    {
        MinLength = 8,
        ExitOnFailure = false,
        RequireDigit = true,
        MinScore = 50,
        UseEntropy = true,
        UseDigit = true,
        UseLowercase = true,
        KeyboardStyle = Easy_Password_Validator.Enums.PatternMapTypes.Qwerty,
        UseUppercase = true,
        UseUnique = true,
        MinUniqueCharacters = 6,
        UseRepeat = true,
        MaxRepeatSameCharacter = 3, 
        MaxNeighboringCharacter = 2,
        RequireLowercase = true,
        RequireUppercase = true,
        RequirePunctuation = true,
        UseBadList = true,
        UseLength = true,
        UsePunctuation = true,
        UsePattern = true
    }; 
    
    var passwordValidator = new PasswordValidatorService(requirements);

    
    passwordValidator.AddTest(new TestFunny(requirements, "data\\bluzgi.csv", "data\\reply-bluzgi.csv"));
    passwordValidator.AddTest(new TestFunny(requirements, "data\\brawlstars.csv", "data\\reply-brawlstars.csv"));
    passwordValidator.AddTest(new TestFunny(requirements, "data\\common.csv", "data\\reply-common.csv"));
    passwordValidator.AddTest(new TestFunny(requirements, "data\\games.csv", "data\\reply-games.csv"));
    passwordValidator.AddTest(new TestFunny(requirements, "data\\names.csv", "data\\reply-names.csv"));
    passwordValidator.AddTest(new TestFunny(requirements, "data\\nick.csv", "data\\reply-nick.csv"));
    passwordValidator.AddTest(new TestFunny(requirements, "data\\numbers.csv", "data\\reply-numbers.csv"));
    passwordValidator.AddTest(new TestFunny(requirements, "data\\petts.csv", "data\\reply-petts.csv"));
    passwordValidator.AddTest(new TestFunny(requirements, "data\\pokemon.csv", "data\\reply-pokemon.csv"));

    var pass = passwordValidator.TestAndScore(password, languageCode: "pl");


    if (pass)
        Console.WriteLine($"Silne hasło z wynikiem: {passwordValidator.Score}");
    else
        Console.WriteLine($"Słabe hasło z wynikiem: {passwordValidator.Score}");


    if (pass == false)
    {
        foreach (var message in passwordValidator.FailureMessages)
            Console.WriteLine(message);
    }
    return pass;
}

static SecureString GetPasswordAsSecureString(String displayMessage)
{
    SecureString pass = new SecureString();
    Console.Write(displayMessage);
    ConsoleKeyInfo key;

    do
    {
        key = Console.ReadKey(true);

        // Backspace Should Not Work
        if (!char.IsControl(key.KeyChar))
        {
            pass.AppendChar(key.KeyChar);
            Console.Write("*");
        }
        else
        {
            if (key.Key == ConsoleKey.Backspace && pass.Length > 0)
            {
                pass.RemoveAt(pass.Length - 1);
                Console.Write("\b \b");
            }
        }
    }
    // Stops Receving Keys Once Enter is Pressed
    while (key.Key != ConsoleKey.Enter);
    return pass;
}

static string GetPasswordAsString(String displayMessage)
{
    string pass = String.Empty;
    Console.Write(displayMessage);
    ConsoleKeyInfo key;

    do
    {
        key = Console.ReadKey(true);

        // Backspace Should Not Work
        if (!char.IsControl(key.KeyChar))
        {
            pass += key.KeyChar;
            Console.Write("*");
        }
        else
        {
            if (key.Key == ConsoleKey.Backspace && pass.Length > 0)
            {
                pass = pass[0..^1];
                Console.Write("\b \b");
            }
        }
    }
    // Stops Receving Keys Once Enter is Pressed
    while (key.Key != ConsoleKey.Enter);
    return pass;
}
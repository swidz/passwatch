﻿
using Easy_Password_Validator;
using Easy_Password_Validator.Models;
using Passwatch.Tests;

while (true)
{
    var password1 = GetPasswordAsString("Podaj hasło:");
    Console.WriteLine();
    var password2 = GetPasswordAsString("Powtórz hasło:");
    Console.WriteLine();

    if (!password1.Equals(password2))
    {
        Console.WriteLine("Hasła nie są takie same. Spróbuj jeszcze raz...");
        Console.WriteLine();
        Console.ReadKey();
        Console.Clear();

        continue;
    }

    Console.WriteLine();

    var result = CheckPassword(password1);

    PrintPasswordFeatures(password1);

    Console.WriteLine("Naciśnij dowolny klawisz i spróbuj ponownie lub Ctrl-C aby zakończyć...");

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
        MaxRepeatSameCharacter = 2, 
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


    passwordValidator.PrependTest(new TestFunny(requirements, "data\\bluzgi.csv", "data\\reply-bluzgi.csv", Passwatch.Enums.BadWordsCheckType.Contains));
    passwordValidator.PrependTest(new TestFunny(requirements, "data\\brawlstars.csv", "data\\reply-brawlstars.csv"));
    passwordValidator.PrependTest(new TestFunny(requirements, "data\\common.csv", "data\\reply-common.csv", Passwatch.Enums.BadWordsCheckType.Equals));
    passwordValidator.PrependTest(new TestFunny(requirements, "data\\games.csv", "data\\reply-games.csv"));
    passwordValidator.PrependTest(new TestFunny(requirements, "data\\names.csv", "data\\reply-names.csv"));
    passwordValidator.PrependTest(new TestFunny(requirements, "data\\nick.csv", "data\\reply-nick.csv"));
    passwordValidator.PrependTest(new TestFunny(requirements, "data\\numbers.csv", "data\\reply-numbers.csv", Passwatch.Enums.BadWordsCheckType.Contains));
    passwordValidator.PrependTest(new TestFunny(requirements, "data\\petts.csv", "data\\reply-petts.csv"));
    passwordValidator.PrependTest(new TestFunny(requirements, "data\\pokemon.csv", "data\\reply-pokemon.csv"));

    var pass = passwordValidator.TestAndScore(password, languageCode: "pl");


    if (passwordValidator.Score < 100)
        Console.WriteLine($"Uważaj! Słabe hasło! Twój wynik to: {passwordValidator.Score}");
    else
        Console.WriteLine($"Twój wynik to: {passwordValidator.Score}");
   

    if (pass == false)
    {
        foreach (var message in passwordValidator.FailureMessages)
            Console.WriteLine(message);
    }
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
    while (key.Key != ConsoleKey.Enter || pass.Length == 0);
    return pass;
}

static void PrintPasswordFeatures(string password)
{
    Console.WriteLine();
    Console.WriteLine("===================================");
    Console.WriteLine($"Długość hasła: {password.Length}");
    Console.WriteLine($"Ilość małych liter: {password.ToCharArray().Where(c => Char.IsLetter(c) && Char.IsLower(c)).Count()}");
    Console.WriteLine($"Ilość dużych liter: {password.ToCharArray().Where(c => Char.IsLetter(c) && Char.IsUpper(c)).Count()}");
    Console.WriteLine($"Ilość cyfr: {password.ToCharArray().Where(c => Char.IsDigit(c)).Count()}");
    Console.WriteLine($"Ilość znaków specjalnych: {password.ToCharArray().Where(c => Char.IsPunctuation(c)).Count()}");
    Console.WriteLine($"Ilość białych znaków: {password.ToCharArray().Where(c => Char.IsWhiteSpace(c)).Count()}");
    Console.WriteLine($"Ilość słów: {password.Split(" \t", StringSplitOptions.RemoveEmptyEntries).Length}");
    Console.WriteLine("===================================");

    Console.WriteLine();
}
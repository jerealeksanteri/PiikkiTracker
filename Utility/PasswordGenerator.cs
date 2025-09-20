using System.Security.Cryptography;
using System.Text;

namespace PiikkiTracker.Utility
{
    public static class PasswordGenerator
    {
        private const string LowerCaseLetters = "abcdefghijklmnopqrstuvwxyz";
        private const string UpperCaseLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string Numbers = "0123456789";
        private const string SpecialCharacters = "!@#$%^&*";

        public static string GenerateStrongPassword(int length = 12)
        {
            if (length < 8)
                length = 8;

            var allCharacters = LowerCaseLetters + UpperCaseLetters + Numbers + SpecialCharacters;
            var password = new StringBuilder();

            // Ensure at least one character from each category
            password.Append(GetRandomCharacter(LowerCaseLetters));
            password.Append(GetRandomCharacter(UpperCaseLetters));
            password.Append(GetRandomCharacter(Numbers));
            password.Append(GetRandomCharacter(SpecialCharacters));

            // Fill the rest with random characters
            for (int i = 4; i < length; i++)
            {
                password.Append(GetRandomCharacter(allCharacters));
            }

            // Shuffle the password to avoid predictable patterns
            return ShuffleString(password.ToString());
        }

        private static char GetRandomCharacter(string characters)
        {
            using var rng = RandomNumberGenerator.Create();
            var bytes = new byte[4];
            rng.GetBytes(bytes);
            var randomValue = BitConverter.ToUInt32(bytes, 0);
            return characters[(int)(randomValue % characters.Length)];
        }

        private static string ShuffleString(string input)
        {
            var chars = input.ToCharArray();
            using var rng = RandomNumberGenerator.Create();

            for (int i = chars.Length - 1; i > 0; i--)
            {
                var bytes = new byte[4];
                rng.GetBytes(bytes);
                var randomIndex = (int)(BitConverter.ToUInt32(bytes, 0) % (i + 1));

                (chars[i], chars[randomIndex]) = (chars[randomIndex], chars[i]);
            }

            return new string(chars);
        }
    }
}
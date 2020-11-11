using System;
using System.Linq;
using System.Text;
using UserManagement.Domain.Services;
using UserManagement.Domain.ValueObjects;

namespace UserManagement.Application.Services
{
    public class PasswordGenerator : IPasswordGenerator
    {
        private static readonly Random _random = new Random();
        const string ALPHA = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string NUMERIC = "0123456789";

        public static string RandomString()
        {
            var alpha = new string(Enumerable.Range(1, 5).Select(_ => ALPHA[_random.Next(ALPHA.Length)]).ToArray());
            var numeric = new string(Enumerable.Range(1, 5).Select(_ => NUMERIC[_random.Next(NUMERIC.Length)]).ToArray());

            var value = new StringBuilder();
            while (alpha != string.Empty && numeric != string.Empty)
            {
                if (_random.Next() % 2 == 0)
                {
                    value.Append(alpha.First());
                    alpha = alpha.Remove(0, 1);
                }
                else
                {
                    value.Append(numeric.First());
                    numeric = numeric.Remove(0, 1);
                }
            }

            if (alpha != string.Empty)
            {
                value.Append(alpha);
            }

            if (numeric != string.Empty)
            {
                value.Append(numeric);
            }

            return value.ToString();
        }

        public Password Generate()
        {
            var value = RandomString();
            var password = new Password(value);
            return password;
        }
    }
}
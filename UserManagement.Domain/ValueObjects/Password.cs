using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace UserManagement.Domain.ValueObjects
{
    public class Password : ValueObject
    {
        // 8 characters. At least one letter and one digit
        private const string REGEX = "^(?=.*[A-Za-z])(?=.*\\d)[A-Za-z\\d]{8,}$";
        public string Value { get; private set; }

        public bool IsValid
        {
            get
            {
                bool isValid = Regex.IsMatch(Value, REGEX);
                return isValid;
            }
        }

        public Password(string value)
        {
            Value = value?.Trim() ?? string.Empty;
        }

        public override string ToString()
        {
            return Value;
        }

        public override IEnumerable<object?> GetAtomicValues()
        {
            yield return Value;
        }
    }
}
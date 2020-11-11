using System.Collections.Generic;

namespace UserManagement.Domain.ValueObjects
{
    public class PasswordHash : ValueObject
    {
        public string Value { get; private set; }
        public string Salt { get; private set; }

        public PasswordHash(string value, string salt)
        {
            Value = value;
            Salt = salt;
        }

        public override IEnumerable<object?> GetAtomicValues()
        {
            yield return Salt;
            yield return Value;
        }
    }
}
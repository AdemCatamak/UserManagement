using System.Collections.Generic;
using System.Linq;

namespace UserManagement.Domain.ValueObjects
{
    public abstract class ValueObject
    {
        public abstract IEnumerable<object?> GetAtomicValues();
        
        public override bool Equals(object? obj)
        {
            if (obj == null || obj.GetType() != GetType())
            {
                return false;
            }

            var other = (ValueObject) obj;
            using var thisValues = GetAtomicValues().GetEnumerator();
            using var otherValues = other.GetAtomicValues().GetEnumerator();
            while (thisValues.MoveNext() && otherValues.MoveNext())
            {
                if (ReferenceEquals(thisValues.Current, null) ^
                    ReferenceEquals(otherValues.Current, null))
                {
                    return false;
                }

                if (thisValues.Current != null &&
                    !thisValues.Current.Equals(otherValues.Current))
                {
                    return false;
                }
            }

            return !thisValues.MoveNext() && !otherValues.MoveNext();
        }

        public override int GetHashCode()
        {
            return GetAtomicValues()
                  .Select(x => x != null
                                   ? x.GetHashCode()
                                   : 0)
                  .Aggregate((x, y) => x ^ y);
        }


        // Other utility methods
    }
}
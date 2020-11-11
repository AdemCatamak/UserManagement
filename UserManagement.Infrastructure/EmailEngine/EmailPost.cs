using System;
using UserManagement.Domain.ValueObjects;

namespace UserManagement.Infrastructure.EmailEngine
{
    public class EmailPost
    {
        public Email From { get; }
        public Email To { get; }
        public string Subject { get; }
        public string Content { get; }

        public EmailPost(Email from, Email to, string subject, string content)
        {
            From = from;
            To = to;
            Subject = subject;
            Content = content;
        }

        public override string ToString()
        {
            var result =
                $"From : {From}{Environment.NewLine}" +
                $"To:{To}{Environment.NewLine}" +
                $"Subject : {Subject}{Environment.NewLine}" +
                $"{Content}";

            return result;
        }
    }
}
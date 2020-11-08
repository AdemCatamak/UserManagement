using System;
using MassTransit;
using Newtonsoft.Json;

namespace UserManagement.Infrastructure.DatabaseContext.Dtos
{
    public class OutboxMessage
    {
        public string Id { get; private set; }
        public OutboxMessageStatus OutboxMessageStatus { get; private set; }
        public string MessagePayload { get; private set; }
        public string? Description { get; private set; }
        public DateTime CreatedOn { get; private set; }

        private static readonly JsonSerializerSettings _jsonSerializerSettings = new JsonSerializerSettings
                                                                                 {
                                                                                     TypeNameHandling = TypeNameHandling.All,
                                                                                     TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Full
                                                                                 };

        public T GetPayload<T>()
        {
            return JsonConvert.DeserializeObject<T>(MessagePayload, _jsonSerializerSettings);
        }


        public OutboxMessage(object payload) : this(NewId.Next().ToString(),
                                                    OutboxMessageStatus.Waiting,
                                                    JsonConvert.SerializeObject(payload, _jsonSerializerSettings),
                                                    DateTime.UtcNow,
                                                    null)
        {
        }

        private OutboxMessage(string id, OutboxMessageStatus outboxMessageStatus, string messagePayload, DateTime createdOn, string? description)
        {
            Id = id;
            OutboxMessageStatus = outboxMessageStatus;
            MessagePayload = messagePayload;
            CreatedOn = createdOn;
            Description = description;
        }

        public void SetCompleted()
        {
            OutboxMessageStatus = OutboxMessageStatus.Completed;
        }

        public void SetFailed(string description)
        {
            OutboxMessageStatus = OutboxMessageStatus.Failed;
            Description = description;
        }
    }

    public enum OutboxMessageStatus
    {
        Waiting = 1,
        InProgress = 2,
        Completed = 3,
        Failed = 4
    }
}
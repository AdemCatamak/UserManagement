using System;
using Newtonsoft.Json;

namespace UserManagement.Infrastructure.IntegrationMessageBroker.MassTransitSection.MassTransitConverters
{
    internal class MassTransitTypeNameHandlingConverter : JsonConverter
    {
        private readonly TypeNameHandling _typeNameHandling;

        public MassTransitTypeNameHandlingConverter(TypeNameHandling typeNameHandling)
        {
            _typeNameHandling = typeNameHandling;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            // Set TypeNameHandling for serializing objects with $type
            new JsonSerializer {TypeNameHandling = _typeNameHandling}.Serialize(writer, value);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            // Set TypeNameHandling for deserializing objects with $type
            var obj = new JsonSerializer {TypeNameHandling = _typeNameHandling}.Deserialize(reader, objectType);
            return obj;
        }

        public override bool CanConvert(Type objectType)
        {
            return !IsMassTransitOrSystemType(objectType);
        }

        private static bool IsMassTransitOrSystemType(Type objectType)
        {
            return objectType.Assembly == typeof(MassTransit.IConsumer).Assembly ||
                   objectType.Assembly.IsDynamic ||
                   objectType.Assembly == typeof(object).Assembly;
        }
    }
}
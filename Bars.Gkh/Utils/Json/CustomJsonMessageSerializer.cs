namespace Bars.Gkh.Utils.Json
{
    using Newtonsoft.Json;
    using System;
    using System.IO;
    using System.Text;

    using Newtonsoft.Json.Serialization;

    using RabbitMQ.Client.Events;

    public class CustomJsonMessageSerializer
    {
        /// <summary>
		/// Включить логирование json в текст сообщения об ошибке при исключении типа <see cref="JsonSerializationException"/>
		/// </summary>
		public static bool EnableLoggingJsonOnError { get; set; } = false;
        private JsonSerializer Serializer { get; set; }

        public CustomJsonMessageSerializer()
        {
            Serializer = new JsonSerializer
            {
                TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple,
                Formatting = Formatting.None,
                CheckAdditionalContent = true,
                ContractResolver = new DefaultContractResolver(),
                ObjectCreationHandling = ObjectCreationHandling.Auto,
                DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate,
                TypeNameHandling = TypeNameHandling.All,
                ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                NullValueHandling = NullValueHandling.Ignore,
                FloatParseHandling = FloatParseHandling.Decimal
            };
        }

        public byte[] Serialize<T>(T obj)
        {
            if (obj == null)
            {
                return Encoding.UTF8.GetBytes(string.Empty);
            }
            string msgStr;
            using (var sw = new StringWriter())
            {
                Serializer.Serialize(sw, obj);
                msgStr = sw.GetStringBuilder().ToString();
            }
            var msgBytes = Encoding.UTF8.GetBytes(msgStr);
            return msgBytes;
        }

        public object Deserialize(BasicDeliverEventArgs args)
        {
            if (args.BasicProperties.Headers.TryGetValue("message_type", out var typeBytes))
            {
                var typeName = Encoding.UTF8.GetString(typeBytes as byte[] ?? new byte[0]);
                var type = Type.GetType(typeName, false);
                return Deserialize(args.Body.ToArray(), type);
            }
            else
            {
                var typeName = args.BasicProperties.Type;
                var type = Type.GetType(typeName, false);
                return Deserialize(args.Body.ToArray(), type);
            }
        }

        public T Deserialize<T>(byte[] bytes)
        {
            var obj = (T)Deserialize(bytes, typeof(T));
            return obj;
        }

        public object Deserialize(byte[] bytes, Type messageType)
        {
            object obj;
            var msgStr = Encoding.UTF8.GetString(bytes);
            using (var jsonReader = new JsonTextReader(new StringReader(msgStr)))
            {
                try
                {
                    obj = Serializer.Deserialize(jsonReader, messageType);
                }
                catch (JsonSerializationException ex)
                {
                    var message = ex.ToString();
                    if (EnableLoggingJsonOnError)
                    {
                        message = $"{message}{Environment.NewLine}JsonText:{msgStr}";
                    }

                    throw;
                }

                return obj;
            }
        }
    }
}

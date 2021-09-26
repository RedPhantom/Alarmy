using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace AlarmyLib
{
    // For ease of use, all Json Serialization and Deserialization should be done here.

    // Reference for serialization and deserialization in this class are taken from here:
    // https://stackoverflow.com/questions/38679972/determine-type-during-json-deserialize.

    /// <summary>
    /// Describes information sent from the server to the client.
    /// </summary>
    public class BaseMessage { }

    /// <summary>
    /// Describes information sent from the client to the server.
    /// </summary>
    public class BaseResponse
    {
        public Instance Instance { get; set; }

        public BaseResponse(Instance instance)
        {
            Instance = instance;
        }
    }

    public class MessageWrapper
    {
        public string MessageType { get; set; }
        public object Message { get; set; }

        public static MessageWrapperContent Deserialize(string data)
        {
            Type t;
            MessageWrapper wrapper = JsonConvert.DeserializeObject<MessageWrapper>(data);

            t = Type.GetType(wrapper.MessageType);

            return new MessageWrapperContent(JsonConvert.DeserializeObject(
                Convert.ToString(wrapper.Message), t), t);
        }
    }

    /// <summary>
    /// Holds an uncasted message and its type.
    /// </summary>
    public class MessageWrapperContent
    {
        public object Message { get; set; }
        public Type Type { get; set; }

        public MessageWrapperContent(object message, Type type)
        {
            Message = message;
            Type = type;
        }
    }

    /// <summary>
    /// Typed message wrapper.
    /// </summary>
    /// <typeparam name="T">Type of the message.</typeparam>
    public class MessageWrapper<T>
    {
        public string MessageType { get { return typeof(T).FullName; } }
        public T Message { get; set; }

        public string Serialize()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    // Message Models

    public class ShowAlarmMessage : BaseMessage
    {
        public Alarm Alarm { get; set; }

        public ShowAlarmMessage(Alarm alarm) : base()
        {
            Alarm = alarm;
        }
    }

    public class PingMessage : BaseMessage
    {
        // No data is included.
    }

    public class ErrorMessage : BaseMessage
    {
        public enum ErrorCode
        {
            MaxConnectionsExceeded
        }

        public ErrorCode Code { get; set; }
        public string Text { get; set; }

        public ErrorMessage(ErrorCode code, string text = null)
        {
            Code = code;
            Text = text;
        }
    }

    // Response Models

    public class ServiceStartedResponse : BaseResponse
    {
        /// <summary>
        /// IDs of the groups the user requests to register to.
        /// </summary>
        public List<Guid> GroupIDs { get; set; } = new List<Guid>();

        public ServiceStartedResponse(Instance instance,
            List<Guid> groupIDs = null) : base(instance)
        {
            Instance = Instance;

            if (groupIDs is null)
            {
                GroupIDs = new List<Guid> { Group.GlobalGroup.ID };
            }
            else
            {
                GroupIDs = groupIDs;
            }
        }
    }

    public class PingResponse : BaseResponse
    {
        public PingResponse(Instance instance) : base(instance)
        {
            // No additional data is included.
        }
    }
}

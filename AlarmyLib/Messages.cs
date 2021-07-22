using System;

namespace AlarmyLib
{
    /// <summary>
    /// Describes information sent from the server to the client.
    /// </summary>
    public class BaseMessage { }

    /// <summary>
    /// Describes information sent from the client to the server.
    /// </summary>
    public class BaseResponse
    {
        public Instance Instance { get; }

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
            MessageWrapper wrapper = System.Text.Json.JsonSerializer.Deserialize<MessageWrapper>(data);
            t = Type.GetType(wrapper.MessageType);
            return new MessageWrapperContent(System.Text.Json.JsonSerializer.Deserialize(data, t), t);
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
            return System.Text.Json.JsonSerializer.Serialize(this);
        }
    }

    // Message Models

    public class ShowAlarmMessage : BaseMessage
    {
        public Alarm Alarm { get; }

        public ShowAlarmMessage(Alarm alarm) : base()
        {
            Alarm = alarm;
        }
    }

    public class PingMessage : BaseMessage
    {

    }

    public class ServiceStartedResponse : BaseResponse
    {
        public ServiceStartedResponse(Instance instance) : base(instance)
        {

        }
    }

    public class KeepAliveResponse : BaseResponse
    {
        public KeepAliveResponse(Instance instance) : base(instance)
        {

        }
    }

    public class PingResponse : BaseResponse
    {
        public PingResponse(Instance instance) : base(instance)
        {

        }
    }
}

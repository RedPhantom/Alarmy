using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace AlarmyLib
{
    // For ease of use, all Json Serialization and Deserialization should be done here.

    // Reference for serialization and deserialization in this class are taken from here:
    // https://stackoverflow.com/questions/38679972/determine-type-during-json-deserialize.

    public class Transmission 
    {
        /// <remarks>
        /// By default, ToString() should return Repr() since we don't plan
        /// on showing nicely formatted Transmissions to the user.
        /// </remarks>
        public override string ToString()
        {
            return Repr();
        }

        public string Repr()
        {
            return $"<Transmission>";
        }
    }

    /// <summary>
    /// Describes information sent from the server to the client.
    /// </summary>
    public class BaseMessage : Transmission
    {
        public new string Repr()
        {
            return $"<BaseMessage>";
        }
    }

    /// <summary>
    /// Describes information sent from the client to the server.
    /// </summary>
    public class BaseResponse : Transmission
    {
        public Instance Instance { get; set; }

        public BaseResponse(Instance instance)
        {
            Instance = instance;
        }

        public new string Repr()
        {
            return $"<BaseResponse Instance={Instance}>";
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

        public override string ToString()
        {
            return Repr();
        }

        public string Repr()
        {
            return $"<MessageWrapperContent Type={Type}, Message={Message}>";
        }

        public override bool Equals(object obj)
        {
            MessageWrapperContent _other;

            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            _other = (MessageWrapperContent)obj;

            // TODO: maybe deserialze in order to compare? maybe instead of adding Equals to MWC and MQI just add a unique id?
            return _other.Message == Message &&
                   _other.Type == Type;
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            // TODO: write your implementation of GetHashCode() here
            throw new NotImplementedException();
            return base.GetHashCode();
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

        public override string ToString()
        {
            return Repr();
        }

        public string Repr()
        {
            return $"<MessageWrapper<T> MessageType={MessageType}, Message={Message}>";
        }
    }

    // Message Models

    public class ShowAlarmMessage : BaseMessage
    {
        public Alarm Alarm { get; set; }
        public AlarmType Type { get; set; }

        public ShowAlarmMessage(Alarm alarm, AlarmType type) : base()
        {
            Alarm = alarm;
            Type = type;
        }

        public new string Repr()
        {
            return $"<ShowAlarmMessage Type={Type}, Alarm={Alarm}>";
        }
    }

    public class PingMessage : BaseMessage
    {
        // No data is included.
        public new string Repr()
        {
            return "<PingMessage>";
        }
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

        public new string Repr()
        {
            return $"<ErrorMessage Code={Code}, Text={Text}>";
        }
    }

    public class GroupQueryMessage : BaseMessage
    {
        public Guid GroupID { get; set; }

        public GroupQueryMessage(Guid groupId)
        {
            GroupID = groupId;
        }

        public new string Repr()
        {
            return $"<GroupQueryMessage GroupID={GroupID}>";
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

        public new string Repr()
        {
            return $"<ServiceStartedResponse Instance={Instance}, GroupIDs={string.Join(", ", GroupIDs)}>";
        }
    }

    public class PingResponse : BaseResponse
    {
        public PingResponse(Instance instance) : base(instance)
        {
            // No additional data is included.
        }

        public new string Repr()
        {
            return $"<PingResponse Instance={Instance}>";
        }
    }

    public class GroupQueryResponse : BaseResponse
    {
        public Group Group { get; set; }

        public GroupQueryResponse(Instance instance, Group group) : base(instance)
        {
            Group = group;
        }

        public new string Repr()
        {
            return $"<GroupQueryResponse Instance={Instance}, Group={Group}>";
        }
    }
}

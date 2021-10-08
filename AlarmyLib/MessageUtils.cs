using System.Diagnostics;
using System.Text;

namespace AlarmyLib
{
    public static class MessageUtils
    {
        public static readonly Encoding Encoding = Encoding.UTF8;

        public static string BuildMessageString(string msg)
        {
            return msg + Consts.EOFTag;
        }

        public static byte[] BuildMessage(string msg)
        {
            return Encoding.GetBytes(BuildMessageString(msg));
        }

        /// <summary>
        /// Parse a message string to receive a <see cref="MessageWrapperContent"/> instance.
        /// Will remove the EOF tag if the <c>removeEofTag</c> is set to <c>true</c> or if the EOF
        /// tag is detected at the end of the message.
        /// </summary>
        public static MessageWrapperContent ParseMessageString(string msg, bool removeEofTag = true)
        {
            int trimEndChars = 0;

            if (removeEofTag || msg.EndsWith(Consts.EOFTag))
            {
                trimEndChars = Consts.EOFTag.Length;
            }

            return MessageWrapper.Deserialize(msg.Substring(0, msg.Length - trimEndChars));
        }

        public static MessageWrapperContent ParseMessage(byte[] rawData)
        {
            string data = Encoding.GetString(rawData).TrimEnd('\0');
            return ParseMessageString(data);
        }
    }
}

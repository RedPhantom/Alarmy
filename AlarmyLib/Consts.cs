namespace AlarmyLib
{
    public static class Consts
    {
        /// <summary>
        /// Tag that should end each message and response bween the server and the clients.
        /// </summary>
        public const string EOFTag = "<EOF>";

        /// <summary>
        /// Size of the buffer in server-client communications, in bytes.
        /// </summary>
        public const int BufferSize = 1024;
    }

    public enum AlarmType
    {
        RTF,
        TextOnly
    }
    
    public enum AlarmStyle
    {
        /// <summary>
        /// Alarms pop in the background.
        /// </summary>
        Background,

        /// <summary>
        /// Alarms pop in the foreground and must be closed.
        /// </summary>
        Interruptive
    }
}

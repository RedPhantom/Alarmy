using System;

namespace AlarmyManager
{
    public abstract class TcpServiceProvider : ICloneable
    {
        /// <summary>
        /// Provides a new instance of the object.
        /// </summary>
        public virtual object Clone()
        {
            throw new Exception("Derived classes must override Clone method.");
        }

        /// <summary>
        /// Gets executed when the server accepts a new connection.
        /// </summary>
        public abstract void OnAcceptConnection(ConnectionState state);

        /// <summary>
        /// Gets executed when the server detects incoming data.
        /// This method is called only if
        /// OnAcceptConnection has already finished.
        /// </summary>
        public abstract void OnReceiveData(ConnectionState state);

        /// <summary>
        /// Gets executed when the server needs to shutdown the connection.
        /// </summary>
        public abstract void OnDropConnection(ConnectionState state);
    }
}

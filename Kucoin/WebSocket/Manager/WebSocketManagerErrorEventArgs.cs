using Kucoin.Utility;
using System;

namespace Kucoin.WebSocket.Manager
{
    public class WebSocketManagerErrorEventArgs : EventArgs
    {
        #region Public Properties

        public WebSocketManagerException Exception { get; }

        #endregion Public Properties

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="exception"></param>
        public WebSocketManagerErrorEventArgs(WebSocketManagerException exception)
        {
            Throw.IfNull(exception, nameof(exception));

            Exception = exception;
        }

        #endregion Constructors
    }
}

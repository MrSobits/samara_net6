namespace Bars.Gkh.B4Events.Payloads
{
    using System;

    using Bars.B4.Events;

    public class SessionEndEventArgs : AppEventArgsBase
    {
        public SessionEndEventArgs(string sessionId)
        {
            this.Date = DateTime.Now;
            this.SessionId = sessionId;
        }

        public string SessionId { get; }
    }
}
namespace Bars.Gkh.Reforma.Impl.Logger
{
    using System;

    using Bars.Gkh.Reforma.Impl.Performer.Action;
    using Bars.Gkh.Reforma.Interface.Logger;

    /// <summary>
    /// Пустой логгер. Используется для silent-режима провайдера синхронизации
    /// </summary>
    public class NullSyncLogger : ISyncLogger
    {
        public void StartActionInvocation(string actionId, string parameters)
        {
        }

        public void LogRequest(string content)
        {
        }

        public void LogResponse(string content)
        {
        }

        public void EndActionInvocation(SyncActionResult result)
        {
        }

        public void SetActionDetails(string details)
        {
        }

        public void SetException(Exception e)
        {
        }

        public void AddMessage(string message)
        {
        }
    }
}
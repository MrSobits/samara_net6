namespace Bars.Gkh.Reforma.Interface.Logger
{
    using System;

    using Bars.Gkh.Reforma.Impl.Performer.Action;

    /// <summary>
    /// Логгер запросов к сервису Реформы
    /// </summary>
    public interface ISyncLogger
    {
        /// <summary>
        /// Начало вызова метода
        /// </summary>
        /// <param name="actionId">Имя метода</param>
        /// <param name="parameters">Параметры вызова</param>
        void StartActionInvocation(string actionId, string parameters);

        /// <summary>
        /// Исходящий запрос
        /// </summary>
        /// <param name="content">Содержимое запроса</param>
        void LogRequest(string content);

        /// <summary>
        /// Ответ от сервера
        /// </summary>
        /// <param name="content">Содержимое ответа</param>
        void LogResponse(string content);

        /// <summary>
        /// Окончание вызова метода
        /// </summary>
        /// <param name="result">Результат действия</param>
        void EndActionInvocation(SyncActionResult result);

        /// <summary>
        /// Указывает детали действия
        /// </summary>
        /// <param name="details">Детали</param>
        void SetActionDetails(string details);

        /// <summary>
        /// Исключение
        /// </summary>
        /// <param name="e">Исключение</param>
        void SetException(Exception e);

        /// <summary>
        /// Добавить сообщение в лог
        /// </summary>
        /// <param name="message">Сообщение</param>
        void AddMessage(string message);
    }
}
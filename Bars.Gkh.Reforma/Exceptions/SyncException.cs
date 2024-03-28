namespace Bars.Gkh.Reforma.Exceptions
{
    using System;
    using System.ServiceModel;

    using Bars.Gkh.Reforma.Impl.Performer;
    using Bars.Gkh.Reforma.Impl.Performer.Action;

    /// <summary>
    /// Исключение в ходе обращения к сервису Реформы
    /// </summary>
    public class SyncException : Exception
    {
        #region Constructors and Destructors

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="code">Код ошибки</param>
        /// <param name="name">Название ошибки</param>
        /// <param name="description">Подробное описание ошибки</param>
        public SyncException(string code, string name, string description)
            : base(string.Format("{0}: {1}", code, name))
        {
            this.Details = new ErrorDetails { Code = code, Name = name, Description = description };
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="fault">Исключение, брошеное клиентом</param>
        public SyncException(FaultException fault)
            : base(fault.Message, fault)
        {
            this.Details = new ErrorDetails(fault);
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Детали ошибки
        /// </summary>
        public ErrorDetails Details { get; protected set; }

        #endregion
    }
}
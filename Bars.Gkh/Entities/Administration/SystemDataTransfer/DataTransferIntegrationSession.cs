namespace Bars.Gkh.Entities.Administration.SystemDataTransfer
{
    using System;
    using System.Collections.Generic;

    using Bars.B4.DataAccess;
    using Bars.Gkh.Enums;
    using Bars.Gkh.SystemDataTransfer.Enums;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Сессия интеграции с внешней системой
    /// </summary>
    public class DataTransferIntegrationSession : BaseEntity
    {
        protected DataTransferIntegrationSession()
        {
            this.DateStart = DateTime.Now;
            this.TransferingState = TransferingState.SendExportTask;
        }

        public DataTransferIntegrationSession(Guid guid, DataTransferOperationType typeIntegration) : this()
        {
            this.TypeIntegration = typeIntegration;
            this.Guid = guid;
        }

        public DataTransferIntegrationSession(DataTransferOperationType typeIntegration) : this(Guid.NewGuid(), typeIntegration)
        {
        }

        /// <summary>
        /// Дата начала интеграции
        /// </summary>
        public virtual DateTime DateStart { get; set; }

        /// <summary>
        /// Дата окончания интеграции
        /// </summary>
        public virtual DateTime? DateEnd { get; set; }

        /// <summary>
        /// Вид интеграции
        /// </summary>
        public virtual DataTransferOperationType TypeIntegration { get; set; }

        /// <summary>
        /// Уникальный идентификатор сессии
        /// </summary>
        public virtual Guid Guid { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Состояние интеграции
        /// </summary>
        public virtual TransferingState TransferingState { get; protected set; }

        /// <summary>
        /// Успешность интеграции
        /// </summary>
        public virtual YesNo? Success { get; protected set; }

        /// <summary>
        /// Код ошибки
        /// </summary>
        public virtual string ErrorCode { get; set; }

        /// <summary>
        /// Сообщение ошибки
        /// </summary>
        public virtual string ErrorMessage { get; set; }

        /// <summary>
        /// Метод переводит состояние сессии в следующее (процесс интеграции успешно выполнил текущий этап)
        /// </summary>
        public virtual void SetSuccessState()
        {
            if (this.Success == YesNo.No)
            {
                return;
            }

            if (this.TransferingState != TransferingState.IntegrationComplete)
            {
                this.TransferingState = this.TransferingState.Next();
            }

            if (this.TransferingState == TransferingState.IntegrationComplete && !this.Success.HasValue)
            {
                this.Success = YesNo.Yes;
            }

            if (this.Success == YesNo.Yes && !this.DateEnd.HasValue)
            {
                this.DateEnd = DateTime.Now;
            }
        }

        /// <summary>
        /// Процесс интеграции завершился с ошибкой на текущем этапе
        /// </summary>
        public virtual void SetErrorState(string message = null, string code = null)
        {
            this.Success = YesNo.No;
            this.ErrorMessage = message;
            this.ErrorCode = code;

            if (!this.DateEnd.HasValue)
            {
                this.DateEnd = DateTime.Now;
            }
        }

        /// <summary>
        /// Типы сущностей, которые будут интегрироваться в текущей сессии
        /// </summary>
        public virtual IDictionary<string, bool?> TypesNames { get; set; }
    }
}
namespace Bars.Gkh.RegOperator.Tasks.PaymentDocuments.V2
{
    using System;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.DomainService.PersonalAccount.PayDoc;

    /// <summary>
    /// Контекст создания задачи
    /// </summary>
    internal class CreateTaskContext
    {
        /// <summary>
        /// Дерево счетов
        /// </summary>
        public TreeNode Root { get; set; }

        /// <summary>
        /// Период
        /// </summary>
        public IPeriod Period { get; set; }

        /// <summary>
        /// Допускаются ли пусты платежи
        /// </summary>
        public bool IsZeroPaymentDoc { get; set; }

        /// <summary>
        /// Один отчёт на счёт
        /// </summary>
        public bool ReportPerAccount { get; set; }

        /// <summary>
        /// Время начала
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Тип исполнителя
        /// </summary>
        public ExecutorType ExecutorType { get; set; }

        /// <summary>
        /// Шаблон печати
        /// </summary>
        public string SourceUiForm { get; set; }

        /// <summary>
        /// Универсальные идентификатор набора
        /// </summary>
        public string Uid { get; set; }

        /// <summary>
        /// Частичный реестр
        /// </summary>
        public bool IsPartially { get; set; }

        public CreateTaskContext Clone()
        {
            return new CreateTaskContext
            {
                Root = this.Root,
                Period = this.Period,
                IsZeroPaymentDoc = this.IsZeroPaymentDoc,
                ReportPerAccount = this.ReportPerAccount,
                StartDate = this.StartDate,
                ExecutorType = this.ExecutorType,
                SourceUiForm = this.SourceUiForm,
                Uid = this.Uid
            };
        }
    }
}

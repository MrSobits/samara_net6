namespace Bars.Gkh.RegOperator.DomainService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.States;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.GkhCr.Enums;

    /// <summary>
    /// Сервис для работы с сущностью "Заявка на перечисление средств подрядчикам"
    /// </summary>
    public interface ITransferCtrService
    {
        /// <summary>
        /// Экспорт в txt формат
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Zip архив со сформированными документами</returns>
        IDataResult ExportToTxt(BaseParams baseParams);

        /// <summary>
        /// Сохранить с детализацией
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Сохраненная запись</returns>
        IDataResult SaveWithDetails(BaseParams baseParams);

        /// <summary>
        /// Сохранить с детализацией
        /// </summary>
        /// <param name="transferCtr">Заявка на перечисление средств подрядчикам</param>
        /// <param name="details">Детализация оплат заявки на перечисление средств подрядчикам</param>
        /// <param name="calcSum">Подсчитывать сумму заявки по оплатам из распределения</param>
        void SaveWithDetails(TransferCtr transferCtr, IEnumerable<TransferCtrPaymentDetail> details, bool calcSum = true);

        /// <summary>
        /// Скопировать заявку
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Результат выполнения запроса</returns>
        IDataResult Copy(BaseParams baseParams);

        /// <summary>
        /// Список записей TransferCtr
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Результат выполнения запроса</returns>
        IQueryable<TransferCtrProxy> List(BaseParams baseParams);
    }

    /// <summary>
    /// Объект для записей для TransferCtr
    /// </summary>
    public class TransferCtrProxy
    {
        public long Id { get; set; }

        public string DocumentNum { get; set; }

        public DateTime? DateFrom { get; set; }

        public State State { get; set; }

        public bool IsExport { get; set; }

        public decimal Sum { get; set; }

        public decimal PaidSum { get; set; }

        public TypeProgramCr TypeProgramRequest { get; set; }

        public string Municipality { get; set; }

        public string ObjectCr { get; set; }

        public string ProgramCr { get; set; }

        public string Builder { get; set; }

        public string Contract { get; set; }

        public string BuilderInn { get; set; }

        public string BuilderSettlAcc { get; set; }

        public string CalcAccNumber { get; set; }

        public string Perfomer { get; set; }

        public FileInfo Document { get; set; }

        public string TypeWorkCr { get; set; }
    }
}
namespace Bars.Gkh.RegOperator.DomainService.PersonalAccount.PayDoc
{
    using System.IO;
    using System.Linq;

    using B4;

    using Bars.B4.Modules.Tasks.Common.Service;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount.PayDoc;

    using Entities;

    /// <summary>
    /// Интерфейс выгрузки документов на оплату
    /// </summary>
    public interface IPaymentDocumentService
    {
        /// <summary>
        /// Выгрузить документы на оплату
        /// </summary>
        IDataResult GetPaymentDocuments(BaseParams baseParams);

        /// <summary>
        /// Создать документы на оплаты напрямую из сохраненных данных
        /// </summary>
        IDataResult CreateDocumentsFromSnapshots(BaseParams prms);

        /// <summary>
        /// Получить документ по ЛС
        /// </summary>
        /// <param name="account">ЛС</param>
        /// <param name="period">Период начисления</param>
        /// <param name="saveSnapshot">Параметр сохранения слепков</param>
        /// <param name="useHistory">Как выбирать владельца лс: из истории или текущего</param>
        Stream GetPaymentDocument(BasePersonalAccount account, ChargePeriod period, bool saveSnapshot, bool useHistory);

        /// <summary>
        /// Сформировать документ на оплату на основе сохраненных данных
        /// </summary>
        /// <param name="snapshotid">Id слепка</param>
        Stream CreateDocumentFromSnapshot(long snapshotid);

        /// <summary>
        /// Удалить снапшоты сохраненных платежек
        /// </summary>
        IDataResult DeleteSnapshots(BaseParams prms);

        /// <summary>
        /// Удалить снапшоты сохраненных платежек
        /// </summary>
        IDataResult DeleteSnapshots(long[] snapshotsId, long period);

        /// <summary>
        /// Получить слепки по лицевым счетам
        /// </summary>
        IQueryable<PaymentInfoSnapshotProxy> GetPaymentInfoSnapshots(long snapshotId);

        /// <summary>
        /// Выгрузить документы на оплату
        /// </summary>
        IDataResult CheckIsBaseSnapshots(BaseParams baseParams);
        
        /// <summary>
        /// Отправить документы на оплату по эл. почте
        /// </summary>
        /// <param name="params">Параметры запроса</param>
        /// <param name="indicator">Индикатор выполнения задачи</param>
        IDataResult SendEmails(BaseParams @params, IProgressIndicator indicator);

        /// <summary>
        /// Установить наличие эл. почты
        /// </summary>
        IDataResult SetEmails(BaseParams @params, IProgressIndicator indicator);
    }

    public class PaymentInfoSnapshotProxy
    {
        public long Id { get; set; }

        public PaymentDocumentSnapshot Snapshot { get; set; }

        public long AccountId { get; set; }

        public string Data { get; set; }

        public string AccountNumber { get; set; }

        public string RoomAddress { get; set; }

        public RoomType RoomType { get; set; }

        public float Area { get; set; }

        public decimal Tariff { get; set; }

        public decimal SaldoOut { get; set; }

        public decimal ChargeSum => this.SaldoOut - this.PenaltySum;

        public decimal BaseTariffSum { get; set; }

        public decimal DecisionTariffSum { get; set; }

        public decimal PenaltySum { get; set; }

        public string Services { get; set; }
    }
}
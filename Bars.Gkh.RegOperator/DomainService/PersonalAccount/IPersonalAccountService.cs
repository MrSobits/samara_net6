      
﻿namespace Bars.Gkh.RegOperator.DomainService.PersonalAccount
{
    using System;
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Enums;

    /// <summary>
    /// Интерфейс сервиса персональных счетов
    /// </summary>
    public interface IPersonalAccountService
    {
        /// <summary>
        /// Получить список операций по периоду
        /// </summary>
        /// <param name="storeParams">Входные параметры</param>
        /// <returns>Список операций</returns>
        IDataResult ListOperations(BaseParams storeParams);

        /// <summary>
        /// Получить список лицевых счетов для сопоставления с выписками росреестра
        /// </summary>
        /// <param name="storeParams">Входные параметры</param>
        /// <returns>Список операций</returns>
        IDataResult ListAccountsForComparsion(BaseParams baseParams);

        /// <summary>
        /// Получить информацию по оплатам за все периоды
        /// </summary>
        /// <param name="baseParams"> baseParams </param>
        /// <returns> IDataResult </returns>
        IDataResult ListPaymentsInfo(BaseParams baseParams);

        /// <summary>
        /// Получение всех приходов за период
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        IDataResult GetAccountChargeInfoInPeriod(BaseParams baseParams);
        
        /// <summary>
        /// Получение номера лицевого счета по идентификатору
        /// </summary>
        /// <param name="account">Счёт</param>
        /// <param name="date">Дата</param>
        /// <returns>Тариф</returns>
        IDataResult GetPersonalNumByAccount(BaseParams baseParams);

        /// <summary>
        /// Получить ЛС по дому
        /// </summary>
        /// <param name="baseParams">Входные параметры</param>
        /// <returns>Список ЛС</returns>
        IDataResult PersonalAccountsByRo(BaseParams baseParams);
        
        /// <summary>
        /// Получить идентификаторы счетов по адресу (полнотекстовый поиск)
        /// </summary>
        /// <param name="loadParams">Входные параметры</param>
        /// <returns>Идентификаторы счетов </returns>
        long[] GetAccountIdsByAddress(LoadParam loadParams);

        /// <summary>
        /// Получение тарифа по дому
        /// </summary>
        /// <param name="baseParams">Входные параметры</param>
        /// <returns>Результат закрытия</returns>
        IDataResult GetTarifForRealtyObject(BaseParams baseParams);

        /// <summary>
        /// Получить тариф
        /// </summary>
        /// <param name="roId">Идентификатор дома</param>
        /// <param name="muId">Идентификатор муниципального образования</param>
        /// <param name="settlementId">Идентификатор поселения</param>
        /// <param name="date">Дата</param>
        /// <param name="roomId">Идентификатор помещения. Если передан, то тариф будет браться из подъезда</param>
        /// <returns>Тариф</returns>
        decimal GetTariff(long roId, long muId, long? settlementId, DateTime date, long roomId = 0);

        /// <summary>
        /// Получить тариф
        /// </summary>
        /// <param name="account">Счёт</param>
        /// <param name="date">Дата</param>
        /// <returns>Тариф</returns>
        decimal GetTariff(BasePersonalAccount account, DateTime date);

        /// <summary>
        /// Получение всех юр. лиц
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        IDataResult ListJurialContragents(BaseParams baseParams);

        /// <summary>
        /// Получить информацию по оплатам за все периоды
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        IList<PaymentProxy> ListPayments(long accountId);
    }

    public class PaymentProxy
    {
        public long Id { get; set; }

        public string Period { get; set; }

        public DateTime PaymentDate { get; set; }

        public string Reason { get; set; }

        public decimal Amount { get; set; }

        public TypeTransferSource Source { get; set; }

        public string DocumentNum { get; set; }

        public DateTime? DocumentDate { get; set; }

        public string PaymentAgentCode { get; set; }

        public string PaymentAgentName { get; set; }

        public DateTime ImportDate { get; set; }

        public string PaymentType { get; set; }

        public string PaymentNumberUs { get; set; }

        public DateTime OperationDate { get; set; }

        public DateTime DateReceipt { get; set; }

        public DateTime? DistributionDate { get; set; }

        public string DistributionCode { get; set; }

        public long DocumentId { get; set; }

        public DateTime? AcceptDate { get; set; }

        public string UserLogin { get; set; }
    }
}

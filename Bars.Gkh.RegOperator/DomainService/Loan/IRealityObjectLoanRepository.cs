namespace Bars.Gkh.RegOperator.DomainService
{
    using System.Collections.Generic;
    using B4;

    using Bars.B4.Modules.Tasks.Common.Contracts;
    using Bars.Gkh.RegOperator.DomainModelServices;

    using Gkh.Entities;
    using GkhCr.Entities;

    using Newtonsoft.Json;

    /// <summary>
    /// Интерфейс для получения информации по займам для объектов
    /// </summary>
    public interface IRealityObjectLoanRepository
    {
        /// <summary>
        /// Получение домов, нуждающихся в займах
        /// </summary>
        IDataResult ListRealtyObjectNeedLoan(BaseParams baseParams);

        /// <summary>
        /// Получение домов, нуждающихся в займах
        /// </summary>
        IEnumerable<RealtyObjectNeedLoan> ListRealtyObjectNeedLoan(RealityObject[] robjects, ProgramCr program);
    }

    /// <summary>
    /// Результат при получении домов, нуждающихся в займах
    /// </summary>
    public class ListRealtyObjectNeedLoanProxyResult : ListDataResult
    {
        /// <summary>
        /// Результат
        /// </summary>
        public IList<RealtyObjectNeedLoan> Data { get; set; }

        /// <summary>
        /// Информация о регоператоре
        /// </summary>
        public RegopLoanInfo AdditionalData { get; set; }
    }

    public class RealtyObjectNeedLoan
    {
        /// <summary>
        /// Идентификатор дома
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Адрес
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Наименования работ
        /// </summary>
        public string WorkNames { get; set; }

        /// <summary>
        /// Год
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// Собираемость %
        /// </summary>
        public decimal Collection { get; set; }

        /// <summary>
        /// Потребность
        /// </summary>
        public decimal NeedSum { get; set; }

        /// <summary>
        /// Текущий баланс
        /// </summary>
        public decimal CurrentBalance { get; set; }

        /// <summary>
        /// Средства собственников
        /// </summary>
        public decimal OwnerSum { get; set; }

        /// <summary>
        /// Сумма неоплаченных займов собственников
        /// </summary>
        public decimal OwnerLoanSum { get; set; }

        /// <summary>
        /// Субсидии
        /// </summary>
        public decimal SubsidySum { get; set; }

        /// <summary>
        /// Иные средства
        /// </summary>
        public decimal OtherSum { get; set; }

        /// <summary>
        /// Номер расчетного счета, к которому привязан дом
        /// </summary>
        public string CalcAccountNumber { get; set; }

        /// <summary>
        /// МО
        /// </summary>
        public string Municipality { get; set; }

        /// <summary>
        /// МО
        /// </summary>
        public string Settlement { get; set; }

        /// <summary>
        /// Информация о выполняемой задаче взятия займа (если имеется)
        /// </summary>
        public TaskInfo Task { get; set; }

        /// <summary>
        /// МО
        /// </summary>
        [JsonIgnore]
        public long MunicipalityId { get; set; }

        /// <summary>
        /// МР
        /// </summary>
        [JsonIgnore]
        public long SettlementId { get; set; }
    }

    public class TaskInfo
    {
        /// <summary>
        /// Идентификатор задачи
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Идентификатор родительской задачи
        /// </summary>
        public long ParentTaskId { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public TaskStatus Status { get; set; }
    }
}
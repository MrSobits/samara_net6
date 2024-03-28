namespace Bars.Gkh.RegOperator.DomainModelServices
{
    using System;
    using System.Collections.Generic;
    using B4.Modules.FileStorage;

    using Bars.B4;
    using Bars.B4.DataModels;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;

    using Entities.ValueObjects;
    using Enums;

    /// <summary>
    /// Распределяемый объект
    /// </summary>
    public interface IDistributable : ITransferParty, IHaveId
    {
        /// <summary>
        /// Источник распределения
        /// </summary>
        DistributionSource Source { get; }

        /// <summary>
        /// Код распределения
        /// </summary>
        string DistributionCode { get; set; }

        /// <summary>
        /// Направление движения средств (приход/расход)
        /// </summary>
        MoneyDirection MoneyDirection { get; }

        /// <summary>
        /// Дата поступления
        /// </summary>
        DateTime DateReceipt { get; }

        /// <summary>
        /// Дата распределения
        /// </summary>
        DateTime? DistributionDate { get; set; }

        /// <summary>
        /// Состояние (распределен/не распределен)
        /// </summary>
        DistributionState DistributeState { get; set; }

        /// <summary>
        /// Сумма
        /// </summary>
        decimal Sum { get; set; }

        /// <summary>
        /// Остаток
        /// </summary>
        decimal RemainSum { get; set; }

        /// <summary>
        /// взыскан РОСП
        /// </summary>
        bool IsROSP { get; set; }

        /// <summary>
        /// Получить операции
        /// </summary>
        /// <returns></returns>
        IEnumerable<MoneyOperation> GetOperations();

        /// <summary>
        /// Отменить текущую операцию
        /// </summary>
        /// <param name="period">Период операции</param>
        /// <returns></returns>
        MoneyOperation CancelOperation(MoneyOperation operation, ChargePeriod period);

        /// <summary>
        /// Создать операцию
        /// </summary>
        /// <param name="period">Период операции</param>
        /// <returns></returns>
        MoneyOperation CreateOperation(DynamicDictionary parameters, ChargePeriod period);



        /// <summary>
        /// Документ-основание
        /// </summary>
        FileInfo Document { get; set; }
    }
}
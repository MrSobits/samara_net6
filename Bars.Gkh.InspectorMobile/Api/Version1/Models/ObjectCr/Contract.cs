namespace Bars.Gkh.InspectorMobile.Api.Version1.Models.ObjectCr
{
    using System;
    using System.Collections.Generic;

    using Bars.B4.Utils;
    using Bars.Gkh.BaseApiIntegration.Attributes;
    using Bars.Gkh.InspectorMobile.Api.Version1.Models.Common;
    using Bars.Gkh.InspectorMobile.Api.Version1.Models.Interfaces;
    using Bars.GkhCr.Entities;
    using Bars.GkhCr.Enums;

    /// <summary>
    /// Договоры. Модель выборки
    /// </summary>
    public class ContractGet : BaseContract<StateInfoGet>
    {
        /// <summary>
        /// Уникальный идентификатор записи
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Работы
        /// </summary>
        public IEnumerable<WorkCr> Work { get; set; }

        /// <summary>
        /// Дата договора
        /// </summary>
        public DateTime? Date { get; set; }

        /// <summary>
        /// Номер договора
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// Заказчик
        /// </summary>
        public string Customer { get; set; }

        /// <summary>
        /// Подрядная организация
        /// </summary>
        public string ContractOrganization { get; set; }

        /// <summary>
        /// Тип договора (перечисление)
        /// </summary>
        internal TypeContractBuild TypeContractBuild { get; set; }

        /// <summary>
        /// Тип договора
        /// </summary>
        public string ContractType => this.TypeContractBuild.GetDisplayName();

        /// <summary>
        /// Сумма договора
        /// </summary>
        public decimal? Sum { get; set; }

        /// <summary>
        /// Дата начала работ
        /// </summary>
        public DateTime? StartDateWork { get; set; }

        /// <summary>
        /// Дата окончания работ
        /// </summary>
        public DateTime? EndDateWork { get; set; }

        /// <summary>
        /// Уникальный идентификатор файла
        /// </summary>
        public long? FileId { get; set; }
    }

    /// <summary>
    /// Договоры. Модель обновления
    /// </summary>
    public class ContractUpdate : BaseContract<StateInfoUpdate>, IEntityId
    {
        /// <inheritdoc />
        [RequiredExistsEntity(typeof(BuildContract))]
        public long? Id { get; set; }
    }

    /// <summary>
    /// Договоры. Базовая модель
    /// </summary>
    public class BaseContract<TStateInfo>
    {
        /// <summary>
        /// Статус записи в разделе "Договоры"
        /// </summary>
        public TStateInfo State { get; set; }
    }
}
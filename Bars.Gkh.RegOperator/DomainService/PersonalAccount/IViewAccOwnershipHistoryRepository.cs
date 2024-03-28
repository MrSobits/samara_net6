namespace Bars.Gkh.RegOperator.DomainService.PersonalAccount
{
    using System;
    using System.Linq;

    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.Views;
    using Bars.Gkh.RegOperator.Enums;

    /// <summary>
    /// ReadOnly репозиторий истории смены абонента ЛС в разрезе периода
    /// </summary>
    public interface IViewAccOwnershipHistoryRepository
    {
        /// <summary>
        /// Метод формирования запроса
        /// </summary>
        /// <param name="periodId">Идентификатор периода <see cref="ChargePeriod"/></param>
        IQueryable<ViewAccountOwnershipHistory> GetAll(long periodId);

        /// <summary>
        /// Метод формирования запроса получения Dto <see cref="ViewPersonalAccountDto"/>
        /// </summary>
        /// <param name="periodId">Идентификатор периода <see cref="ChargePeriod"/></param>
        IQueryable<ViewAccOwnershipHistoryDto> GetAllDto(long periodId = 0);
    }

    public class ViewAccOwnershipHistoryDto
    {
        /// <summary>
        /// Идентификатор ЛС <see cref="BasePersonalAccount"/>
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Номер ЛС
        /// </summary>
        public string PersonalAccountNum { get; set; }

        /// <summary>
        /// Дата редактирования ЛС
        /// </summary>
        public DateTime ObjectEditDate { get; set; }

        /// <summary>
        /// Идентификатор владельца ЛС <see cref="PersonalAccountOwner"/>
        /// </summary>
        public long OwnerId { get; set; }

        /// <summary>
        /// Тип владельца
        /// </summary>
        public PersonalAccountOwnerType OwnerType { get; set; }

        /// <summary>
        /// Имя владельца
        /// </summary>
        public string AccountOwner { get; set; }

        /// <summary>
        /// Идентификатор контрагента <see cref="IHaveExportId"/>
        /// </summary>
        public long? LegalOwnerExportId { get; set; }

        /// <summary>
        /// Мунинципальный район (id)
        /// </summary>
        public long MunicipalityId { get; set; }

        /// <summary>
        /// Идентификатор дома
        /// </summary>
        public long RoId { get; set; }

        /// <summary>
        /// Идентификатор помещения
        /// </summary>
        public long RoomId { get; set; }

        /// <summary>
        /// Мунинципальный район
        /// </summary>
        public string Municipality { get; set; }

        /// <summary>
        /// Адрес дома
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Адрес помещения
        /// </summary>
        public string RoomAddress { get; set; }

        /// <summary>
        /// Площадь помещения
        /// </summary>
        public decimal Area { get; set; }

        /// <summary>
        /// Доля собственности
        /// </summary>
        public decimal AreaShare { get; set; }

        /// <summary>
        /// Дата открытия
        /// </summary>
        public DateTime OpenDate { get; set; }

        /// <summary>
        /// Дата закрытия
        /// </summary>
        public DateTime? CloseDate { get; set; }

        /// <summary>
        /// Наименование статуса
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// способ формирования фонда кр на текущий момент
        /// </summary>
        public virtual CrFundFormationType AccountFormationVariant { get; set; }
    }
}
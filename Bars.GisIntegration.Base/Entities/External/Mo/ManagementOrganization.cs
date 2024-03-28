namespace Bars.GisIntegration.Base.Entities.External.Mo
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.GisIntegration.Base.Entities.External.Administration.System;
    using Bars.GisIntegration.Base.Entities.External.Contragent;

    /// <summary>
    /// Управляющая организация
    /// </summary>
    public class ManagementOrganization : BaseEntity
    {
        /// <summary>
        /// Поставщик информации
        /// </summary>
        public virtual DataSupplier DataSupplier { get; set; }
        /// <summary>
        /// Контрагент
        /// </summary>
        public virtual ExtContragent Contragent { get; set; }

        /// <summary>
        /// ФИО председателя
        /// </summary>
        public virtual string ChairmanFio { get; set; }
        /// <summary>
        /// Контактный номер председателя
        /// </summary>
        public virtual string ChairmanPhone { get; set; }
        /// <summary>
        /// Штатная численность административного персонала
        /// </summary>
        public virtual int? ManagStaffCnt { get; set; }
        /// <summary>
        /// Штатная численность инженеров
        /// </summary>
        public virtual int? EngineerCnt { get; set; }
        /// <summary>
        /// Штатная численность рабочих
        /// </summary>
        public virtual int? WorkerCnt { get; set; }
        /// <summary>
        /// Доля участия субъекта РФ в уставном капитале организации
        /// </summary>
        public virtual decimal? RfCapitalShare { get; set; }
        /// <summary>
        /// Доля участия МО в уставном капитале организации
        /// </summary>
        public virtual decimal? MoCapitalShare { get; set; }
        /// <summary>
        /// ТСЖ/Кооператив
        /// </summary>
        public virtual bool IsTsg { get; set; }
        /// <summary>
        /// Телефон диспетчерской службы
        /// </summary>
        public virtual string DispatchPhone { get; set; }
        /// <summary>
        /// Адрес диспетчерской службы
        /// </summary>
        public virtual string DispatchAddress { get; set; }
        /// <summary>
        /// Пользователь 
        /// </summary>
        public virtual int ChangedBy { get; set; }
        /// <summary>
        /// Дата изменения 
        /// </summary>
        public virtual DateTime ChangedOn { get; set; }
    }
}

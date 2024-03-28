namespace Bars.GisIntegration.Base.Entities.External.Housing.OKI
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FIAS;
    using Bars.GisIntegration.Base.Entities.External.Administration.System;
    using Bars.GisIntegration.Base.Entities.External.Dict.Common;
    using Bars.GisIntegration.Base.Entities.External.Dict.Oki;
    using Bars.GisIntegration.Base.Entities.External.Rso;

    /// <summary>
    /// ОКИ
    /// </summary>
    public class OkiObject : BaseEntity
    {
        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string ObjectName { get; set; }

        /// <summary>
        /// Поставщик информации
        /// </summary>
        public virtual DataSupplier DataSupplier { get; set; }

        /// <summary>
        /// ГУИД из ГИС
        /// </summary>
        public virtual string GisGuid { get; set; }

        /// <summary>
        /// Основание эксплуатации объекта инфраструктуры.
        /// </summary>
        public virtual RunBase RunBase { get; set; }
        /// <summary>
        /// Основание эксплуатации объекта инфраструктуры Id
        /// </summary>
        public virtual long RunBaseId { get; set; }
        /// <summary>
        /// Окончание управления
        /// </summary>
        public virtual DateTime? ManagTo { get; set; }
        /// <summary>
        /// Бессрочное управление
        /// </summary>
        public virtual bool IsUnlimManag { get; set; }
        /// <summary>
        /// ??
        /// </summary>
        public virtual bool? IsMoBalance { get; set; }
        /// <summary>
        /// РСО Компания 
        /// </summary>
        public virtual RsoCompany RsoCompany { get; set; }
        /// <summary>
        /// РСО Компания Id
        /// </summary>
        public virtual long RsoCompanyId { get; set; }
        /// <summary>
        ///  МО
        /// </summary>
        public virtual MoTerritory MoTerritory { get; set; }
        /// <summary>
        ///  МО Id
        /// </summary>
        public virtual long MoTerritoryId { get; set; }
        /// <summary>
        /// Вид ОКИ (8)
        /// </summary>
        public virtual OkiType OkiType { get; set; }
        /// <summary>
        ///  Вид ОКИ Id
        /// </summary>
        public virtual long OkiTypeId { get; set; }
        /// <summary>
        /// ??
        /// </summary>
        public virtual int? StartUpFrom { get; set; }
        /// <summary>
        /// ??
        /// </summary>
        public virtual bool? IsAutonom { get; set; }
        /// <summary>
        /// ??
        /// </summary>
        public virtual decimal? Wearout { get; set; }
        /// <summary>
        /// Комментарий
        /// </summary>
        public virtual string Comment { get; set; }
        /// <summary>
        /// Количество поломок
        /// </summary>
        public virtual decimal? CrashCount { get; set; }
        /// <summary>
        /// Запись сформированного Адреса на основе составляющих элементов ФИАС
        /// </summary>
        public virtual FiasAddress ObjectAddress { get; set; }
        /// <summary>
        /// Признак удаления
        /// </summary>
        public virtual bool IsDel { get; set; }

        /// <summary>
        /// Тип электрической подстанции 
        /// </summary>
        public virtual ElectroSubstantionType ElectroSubstantionType { get; set; }
        /// <summary>
        /// Вид электростанции
        /// </summary>
        public virtual ElectroStationType ElectroStationType { get; set; }
        /// <summary>
        /// Вид топлива (21)
        /// </summary>
        public virtual FuelType FuelType { get; set; }
        /// <summary>
        /// Тип газораспределительной сети
        /// </summary>
        public virtual GasNetType GasNetType { get; set; }
        /// <summary>
        /// Вид водозаборного сооружения
        /// </summary>
        public virtual WaterIntakeType WaterIntakeType { get; set; }

        /// <summary>
        /// Кем изменено
        /// </summary>
        public virtual int ChangedBy { get; set; }
        /// <summary>
        /// Когда изменено
        /// </summary>
        public virtual DateTime ChangedOn { get; set; }
    }
}

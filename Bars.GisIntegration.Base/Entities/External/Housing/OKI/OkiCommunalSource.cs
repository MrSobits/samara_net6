namespace Bars.GisIntegration.Base.Entities.External.Housing.OKI
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.GisIntegration.Base.Entities.External.Administration.System;
    using Bars.GisIntegration.Base.Entities.External.Dict.House;
    using Bars.GisIntegration.Base.Entities.External.Dict.Oki;
    using Bars.GisIntegration.Base.Enums;

    /// <summary>
    /// Новый ресурс
    /// </summary>
    public class OkiCommunalSource : BaseEntity
    {
        /// <summary>
        /// Поставщик информации
        /// </summary>
        public virtual DataSupplier DataSupplier { get; set; }
        /// <summary>
        /// Идентификатор ОКИ
        /// </summary>
        public virtual long OkiId { get; set; }
        /// <summary>
        /// ОКИ
        /// </summary>
        public virtual OkiObject OkiObject { get; set; }
        /// <summary>
        /// Секции ОКИ
        /// </summary>
        public virtual OkiSection OkiSection { get; set; }
        /// <summary>
        ///  Вид коммунального ресурса (4)
        /// </summary>
        public virtual CommunalSource CommunalSource { get; set; }
        /// <summary>
        ///  Присоединенная нагрузка
        /// </summary>
        public virtual decimal ConnectLoad { get; set; }
        /// <summary>
        ///  Промышленность
        /// </summary>
        public virtual decimal Industry { get; set; }
        /// <summary>
        ///  Социальная сфера
        /// </summary>
        public virtual decimal LossVolume { get; set; }
        /// <summary>
        ///  Социальная сфера
        /// </summary>
        public virtual decimal SocialArea { get; set; }
        /// <summary>
        ///  Население (включая УК, ТСЖ)
        /// </summary>
        public virtual decimal Populance { get; set; }
        /// <summary>
        ///  Вид теплоносителя
        /// </summary>
        public virtual HeatType HeatType { get; set; }
        /// <summary>
        ///  Установленное напряжение
        /// </summary>
        public virtual decimal InstallPower { get; set; }
        /// <summary>
        ///  Объем потерь
        /// </summary>
        public virtual decimal AvailPower { get; set; }
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

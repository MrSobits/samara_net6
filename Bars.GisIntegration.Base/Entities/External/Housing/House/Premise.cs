namespace Bars.GisIntegration.Base.Entities.External.Housing.House
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.GisIntegration.Base.Entities.External.Administration.System;
    using Bars.GisIntegration.Base.Entities.External.Dict.House;

    /// <summary>
    /// Помещение
    /// </summary>
    public class Premise : BaseEntity
    {
        /// <summary>
        /// Поставщик информации
        /// </summary>
        public virtual DataSupplier DataSupplier { get; set; }
        /// <summary>
        /// ГУИД подъезда
        /// </summary>
        public virtual string PremiseGuid { get; set; }
        /// <summary>
        /// Категория помещения
        /// </summary>
        public virtual PremiseCategory PremiseCategory { get; set; }
        /// <summary>
        /// Категория помещения - идентификатор
        /// </summary>
        public virtual long PremiseCategoryId { get; set; }
        /// <summary>
        /// Категория помещения - наименование
        /// </summary>
        public virtual string PremiseCategoryName { get; set; }
        /// <summary>
        /// Кадастровый номер
        /// </summary>
        public virtual string CadastrNumber { get; set; }
        /// <summary>
        /// Связь с ГКН отсутствует
        /// </summary>
        public virtual bool IsGknNone { get; set; }
        /// <summary>
        ///  Номер помещения
        /// </summary>
        public virtual string PremiseNumber { get; set; }
        /// <summary>
        /// Этаж
        /// </summary>
        public virtual string Floor { get; set; }
        /// <summary>
        /// Дата окончания
        /// </summary>
        public virtual DateTime? EndDate { get; set; }
        /// <summary>
        /// Подъезд
        /// </summary>
        public virtual Porch Porch { get; set; }
        /// <summary>
        /// Дом
        /// </summary>
        public virtual House House { get; set; }
        /// <summary>
        /// Вид жилого помещения
        /// </summary>
        public virtual LivePremiseKind LivePremiseKind { get; set; }
        /// <summary>
        /// Общая площадь
        /// </summary>
        public virtual decimal TotalSquare { get; set; }
        /// <summary>
        /// Условный номер в ЕГРП
        /// </summary>
        public virtual string EgrpNumber { get; set; }
        /// <summary>
        /// Жилая площадь
        /// </summary>
        public virtual decimal LiveSquare { get; set; }
        /// <summary>
        /// Входит в состав общего имущества
        /// </summary>
        public virtual bool IsCommonProperty { get; set; }
        /// <summary>
        /// Другие характеристики
        /// </summary>
        public virtual string NonliveOtherChar { get; set; }
        /// <summary>
        ///  Характеристика жилых помещений
        /// </summary>
        public virtual LivePremiseChar LivePremiseChar { get; set; }
        /// <summary>
        /// Пользователь 
        /// </summary>
        public virtual int ChangedBy { get; set; }
        /// <summary>
        /// Дата изменения 
        /// </summary>
        public virtual DateTime ChangedOn { get; set; }
        public virtual long Count { get; set; }
        public virtual int? PorchNumber { get; set; }
    }
}

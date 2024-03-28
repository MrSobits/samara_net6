namespace Bars.Gkh.Overhaul.Entities
{
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Overhaul.Enum;

    using Gkh.Entities.CommonEstateObject;

    /// <summary>
    ///  Конструктивный элемент дома
    /// </summary>
    public class RealityObjectStructuralElement : BaseImportableEntity, IStatefulEntity
    {
        /// <summary>
        /// Объект недвижимости
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }

        /// <summary>
        /// Конструктивный элемент
        /// </summary>
        public virtual StructuralElement StructuralElement { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public virtual State State { get; set; }

        /// <summary>
        /// Объем
        /// </summary>
        public virtual decimal Volume { get; set; }

        /// <summary>
        /// Флаг: ремонт проведен
        /// </summary>
        public virtual bool Repaired { get; set; }

        /// <summary>
        /// Год последнего ремонта
        /// </summary>
        public virtual int LastOverhaulYear { get; set; }

        /// <summary>
        /// Износ
        /// </summary>
        public virtual decimal Wearout { get; set; }

        /// <summary>
        /// Износ
        /// </summary>
        public virtual decimal WearoutActual { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Состояние КЭ
        /// </summary>
        public virtual ConditionStructElement Condition { get; set; }

        /// <summary>
        /// Тип инженерной системы
        /// </summary>
        public virtual EngineeringSystemType SystemType { get; set; }

        /// <summary>
        /// Протяженность сетей
        /// </summary>
        public virtual string NetworkLength { get; set; }

        /// <summary>
        /// Мощность
        /// </summary>
        public virtual string NetworkPower { get; set; }

        /// <summary>
        /// Файл
        /// </summary>
        public virtual FileInfo FileInfo { get; set; }
    }
}
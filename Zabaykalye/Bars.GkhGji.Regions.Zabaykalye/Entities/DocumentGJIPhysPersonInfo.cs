namespace Bars.GkhGji.Regions.Zabaykalye.Entities
{
    using Bars.B4.DataAccess;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Реквизиты физ. лица, которому выдан документ ГЖИ
    /// </summary>
    public class DocumentGJIPhysPersonInfo : BaseEntity
    {
        /// <summary>
        /// документ ГЖИ
        /// </summary>
        public virtual DocumentGji Document { get; set; }

        /// <summary>
        /// Адрес (место жительства, телефон)
        /// </summary>
        public virtual string PhysPersonAddress { get; set; }

        /// <summary>
        /// Место работы
        /// </summary>
        public virtual string PhysPersonJob { get; set; }

        /// <summary>
        /// Должность
        /// </summary>
        public virtual string PhysPersonPosition { get; set; }

        /// <summary>
        /// Дата, место рождения
        /// </summary>
        public virtual string PhysPersonBirthdayAndPlace { get; set; }

        /// <summary>
        /// Документ, удостоверяющий личность
        /// </summary>
        public virtual string PhysPersonDocument { get; set; }

        /// <summary>
        /// Заработная плата
        /// </summary>
        public virtual string PhysPersonSalary { get; set; }

        /// <summary>
        /// Семейное положение, кол-во иждивенцев
        /// </summary>
        public virtual string PhysPersonMaritalStatus { get; set; }
    }
}
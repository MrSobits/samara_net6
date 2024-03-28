namespace Bars.GkhGji.Regions.Tomsk.Entities
{
    using B4.DataAccess;
    using Enums;
    using GkhGji.Entities;

    /// <summary>
    /// Реквизиты физ.лица, привязанные к документу
    /// </summary>
    public class DocumentPhysInfo : BaseEntity
    {
        /// <summary>
        /// Документ
        /// </summary>
        public virtual DocumentGji Document { get; set; }

        /// <summary>
        /// Пол физ.лица
        /// </summary>
        public virtual TypeGender TypeGender { get; set; }

        /// <summary>
        /// Адрес
        /// </summary>
        public virtual string PhysAddress { get; set; }

        /// <summary>
        /// Место работы
        /// </summary>
        public virtual string PhysJob { get; set; }

        /// <summary>
        /// Должность
        /// </summary>
        public virtual string PhysPosition { get; set; }

        /// <summary>
        /// Дата, место рождения
        /// </summary>
        public virtual string PhysBirthdayAndPlace { get; set; }

        /// <summary>
        /// Документ, удостоверяющий личность (в Томске не используется, но вдруг понадобится)
        /// </summary>
        public virtual string PhysIdentityDoc { get; set; }

        /// <summary>
        /// З/п (в Томске не используется, но вдруг понадобится)
        /// </summary>
        public virtual string PhysSalary { get; set; }
    }
}
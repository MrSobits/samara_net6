namespace Bars.GkhGji.Regions.Tatarstan.Entities.TatarstanProtocolGji
{
    using Bars.B4.DataAccess;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Tatarstan.Enums;

    public class TatarstanDocumentWitness : BaseEntity
    {
        /// <summary>
        /// Документ
        /// </summary>
        public virtual DocumentGji DocumentGji { get; set; }

        /// <summary>
        /// Факт правонарушения удостоверяет
        /// </summary>
        public virtual WitnessType? WitnessType { get; set; }

        /// <summary>
        /// ФИО
        /// </summary>
        public virtual string Fio { get; set; }

        /// <summary>
        /// Фактический адрес проживания
        /// </summary>
        public virtual string FactAddress { get; set; }

        /// <summary>
        /// Телефон
        /// </summary>
        public virtual string Phone { get; set; }
    }
}

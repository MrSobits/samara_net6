namespace Bars.GkhGji.Regions.Tatarstan.Entities.Resolution
{
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Entities.Dict;
    using Bars.GkhGji.Enums;

    public class TatarstanProtocolMvd : ProtocolMvd
    {
        /// <summary>
        /// Фамилия
        /// </summary>
        public virtual string SurName { get; set; }

        /// <summary>
        /// Имя
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Отчество
        /// </summary>
        public virtual string Patronymic { get; set; }

        /// <summary>
        /// Гражданство
        /// </summary>
        public virtual Citizenship Citizenship { get; set; }

        /// <summary>
        /// Тип гражданства
        /// </summary>
        public virtual CitizenshipType? CitizenshipType { get; set; }

        /// <summary>
        /// Комментарий
        /// </summary>
        public virtual string Commentary { get; set; }
    }
}
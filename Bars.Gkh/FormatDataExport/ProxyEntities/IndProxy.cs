namespace Bars.Gkh.FormatDataExport.ProxyEntities
{
    using System;

    using Bars.B4.DataModels;
    using Bars.Gkh.Enums;

    /// <summary>
    /// Физические лица
    /// </summary>
    public class IndProxy : IHaveId
    {
        /// <summary>
        /// 1. Уникальный код физ.лица в системе отправителя
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 2. Фамилия
        /// </summary>
        public string Surname { get; set; }

        /// <summary>
        /// 3. Имя
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// 4. Отчество
        /// </summary>
        public string SecondName { get; set; }

        /// <summary>
        /// 5. Пол
        /// </summary>
        public Gender? Gender { get; set; }

        /// <summary>
        /// 6. Дата рождения
        /// </summary>
        public DateTime? BirthDate { get; set; }

        /// <summary>
        /// 7. Документ, удостоверяющий личность
        /// </summary>
        public string IdentityType { get; set; }

        /// <summary>
        /// 8. СНИЛС
        /// </summary>
        public string SnilsNumber { get; set; }

        /// <summary>
        /// 9. Серия документа
        /// </summary>
        public string IdentitySerial { get; set; }

        /// <summary>
        /// 10. Номер документа
        /// </summary>
        public string IdentityNumber { get; set; }

        /// <summary>
        /// 11. Дата выдачи документа
        /// </summary>
        public DateTime? DateDocumentIssuance { get; set; }

        /// <summary>
        /// 12. Место рождения
        /// </summary>
        public string BirthPlace { get; set; }

        /// <summary>
        /// 13. ИНН
        /// </summary>
        public string Inn { get; set; }
    }
}

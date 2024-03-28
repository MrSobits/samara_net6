namespace Bars.Gkh.Modules.ClaimWork.Entities
{
    using B4.DataAccess;
    using B4.Modules.FIAS;
    using Enums;
    using Gkh.Entities;

    /// <summary>
    /// Учреждения в судебной практике (JurisprudenceInstitution)
    /// </summary>
    public class JurInstitution : BaseEntity
    {
        /// <summary>
        /// Тип учреждения в судебной практике
        /// </summary>
        public virtual JurInstitutionType JurInstitutionType { get; set; }

        /// <summary>
        /// Тип суда
        /// </summary>
        public virtual CourtType CourtType { get; set; }

        /// <summary>
        /// Муниципальное образование
        /// </summary>
        public virtual Municipality Municipality { get; set; }

        /// <summary>
        /// Полное наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Краткое наименование
        /// </summary>
        public virtual string ShortName { get; set; }

        /// <summary>
        /// Aдрес ФИАС
        /// </summary>
        public virtual FiasAddress FiasAddress { get; set; }

        /// <summary>
        /// Адрес
        /// </summary>
        public virtual string Address { get; set; }

        /// <summary>
        /// Адрес
        /// </summary>
        public virtual string OutsideAddress { get; set; }

        /// <summary>
        /// Почтовый индекс
        /// </summary>
        public virtual string PostCode { get; set; }

        /// <summary>
        /// Телефон
        /// </summary>
        public virtual string Phone { get; set; }

        /// <summary>
        /// Электронная почта
        /// </summary>
        public virtual string Email { get; set; }

        /// <summary>
        /// Сайт
        /// </summary>
        public virtual string Website { get; set; }

        /// <summary>
        /// Судья - Должность
        /// </summary>
        public virtual string JudgePosition { get; set; }

        /// <summary>
        /// Судья - Фамилия
        /// </summary>
        public virtual string JudgeSurname { get; set; }

        /// <summary>
        /// Судья - Имя
        /// </summary>
        public virtual string JudgeName { get; set; }

        /// <summary>
        /// Судья - Отчество
        /// </summary>
        public virtual string JudgePatronymic { get; set; }

        /// <summary>
        /// Судья - Фамилия и инициалы
        /// </summary>
        public virtual string JudgeShortFio { get; set; }

        /// <summary>
        /// Код
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// Текст в заголовке печатной формы
        /// </summary>
        public virtual string HeaderText { get; set; }
    }
}
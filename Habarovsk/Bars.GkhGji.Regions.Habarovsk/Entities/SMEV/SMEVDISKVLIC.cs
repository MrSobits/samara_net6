namespace Bars.GkhGji.Regions.Habarovsk.Entities
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using Enums;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Enums.SMEV;
    using System;

    public class SMEVDISKVLIC : BaseEntity
    {
        /// <summary>
        /// Инициатор запроса
        /// </summary>
        public virtual Inspector Inspector { get; set; }

        /// <summary>
        /// дата запроса
        /// </summary>
        public virtual DateTime CalcDate { get; set; }

        /// <summary>
        /// ИдЗапрос
        /// </summary>
        public virtual string RequestId { get; set; }

        /// <summary>
        /// дата рождения
        /// </summary>
        public virtual DateTime BirthDate { get; set; }

        /// <summary>
        /// Место рождения
        /// </summary>
        public virtual string BirthPlace { get; set; }

        /// <summary>
        /// Ф
        /// </summary>
        public virtual string FamilyName { get; set; }

        /// <summary>
        /// И
        /// </summary>
        public virtual string FirstName { get; set; }

        /// <summary>
        /// О
        /// </summary>
        public virtual string Patronymic { get; set; }

        // остальное для ответа
        /// <summary>
        /// Дата формирования сведений из Реестра дисквалифицированных лиц 
        /// </summary>
        public virtual DateTime? FormDate { get; set; }

        // остальное для ответа
        /// <summary>
        /// Дата окончания срока дисквалификации
        /// </summary>
        public virtual DateTime? EndDisqDate { get; set; }

        /// <summary>
        /// Регистрационный номер записи в Реестре дисквалифицированных лиц
        /// </summary>
        public virtual string RegNumber { get; set; }

        /// <summary>
        /// Срок дисквалификации (дней)
        /// </summary>
        public virtual string DisqDays { get; set; }

        /// <summary>
        /// Срок дисквалификации (лет)
        /// </summary>
        public virtual string DisqYears { get; set; }

        /// <summary>
        /// Срок дисквалификации (месяц)
        /// </summary>
        public virtual string DisqMonths { get; set; }

        /// <summary>
        /// Ст. КоАП
        /// </summary>
        public virtual string Article { get; set; }

        /// <summary>
        /// Дата вынесения постановления
        /// </summary>
        public virtual DateTime? LawDate { get; set; }

        /// <summary>
        /// Наименование суда, вынесшего постановление о дисквалификации
        /// </summary>
        public virtual string LawName { get; set; }

        /// <summary>
        /// Номер дела
        /// </summary>
        public virtual string CaseNumber { get; set; }

        /// <summary>
        /// Ответ
        /// </summary>
        public virtual string Answer { get; set; }

        /// <summary>
        /// Статус запроса
        /// </summary>
        public virtual RequestState RequestState { get; set; }

        /// <summary>
        /// MessageId
        /// </summary>
        public virtual string MessageId { get; set; }
    }
}

namespace Bars.GkhGji.Regions.Nso.Entities
{
    using System;

    using Bars.Gkh.Entities.Dicts;
    using Bars.GkhGji.Entities.Dict;
    using Bars.GkhGji.Regions.Nso.Enums;

    /// <summary>
    /// Протокол ГЖИ для Томск (расширяется дополнительными полями)
    /// </summary>
    public class NsoProtocol : Bars.GkhGji.Entities.Protocol
    {
        /// <summary>
        /// Адрес регистрации (место жительства, телефон)
        /// </summary>
        public virtual string PersonRegistrationAddress { get; set; }

        /// <summary>
        /// Фактический адрес
        /// </summary>
        public virtual string PersonFactAddress { get; set; }

        /// <summary>
        /// Место работы
        /// </summary>
        public virtual string PersonJob { get; set; }
        /// <summary>
        /// Должность
        /// </summary>
        public virtual string PersonPosition { get; set; }
        /// <summary>
        /// Дата, место рождения
        /// </summary>
        public virtual string PersonBirthDatePlace { get; set; }
        /// <summary>
        /// Документ, удостоверяющий личность
        /// </summary>
        public virtual string PersonDoc { get; set; }
        /// <summary>
        /// Заработная плата
        /// </summary>
        public virtual string PersonSalary { get; set; }
        /// <summary>
        /// Семейное положение, кол-во иждивенцев
        /// </summary>
        public virtual string PersonRelationship { get; set; }

        /// <summary>
        /// Протокол - Реквизиты - В присуствии/отсутствии
        /// </summary>
        public virtual TypeRepresentativePresence TypePresence { get; set; }

        /// <summary>
        /// Представитель
        /// </summary>
        public virtual string Representative { get; set; }

        /// <summary>
        /// Вид и реквизиты основания
        /// </summary>
        public virtual string ReasonTypeRequisites { get; set; }

        /// <summary>
        /// Нарушения - Дата правонарушения
        /// </summary>
        public virtual DateTime? DateOfViolation { get; set; }

        /// <summary>
        /// Нарушения - Час правонарушения
        /// </summary>
        public virtual int? HourOfViolation { get; set; }

        /// <summary>
        /// Нарушения - Минута правонарушения
        /// </summary>
        public virtual int? MinuteOfViolation { get; set; }

        /// <summary>
        /// Нарушения - Наименование требования
        /// </summary>
        public virtual ResolveViolationClaim ResolveViolationClaim { get; set; }

        /// <summary>
        /// Правовое основание
        /// </summary>
        public virtual NormativeDoc NormativeDoc { get; set; }
    }
}
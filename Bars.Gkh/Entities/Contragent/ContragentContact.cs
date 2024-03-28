namespace Bars.Gkh.Entities
{
    using System;
    using System.Text;
    using B4.Utils;
    using Enums;

    /// <summary>
    /// Контактная информация по контрагенту
    /// </summary>
    public class ContragentContact : BaseGkhEntity
    {
        /// <summary>
        /// Контрагент
        /// </summary>
        public virtual Contragent Contragent { get; set; }

        /// <summary>
        /// Должность
        /// </summary>
        public virtual Position Position { get; set; }

        /// <summary>
        /// СНИЛС
        /// </summary>
        public virtual string Snils { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        public virtual string Email { get; set; }

        /// <summary>
        /// Телефон
        /// </summary>
        public virtual string Phone { get; set; }

        /// <summary>
        /// Примечание
        /// </summary>
        public virtual string Annotation { get; set; }

        /// <summary>
        /// Пол
        /// </summary>
        public virtual Gender Gender { get; set; }

        /// <summary>
        /// Дата рождения
        /// </summary>
        public virtual DateTime? BirthDate { get; set; }

        /// <summary>
        /// Дата начала работы
        /// </summary>
        public virtual DateTime? DateStartWork { get; set; }

        /// <summary>
        /// Дата окончания работы
        /// </summary>
        public virtual DateTime? DateEndWork { get; set; }

        /// <summary>
        /// Имя
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Фамилия
        /// </summary>
        public virtual string Surname { get; set; }

        /// <summary>
        /// Отчество
        /// </summary>
        public virtual string Patronymic { get; set; }

        /// <summary>
        /// ФИО
        /// </summary>
        public virtual string FullName { get; set; }

        /// <summary>
        /// Дата приказа
        /// </summary>
        public virtual DateTime? OrderDate { get; set; }

        /// <summary>
        /// Дата выдачи паспорта
        /// </summary>
        public virtual DateTime? FLDocIssuedDate { get; set; }


        /// <summary>
        /// серия паспорта
        /// </summary>
        public virtual string FLDocSeries { get; set; }

        /// <summary>
        /// Номер паспорта
        /// </summary>
        public virtual string FLDocNumber { get; set; }

        /// <summary>
        /// Кем выдан
        /// </summary>
        public virtual string FLDocIssuedBy { get; set; }

        /// <summary>
        /// Наименование приказа
        /// </summary>
        public virtual string OrderName { get; set; }

        /// <summary>
        /// Номер приказа
        /// </summary>
        public virtual string OrderNum { get; set; }


        /// <summary>
        /// Имя, родительный падеж
        /// </summary>
        public virtual string NameGenitive { get; set; }

        /// <summary>
        /// Фамилия, родительский падеж
        /// </summary>
        public virtual string SurnameGenitive { get; set; }

        /// <summary>
        /// Отчество, родительский падеж
        /// </summary>
        public virtual string PatronymicGenitive { get; set; }


        /// <summary>
        /// Имя, Дательный падеж
        /// </summary>
        public virtual string NameDative { get; set; }

        /// <summary>
        /// Фамилия, Дательный падеж
        /// </summary>
        public virtual string SurnameDative { get; set; }

        /// <summary>
        /// Отчество, Дательный падеж
        /// </summary>
        public virtual string PatronymicDative { get; set; }


        /// <summary>
        /// Имя, Винительный падеж
        /// </summary>
        public virtual string NameAccusative { get; set; }

        /// <summary>
        /// Фамилия, Винительный падеж
        /// </summary>
        public virtual string SurnameAccusative { get; set; }

        /// <summary>
        /// Отчество, Винительный падеж
        /// </summary>
        public virtual string PatronymicAccusative { get; set; }


        /// <summary>
        /// Имя, Творительный падеж
        /// </summary>
        public virtual string NameAblative { get; set; }

        /// <summary>
        /// Фамилия, Творительный падеж
        /// </summary>
        public virtual string SurnameAblative { get; set; }

        /// <summary>
        /// Отчество, Творительный падеж
        /// </summary>
        public virtual string PatronymicAblative { get; set; }


        /// <summary>
        /// Имя, Предложный падеж
        /// </summary>
        public virtual string NamePrepositional { get; set; }

        /// <summary>
        /// Фамилия, Предложный падеж
        /// </summary>
        public virtual string SurnamePrepositional { get; set; }

        /// <summary>
        /// Отчество, Предложный падеж
        /// </summary>
        public virtual string PatronymicPrepositional { get; set; }

        /// <summary>
        /// ГИС ЖКХ GUID
        /// </summary>
        public virtual string GisGkhGuid { get; set; }


        /// <summary>
        /// Получить "Фамилия И.О."
        /// </summary>
        /// <returns></returns>
        public virtual string GetShortFio()
        {
            var sb = new StringBuilder();

            sb.Append(this.Surname).Append(this.Surname.IsEmpty() ? null : " ");

            if (!this.Name.IsEmpty())
            {
                var firstNameChar = this.Name.Substring(0, 1).ToUpper();

                sb.Append(firstNameChar).Append('.');
            }

            if (!this.Patronymic.IsEmpty())
            {
                var firstNameChar = this.Patronymic.Substring(0, 1).ToUpper();

                sb.Append(firstNameChar).Append('.');
            }

            return sb.ToString();
        }

        /// <summary>
        /// Получить ФИО в родительном падеже
        /// Не использовать в запросах
        /// </summary>
        /// <returns></returns>
        public virtual string GetFioGenetive()
        {
            var sb = new StringBuilder();
            sb.Append(this.SurnameGenitive).Append(this.SurnameGenitive.IsEmpty() ? null : " ")
                .Append(this.NameGenitive).Append(this.NameGenitive.IsEmpty() ? null : " ")
                .Append(this.PatronymicGenitive);

            return sb.ToString();
        }

        /// <summary>
        /// Получить ФИО в родительном падеже
        /// Не использовать в запросах
        /// </summary>
        /// <returns></returns>
        public virtual string GetFioDative()
        {
            var sb = new StringBuilder();
            sb.Append(this.SurnameDative).Append(this.SurnameDative.IsEmpty() ? null : " ")
                .Append(this.NameDative).Append(this.NameDative.IsEmpty() ? null : " ")
                .Append(this.PatronymicDative);

            return sb.ToString();
        }
    }
}
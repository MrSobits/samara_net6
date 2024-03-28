namespace Bars.Gkh.LogMap
{
    using Bars.B4.Modules.NHibernateChangeLog;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Регистрация изменений полей сущности <see cref="ContragentContact"/>
    /// </summary>
    /// <remarks>
    /// Этот файл сгенерирован автоматичеески
    /// 21.08.2017 14:00:32
    /// УКАЖИТЕ ПОЛЯ ОТОБРАЖЕНИЯ ДЛЯ ССЫЛОЧНЫХ ТИПОВ
    /// </remarks>
    public class ContragentContactLogMap : AuditLogMap<ContragentContact>
    {
        public ContragentContactLogMap()
        {
            this.Name("Контакты");
            this.Description(x => x.Name);

            // DateTime?
            this.MapProperty(x => x.BirthDate, "BirthDate", "Дата рождения");
            this.MapProperty(x => x.DateStartWork, "DateStartWork", "Дата начала работы");
            this.MapProperty(x => x.DateEndWork, "DateEndWork", "Дата окончания работы");
            this.MapProperty(x => x.OrderDate, "OrderDate", "Дата приказа");

            // Gender
            this.MapProperty(x => x.Gender, "Gender", "Пол", x => x.GetDisplayName());

            // Position
            this.MapProperty(x => x.Position, "Position", "Должность", x => x?.Name);

            // string
            this.MapProperty(x => x.Snils, "Snils", "СНИЛС");
            this.MapProperty(x => x.Email, "Email", "Email");
            this.MapProperty(x => x.Phone, "Phone", "Телефон");
            this.MapProperty(x => x.Annotation, "Annotation", "Примечание");
            this.MapProperty(x => x.Name, "Name", "Имя");
            this.MapProperty(x => x.Surname, "Surname", "Фамилия");
            this.MapProperty(x => x.Patronymic, "Patronymic", "Отчество");
            this.MapProperty(x => x.FullName, "FullName", "ФИО");
            this.MapProperty(x => x.OrderName, "OrderName", "Наименование приказа");
            this.MapProperty(x => x.OrderNum, "OrderNum", "Номер приказа");
            this.MapProperty(x => x.NameGenitive, "NameGenitive", "Имя, родительный падеж");
            this.MapProperty(x => x.SurnameGenitive, "SurnameGenitive", "Фамилия, родительский падеж");
            this.MapProperty(x => x.PatronymicGenitive, "PatronymicGenitive", "Отчество, родительский падеж");
            this.MapProperty(x => x.NameDative, "NameDative", "Имя, Дательный падеж");
            this.MapProperty(x => x.SurnameDative, "SurnameDative", "Фамилия, Дательный падеж");
            this.MapProperty(x => x.PatronymicDative, "PatronymicDative", "Отчество, Дательный падеж");
            this.MapProperty(x => x.NameAccusative, "NameAccusative", "Имя, Винительный падеж");
            this.MapProperty(x => x.SurnameAccusative, "SurnameAccusative", "Фамилия, Винительный падеж");
            this.MapProperty(x => x.PatronymicAccusative, "PatronymicAccusative", "Отчество, Винительный падеж");
            this.MapProperty(x => x.NameAblative, "NameAblative", "Имя, Творительный падеж");
            this.MapProperty(x => x.SurnameAblative, "SurnameAblative", "Фамилия, Творительный падеж");
            this.MapProperty(x => x.PatronymicAblative, "PatronymicAblative", "Отчество, Творительный падеж");
            this.MapProperty(x => x.NamePrepositional, "NamePrepositional", "Имя, Предложный падеж");
            this.MapProperty(x => x.SurnamePrepositional, "SurnamePrepositional", "Фамилия, Предложный падеж");
            this.MapProperty(x => x.PatronymicPrepositional, "PatronymicPrepositional", "Отчество, Предложный падеж");
        }
    }
}

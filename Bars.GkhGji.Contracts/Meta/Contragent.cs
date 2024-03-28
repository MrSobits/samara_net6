namespace Bars.GkhGji.Contracts.Meta
{
    using System;
    using Bars.B4.Utils;

    public class Contragent
    {
        [Display("Наименование")]
        public string Name { get; set; }

        [Display("Наименование (Родит.падеж)")]
        public string Name_РодитПадеж { get; set; }

        [Display("Краткое наименование ")]
        public string ShortName { get; set; }

        [Display("Должность руководителя")]
        public string ManagerPosition { get; set; }

        [Display("Фамилия руководителя")]
        public string ManagerSurname { get; set; }

        [Display("Имя руководителя")]
        public string ManagerName { get; set; }

        [Display("Отчество руководителя")]
        public string ManagerMiddleName { get; set; }

        [Display("АдресФакт")]
        public string FactAddress { get; set; }

        [Display("Юридический адрес")]
        public string JurAddress { get; set; }

        [Display("ИНН")]
        public string Inn { get; set; }

        [Display("ОГРН")]
        public string Ogrn { get; set; }

        [Display("Дата регистрации")]
        public DateTime RegistrationDate { get; set; }
    }
}

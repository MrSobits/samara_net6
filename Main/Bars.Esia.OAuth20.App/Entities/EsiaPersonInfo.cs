namespace Bars.Esia.OAuth20.App.Entities
{
    using System;
    using System.Globalization;

    using Bars.B4.Utils;
    using Bars.Esia.OAuth20.App.Extensions;

    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Информация о пользователе из ЕСИА
    /// </summary>
    public class EsiaPersonInfo
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Полное имя (ФИО)
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Имя
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Фамилия
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Отчество
        /// </summary>
        public string MiddleName { get; set; }

        /// <summary>
        /// Дата рождения
        /// </summary>
        public DateTime? BirthDate { get; set; }

        /// <summary>
        /// Место рождения
        /// </summary>
        public string BirthPlace { get; set; }

        /// <summary>
        /// Пол
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// Признак подтвержденности учетной записи
        /// </summary>
        public bool Trusted { get; set; }

        /// <summary>
        /// Гражданство
        /// </summary>
        public string Citizenship { get; set; }

        /// <summary>
        /// СНИЛС
        /// </summary>
        public string Snils { get; set; }

        /// <summary>
        /// ИНН
        /// </summary>
        public string Inn { get; set; }

        public EsiaPersonInfo()
        {
        }

        public EsiaPersonInfo(JObject personInfo, string personId)
        {
            if (personInfo == null)
                return;

            this.Id = personId;
            this.FirstName = personInfo.GetPropertyValue("firstName");
            this.LastName = personInfo.GetPropertyValue("lastName");
            this.MiddleName = personInfo.GetPropertyValue("middleName");
            this.BirthPlace = personInfo.GetPropertyValue("birthPlace");
            this.Gender = personInfo.GetPropertyValue("gender");
            this.Citizenship = personInfo.GetPropertyValue("citizenship");
            this.Snils = personInfo.GetPropertyValue("snils");
            this.Inn = personInfo.GetPropertyValue("inn");
            this.Trusted = personInfo.GetPropertyValue("trusted").ToBool();

            if (!string.IsNullOrWhiteSpace(this.LastName))
            {
                this.Name = this.LastName;
                if (!string.IsNullOrWhiteSpace(this.FirstName))
                    this.Name = $"{this.Name} {this.FirstName}";
                if (!string.IsNullOrWhiteSpace(this.MiddleName))
                    this.Name = $"{this.Name} {this.MiddleName}";
            }

            if (DateTime.TryParse(personInfo.GetPropertyValue("birthDate"), new CultureInfo("ru-RU"), DateTimeStyles.AssumeLocal, out var birthDate))
            {
                this.BirthDate = birthDate;
            }
        }
    }
}
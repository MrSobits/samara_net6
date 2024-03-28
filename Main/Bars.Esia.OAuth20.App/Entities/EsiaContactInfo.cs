namespace Bars.Esia.OAuth20.App.Entities
{
    using System.Collections.Generic;

    using Bars.B4.Utils;
    using Bars.Esia.OAuth20.App.Enums;
    using Bars.Esia.OAuth20.App.Extensions;

    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Информация о контактных данных в ЕСИА
    /// </summary>
    public class EsiaContactInfo
    {
        /// <summary>
        /// Тип данных
        /// </summary>
        public ContactType ContactType { get; }

        /// <summary>
        /// Значение
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// Признак подтвержденности
        /// </summary>
        public bool Verified { get; }

        public EsiaContactInfo()
        {
        }

        public EsiaContactInfo(JObject contactInfo)
        {
            this.Value = contactInfo.GetPropertyValue("value");
            this.Verified = contactInfo.GetPropertyValue("vrfStu") == "VERIFIED";

            var contactTypeDict = new Dictionary<string, ContactType>
            {
                { "MBT", ContactType.Mobile },
                { "PHN", ContactType.Phone },
                { "EML", ContactType.Email },
                { "CPH", ContactType.WorkPhone },
                { "CEM", ContactType.WorkEmail },
            };

            this.ContactType = contactTypeDict.Get(contactInfo.GetPropertyValue("type"));
        }
    }
}
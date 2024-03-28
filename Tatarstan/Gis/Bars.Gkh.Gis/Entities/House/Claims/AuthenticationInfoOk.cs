namespace Bars.Gkh.Gis.Entities.House.Claims
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Информаци атутентификации
    /// </summary>
    [DataContract]
    public class AuthenticationInfoOk
    {
        /// <summary>
        /// Дата истечения
        /// </summary>
        [DataMember(Name="expired")]
        public DateTime Expired { get; set; }

        /// <summary>
        /// Токен
        /// </summary>
        [DataMember(Name = "token")]
        public string Token { get; set; }

        /// <summary>
        /// Ключ
        /// </summary>
        [DataMember(Name = "key")]
        public string Key { get; set; }
    }
}

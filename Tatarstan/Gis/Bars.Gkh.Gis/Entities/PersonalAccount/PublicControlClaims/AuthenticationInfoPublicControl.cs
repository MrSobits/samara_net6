namespace Bars.Gkh.Gis.Entities.PersonalAccount.PublicControlClaims
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Информация атутентификации
    /// </summary>
    [DataContract]
    public class AuthenticationInfoPublicControl
    {
        /// <summary>
        /// Токен
        /// </summary>
        [DataMember(Name = "data")]
        public DataProxy Data { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        [DataMember(Name = "status")]
        public string State { get; set; }
    }

    public class DataProxy
    {
        /// <summary>
        /// Id
        /// </summary>
        [DataMember(Name = "id")]
        public string Id { get; set; }

        /// <summary>
        /// Токен
        /// </summary>
        [DataMember(Name = "token")]
        public string Token { get; set; }
    }
}
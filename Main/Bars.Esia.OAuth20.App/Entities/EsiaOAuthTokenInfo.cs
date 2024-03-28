namespace Bars.Esia.OAuth20.App.Entities
{
    using System;
    using System.Globalization;

    using Bars.Esia.OAuth20.App.Extensions;

    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Расширенная информация о токене из ЕСИА
    /// </summary>
    public class EsiaOAuthTokenInfo : EsiaOAuthToken
    {
        /// <summary>
        /// Срок действия токена
        /// </summary>
        public new TimeSpan? ExpiresIn { get; }

        /// <summary>
        /// Время начала действия
        /// </summary>
        public DateTime? BeginDate { get; }

        /// <summary>
        /// Время окончания действия
        /// </summary>
        public DateTime? EndDate { get; }

        /// <summary>
        /// Время создания
        /// </summary>
        public DateTime? CreateDate { get; }

        /// <summary>
        /// Идентификатор токена
        /// </summary>
        public string Sid { get; }

        /// <summary>
        /// Идентификатор субъкта (учетной записи пользователя)
        /// </summary>
        public string SbjId { get; }

        public EsiaOAuthTokenInfo()
        {
        }

        public EsiaOAuthTokenInfo(string accessToken, string refreshToken, string expiresIn, JObject payload)
        {
            this.AccessToken = !string.IsNullOrEmpty(accessToken)
                ? accessToken
                : throw new ArgumentNullException(nameof(accessToken));

            this.RefreshToken = refreshToken;

            if (int.TryParse(expiresIn, NumberStyles.Integer, CultureInfo.InvariantCulture, out var intParseResult))
            {
                this.ExpiresIn = TimeSpan.FromSeconds(intParseResult);
            }

            if (payload == null)
                return;

            this.Sid = payload.GetPropertyValue("urn:esia:sid");
            this.SbjId = payload.GetPropertyValue("urn:esia:sbj_id");

            this.EndDate = payload.GetPropertyValue("exp").DateFromUnixSeconds();
            this.BeginDate = payload.GetPropertyValue("nbf").DateFromUnixSeconds();
            this.CreateDate = payload.GetPropertyValue("iat").DateFromUnixSeconds();
        }
    }
}
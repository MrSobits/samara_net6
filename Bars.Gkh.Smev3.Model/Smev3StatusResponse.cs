namespace Bars.Gkh.Smev3
{
    using System.Collections.Generic;

    /// <summary>
    /// Ответ с промежуточным статусом
    /// </summary>
    public class Smev3StatusResponse : Smev3Response
    {
        /// <summary>
        /// Код статуса запроса по виду сведений
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// Параметры
        /// </summary>
        public Dictionary<string, string> Parameters { get; set; }
    }
}
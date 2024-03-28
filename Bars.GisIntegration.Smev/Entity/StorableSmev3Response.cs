namespace Bars.GisIntegration.Smev.Entity
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.Smev3;

    /// <summary>
    /// Ответ от шлюза СМЭВ для хранения
    /// </summary>
    public class StorableSmev3Response : PersistentObject
    {
        /// <summary>
        /// Идентификатор запроса
        /// </summary>
        public virtual string requestGuid { get; set; }

        /// <summary>
        /// Объект ответа
        /// </summary>
        public virtual Smev3Response Response { get; set; }
    }
}
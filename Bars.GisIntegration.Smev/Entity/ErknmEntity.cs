namespace Bars.GisIntegration.Smev.Entity.ERKNM
{
    using System;

    /// <summary>
    /// Модель хранения информации о размещении объектов в ЕРКНМ
    /// </summary>
    public class ErknmEntity : BaseIntegrationEntity
    {
        /// <summary>
        /// Дата ответа
        /// </summary>
        public virtual DateTime ReplyDate { get; set; }
    }
}
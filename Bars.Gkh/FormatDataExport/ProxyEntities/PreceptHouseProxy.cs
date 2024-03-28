namespace Bars.Gkh.FormatDataExport.ProxyEntities
{
    using Bars.B4.DataModels;
    using Bars.Gkh.FormatDataExport.ProxySelectors;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Дома в предписании
    /// </summary>
    public class PreceptHouseProxy : IHaveId
    {
        /// <summary>
        /// Уникальный идентификатор файла
        /// </summary>
        public long Id => UniqueIdTool.GetId(this.PreceptId, this.DomId);

        /// <summary>
        /// 1. Уникальный идентификатор предписания
        /// </summary>
        public long PreceptId { get; set; }

        /// <summary>
        /// 2. Уникальный идентификатор дома
        /// </summary>
        [ProxyId(typeof(HouseProxy))]
        public long DomId { get; set; }
    }
}
namespace Bars.Gkh.FormatDataExport.ProxyEntities
{
    using System;

    using Bars.B4.DataModels;

    /// <summary>
    /// Комната
    /// </summary>
    [Obsolete("Не выгружаем", true)]
    public class RoomProxy : IHaveId
    {
        /// <summary>
        /// Id комнаты
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 2. Уникальный идентификатор Дома
        /// </summary>
        public long? RealityObjectId { get; set; }

        /// <summary>
        /// 3. Уникальный идентификатор Помещения
        /// </summary>
        public long? PremisesId { get; set; }

        /// <summary>
        /// 4. Признак комнаты коммунального заселения
        /// </summary>
        public bool IsCommunalRoom { get; set; }

        /// <summary>
        /// 5. Кадастровый номер в ГКН
        /// </summary>
        public string CadastralHouseNumber { get; set; }

        /// <summary>
        /// 7. Номер комнаты
        /// </summary>
        public string ChamberNum { get; set; }

        /// <summary>
        /// 8. Площадь комнаты
        /// </summary>
        public decimal? Area { get; set; }
    }
}
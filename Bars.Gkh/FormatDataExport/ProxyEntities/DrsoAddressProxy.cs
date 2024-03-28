namespace Bars.Gkh.FormatDataExport.ProxyEntities
{
    using Bars.B4.DataModels;

    /// <summary>
    /// Адреса объектов жилищного фонда к договору ресурсоснабжения
    /// </summary>
    public class DrsoAddressProxy : IHaveId
    {
        /// <inheritdoc />
        public long Id { get; set; }

        /// <summary>
        /// 2. Договор ресурсоснабжения
        /// </summary>
        public long? DrsoId { get; set; }

        /// <summary>
        /// 3. Дом
        /// </summary>
        public long? DomId { get; set; }

        /// <summary>
        /// 4. Номер  помещения (квартиры)
        /// </summary>
        public long? PremisesId { get; set; }

        /// <summary>
        /// 5. Номер комнаты
        /// </summary>
        public long? RoomId { get; set; }
    }
}
namespace Bars.Gkh.Regions.Voronezh.Map.Dicts
{
    using Bars.Gkh.Map;
    using Bars.Gkh.Regions.Voronezh.Entities.Dicts;

    public class ZonalInspectionPrefixMap : BaseImportableEntityMap<ZonalInspectionPrefix>
    {
        /// <summary>
        /// Имя таблицы
        /// </summary>
        public const string TableName = "ZONAL_INSPECTION_PREFIX";

        /// <summary>
        /// Префикс акта проверки
        /// </summary>
        public const string ActCheckPrefix = "ACTCHECK_PREFIX";

        /// <summary>
        /// Префикс протокола
        /// </summary>
        public const string ProtocolPrefix = "PROTOCOL_PREFIX";

        /// <summary>
        /// Префикс акта предписания
        /// </summary>
        public const string PrescriptionPrefix = "PRESCRIPTION_PREFIX";

        /// <summary>
        /// Зональная инспекция
        /// </summary>
        public const string ZonalInspection = "ZONAL_INSPECTION_ID";

        public ZonalInspectionPrefixMap()
            : base("Префикс зональной жилищной инспекции", ZonalInspectionPrefixMap.TableName)
        {
        }

        protected override void Map()
        {
            this.Property(x => x.ActCheckPrefix, "ActCheckPrefix").Column(ZonalInspectionPrefixMap.ActCheckPrefix);
            this.Property(x => x.ProtocolPrefix, "ProtocolPrefix").Column(ZonalInspectionPrefixMap.ProtocolPrefix);
            this.Property(x => x.PrescriptionPrefix, "PrescriptionPrefix").Column(ZonalInspectionPrefixMap.PrescriptionPrefix);
            this.Reference(x => x.ZonalInspection, "ZonalInspection").Column(ZonalInspectionPrefixMap.ZonalInspection).NotNull().Fetch();
        }
    }
}
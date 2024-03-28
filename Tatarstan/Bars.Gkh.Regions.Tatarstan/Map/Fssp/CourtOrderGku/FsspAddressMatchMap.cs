namespace Bars.Gkh.Regions.Tatarstan.Map.Fssp.CourtOrderGku
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Regions.Tatarstan.Entities.Fssp.CourtOrderGku;

    public class FsspAddressMatchMap : BaseEntityMap<FsspAddressMatch>
    {
        /// <inheritdoc />
        public FsspAddressMatchMap()
            : base(typeof(FsspAddressMatch).FullName, "FSSP_ADDRESS_MATCH")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Reference(x => x.UploadDownloadInfo, "Информация о загрузке файла").Column("FSSP_UPLOAD_DOWNLOAD_INFO_ID").Fetch();
            this.Reference(x => x.FsspAddress, "Адрес ФССП").Column("FSSP_ADDRESS_ID").Fetch();
        }
    }
}
namespace Bars.Gkh.Regions.Tatarstan.Map.Fssp.CourtOrderGku
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Regions.Tatarstan.Entities.Fssp.CourtOrderGku;

    public class UploadDownloadInfoMap : BaseEntityMap<UploadDownloadInfo>
    {
        /// <inheritdoc />
        public UploadDownloadInfoMap()
            : base(typeof(UploadDownloadInfo).FullName, "FSSP_UPLOAD_DOWNLOAD_INFO")
        {
        }

        protected override void Map()
        {
            this.Reference(x => x.DownloadFile, "Загруженный файл").Column("DOWNLOAD_FILE_ID").Fetch();
            this.Property(x => x.DateDownloadFile, "Дата загрузки файла").Column("DATE_DOWNLOAD_FILE");
            this.Reference(x => x.User, "Пользователь").Column("USER_ID").Fetch();
            this.Property(x => x.Status, "Статус загрузки").Column("STATUS");
            this.Reference(x => x.LogFile, "Файл лога загрузки").Column("LOG_FILE_ID").Fetch();
            this.Reference(x => x.UploadFile, "Выгруженный файл").Column("UPLOAD_FILE_ID").Fetch();
        }
    }
}
namespace Bars.Gkh.Regions.Tatarstan.Entities.Fssp.CourtOrderGku
{
    using Bars.B4.DataAccess;

    /// <summary>
    /// Сопоставление адреса из загруженного файла с адресом ФССП
    /// </summary>
    public class FsspAddressMatch : BaseEntity
    {
        /// <summary>
        /// Информация о загрузке файла
        /// </summary>
        public virtual UploadDownloadInfo UploadDownloadInfo { get; set; }

        /// <summary>
        /// Адрес ФССП
        /// </summary>
        public virtual FsspAddress FsspAddress { get; set; }
    }
}
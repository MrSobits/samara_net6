namespace Bars.GkhGji.Regions.BaseChelyabinsk.Entities
{
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Enums;
    public class LicenseActionFile : BaseEntity
    {
        /// <summary>
        /// Запрос к ВС ФНС
        /// </summary>
        public virtual LicenseAction LicenseAction { get; set; }

        /// <summary>
        ///Файл
        /// </summary>
        public virtual FileInfo FileInfo { get; set; }

        /// <summary>
        ///Файл
        /// </summary>
        public virtual string FileName { get; set; }

        /// <summary>
        ///Файл
        /// </summary>
        public virtual string SignedInfo { get; set; }
    }
}

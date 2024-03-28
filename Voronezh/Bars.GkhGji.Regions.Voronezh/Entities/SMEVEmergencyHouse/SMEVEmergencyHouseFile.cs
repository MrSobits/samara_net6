namespace Bars.GkhGji.Regions.Voronezh.Entities.SMEVEmergencyHouse
{
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Enums;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Enums.SMEV;
    public class SMEVEmergencyHouseFile : BaseEntity
    {
        /// <summary>
        /// Запрос 
        /// </summary>
        public virtual SMEVEmergencyHouse SMEVEmergencyHouse { get; set; }

        /// <summary>
        /// Тип запроса/ответа
        /// </summary>
        public virtual SMEVFileType SMEVFileType { get; set; }

        /// <summary>
        ///Файл
        /// </summary>
        public virtual FileInfo FileInfo { get; set; }
    }
}


namespace Bars.GkhGji.Regions.Chelyabinsk.Entities
{
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Enums.SMEV;

    public class ERKNMDictFile : BaseEntity
    {
        /// <summary>
        /// Запрос к ВС ГИС ЕРП
        /// </summary>
        public virtual ERKNMDict ERKNMDict { get; set; }

        /// <summary>
        /// Тип файла
        /// </summary>
        public virtual SMEVFileType SMEVFileType { get; set; }

        /// <summary>
        /// Файл
        /// </summary>
        public virtual FileInfo FileInfo { get; set; }
    }
}


namespace Sobits.GisGkh.Entities
{
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Enums;
    public class GisGkhRequestsFile : BaseEntity
    {
        /// <summary>
        /// Запрос ГИС ЖКХ
        /// </summary>
        public virtual GisGkhRequests GisGkhRequests { get; set; }

        /// <summary>
        /// Тип запроса/ответа
        /// </summary>
        public virtual GisGkhFileType GisGkhFileType { get; set; }

        /// <summary>
        ///Файл
        /// </summary>
        public virtual  FileInfo FileInfo { get; set; }
    }
}

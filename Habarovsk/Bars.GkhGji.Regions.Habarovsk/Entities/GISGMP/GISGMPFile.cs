﻿
namespace Bars.GkhGji.Regions.Habarovsk.Entities
{
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Enums;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Enums.SMEV;
    public class GisGmpFile : BaseEntity
    {
        /// <summary>
        /// Запрос к ВС ЕГРЮЛ
        /// </summary>
        public virtual GisGmp GisGmp { get; set; }

        /// <summary>
        /// Тип запроса/ответа
        /// </summary>
        public virtual SMEVFileType SMEVFileType { get; set; }

        /// <summary>
        ///Файл
        /// </summary>
        public virtual  FileInfo FileInfo { get; set; }
    }
}

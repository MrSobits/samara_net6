﻿namespace Bars.GkhGji.Regions.BaseChelyabinsk.Entities
{
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Enums.SMEV;
    using Enums;
    public class MVDPassportFile : BaseEntity
    {
        /// <summary>
        /// Запрос к ВС МВД
        /// </summary>
        public virtual MVDPassport MVDPassport { get; set; }

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

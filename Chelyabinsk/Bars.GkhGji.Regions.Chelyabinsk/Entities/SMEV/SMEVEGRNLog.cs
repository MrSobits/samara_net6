﻿namespace Bars.GkhGji.Regions.Chelyabinsk.Entities
{
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Enums;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Enums.SMEV;
    using Enums;
    public class SMEVEGRNLog : BaseEntity
    {
        /// <summary>
        /// Запрос к ВС МВД
        /// </summary>
        public virtual SMEVEGRN SMEVEGRN { get; set; }

        /// <summary>
        /// Тип запроса/ответа
        /// </summary>
        public virtual string OperationType { get; set; }

        /// <summary>
        /// Тип запроса/ответа
        /// </summary>
        public virtual string UserName { get; set; }

        /// <summary>
        /// Тип запроса/ответа
        /// </summary>
        public virtual string Login { get; set; }

        /// <summary>
        ///Файл
        /// </summary>
        public virtual  FileInfo FileInfo { get; set; }
    }
}

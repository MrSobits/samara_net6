namespace Bars.GkhDi.Entities
{
    using System;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Документы объекта недвижимости протоколы
    /// </summary>
    public class DocumentsRealityObjProtocol : BaseGkhEntity
    {
        /// <summary>
        /// Объект недвижимости
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }

        /// <summary>
        /// Раскрытие информации объекта недвижимости
        /// </summary>
        public virtual DisclosureInfoRealityObj DisclosureInfoRealityObj { get; set; }

        /// <summary>
        /// Год проверки
        /// </summary>
        public virtual int Year { get; set; }

        /// <summary>
        /// Номер документа
        /// </summary>
        public virtual string DocNum { get; set; }

        /// <summary>
        /// Дата документа
        /// </summary>
        public virtual DateTime? DocDate { get; set; }

        /// <summary>
        /// Файл
        /// </summary>
        public virtual FileInfo File { get; set; }
    }
}

namespace Bars.Gkh.Overhaul.Hmao.FormatDataExport.ProxyEntities
{
    using System;

    using Bars.B4.DataModels;
    using Bars.B4.Modules.FileStorage;

    /// <summary>
    /// Документы ПКР (pkrdoc.csv)
    /// </summary>
    public class PkrDocProxy : IHaveId
    {
        /// <summary>
        /// 1. Уникальный код документа КПР
        /// </summary>
        public long Id { get; set; }
        
        /// <summary>
        /// 2. Идентификатор программы
        /// </summary>
        public long PkrId { get; set; }

        /// <summary>
        /// 3. Вид документа
        /// </summary>
        public string DocType { get; set; }

        /// <summary>
        /// 4. Наименование документа
        /// </summary>
        public string DocName { get; set; }

        /// <summary>
        /// 5. Номер документа
        /// </summary>
        public string DocNum { get; set; }

        /// <summary>
        /// 6. Дата документа
        /// </summary>
        public DateTime? DocDate { get; set; }

        /// <summary>
        /// 7. Орган, принявший документ
        /// </summary>
        public string AcceptedGoverment { get; set; }

        /// <summary>
        /// 8. Статус документа
        /// </summary>
        public int DocState { get; set; }

        /// <summary>
        /// Файлы документов ПКР (pkrdocfiles.csv)
        /// 2. Идентификатор файла
        /// </summary>
        public FileInfo File { get; set; }
    }
}
namespace Bars.GkhDi.Import
{
    using System.Collections.Generic;

    using Bars.Gkh.Import;
    using Bars.GkhDi.Import.Data;

    public class ImportParams
    {
        /// <summary>
        /// id периода в который импортируем
        /// </summary>
        public long PeriodDiId { get; set; }

        /// <summary>
        /// инн организации для которой импортируем
        /// </summary>
        public string Inn { get; set; }

        /// <summary>
        /// Имя zip архива
        /// </summary>
        public string zipName { get; set; }

        /// <summary>
        /// id домов с которыми есть договора в данном периоде
        /// </summary>
        public List<long> RealityObjectIds { get; set; }

        /// <summary>
        /// Считанные данные
        /// </summary>
        public SectionsData SectionData { get; set; }

        /// <summary>
        /// Лог
        /// </summary>
        public ILogImport LogImport;

        /// <summary>
        /// id и адрес дома, по коду Ерц
        /// </summary>
        public Dictionary<string, RealObjImportInfo> RealObjsImportInfo { get; set; }
    }

    public class RealObjImportInfo
    {
        /// <summary>
        /// id дома
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// адрес дома
        /// </summary>
        public string FiasAddressName { get; set; }
    }
}

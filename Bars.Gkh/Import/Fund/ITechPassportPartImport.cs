namespace Bars.Gkh.Import.Fund
{
    using System.Collections.Generic;
    using System.IO;

    using Bars.B4;
    using Bars.Gkh.Import;

    public interface ITechPassportPartImport
    {
        /// <summary>
        /// The import.
        /// </summary>
        /// <param name="memoryStream">
        /// Поток
        /// </param>
        /// <param name="logImport">
        /// Лог
        /// </param>
        /// <param name="robjectIdByFederalNumDict">
        /// словарь соответствия федерального номера и идентификатора жилого дома в нашей системе
        /// </param>
        /// <param name="tehPassportIdByRobjectIdDict">
        /// словарь соответствия идентификатора жилого дома и его технического паспорта
        /// </param>
        /// <param name="existingDataDict">
        /// словарь существующих значений техпаспорта
        /// id дома -> FormCode -> CellCode -> ExistingTechPassportValueProxy
        /// </param>
        /// <param name="replaceData">
        /// Заменять текущие данные
        /// </param>
        void Import(
            MemoryStream memoryStream,
            ILogImport logImport,
            Dictionary<string, long> robjectIdByFederalNumDict,
            Dictionary<long, long> tehPassportIdByRobjectIdDict,
            Dictionary<long, Dictionary<string, Dictionary<string, ExistingTechPassportValueProxy>>> existingDataDict,
            bool replaceData);

        string Code { get; }
    }
}
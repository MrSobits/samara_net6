namespace Bars.Gkh.Regions.Tatarstan.FormatDataExport.Domain
{
    using Bars.B4;

    /// <summary>
    /// Интерфейс сервиса экспорта в формате 4.0
    /// </summary>
    public interface IFormat40DataExportService
    {
        /// <summary>
        /// Метод отправки ТехПаспорта в ГИС ЖКХ
        /// </summary>
        IDataResult SendTechPassport(BaseParams baseParams);

        /// <summary>
        /// Метод отправки договора/устава в ГИС ЖКХ
        /// </summary>
        IDataResult SendDuUstav(BaseParams baseParams);
    }
}
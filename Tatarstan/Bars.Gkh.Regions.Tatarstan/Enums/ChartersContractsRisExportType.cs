namespace Bars.Gkh.Regions.Tatarstan.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип выгрузки ДУ/уставов в РИС ЖКХ
    /// </summary>
    public enum ChartersContractsRisExportType
    {
        /// <summary>
        /// Выгрузка всех договоров управления и уставов
        /// </summary>
        [Display("Выгрузка всех договоров управления и уставов")]
        AllChartersContracts = 1,
        
        /// <summary>
        /// Выгрузка измененных договоров управления и уставов
        /// </summary>
        [Display("Выгрузка измененных договоров управления и уставов")]
        ChangedChartersContracts = 2
    }
}
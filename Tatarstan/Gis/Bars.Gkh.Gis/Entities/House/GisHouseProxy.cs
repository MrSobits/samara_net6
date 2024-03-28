namespace Bars.Gkh.Gis.Entities.House
{
    using ImportIncrementalData.LoadFromOtherSystems;

    /// <summary>
    /// Дом
    /// </summary>
    public class GisHouseProxy
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public virtual long Id { get; set; }

        /// <summary>
        /// "Хранилище банка данных"
        /// </summary>
        public virtual DataBankStogare DataBankStorage { get; set; }
    }
}

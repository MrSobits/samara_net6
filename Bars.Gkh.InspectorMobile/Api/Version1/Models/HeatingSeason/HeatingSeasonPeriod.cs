namespace Bars.Gkh.InspectorMobile.Api.Version1.Models.HeatingSeason
{
    /// <summary>
    /// Период по отопительному сезону
    /// </summary>
    public class HeatingSeasonPeriod
    {
        /// <summary>
        /// Уникальный идентификатор периода по отопительному сезону
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Наименование по отопительному сезону
        /// </summary>
        public string Name { get; set; }
    }
}
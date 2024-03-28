namespace Bars.GkhGji.Regions.Stavropol.Entities
{
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Расширение сущности "Постановление прокуратуры" для Ставрополя
    /// </summary>
    public class StavropolResolPros : ResolPros
    {
        /// <summary>
        /// Должностное лицо
        /// </summary>
        public virtual string Official { get; set; }
    }
}
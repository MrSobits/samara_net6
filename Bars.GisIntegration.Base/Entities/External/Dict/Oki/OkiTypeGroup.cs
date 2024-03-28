namespace Bars.GisIntegration.Base.Entities.External.Dict.Oki
{
    using Bars.B4.DataAccess;

    /// <summary>
    /// Группы видов ОКИ
    /// </summary>
    public class OkiTypeGroup : BaseEntity
    {
        /// <summary>
        /// наименование
        /// </summary>
        public virtual string Name { get; set; }
    }
}

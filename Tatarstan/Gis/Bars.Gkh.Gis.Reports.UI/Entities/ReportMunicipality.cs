namespace Bars.Gkh.Gis.Reports.UI.Entities
{
    using B4.DataAccess;

    /// <summary>
    /// Список мануципальных образований
    /// </summary>
    public class ReportMunicipality : PersistentObject
    {
        /// <summary>
        /// Название муниципального образования
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Район
        /// </summary>
        public virtual ReportArea Area { get; set; }
    }
}

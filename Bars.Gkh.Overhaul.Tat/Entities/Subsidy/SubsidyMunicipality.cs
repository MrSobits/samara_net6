namespace Bars.Gkh.Overhaul.Tat.Entities
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;

    /// <summary>
    ///  Субсидия для муниципального образования
    /// </summary>
    public class SubsidyMunicipality : BaseEntity
    {
        /// <summary>
        /// МО
        /// </summary>
        public virtual Municipality Municipality { get; set; }
    }
}
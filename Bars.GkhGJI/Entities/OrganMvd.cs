namespace Bars.GkhGji.Entities
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// Справочник "Органы МВД"
    /// </summary>
    public class OrganMvd : BaseGkhEntity
    {
        ///// <summary>
        ///// Муниципальное образование
        ///// </summary>
        //public virtual Municipality Municipality { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Код
        /// </summary>
        public virtual int? Code { get; set; }
    }
}
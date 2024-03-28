using Bars.B4.DataAccess;

namespace Bars.Gkh.Entities
{

    /// <summary>
    /// Связь контрагента ПИР с МО
    /// </summary>
    public class ContragentClwMunicipality : BaseEntity
    {
        /// <summary>
        /// Контрагент ПИР
        /// </summary>
        public virtual ContragentClw ContragentClw { get; set; }

        /// <summary>
        /// МО
        /// </summary>
        public virtual Municipality Municipality { get; set; }
    }
}
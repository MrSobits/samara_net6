using Bars.B4.DataAccess;
using Bars.Gkh.Entities;

namespace Bars.GkhGji.Regions.Chelyabinsk.Entities
{
    /// <summary>
    /// Должностное лицо заявки на лицензию
    /// </summary>
    public class LicenseReissuancePerson : BaseEntity
    {
        /// <summary>
        /// Заявка на лицензию
        /// </summary>
        public virtual LicenseReissuance LicenseReissuance { get; set; }

        /// <summary>
        /// Должностное лицо
        /// </summary>
        public virtual Person Person { get; set; }

    }
}

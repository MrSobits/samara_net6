
namespace Bars.GkhGji.Entities
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;

    /// <summary>
    /// МКД в заявлении
    /// </summary>
    public class MKDLicRequestRealityObject : BaseEntity
    {
        /// <summary>
        /// MKDLicRequest
        /// </summary>
        public virtual MKDLicRequest MKDLicRequest { get; set; }

        /// <summary>
        ///Файл
        /// </summary>
        public virtual  RealityObject RealityObject { get; set; }
    }
}

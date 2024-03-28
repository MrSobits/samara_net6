
using Bars.B4.DataAccess;

namespace Bars.Gkh.Gis.Entities.Kp50
{
  
    /// <summary>
    /// Ссылки на расположения домов
    /// </summary>
    public class BilHouseCodeStorage : PersistentObject
    {
        /// <summary>
        /// Код дома биллинга
        /// </summary>
        public virtual long BillingHouseCode { get; set; }


        /// <summary>
        /// Код префикса схемы в БД биллинга
        /// </summary>
        public virtual BilDictSchema Schema { get; set; }
    }
}

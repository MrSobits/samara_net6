using Bars.B4.DataAccess;
using Bars.Gkh.Gis.Enum;

namespace Bars.Gkh.Gis.Entities.Kp50
{
    /// <summary>
    /// Подключение к БД биллинга
    /// </summary>
    public class BilConnection : BaseEntity
    {
        /// <summary>
        /// Url приложения
        /// </summary>
        public virtual string AppUrl { get; set; }

        /// <summary>
        /// Подключение к БД биллинга
        /// </summary>
        public virtual string Connection { get; set; }

        /// <summary>
        /// Тип подключения
        /// </summary>
        public virtual ConnectionType ConnectionType { get; set; }
    }
}

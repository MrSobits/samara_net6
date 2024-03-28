using Bars.B4.DataAccess;

namespace Bars.Gkh.Entities
{
    /// <summary>
    /// Настройки для выгрузки в Клиент-СБербанк
    /// </summary>
    public class ASSberbankClient : BaseEntity
    {
        /// <summary>
        /// Код клиента в АС "Клиент-Сбербанк"
        /// </summary>
        public virtual string ClientCode { get; set; }
    }
}

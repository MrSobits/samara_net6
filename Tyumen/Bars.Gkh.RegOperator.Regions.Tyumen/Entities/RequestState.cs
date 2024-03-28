using Bars.B4.DataAccess;
using Bars.Gkh.Entities;
using Bars.B4.Modules.FileStorage;

namespace Bars.Gkh.RegOperator.Regions.Tyumen.Entities
{
    public class RequestState : BaseEntity
    {
        /// <summary>
        /// Id пользователя, запрашивающего доступ
        /// </summary>
        public virtual long UserId { get; set; }

        /// <summary>
        /// Имя пользователя, запрашивающего доступ
        /// </summary>
        public virtual string UserName { get; set; }

        /// <summary>
        /// Дом, для которого запрашивается доступ
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }

        /// <summary>
        /// Основание
        /// </summary>
        public virtual FileInfo File { get; set; }

        /// <summary>
        /// Примечание
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Уведомлен отправитель запроса
        /// </summary>
        public virtual bool NotifiedUser { get; set; }

        /// <summary>
        /// Уведомлен поучатель запроса
        /// </summary>
        public virtual bool NotifiedPerson { get; set; }

    }
}

using Bars.B4.DataAccess;
using Bars.Gkh.Entities;
using Bars.B4.Modules.FileStorage;
using Bars.Gkh.RegOperator.Regions.Tyumen.Enums;

namespace Bars.Gkh.RegOperator.Regions.Tyumen.Entities
{
    public class RequestStatePerson : BaseEntity
    {
        /// <summary>
        /// Электронная почта
        /// </summary>
        public virtual string Email { get; set; }

        /// <summary>
        /// Имя пользователя, разрешающего доступ
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Примечание
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Должность
        /// </summary>
        public virtual string Position { get; set; }

        public virtual RequestStatePersonEnum Status { get; set; }
    }
}

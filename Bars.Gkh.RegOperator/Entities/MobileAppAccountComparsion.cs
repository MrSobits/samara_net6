using Bars.Gkh.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bars.Gkh.Enums;
using Bars.B4.Modules.FileStorage;
using Bars.Gkh.RegOperator.Enums;

namespace Bars.Gkh.RegOperator.Entities
{
    public class MobileAppAccountComparsion : BaseImportableEntity
    {
        /// <summary>
        /// Л/С в системе
        /// </summary>
        public virtual string PersonalAccountNumber { get; set; }

        /// <summary>
        /// Л/С в мобильном приложении
        /// </summary>
        public virtual string MobileAccountNumber { get; set; }

        /// <summary>
        /// Л/С внешний
        /// </summary>
        public virtual string ExternalAccountNumber { get; set; }

        /// <summary>
        /// Дата захода юзером в приложение
        /// </summary>
        public virtual DateTime OperatinDate { get; set; }

        /// <summary>
        /// ФИО владельца ЛС в мобильном приложении
        /// </summary>
        public virtual string MobileAccountOwnerFIO { get; set; }

        /// <summary>
        /// ФИО владельца ЛС в системе
        /// </summary>
        public virtual string PersonalAccountOwnerFIO { get; set; }

        /// <summary>
        /// ФИО сотрудника ФКР
        /// </summary>
        public virtual string FkrUserFio { get; set; }

        /// <summary>
        /// Признак "просмотра"
        /// </summary>
        public virtual bool IsViewed { get; set; }

        /// <summary>
        /// Признак "отработанно"
        /// </summary>
        public virtual bool IsWorkOut { get; set; }

        /// <summary>
        /// Решение по сопоставлению
        /// </summary>
        public virtual MobileAccountComparsionDecision DecisionType { get; set; }

    }
}

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

namespace Bars.Gkh.RegOperator.Map
{
    using Bars.Gkh.Map;
    using Bars.Gkh.RegOperator.Entities;
    class MobileAccountComparsionMap : BaseImportableEntityMap<MobileAppAccountComparsion>
    {
        public MobileAccountComparsionMap() :
                base("Сравнение лицевых счётов", "REGOP_MOBILE_APP_COMPARSION")
        {
        }

        protected override void Map()
        {
            Property(x => x.PersonalAccountNumber, "Номер Л/С в системе").Column("PERSONALACCOUNT_NUM");
            Property(x => x.MobileAccountNumber, "Номер Л/С в приложении").Column("MOBILEACCOUNT_NUM");
            Property(x => x.ExternalAccountNumber, "Номер Л/С внешний").Column("EXTERNALACCOUNT_NUM");
            Property(x => x.OperatinDate, "Дата входа").Column("OPERATION_DATE");
            Property(x => x.PersonalAccountOwnerFIO, "ФИО пользователя в системе").Column("FIO_ACCOUNT_OWNER");
            Property(x => x.MobileAccountOwnerFIO, "ФИО пользователя в приложении").Column("FIO_MOBILE_APP_ACCOUNT_OWNER");
            Property(x => x.FkrUserFio, "ФИО сотрудника ФКР").Column("FIO_SYSTEM_USER");
            Property(x => x.IsViewed, "Признак просмотра").Column("IS_VIEWED");
            Property(x => x.IsWorkOut, "Признак отработки").Column("IS_WORKED_OUT");
            Property(x => x.DecisionType, "Решение").Column("DECISION_TYPE");
        }
    }
}

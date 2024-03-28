using Bars.B4.Modules.NHibernateChangeLog;
using Bars.Gkh.RegOperator.Entities;

namespace Bars.Gkh.RegOperator.LogMap
{
    using Bars.B4.Utils;

    public class FundFormationContractLogMap : AuditLogMap<FundFormationContract>
    {
        public FundFormationContractLogMap()
        {
            Name("Реестр договоров на формирование фонда");

            Description(x => x.ReturnSafe(y => y.LongTermPrObject.RealityObject.Address));

            MapProperty(x => x.LongTermPrObject.RealityObject, "RealityObject", "Объект капитального ремонта", x => x.Return(y => y.Address));
            MapProperty(x => x.RegOperator.Contragent, "Contragent", "Региональный оператор", x => x.Return(y => y.Name));
            MapProperty(x => x.DateStart, "DateStart", "Дата начала");

            MapProperty(x => x.TypeContract, "TypeContract", "Тип договора");
            MapProperty(x => x.ContractNumber, "ContractNumber", "Номер");

            MapProperty(x => x.DateStart, "DateStart", "Дата начала");
            MapProperty(x => x.DateEnd, "DateEnd", "Дата окончания");
            MapProperty(x => x.ContractDate, "ContractDate", "Дата договора");

            MapProperty(x => x.ContractDate, "ContractDate", "Дата договора");
        }
    }
}
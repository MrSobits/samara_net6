using Bars.B4.Utils;

namespace Bars.GkhCr.LogMap
{
    using Bars.B4.Modules.NHibernateChangeLog;
    using Bars.GkhCr.Entities;

    public class BuildContractLogMap : AuditLogMap<BuildContract>
    {
        public BuildContractLogMap()
        {
            Name("Договор подряда КР");

            Description(x => x.ReturnSafe(y => y.ObjectCr.RealityObject.Address));

            MapProperty(x => x.State, "State", "Статус");
            MapProperty(x => x.DocumentNum, "DocumentNum", "Номер документа");
            MapProperty(x => x.Inspector, "Inspector", "Инспектор", x => x.Return(y => y.Fio, ""));
            MapProperty(x => x.Builder.Contragent, "Contragent", "Подрядчик", x => x.Return(y => y.Name));
            MapProperty(x => x.TypeContractBuild, "TypeContractBuild", "Тип договора");
            MapProperty(x => x.DateStartWork, "DateStartWork", "Дата начала работ");
            MapProperty(x => x.DateEndWork, "DateEndWork", "Дата окончания работ");
            MapProperty(x => x.DateInGjiRegister, "DateInGjiRegister", "Договор внесен в реестр ГЖИ");
            MapProperty(x => x.DocumentDateFrom, "DocumentDateFrom", "Дата от (документ)");
            MapProperty(x => x.DateCancelReg, "DateCancelReg", "Дата отклонения от регистрации");
            MapProperty(x => x.DateAcceptOnReg, "DateAcceptOnReg", " Дата принятия на регистрацию");
            MapProperty(x => x.DocumentName, "DocumentName", "Документ");
            MapProperty(x => x.Description, "Description", "Описание");
            MapProperty(x => x.Sum, "Sum", "Сумма договора подряда");
            MapProperty(x => x.ProtocolName, "ProtocolName", "Протокол");
            MapProperty(x => x.ProtocolNum, "ProtocolNum", "Номер протокола");
            MapProperty(x => x.ProtocolDateFrom, "ProtocolDateFrom", " Дата от (протокол)");                                    
        }
    }
}

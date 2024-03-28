namespace Bars.GkhCr.LogMap
{
    using Bars.B4.Modules.NHibernateChangeLog;
    using Bars.B4.Utils;
    using Bars.GkhCr.Entities;

    public class ContractCrLogMap : AuditLogMap<ContractCr>
    {
         public ContractCrLogMap()
        {
            Name("Договор КР");

            Description(x => x.ReturnSafe(y => y.ObjectCr.RealityObject.Address));

            MapProperty(x => x.State, "State", "Статус");
            MapProperty(x => x.DocumentNum, "DocumentNum", "Номер документа");
            MapProperty(x => x.Contragent, "DocumentNum", "Номер документа");
            MapProperty(x => x.Contragent, "Contragent", "Контрагент", x => x.Return(y => y.Name));
            MapProperty(x => x.FinanceSource, "FinanceSource", "Разрез финансирования", x => x.Return(y => y.Name));
            MapProperty(x => x.SumContract, "SumContract", "Сумма договора");
            MapProperty(x => x.File, "File", "Файл", x => x.Return(y => y.Name));
        }
    }
}

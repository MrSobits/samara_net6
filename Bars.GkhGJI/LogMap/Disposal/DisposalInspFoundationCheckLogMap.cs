namespace Bars.GkhGji.LogMap.Disposal
{
    using System;

    using Bars.B4.Modules.NHibernateChangeLog;
    using Bars.GkhGji.Entities;

    public class DisposalInspFoundCheckNormDocItemLogMap : AuditLogMap<DisposalInspFoundCheckNormDocItem>
    {
        public DisposalInspFoundCheckNormDocItemLogMap()
        {
            this.Name("Распоряжение - НПА проверки");
            this.Description(x => x?.DisposalInspFoundationCheck?.Disposal?.DocumentNumber ?? string.Empty);

            this.MapProperty(x => x.DisposalInspFoundationCheck.InspFoundationCheck.Code, "Code", "Код");
            this.MapProperty(x => x.DisposalInspFoundationCheck.InspFoundationCheck.Name, "Name", "Наименование");
            this.MapProperty(x => x.NormativeDocItem, "Number", "Номер");
            this.MapProperty(x => x.NormativeDocItem.Text, "Text", "Текст");
        }
    }
}
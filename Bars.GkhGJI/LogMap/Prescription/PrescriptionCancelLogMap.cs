namespace Bars.GkhGji.LogMap.Prescription
{
    using B4.Modules.NHibernateChangeLog;
    using B4.Utils;
    using Entities;

    public class PrescriptionCancelLogMap : AuditLogMap<PrescriptionCancel>
    {
        public PrescriptionCancelLogMap()
        {
            this.Name("Решение об отмене в предписании ГЖИ");
            this.Description(x => x.Prescription.DocumentNumber ?? "");

            this.MapProperty(x => x.DocumentNum, "DocumentNum", "Номер");
            this.MapProperty(x => x.DocumentDate, "DocumentDate", "Номер");
            this.MapProperty(x => x.DateCancel, "DateCancel", "Номер");
            this.MapProperty(x => x.IsCourt, "IsCourt", "Номер");
            this.MapProperty(x => x.IssuedCancel.Fio, "IssuedCancel", "Номер");
            this.MapProperty(x => x.Reason, "Reason", "Номер");
        }
    }
}
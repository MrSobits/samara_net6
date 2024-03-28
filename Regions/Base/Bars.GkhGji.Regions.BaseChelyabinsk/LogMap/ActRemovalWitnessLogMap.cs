namespace Bars.GkhGji.Regions.BaseChelyabinsk.LogMap
{
    using B4.Modules.NHibernateChangeLog;
    using B4.Utils;
    using Entities.ActRemoval;

    public class ActRemovalWitnessLogMap : AuditLogMap<ActRemovalWitness>
    {
        public ActRemovalWitnessLogMap()
        {
            this.Name("Акт проверки предписания - Лица, присутствующие при проверке");
            this.Description(x => x.ActRemoval.DocumentNumber ?? "");

            this.MapProperty(x => x.ActRemoval.DocumentDate, "DocumentDate", "Дата документа");
            this.MapProperty(x => x.ActRemoval.DocumentNumber, "DocumentNumber", "Номер документа", x => x.Return(y => y ?? ""));
            this.MapProperty(x => x.Fio, "Fio", "ФИО");
            this.MapProperty(x => x.Position, "Position", "Должность");
            this.MapProperty(x => x.IsFamiliar, "IsFamiliar", "С актом ознакомлен", x => x.Return(y => y ? "Да" : "Нет"));
        }
    }
}
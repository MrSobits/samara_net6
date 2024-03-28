namespace Bars.GkhGji.Regions.Nso.LogMap
{
    using Bars.B4.Modules.NHibernateChangeLog;
    using Bars.B4.Utils;
    using Bars.GkhGji.Regions.Nso.Entities;

    public class ActRemovalWitnessLogMap : AuditLogMap<ActRemovalWitness>
    {
        public ActRemovalWitnessLogMap()
        {
            this.Name("Акт проверки предписания - Лица, присутствующие при проверке");
            this.Description(x => x.ActRemoval.DocumentNumber ?? "");

            this.MapProperty(x => x.Fio, "Fio", "ФИО");
            this.MapProperty(x => x.Position, "Position", "Должность");
            this.MapProperty(x => x.IsFamiliar, "DocumentDate", "Дата документа", x => x.Return(y => y) ? "Да" : "Нет");
        }
    }
}
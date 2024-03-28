namespace Bars.GkhGji.LogMap.ActCheck
{
    using B4.Modules.NHibernateChangeLog;
    using B4.Utils;
    using Entities;

    public class ActCheckWitnessLogMap : AuditLogMap<ActCheckWitness>
    {
        public ActCheckWitnessLogMap()
        {
            this.Name("Лица, присутствующие при проверке (или свидетели)");
            this.Description(x => x.ActCheck.DocumentNumber ?? "");

            this.MapProperty(x => x.Fio, "Fio", "ФИО");
            this.MapProperty(x => x.Position, "Position", "Должность");
            this.MapProperty(x => x.IsFamiliar, "IsFamiliar", "С актом ознакомлен");
        }
    }
}
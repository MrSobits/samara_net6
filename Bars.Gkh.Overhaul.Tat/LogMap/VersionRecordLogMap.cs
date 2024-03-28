namespace Bars.Gkh.Overhaul.Tat.LogMap
{
    using B4.Modules.NHibernateChangeLog;
    using Entities;

    public class VersionRecordLogMap : AuditLogMap<VersionRecord>
    {
        public VersionRecordLogMap()
        {
            Name("Результат корректировки");

            Description(v => v.CommonEstateObjects);

            MapProperty(v => v.FixedYear, "FixedYear", "Фиксация скорректированного года");
            MapProperty(v => v.IndexNumber, "IndexNumber", "Номер записи");
        }
    }
}

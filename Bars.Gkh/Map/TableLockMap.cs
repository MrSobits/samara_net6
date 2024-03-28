#warning MappingConverter: требуется ручной маппинг для Bars.Gkh.Entities.TableLock
namespace Bars.Gkh.Map
{
    using Bars.Gkh.Entities;

    using NHibernate.Mapping.ByCode.Conformist;

    public class TableLockNHMapping : ClassMapping<TableLock>
    {
        public TableLockNHMapping()
        {
            Table("TABLE_LOCK");
            Property(
                x => x.LockStart,
                m =>
                    {
                        m.Column("LOCK_START");
                        m.NotNullable(true);
                    });
            Property(
                x => x.TableName,
                m =>
                    {
                        m.Column("TABLE_NAME");
                        m.NotNullable(true);
                        m.Length(30);
                    });
            Property(
                x => x.Action,
                m =>
                    {
                        m.Column("ACTION");
                        m.NotNullable(true);
                        m.Length(6);
                    });
        }
    }
}

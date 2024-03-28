namespace Bars.Gkh.Gasu.Migration.Version_2014112700
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014112700")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Gasu.Migration.Version_2014112600.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.ChangeColumn("GASU_INDICATOR", new Column("PERIODICITY", DbType.Int16, ColumnProperty.NotNull, 1));
            Database.ChangeColumn("GASU_INDICATOR", new Column("EBIR_MODULE", DbType.Int16, ColumnProperty.NotNull, 1));
        }

        public override void Down()
        {
            Database.ChangeColumn("GASU_INDICATOR", new Column("PERIODICITY", DbType.Int16, ColumnProperty.NotNull, 10));
            Database.ChangeColumn("GASU_INDICATOR", new Column("EBIR_MODULE", DbType.Int16, ColumnProperty.NotNull, 10));
        }
    }
}
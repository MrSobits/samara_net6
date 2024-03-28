namespace Bars.Gkh.Migrations._2015.Version_2015120500
{
    using System;
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015120500")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations._2015.Version_2015120200.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.ChangeColumn("GKH_CIT_SUG_COMMENT", new Column("CREATION_DATE", DbType.DateTime, null));
            Database.ChangeColumn("GKH_CIT_SUG", new Column("DESCRIPTION", DbType.String, 2000, ColumnProperty.Null));
            Database.ChangeColumn("GKH_SUGG_HISTORY", new Column("RECORD_DATE", DbType.DateTime, null));
            Database.ChangeColumn("GKH_SUGG_HISTORY", new Column("EXEC_DEADLINE", DbType.DateTime, null));
        }

        public override void Down()
        {
        }
    }
}

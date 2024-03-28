namespace Bars.Gkh1468.Migrations.Version_2015092900
{
    using System.Data;
    using Bars.Gkh.Utils;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015092900")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh1468.Migrations.Version_2015051500.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.ChangeColumn("GKH_PUBLIC_SERV", new Column("NAME", DbType.String, 500, ColumnProperty.Null));
        }

        public override void Down()
        {
            Database.ExecuteNonQuery("UPDATE GKH_PUBLIC_SERV SET NAME = ' ' WHERE NAME is NULL");
            if (Database.DatabaseKind == DbmsKind.Oracle)
            {
                Database.AlterColumnSetNullable("GKH_PUBLIC_SERV", "NAME", false);
            }
        }
    }
}

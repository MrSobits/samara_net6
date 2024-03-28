namespace Bars.GkhGji.Migrations._2021.Version_2021122401
{
    using System.Data;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2021122401")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(Version_2021122400.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {

            Database.AddColumn("GJI_DICISION", new Column("REQ_NUM", DbType.String));
            Database.AddColumn("GJI_DICISION", new Column("REQ_DATE", DbType.DateTime, ColumnProperty.None));

        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_DICISION", "REQ_DATE");
            Database.RemoveColumn("GJI_DICISION", "REQ_NUM");
        }
    }
}
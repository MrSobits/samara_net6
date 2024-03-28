namespace Bars.GkhGji.Migrations._2021.Version_2021040900
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2021040900")]
    [MigrationDependsOn(typeof(Version_2021011200.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_PROTOCOL", new Column("PP_POSITION", DbType.String, 150));
            Database.AddColumn("GJI_RESOLPROS", new Column("PP_POSITION", DbType.String, 150));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_PROTOCOL", "XML_FILE");
            Database.RemoveColumn("GJI_RESOLPROS", "PP_POSITION");
        }
    }
}
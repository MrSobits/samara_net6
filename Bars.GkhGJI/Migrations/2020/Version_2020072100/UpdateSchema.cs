namespace Bars.GkhGji.Migrations._2020.Version_2020072100
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2020072100")]
    [MigrationDependsOn(typeof(Version_2020072000.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_RESOLUTION", new RefColumn("PHYSICALPERSON_DOCTYPE_ID", "GJI_PROTOCOL_PHYSICALPERSONDOCTYPE_ID", "GJI_DICT_PHYSICAL_PERSON_DOC_TYPE", "ID"));
            Database.AddColumn("GJI_RESOLUTION", new Column("PHYSICALPERSON_DOC_NUM", DbType.String, 300));
            Database.AddColumn("GJI_RESOLUTION", new Column("PHYSICALPERSON_DOC_SERIAL", DbType.String, 300));
            Database.AddColumn("GJI_RESOLUTION", new Column("PHYSICALPERSON_NOT_CITIZENSHIP", DbType.Boolean, false));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_RESOLUTION", "PHYSICALPERSON_NOT_CITIZENSHIP");
            Database.RemoveColumn("GJI_RESOLUTION", "PHYSICALPERSON_DOC_SERIAL");
            Database.RemoveColumn("GJI_RESOLUTION", "PHYSICALPERSON_DOC_NUM");
            Database.RemoveColumn("GJI_RESOLUTION", "PHYSICALPERSON_DOCTYPE_ID");
        }
    }
}
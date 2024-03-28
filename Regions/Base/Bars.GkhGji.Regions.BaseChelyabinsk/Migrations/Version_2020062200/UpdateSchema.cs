namespace Bars.GkhGji.Regions.BaseChelyabinsk.Migrations.Version_2020062200
{
    using B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2020062200")]
    [MigrationDependsOn(typeof(Version_2020040900.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_PROTOCOL197", new RefColumn("PHYSICALPERSON_DOCTYPE_ID", "GJI_PROTOCOL197_PHYSICALPERSONDOCTYPE_ID", "GJI_DICT_PHYSICAL_PERSON_DOC_TYPE", "ID"));
            Database.AddColumn("GJI_PROTOCOL197", new Column("PHYSICALPERSON_DOC_NUM", DbType.String, 300));
            Database.AddColumn("GJI_PROTOCOL197", new Column("PHYSICALPERSON_DOC_SERIAL", DbType.String, 300));
            Database.AddColumn("GJI_PROTOCOL197", new Column("PHYSICALPERSON_NOT_CITIZENSHIP", DbType.Boolean, false));
        }
        public override void Down()
        {
            Database.RemoveColumn("GJI_PROTOCOL197", "PHYSICALPERSON_NOT_CITIZENSHIP");
            Database.RemoveColumn("GJI_PROTOCOL197", "PHYSICALPERSON_DOC_SERIAL");
            Database.RemoveColumn("GJI_PROTOCOL197", "PHYSICALPERSON_DOC_NUM");
            Database.RemoveColumn("GJI_PROTOCOL197", "PHYSICALPERSON_DOCTYPE_ID");
        }
    }
}
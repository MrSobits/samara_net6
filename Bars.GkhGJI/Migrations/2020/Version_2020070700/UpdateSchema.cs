namespace Bars.GkhGji.Migrations._2020.Version_2020070700
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2020070700")]
    [MigrationDependsOn(typeof(Version_2020070200.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
                "GJI_DICT_PHYSICAL_PERSON_DOC_TYPE",
                new Column("NAME", DbType.String, 300, ColumnProperty.NotNull),
                new Column("CODE", DbType.String, 300, ColumnProperty.NotNull));

            Database.AddColumn("GJI_PROTOCOL", new RefColumn("PHYSICALPERSON_DOCTYPE_ID", "GJI_PROTOCOL_PHYSICALPERSONDOCTYPE_ID", "GJI_DICT_PHYSICAL_PERSON_DOC_TYPE", "ID"));
            Database.AddColumn("GJI_PROTOCOL", new Column("PHYSICALPERSON_DOC_NUM", DbType.String, 300));
            Database.AddColumn("GJI_PROTOCOL", new Column("PHYSICALPERSON_DOC_SERIAL", DbType.String, 300));
            Database.AddColumn("GJI_PROTOCOL", new Column("PHYSICALPERSON_NOT_CITIZENSHIP", DbType.Boolean, false));

            Database.ChangeColumn("GJI_APPEAL_CITIZEN_ANSWER_FILES", new Column("NAME", DbType.String, 250, ColumnProperty.NotNull));
            Database.ChangeColumn("GJI_APPEAL_CITIZENS", new Column("MUNICIPALITY", DbType.String, 250));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_PROTOCOL", "PHYSICALPERSON_NOT_CITIZENSHIP");
            Database.RemoveColumn("GJI_PROTOCOL", "PHYSICALPERSON_DOC_SERIAL");
            Database.RemoveColumn("GJI_PROTOCOL", "PHYSICALPERSON_DOC_NUM");
            Database.RemoveColumn("GJI_PROTOCOL", "PHYSICALPERSON_DOCTYPE_ID");
            Database.RemoveTable("GJI_DICT_PHYSICAL_PERSON_DOC_TYPE");
        }
    }
}
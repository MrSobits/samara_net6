namespace Bars.GkhGji.Regions.Tomsk.Migrations.Version_2014072103
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014072103")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.Tomsk.Migrations.Version_2014072102.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("GJI_DOCUMENT_PHYS_INFO",
                new RefColumn("DOCUMENT_ID", ColumnProperty.NotNull, "GJI_DOC_PHYS_INFO_DOC", "GJI_DOCUMENT", "ID"),
                new Column("TYPE_GENDER", DbType.Int32, ColumnProperty.NotNull, 0),
                new Column("PADDRESS", DbType.String, 2000),
                new Column("PJOB", DbType.String, 2000),
                new Column("PPOSITION", DbType.String, 2000),
                new Column("BIRTHDAY_AND_PLACE", DbType.String, 2000),
                new Column("PIDENTITY_DOC", DbType.String, 2000),
                new Column("PSALARY", DbType.String, 2000));

            Database.AddColumn("GJI_TOMSK_PROTOCOL_DESCR", new Column("DESCRIPTION_SET", DbType.Binary, ColumnProperty.Null));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_TOMSK_PROTOCOL_DESCR", "DESCRIPTION_SET");

            Database.RemoveTable("GJI_DOCUMENT_PHYS_INFO");
        }
    }
}
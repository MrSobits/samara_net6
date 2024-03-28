namespace Bars.GkhGji.Regions.Nnovgorod.Migrations.Version_2014020700
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014020700")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.Nnovgorod.Migrations.Version_2014020600.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("GJI_NNOV_DOCGJI_PERSINFO",
                new Column("ADDRESS", DbType.String),
                new Column("JOB", DbType.String),
                new Column("POSITION", DbType.String),
                new Column("BIRTHDAY_AND_PLACE", DbType.String),
                new Column("IDENTITY_DOCUMENT", DbType.String),
                new Column("SALARY", DbType.String),
                new Column("MARITAL_STATUS", DbType.String),
                new RefColumn("DOCUMENT_ID", ColumnProperty.NotNull, "GJI_NNOV_DOCGJI_PERSINFO_DOC", "GJI_DOCUMENT", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("GJI_NNOV_DOCGJI_PERSINFO");
        }
    }
}
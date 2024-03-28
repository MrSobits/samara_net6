namespace Bars.GkhGji.Regions.Nso.Migrations.Version_2014121900
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014121900")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.Nso.Migrations.Version_2014121701.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_NSO_PROTOCOL", new Column("PERSON_ADDRESS", DbType.String, 2000, ColumnProperty.Null));
            Database.AddColumn("GJI_NSO_PROTOCOL", new Column("PERSON_JOB", DbType.String, 2000, ColumnProperty.Null));
            Database.AddColumn("GJI_NSO_PROTOCOL", new Column("PERSON_POSITION", DbType.String, 2000, ColumnProperty.Null));
            Database.AddColumn("GJI_NSO_PROTOCOL", new Column("PERSON_BIRTHDATE", DbType.String, 2000, ColumnProperty.Null));
            Database.AddColumn("GJI_NSO_PROTOCOL", new Column("PERSON_DOC", DbType.String, 2000, ColumnProperty.Null));
            Database.AddColumn("GJI_NSO_PROTOCOL", new Column("PERSON_SALARY", DbType.String, 2000, ColumnProperty.Null));
            Database.AddColumn("GJI_NSO_PROTOCOL", new Column("PERSON_RELAT", DbType.String, 2000, ColumnProperty.Null));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_NSO_PROTOCOL", "PERSON_RELAT");
            Database.RemoveColumn("GJI_NSO_PROTOCOL", "PERSON_SALARY");
            Database.RemoveColumn("GJI_NSO_PROTOCOL", "PERSON_DOC");
            Database.RemoveColumn("GJI_NSO_PROTOCOL", "PERSON_BIRTHDATE");
            Database.RemoveColumn("GJI_NSO_PROTOCOL", "PERSON_POSITION");
            Database.RemoveColumn("GJI_NSO_PROTOCOL", "PERSON_JOB");
            Database.RemoveColumn("GJI_NSO_PROTOCOL", "PERSON_ADDRESS");
        }
    }
}
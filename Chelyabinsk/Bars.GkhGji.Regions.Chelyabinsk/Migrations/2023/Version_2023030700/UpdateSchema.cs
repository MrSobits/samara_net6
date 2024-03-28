namespace Bars.GkhGji.Regions.Chelyabinsk.Migrations.Version_2023030700
{
    using B4.Modules.Ecm7.Framework;
    using B4.Modules.NH.Migrations.DatabaseExtensions;
    using Gkh;
    using System.Data;

    [Migration("2023030700")]
    [MigrationDependsOn(typeof(Version_2022110400.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Накатить миграцию
        /// </summary>
        public override void Up()
        {
            Database.AddRefColumn("GJI_CH_GIS_ERKNM", new RefColumn("ADMON_ID", "GJI_CH_GIS_ERKNM_ADMONITION", "GJI_CH_APPCIT_ADMONITION", "ID"));
            Database.AddColumn("GJI_CH_GIS_ERKNM", new Column("DOC_TYPE", DbType.Int16, ColumnProperty.NotNull, 10));
            Database.AddColumn("GJI_CH_APPCIT_ADMONITION", new Column("ERKNMID", DbType.String, 100));
            Database.AddColumn("GJI_CH_APPCIT_ADMONITION", new Column("KIND_KND", DbType.Int32, ColumnProperty.NotNull, 0));
            Database.AddColumn("GJI_CH_APPCIT_ADMONITION", new Column("ERKNMGUID", DbType.String, 100));
            Database.AddColumn("GJI_CH_APPCIT_ADMONITION", new Column("IS_SENT", DbType.Boolean, ColumnProperty.NotNull, false));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_CH_APPCIT_ADMONITION", "IS_SENT");
            Database.RemoveColumn("GJI_CH_APPCIT_ADMONITION", "ERKNMID");
            Database.RemoveColumn("GJI_CH_APPCIT_ADMONITION", "ERKNMGUID");
            Database.RemoveColumn("GJI_CH_GIS_ERKNM", "DOC_TYPE");
            Database.RemoveColumn("GJI_CH_GIS_ERKNM", "ADMON_ID");
        }
    }
}
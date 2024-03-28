namespace Bars.GkhGji.Regions.Tatarstan.Migration.Version_2019053000
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    using Migration = Bars.B4.Modules.Ecm7.Framework.Migration;

    [Migration("2019053000")]
    [MigrationDependsOn(typeof(Version_2019042500.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddColumn("GJI_DICT_PROSECUTOR_OFFICE", new Column("OKATO_TER", DbType.String, 3));
            this.Database.AddColumn("GJI_DICT_PROSECUTOR_OFFICE", new Column("OKATO_KOD1", DbType.String, 2));
            this.Database.AddColumn("GJI_DICT_PROSECUTOR_OFFICE", new Column("OKATO_KOD2", DbType.String, 2));
            this.Database.AddColumn("GJI_DICT_PROSECUTOR_OFFICE", new Column("OKATO_KOD3", DbType.String, 2));
            this.Database.AddColumn("GJI_DICT_PROSECUTOR_OFFICE", new Column("USE_DEFAULT", DbType.Boolean, ColumnProperty.NotNull, false));
        }

        public override void Down()
        {
            this.Database.RemoveColumn("GJI_DICT_PROSECUTOR_OFFICE", "OKATO_TER");
            this.Database.RemoveColumn("GJI_DICT_PROSECUTOR_OFFICE", "OKATO_KOD1");
            this.Database.RemoveColumn("GJI_DICT_PROSECUTOR_OFFICE", "OKATO_KOD2");
            this.Database.RemoveColumn("GJI_DICT_PROSECUTOR_OFFICE", "OKATO_KOD3");
            this.Database.RemoveColumn("GJI_DICT_PROSECUTOR_OFFICE", "USE_DEFAULT");
        }
    }
}
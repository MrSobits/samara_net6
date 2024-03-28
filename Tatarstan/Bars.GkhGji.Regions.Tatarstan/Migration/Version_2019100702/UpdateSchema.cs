namespace Bars.GkhGji.Regions.Tatarstan.Migration.Version_2019100702
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    using Migration = Bars.B4.Modules.Ecm7.Framework.Migration;

    [Migration("2019100702")]
    [MigrationDependsOn(typeof(Version_2019100701.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.ChangeColumn("GJI_DICT_PROSECUTOR_OFFICE", new Column("OKATO_TER", DbType.String, 2));
            this.Database.ChangeColumn("GJI_DICT_PROSECUTOR_OFFICE", new Column("OKATO_KOD1", DbType.String, 3));
            this.Database.ChangeColumn("GJI_DICT_PROSECUTOR_OFFICE", new Column("OKATO_KOD2", DbType.String, 3));
            this.Database.ChangeColumn("GJI_DICT_PROSECUTOR_OFFICE", new Column("OKATO_KOD3", DbType.String, 3));

            this.Database.AddColumn("GJI_TAT_DISPOSAL", new Column("ERP_GUID", DbType.String));
        }

        public override void Down()
        {
            // ничего не делаем
        }
    }
}
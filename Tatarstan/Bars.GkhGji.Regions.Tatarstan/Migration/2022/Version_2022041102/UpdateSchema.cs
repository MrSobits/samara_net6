namespace Bars.GkhGji.Regions.Tatarstan.Migration._2022.Version_2022041102
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2022041102")]
    [MigrationDependsOn(typeof(Version_2022041101.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        private const string KnmCharacterTable = "GJI_DICT_KNM_CHARACTER";
        private const string KnmCharacterKindCheckTable = "KNM_CHARACTER_KIND_CHECK";

        public override void Up()
        {
            this.Database.AddEntityTable(KnmCharacterTable,
                new Column("ERKNM_CODE", System.Data.DbType.Int32));

            this.Database.AddEntityTable(KnmCharacterKindCheckTable,
                new RefColumn("KNM_CHARACTER_ID", "KNM_CHARACTER_KIND_CHECK_CHARACTER", "GJI_DICT_KNM_CHARACTER", "ID"),
                new RefColumn("KIND_CHECK_ID", "KNM_CHARACTER_KIND_CHECK_KIND_CHECK", "GJI_DICT_KIND_CHECK", "ID"));
        }

        public override void Down()
        {
            this.Database.RemoveTable(KnmCharacterKindCheckTable);
            this.Database.RemoveTable(KnmCharacterTable);
        }
    }
}

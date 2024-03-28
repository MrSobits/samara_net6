namespace Bars.GkhGji.Regions.BaseChelyabinsk.Migrations.Version_2017012000
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    /// <summary>
    /// Миграция 2017012000
    /// </summary>
    [Migration("2017012000")]
    [MigrationDependsOn(typeof(Version_2015102300.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Вверх
        /// </summary>
        public override void Up()
        {
            this.Database.AddEntityTable("GJI_DISPOSAL_INSP_FOUND_CHECK_NORM_DOC",
                new RefColumn("FOUND_CHECK_ID", "GJI_FOUND_CHECK_NORM_DOC_FOUND_CHECK_ID", "GJI_NSO_DISPOSAL_INSPFOUNDCHECK", "ID"),
                new RefColumn("DOC_ITEM_ID", "GJI_FOUND_CHECK_NORM_DOC_DOC_ITEM_ID", "GKH_DICT_NORMATIVE_DOC_ITEM", "ID"));
        }

        /// <summary>
        /// Вниз
        /// </summary>
        public override void Down()
        {
            this.Database.RemoveTable("GJI_DISPOSAL_INSP_FOUND_CHECK_NORM_DOC");
        }
    }
}
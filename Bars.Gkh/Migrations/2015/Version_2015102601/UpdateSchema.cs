namespace Bars.Gkh.Migrations._2015.Version_2015102601
{
    using global::Bars.B4.Modules.Ecm7.Framework;

    /// <summary>
    /// Миграция
    /// </summary>
    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015102601")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations._2015.Version_2015102600.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        /// <summary>
        /// Накат миграции
        /// </summary>
        public override void Up()
        {
            if (Database.ConstraintExists("GKH_MANORG_REQ_ANNEX", "FK_GKH_MANORG_REQ_ANNEX_DOC_TYPE"))
            {
                Database.RemoveConstraint("GKH_MANORG_REQ_ANNEX", "FK_GKH_MANORG_REQ_ANNEX_DOC_TYPE");
            }
            if (Database.ConstraintExists("GKH_MANORG_REQ_ANNEX", "IND_GKH_MANORG_REQ_ANNEX_DOC_TYPE"))
            {
                Database.RemoveIndex("IND_GKH_MANORG_REQ_ANNEX_DOC_TYPE", "GKH_MANORG_REQ_ANNEX");
            }


            if (!Database.ConstraintExists("GKH_MANORG_REQ_ANNEX", "FK_GKH_MANORG_REQ_ANNEX_DT"))
            {
                Database.AddForeignKey("FK_GKH_MANORG_REQ_ANNEX_DT", "GKH_MANORG_REQ_ANNEX", "DOCUMENT_TYPE_ID", "GKH_DICT_ANNEX_APPEAL_LIC_ISS", "ID");
            }
            if (!Database.ConstraintExists("GKH_MANORG_REQ_ANNEX", "IND_GKH_MANORG_REQ_ANNEX_DT"))
            {
                Database.AddIndex("IND_GKH_MANORG_REQ_ANNEX_DT", false, "GKH_MANORG_REQ_ANNEX", "DOCUMENT_TYPE_ID");
            }
        }

        /// <summary>
        /// Откат миграции
        /// </summary>
        public override void Down()
        {
        }
    }
}
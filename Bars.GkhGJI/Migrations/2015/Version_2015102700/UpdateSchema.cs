namespace Bars.GkhGji.Migrations._2015.Version_2015102700
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    /// <summary>
    /// Миграция 2015102700
    /// </summary>
    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015102700")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Migrations._2015.Version_2015102300.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        /// <summary>
        /// Накатить
        /// </summary>
        public override void Up()
        {
            if (Database.ConstraintExists("GJI_DICT_LEGFOUND_INSPECTCHECK", "FK_TYPE_SUR_LEGFCHECK_NORM_DOC"))
            {
                Database.RemoveConstraint("GJI_DICT_LEGFOUND_INSPECTCHECK", "FK_TYPE_SUR_LEGFCHECK_NORM_DOC");
            }
            if (Database.ConstraintExists("GJI_DICT_LEGFOUND_INSPECTCHECK", "IND_TYPE_SUR_LEGFCHECK_NORM_DOC"))
            {
                Database.RemoveIndex("IND_TYPE_SUR_LEGFCHECK_NORM_DOC", "GJI_DICT_LEGFOUND_INSPECTCHECK");
            }

            if (!Database.ConstraintExists("GJI_DICT_LEGFOUND_INSPECTCHECK", "FK_TYPE_SUR_LEGFCHECK_NDOC"))
            {
                Database.AddForeignKey("FK_TYPE_SUR_LEGFCHECK_NDOC", "GJI_DICT_LEGFOUND_INSPECTCHECK", "NORM_DOC_ID", "GKH_DICT_NORMATIVE_DOC", "ID");
            }
            if (!Database.ConstraintExists("GJI_DICT_LEGFOUND_INSPECTCHECK", "IND_TYPE_SUR_LEGFCHECK_NDOC"))
            {
                Database.AddIndex("IND_TYPE_SUR_LEGFCHECK_NDOC", false, "GJI_DICT_LEGFOUND_INSPECTCHECK", "NORM_DOC_ID");
            }
        }

        /// <summary>
        /// Откатить
        /// </summary>
        public override void Down()
        {
        }
    }
}
namespace Bars.Gkh.RegOperator.Migrations._2015.Version_2015032002
{
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.Utils;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015032002")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2015.Version_2015032001.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        #region Overrides of Migration

        public override void Up()
        {
            Database.ExecuteNonQuery("update REGOP_PERS_ACC_CHANGE set CHANGE_TYPE = 16 where CHANGE_TYPE = 10");
            Database.AddRefColumn("REGOP_PERS_ACC_CHANGE", new FileColumn("doc_id", ColumnProperty.Null, "regop_p_acc_ch_doc"));
        }

        public override void Down()
        {
            Database.RemoveColumn("REGOP_PERS_ACC_CHANGE", "doc_id");
        }

        #endregion
    }
}

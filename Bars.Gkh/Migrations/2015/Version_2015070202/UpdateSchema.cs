namespace Bars.Gkh.Migrations._2015.Version_2015070202
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    /// <summary>
    /// <see cref="Bars.Gkh.ExecutionAction.Impl.FillAccountFormationVariantAction"/>
    /// </summary>
    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015070202")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations._2015.Version_2015070201.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GKH_REALITY_OBJECT", "ACC_FORM_VARIANT", DbType.Int32, ColumnProperty.Null);
        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_REALITY_OBJECT", "ACC_FORM_VARIANT");
        }
    }
}
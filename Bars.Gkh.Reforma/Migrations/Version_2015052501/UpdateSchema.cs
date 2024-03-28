namespace Bars.Gkh.Reforma.Migrations.Version_2015052501
{
    using global::Bars.B4.Modules.Ecm7.Framework;

    /// <summary>
    /// Каскадное удаление для ключа FK_RFRM_CHANGED_MANOR_MANOR.
    /// </summary>
    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015052501")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Reforma.Migrations.Version_2015011200.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.RemoveConstraint("RFRM_CHANGED_MAN_ORG", "FK_RFRM_CHANGED_MANOR_MANOR");
            Database.AddForeignKey("FK_RFRM_CHANGED_MANOR_MANOR", "RFRM_CHANGED_MAN_ORG", "MAN_ORG_ID",
                "GKH_MANAGING_ORGANIZATION", "ID", ForeignKeyConstraint.Cascade);
        }

        public override void Down()
        {
            Database.RemoveConstraint("RFRM_CHANGED_MAN_ORG", "FK_RFRM_CHANGED_MANOR_MANOR");
            Database.AddForeignKey("FK_RFRM_CHANGED_MANOR_MANOR", "RFRM_CHANGED_MAN_ORG", "MAN_ORG_ID",
                "GKH_MANAGING_ORGANIZATION", "ID");
        }
    }
}

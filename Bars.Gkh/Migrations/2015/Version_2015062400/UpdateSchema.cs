namespace Bars.Gkh.Migrations._2015.Version_2015062400
{
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015062400")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations._2015.Version_2015062200.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            // проверка если будут накатывать/откатывать чтобы не удалились id пользователя
            if (Database.ColumnExists("GKH_LOG_OPERATION", "OPERATOR_ID"))
            {
                Database.RemoveConstraint("GKH_LOG_OPERATION", "FK_GKH_LOG_OPER_OP");

                Database.RenameColumn("GKH_LOG_OPERATION", "OPERATOR_ID", "USER_ID");

                Database.ExecuteNonQuery(
                    "UPDATE GKH_LOG_OPERATION O SET USER_ID = (SELECT OP.USER_ID FROM GKH_OPERATOR OP WHERE OP.ID = O.USER_ID )");

                Database.AddIndex("IND_GKH_LOG_OPER_US", false, "GKH_LOG_OPERATION", "USER_ID");
                Database.AddForeignKey("FK_GKH_LOG_OPER_US", "GKH_LOG_OPERATION", "USER_ID", "B4_USER", "ID");
                
            }
        }

        public override void Down()
        {
            // не требуется
        }
    }
}
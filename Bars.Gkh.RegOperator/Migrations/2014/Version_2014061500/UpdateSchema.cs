namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014061500
{
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014061500")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014061206.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            if (Database.TableExists("OVRHL_REG_OP_CALC_ACC"))
            {
                if (Database.ColumnExists("OVRHL_REG_OP_CALC_ACC", "OBJECT_VERSION"))
                {
                    Database.RemoveColumn("OVRHL_REG_OP_CALC_ACC", "OBJECT_VERSION");
                }

                if (Database.ColumnExists("OVRHL_REG_OP_CALC_ACC", "OBJECT_CREATE_DATE"))
                {
                    Database.RemoveColumn("OVRHL_REG_OP_CALC_ACC", "OBJECT_CREATE_DATE");
                }

                if (Database.ColumnExists("OVRHL_REG_OP_CALC_ACC", "OBJECT_EDIT_DATE"))
                {
                    Database.RemoveColumn("OVRHL_REG_OP_CALC_ACC", "OBJECT_EDIT_DATE");
                }
            }
        }

        public override void Down()
        {
            
        }
    }
}

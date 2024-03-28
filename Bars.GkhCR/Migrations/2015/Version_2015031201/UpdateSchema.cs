namespace Bars.GkhCr.Migration.Version_2015031201
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015031201")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhCr.Migration.Version_2015031200.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddRefColumn("CR_OBJ_CONTRACT", new RefColumn("TYPE_CONTRACT_ID", "CONTRACT_TYPE_CONTRACT", "GKH_DICT_MULTIITEM", "ID"));

            if (Database.ColumnExists("CR_OBJ_CONTRACT", "TYPE_CONTRACT_OBJ"))
            {
                Database.ExecuteNonQuery(@"update CR_OBJ_CONTRACT
set TYPE_CONTRACT_ID = (select min(id) from gkh_dict_multiitem
where key = 'Psd')
where TYPE_CONTRACT_OBJ = 10");

                Database.ExecuteNonQuery(@"update CR_OBJ_CONTRACT
set TYPE_CONTRACT_ID = (select min(id) from gkh_dict_multiitem
where key = 'Expertise')
where TYPE_CONTRACT_OBJ = 20");

                Database.ExecuteNonQuery(@"update CR_OBJ_CONTRACT
set TYPE_CONTRACT_ID = (select min(id) from gkh_dict_multiitem
where key = 'TechSepervision')
where TYPE_CONTRACT_OBJ = 30");

                Database.ExecuteNonQuery(@"update CR_OBJ_CONTRACT
set TYPE_CONTRACT_ID = (select min(id) from gkh_dict_multiitem
where key = 'Insurance')
where TYPE_CONTRACT_OBJ = 40");

                Database.RemoveColumn("CR_OBJ_CONTRACT", "TYPE_CONTRACT_OBJ");
            }
        }

        public override void Down()
        {
            Database.AddColumn("CR_OBJ_CONTRACT", "TYPE_CONTRACT_OBJ", DbType.Int32, ColumnProperty.NotNull, 10);

            Database.ExecuteNonQuery(@"update CR_OBJ_CONTRACT
set TYPE_CONTRACT_OBJ = 20
where TYPE_CONTRACT_ID = (select min(id) from gkh_dict_multiitem
where key = 'Expertise')");

            Database.ExecuteNonQuery(@"update CR_OBJ_CONTRACT
set TYPE_CONTRACT_OBJ = 30
where TYPE_CONTRACT_ID = (select min(id) from gkh_dict_multiitem
where key = 'TechSepervision')");

            Database.ExecuteNonQuery(@"update CR_OBJ_CONTRACT
set TYPE_CONTRACT_OBJ = 40
where TYPE_CONTRACT_ID = (select min(id) from gkh_dict_multiitem
where key = 'Insurance')");

            Database.RemoveColumn("CR_OBJ_CONTRACT", "TYPE_CONTRACT_ID");
        }
    }
}
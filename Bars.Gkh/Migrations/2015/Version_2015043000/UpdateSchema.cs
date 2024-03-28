namespace Bars.Gkh.Migrations._2015.Version_2015043000
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015043000")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migration.Version_2015042900.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            if (Database.ColumnExists("CLW_LAWSUIT", "JURSECTOR_NUMBER"))
            {
                Database.RemoveColumn("CLW_LAWSUIT", "JURSECTOR_NUMBER");
            }
            
            if (Database.ColumnExists("CLW_LAWSUIT", "CB_STATION_SSP"))
            {
                Database.RemoveColumn("CLW_LAWSUIT", "CB_STATION_SSP");
            }

            Database.AddRefColumn("CLW_LAWSUIT", new RefColumn("JINST_ID", "CLW_LAWSUIT_JINST", "CLW_JUR_INSTITUTION", "ID"));

            Database.AddRefColumn("CLW_LAWSUIT", new RefColumn("CB_SSP_JINST_ID", "CLW_LAWSUIT_CB_SSP_JINST", "CLW_JUR_INSTITUTION", "ID"));

            Database.AddRefColumn("CLW_LAWSUIT", new RefColumn("PETITION_ID", "CLW_LAWSUIT_PET", "CLW_DICT_PETITION_TYPE", "ID"));
        }

        public override void Down()
        {
            ViewManager.DropAll(Database);

            Database.RemoveColumn("CLW_LAWSUIT", "JINST_ID");
            Database.AddColumn("CLW_LAWSUIT", new Column("JURSECTOR_NUMBER", DbType.String, 100));

            Database.RemoveColumn("CLW_LAWSUIT", "CB_SSP_JINST_ID");
            Database.AddColumn("CLW_LAWSUIT", new Column("CB_STATION_SSP", DbType.String, 500));

            Database.RemoveColumn("CLW_LAWSUIT", "PETITION_ID");
        }
    }
}
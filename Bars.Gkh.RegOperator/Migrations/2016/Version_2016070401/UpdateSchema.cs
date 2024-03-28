namespace Bars.Gkh.RegOperator.Migrations._2016.Version_2016070401
{ 
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    /// <summary>
    /// Миграция RegOperator 2016070401
    /// </summary>
    [Migration("2016070401")]
    [MigrationDependsOn(typeof(Version_2016070400.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Вверх
        /// </summary>
        public override void Up()
        {
	        this.Database.AddEntityTable(
                "LAWSUIT_OWNER_INFO",
		        new RefColumn("LAWSUIT_ID", ColumnProperty.NotNull, "LAWSUIT_LAWSUIT_OWNER_INFO", "CLW_LAWSUIT", "ID"),
		        new RefColumn("PERSONAL_ACCOUNT_OWNER_INFORMATION_ID", ColumnProperty.NotNull, "PERSONAL_ACCOUNT_OWNER_INFORMATION_LAWSUIT_OWNER_INFO", "REGOP_PERS_ACC_OWNER_INFO", "ID")
		        );

        }

        /// <summary>
        /// Вниз
        /// </summary>
        public override void Down()
        {
			this.Database.RemoveTable("LAWSUIT_OWNER_INFO");
        }
    }
}

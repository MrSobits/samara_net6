namespace Bars.Gkh.RegOperator.Migrations._2017.Version_2017032400
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    /// <summary>
    /// Миграция RegOperator 2017032400
    /// </summary>
    [Migration("2017032400")]
    [MigrationDependsOn(typeof(Version_2017032200.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Вверх
        /// </summary>
        public override void Up()
        {
            this.Database.AddRefColumn("REGOP_INDIVIDUAL_ACC_OWN", new RefColumn("REGISTRATION_RO_ID", ColumnProperty.Null, "FK_INDIV_OWN_REALITY_OBJ", "GKH_REALITY_OBJECT", "ID"));
        }

        /// <summary>
        /// Вниз
        /// </summary>
        public override void Down()
        {
            this.Database.RemoveColumn("REGOP_INDIVIDUAL_ACC_OWN", "REGISTRATION_RO_ID");
        }
    }
}

namespace Bars.Gkh.RegOperator.Migrations._2017.Version_2017121800
{
    using B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2017121800")]
    [MigrationDependsOn(typeof(Version_2017121301.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc/>
        public override void Up()
        {
            this.Database.AddColumn("REGOP_INDIVIDUAL_ACC_OWN",
                new RefColumn("REGISTRATION_ROOM_ID", ColumnProperty.Null, "REGOP_INDIVIDUAL_ACC_REGISTRATION_ROOM", "GKH_ROOM", "ID"));
        }
    }
}
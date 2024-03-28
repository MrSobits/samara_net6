namespace Bars.Gkh.RegOperator.Migrations._2019.Version_2019121700
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2019121700")]
   
    [MigrationDependsOn(typeof(Version_2019112900.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc/>
        public override void Up()
        {
            this.Database.AddEntityTable("REGOP_SALDO_REFRESH",
                   new RefColumn("GROUP_ID", ColumnProperty.NotNull, "GROUP_REF_ID", "REGOP_PA_GROUP", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("REGOP_SALDO_REFRESH");
        }
    }
}
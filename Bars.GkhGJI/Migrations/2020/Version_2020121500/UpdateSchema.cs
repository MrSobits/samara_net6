namespace Bars.GkhGji.Migrations._2020.Version_2020121500
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2020121500")]
    [MigrationDependsOn(typeof(Version_2020121200.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_PRESCRIPTION", new Column("TYPE_EXECUTION", DbType.Int32, 4, ColumnProperty.NotNull, 0));
            Database.AddColumn("GJI_PRESCRIPTION", new Column("CANCELLED_BY_GJI", DbType.Boolean, ColumnProperty.NotNull, false));
            Database.AddColumn("GJI_PRESCRIPTION", new Column("PRESCRIPTION_STATE", DbType.Int32, 4, ColumnProperty.NotNull, 0));

            this.Database.ExecuteNonQuery(@"update GJI_PRESCRIPTION set TYPE_EXECUTION = 0, CANCELLED_BY_GJI = false, PRESCRIPTION_STATE = 0");
        }

        public override void Down()
        {          
            Database.RemoveColumn("GJI_PRESCRIPTION", "PRESCRIPTION_STATE");
            Database.RemoveColumn("GJI_PRESCRIPTION", "CANCELLED_BY_GJI");
            Database.RemoveColumn("GJI_PRESCRIPTION", "TYPE_EXECUTION");
        }
    }
}
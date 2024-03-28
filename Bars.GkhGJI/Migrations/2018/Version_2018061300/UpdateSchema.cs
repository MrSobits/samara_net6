namespace Bars.GkhGji.Migrations._2018.Version_2018061300
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2018061300")]
    [MigrationDependsOn(typeof(Bars.GkhGji.Migrations._2018.Version_2018040200.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddEntityTable(
                "GJI_APPEAL_CITIZENS_INFO",
                new Column("DOC_NUM", DbType.String, ColumnProperty.NotNull),
                new Column("APPEAL_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("OPERATION_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("CORRESPONDENT_NAME", DbType.String),
                new Column("OPERATION_TYPE", DbType.Int32, ColumnProperty.NotNull),
                new Column("OPERATOR", DbType.String, ColumnProperty.NotNull));
        }
        public override void Down()
        {
            this.Database.RemoveTable("GJI_APPEAL_CITIZENS_INFO");
        }
    }
}
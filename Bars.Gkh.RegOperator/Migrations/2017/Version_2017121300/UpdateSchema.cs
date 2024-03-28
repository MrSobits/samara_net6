namespace Bars.Gkh.RegOperator.Migrations._2017.Version_2017121300
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2017121300")]
    [MigrationDependsOn(typeof(Version_2017112700.UpdateSchema))]
    [MigrationDependsOn(typeof(Version_2017113000.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc/>
        public override void Up()
        {
            this.Database.AddPersistentObjectTable("CLW_RESTRUCT_SCHEDULE_DETAIL",
                new Column("TRANSFER_ID", DbType.Int64, ColumnProperty.NotNull, 0),
                new Column("SUM", DbType.Decimal, ColumnProperty.NotNull, 0),
                new RefColumn("SCHEDULE_ID",
                    ColumnProperty.NotNull,
                    "CLW_RESTRUCT_SCHEDULE_DETAIL_SCHEDULE",
                    "CLW_RESTRUCT_SCHEDULE",
                    "ID"));

            this.Database.AddIndex("IND_CLW_RESTRUCT_SCHEDULE_DETAIL_TRANSFER",
                false,
                "CLW_RESTRUCT_SCHEDULE_DETAIL",
                "TRANSFER_ID");
        }

        public override void Down()
        {
            this.Database.RemoveTable("CLW_RESTRUCT_SCHEDULE_DETAIL");
        }
    }
}
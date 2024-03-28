namespace Bars.GkhGji.Regions.Tatarstan.Migration._2022.Version_2022041601
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Utils;
    using Bars.Gkh.Enums;
    using System.Data;

    [Migration("2022041601")]
    [MigrationDependsOn(typeof(Version_2022041600.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        private const string DecisionTable = "GJI_DECISION";

        private Column[] Columns => new[]
        {
            new Column("SUBMISSION_DATE", DbType.DateTime),
            new Column("RECEIPT_DATE", DbType.DateTime),
            new Column("USING_MEANS_REMOTE_INTERACTION", DbType.Int32, ColumnProperty.NotNull, (int)YesNoNotSet.NotSet),
            new Column("INFO_USING_MEANS_REMOTE_INTERACTION", DbType.String)
        };

        /// <inheritdoc />
        public override void Up()
        {
            Columns.ForEach(column => { this.Database.AddColumn(DecisionTable, column); });
        }

        /// <inheritdoc />
        public override void Down()
        {
            Columns.ForEach(column => { this.Database.RemoveColumn(DecisionTable, column.Name); });
        }
    }
}

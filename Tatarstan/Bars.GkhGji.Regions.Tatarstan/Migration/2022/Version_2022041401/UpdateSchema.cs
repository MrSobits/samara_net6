namespace Bars.GkhGji.Regions.Tatarstan.Migration._2022.Version_2022041401
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Utils;

    [Migration("2022041401")]
    [MigrationDependsOn(typeof(Version_2022041400.UpdateSchema))]
    public class UpdateSchema: Migration
    {
        private string DecisionTableName => "GJI_WARNING_DOC";

        private Column[] ErknmColumns => new[]
        {
            new Column("ERKNM_REGISTRATION_NUMBER", DbType.String.WithSize(30)),
            new Column("ERKNM_REGISTRATION_DATE", DbType.DateTime)
        };

        /// <inheritdoc />
        public override void Up()
        {
            this.ErknmColumns.ForEach(column => { this.Database.AddColumn(this.DecisionTableName, column); });
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.ErknmColumns.ForEach(column => { this.Database.RemoveColumn(this.DecisionTableName, column.Name); });
        }
    }
}
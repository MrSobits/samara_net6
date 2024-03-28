namespace Bars.GkhGji.Regions.Tatarstan.Migration._2021.Version_2021122000
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.Enums;

    [Migration("2021122000")]
    [MigrationDependsOn(typeof(Version_2021121900.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        private readonly SchemaQualifiedObjectName gjiActCheckInspectionActionTable =
            new SchemaQualifiedObjectName { Name = "GJI_ACTCHECK_INSPECTION_ACTION", Schema = "PUBLIC" };

        /// <inheritdoc />
        public override void Up()
        {
            this.Database.AddJoinedSubclassTable(this.gjiActCheckInspectionActionTable.Name,
                "GJI_ACTCHECK_ACTION",
                this.gjiActCheckInspectionActionTable.Name + "_ACTION",
                new Column("CONTINUE_DATE", DbType.Date),
                new Column("CONTINUE_START_TIME", DbType.DateTime),
                new Column("CONTINUE_END_TIME", DbType.DateTime),
                new Column("HAS_VIOLATION", DbType.Int16, ColumnProperty.NotNull, (int)YesNoNotSet.NotSet),
                new Column("HAS_REMARK", DbType.Int16, ColumnProperty.NotNull, (int)HasValuesNotSet.NotSet));
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveTable(this.gjiActCheckInspectionActionTable);
        }
    }
}
namespace Bars.GkhGji.Regions.Tatarstan.Migration._2021.Version_2021121200
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.B4.Utils;
    using Bars.Gkh.Enums;

    [Migration("2021121200")]
    [MigrationDependsOn(typeof(Version_2021120801.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        private readonly SchemaQualifiedObjectName gjiActCheckActionTable =
            new SchemaQualifiedObjectName { Name = "GJI_ACTCHECK_ACTION", Schema = "PUBLIC" };

        private readonly SchemaQualifiedObjectName gjiActCheckActionCarriedOutEventTable =
            new SchemaQualifiedObjectName { Name = "GJI_ACTCHECK_ACTION_CARRIED_OUT_EVENT", Schema = "PUBLIC" };

        private readonly SchemaQualifiedObjectName gjiActCheckActionFileTable =
            new SchemaQualifiedObjectName { Name = "GJI_ACTCHECK_ACTION_FILE", Schema = "PUBLIC" };

        private readonly SchemaQualifiedObjectName gjiActCheckActionInspectorTable =
            new SchemaQualifiedObjectName { Name = "GJI_ACTCHECK_ACTION_INSPECTOR", Schema = "PUBLIC" };

        private readonly SchemaQualifiedObjectName gjiActCheckActionRemarkTable =
            new SchemaQualifiedObjectName { Name = "GJI_ACTCHECK_ACTION_REMARK", Schema = "PUBLIC" };

        private readonly SchemaQualifiedObjectName gjiActCheckActionViolationTable =
            new SchemaQualifiedObjectName { Name = "GJI_ACTCHECK_ACTION_VIOLATION", Schema = "PUBLIC" };

        private readonly SchemaQualifiedObjectName gjiActCheckIntrExamActionTable =
            new SchemaQualifiedObjectName { Name = "GJI_ACTCHECK_INSTR_EXAM_ACTION", Schema = "PUBLIC" };

        private readonly SchemaQualifiedObjectName gjiActCheckIntrExamActionNormativeDocTable =
            new SchemaQualifiedObjectName { Name = "GJI_ACTCHECK_INSTR_EXAM_ACTION_NORMATIVE_DOC", Schema = "PUBLIC" };

        private readonly Column[] newActCheckActionColumns =
        {
            new Column("NUMBER", DbType.String.WithSize(50)),
            new Column("START_DATE", DbType.Date),
            new Column("START_TIME", DbType.DateTime),
            new Column("END_DATE", DbType.Date),
            new Column("END_TIME", DbType.DateTime),
            new RefColumn("EXECUTION_PLACE_ID",
                "GJI_ACTCHECK_ACTION_EXECUTION_PLACE",
                "B4_FIAS_ADDRESS",
                "ID"),
            new Column("CONTR_PERS_FIO", DbType.String.WithSize(255)),
            new Column("CONTR_PERS_BIRTH_DATE", DbType.Date),
            new Column("CONTR_PERS_BIRTH_PLACE", DbType.String.WithSize(500)),
            new Column("CONTR_PERS_REG_ADDRESS", DbType.String.WithSize(500)),
            new Column("CONTR_PERS_LIVING_ADDRESS_MATCHED", DbType.Boolean, false),
            new Column("CONTR_PERS_LIVING_ADDRESS", DbType.String.WithSize(500)),
            new Column("CONTR_PERS_IS_HIRER", DbType.Boolean, false),
            new Column("CONTR_PERS_PHONE_NUMBER", DbType.String.WithSize(50)),
            new Column("CONTR_PERS_WORK_PLACE", DbType.String.WithSize(255)),
            new Column("CONTR_PERS_POST", DbType.String.WithSize(255)),
            new RefColumn("IDENTITY_DOC_TYPE_ID",
                "GJI_ACTCHECK_ACTION_IDENTITY_DOC_TYPE",
                "GKH_DICT_IDENTITY_DOC_TYPE",
                "ID"),
            new Column("IDENTITY_DOC_SERIES", DbType.String.WithSize(25)),
            new Column("IDENTITY_DOC_NUMBER", DbType.String.WithSize(25)),
            new Column("IDENTITY_DOC_ISSUED_ON", DbType.Date),
            new Column("IDENTITY_DOC_ISSUED_BY", DbType.String.WithSize(255)),
            new Column("REPRESENT_FIO", DbType.String.WithSize(255)),
            new Column("REPRESENT_WORK_PLACE", DbType.String.WithSize(255)),
            new Column("REPRESENT_POST", DbType.String.WithSize(255)),
            new Column("REPRESENT_PROCURATION_NUMBER", DbType.String.WithSize(50)),
            new Column("REPRESENT_PROCURATION_ISSUED_ON", DbType.Date),
            new Column("REPRESENT_PROCURATION_VALID_PERIOD", DbType.String.WithSize(25))
        };

        /// <inheritdoc />
        public override void Up()
        {
            this.newActCheckActionColumns.ForEach(column =>
            {
                if (column is RefColumn refColumn)
                {
                    this.Database.AddRefColumn(this.gjiActCheckActionTable.Name, refColumn);
                }
                else
                {
                    this.Database.AddColumn(this.gjiActCheckActionTable, column);
                }
            });

            this.Database.AddEntityTable(this.gjiActCheckActionCarriedOutEventTable.Name,
                new RefColumn("ACTCHECK_ACTION_ID",
                    ColumnProperty.NotNull,
                    this.gjiActCheckActionCarriedOutEventTable.Name + "_ACTCHECK_ACTION",
                    this.gjiActCheckActionTable.Name,
                    "ID"),
                new Column("EVENT_TYPE", DbType.Int16, ColumnProperty.NotNull));

            this.Database.AddEntityTable(this.gjiActCheckActionFileTable.Name,
                new RefColumn("ACTCHECK_ACTION_ID",
                    ColumnProperty.NotNull,
                    this.gjiActCheckActionFileTable.Name + "_ACTCHECK_ACTION",
                    this.gjiActCheckActionTable.Name,
                    "ID"),
                new Column("NAME", DbType.String.WithSize(50), ColumnProperty.NotNull),
                new Column("DOCUMENT_DATE", DbType.Date, ColumnProperty.NotNull),
                new Column("DESCRIPTION", DbType.String.WithSize(255)),
                new RefColumn("FILE_ID",
                    this.gjiActCheckActionFileTable.Name + "_FILE",
                    "B4_FILE_INFO",
                    "ID"));

            this.Database.AddEntityTable(this.gjiActCheckActionInspectorTable.Name,
                new RefColumn("ACTCHECK_ACTION_ID",
                    ColumnProperty.NotNull,
                    this.gjiActCheckActionInspectorTable.Name + "_ACTCHECK_ACTION",
                    this.gjiActCheckActionTable.Name,
                    "ID"),
                new RefColumn("INSPECTOR_ID",
                    ColumnProperty.NotNull,
                    this.gjiActCheckActionInspectorTable.Name + "_INSPECTOR",
                    "GKH_DICT_INSPECTOR",
                    "ID"));

            this.Database.AddEntityTable(this.gjiActCheckActionRemarkTable.Name,
                new RefColumn("ACTCHECK_ACTION_ID",
                    ColumnProperty.NotNull,
                    this.gjiActCheckActionRemarkTable.Name + "_ACTCHECK_ACTION",
                    this.gjiActCheckActionTable.Name,
                    "ID"),
                new Column("REMARK", DbType.String.WithSize(500)),
                new Column("MEMBER_FIO", DbType.String.WithSize(500)));

            this.Database.AddEntityTable(this.gjiActCheckActionViolationTable.Name,
                new RefColumn("ACTCHECK_ACTION_ID",
                    ColumnProperty.NotNull,
                    this.gjiActCheckActionViolationTable.Name + "_ACTCHECK_ACTION",
                    this.gjiActCheckActionTable.Name,
                    "ID"),
                new Column("VIOLATION", DbType.String.WithSize(255)),
                new Column("CONTR_PERS_RESPONSE", DbType.String.WithSize(500)));

            this.Database.AddJoinedSubclassTable(this.gjiActCheckIntrExamActionTable.Name,
                this.gjiActCheckActionTable.Name,
                this.gjiActCheckIntrExamActionTable.Name + "_ACTION",
                new Column("TERRITORY", DbType.String.WithSize(25)),
                new Column("PREMISE", DbType.String.WithSize(25)),
                new Column("TERRITORY_ACCESS_DENIED", DbType.Boolean, false),
                new Column("HAS_VIOLATION", DbType.Int16, ColumnProperty.NotNull, (int)YesNoNotSet.NotSet),
                new Column("USING_EQUIPMENT", DbType.String.WithSize(500)),
                new Column("HAS_REMARK", DbType.Int16, ColumnProperty.NotNull, (int)HasValuesNotSet.NotSet));

            this.Database.AddEntityTable(this.gjiActCheckIntrExamActionNormativeDocTable.Name,
                new RefColumn("INSTR_EXAM_ACTION_ID",
                    this.gjiActCheckIntrExamActionNormativeDocTable.Name + "_ACTCHECK_INSTR_EXAM_ACTION",
                    this.gjiActCheckIntrExamActionTable.Name,
                    "ID"),
                new RefColumn("NORMATIVE_DOC_ID",
                    this.gjiActCheckIntrExamActionNormativeDocTable.Name + "_ACTCHECK_ACTION",
                    "GKH_DICT_NORMATIVE_DOC",
                    "ID"));
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.newActCheckActionColumns.ForEach(x => this.Database.RemoveColumn(this.gjiActCheckActionTable, x.Name));

            this.Database.RemoveTable(this.gjiActCheckActionCarriedOutEventTable);
            this.Database.RemoveTable(this.gjiActCheckActionFileTable);
            this.Database.RemoveTable(this.gjiActCheckActionInspectorTable);
            this.Database.RemoveTable(this.gjiActCheckActionRemarkTable);
            this.Database.RemoveTable(this.gjiActCheckActionViolationTable);
            this.Database.RemoveTable(this.gjiActCheckIntrExamActionNormativeDocTable);
            this.Database.RemoveTable(this.gjiActCheckIntrExamActionTable);
        }
    }
}
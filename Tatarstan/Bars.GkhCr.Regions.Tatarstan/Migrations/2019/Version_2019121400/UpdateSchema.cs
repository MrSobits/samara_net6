namespace Bars.GkhCr.Regions.Tatarstan.Migrations._2019.Version_2019121400
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2019121400")]
    public class UpdateSchema : Migration
    {
        private const string OutdoorProgramTableName = "CR_DICT_PROGRAM_OUTDOOR";
        private const string OutdoorProgramJournalTableName = "CR_DICT_PROG_OUTDOOR_CHANGE_JOUR";
        private const string IndexName = "IND_CR_PROG_OUTDOOR_NAME";

        /// <inheritdoc />
        public override void Up()
        {
            this.Database.AddEntityTable(
                UpdateSchema.OutdoorProgramTableName,
                new Column("NAME", DbType.String),
                new Column("CODE", DbType.String),
                new Column("TYPE_VISIBILITY", DbType.Int32, 4, ColumnProperty.NotNull, 10),
                new Column("TYPE_PROGRAM_STATE", DbType.Int32, 4, ColumnProperty.NotNull, 10),
                new Column("TYPE_PROGRAM", DbType.Int32, 4, ColumnProperty.NotNull, 10),
                new Column("DOC_NUMBER", DbType.String, ColumnProperty.NotNull),
                new Column("DOC_DATE", DbType.DateTime, ColumnProperty.Null),
                new Column("IS_NOT_ADD_OUTDOOR", DbType.Boolean, ColumnProperty.NotNull, false),
                new Column("DESCRIPTION", DbType.String),
                new Column("DOCUMENT_DEPARTMENT", DbType.String),
                new RefColumn("GOV_CUSTOMER_ID", ColumnProperty.Null, "CR_DICT_OUTDOOR_PROGRAM_GOV_CUSTOMER",
                    "GKH_CONTRAGENT", "ID"),
                new RefColumn("PERIOD_ID", ColumnProperty.Null, "CR_DICT_OUTDOOR_PROGRAM_PERIOD",
                    "GKH_DICT_PERIOD", "ID"),
                new RefColumn("NORMATIVE_DOC_ID", ColumnProperty.Null, "CR_DICT_OUTDOOR_PROGRAM_NORMATIVE_DOC",
                    "GKH_DICT_NORMATIVE_DOC", "ID"),
                new RefColumn("FILE_ID", ColumnProperty.Null, "CR_DICT_OUTDOOR_PROGRAM_FILE",
                    "B4_FILE_INFO", "ID"));

            this.Database.AddIndex(UpdateSchema.IndexName, false, UpdateSchema.OutdoorProgramTableName, "NAME");

            this.Database.AddEntityTable(UpdateSchema.OutdoorProgramJournalTableName,
                new RefColumn("PROGRAM_ID", ColumnProperty.NotNull, "CR_PROG_CHAN_JOUR_DICT_OUTDOOR_PROGRAM", UpdateSchema.OutdoorProgramTableName, "ID"),
                new Column("CHANGE_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("MU_COUNT", DbType.Int32),
                new Column("USER_NAME", DbType.String),
                new Column("DESCRIPTION", DbType.String));
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveIndex(UpdateSchema.IndexName, UpdateSchema.OutdoorProgramTableName);
            this.Database.RemoveTable(UpdateSchema.OutdoorProgramJournalTableName);
            this.Database.RemoveTable(UpdateSchema.OutdoorProgramTableName);
        }
    }
}

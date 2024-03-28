namespace Bars.GkhGji.Regions.Tatarstan.Migration.Version_2018090400
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.GkhGji.Enums;

    using Migration = B4.Modules.Ecm7.Framework.Migration;

    [Migration("2018090400")]
    [MigrationDependsOn(typeof(Version_2018070500.UpdateSchema))]
    public class UpdateSchema : Migration 
    {
        /// <inheritdoc />
        public override void Up()
        {
            this.Database.AddEntityTable(
                "GJI_TAT_GIS_GMP_PATTERN_DICT",
                new Column("PATTERN_NAME", DbType.String.WithSize(255)),
                new Column("PATTERN_CODE", DbType.String.WithSize(255)));

            this.Database.AddJoinedSubclassTable("GJI_TATARSTAN_RESOLUTION", "GJI_RESOLUTION", "TATARSTAN_RESOLUTION",
                new RefColumn("PATTERN_DICT_ID", ColumnProperty.None, "GJI_RESOLUTION_PATTERN_DICT", "GJI_TAT_GIS_GMP_PATTERN_DICT", "ID"),
                new Column("SUR_NAME", DbType.String.WithSize(255)),
                new Column("FIRST_NAME", DbType.String.WithSize(255)),
                new Column("PATRONYMIC", DbType.String.WithSize(255)),
                new Column("BIRTH_DATE", DbType.DateTime),
                new Column("BIRTH_PLACE", DbType.String.WithSize(255)),
                new Column("FACT_ADDRESS", DbType.String.WithSize(255)),
                new RefColumn("GJI_DICT_CITIZENSHIP_ID", "GJI_TATARSTAN_RESOLUTION_GJI_DICT_CITIZENSHIP", "GJI_DICT_CITIZENSHIP", "ID"),
                new Column("CITIZENSHIP_TYPE", DbType.Int32),
                new Column("SERIAL_AND_NUMBER", DbType.String.WithSize(255)),
                new Column("ISSUE_DATE", DbType.DateTime),
                new Column("ISSUING_AUTHORITY", DbType.String),
                new Column("COMPANY", DbType.String.WithSize(255)));

            this.Database.ExecuteNonQuery(
                $@"WITH FIO AS(
                        SELECT ID, REGEXP_SPLIT_TO_ARRAY(PHYSICAL_PERSON, E'\\s+') as ARR FROM GJI_RESOLUTION
                   )
                   INSERT INTO GJI_TATARSTAN_RESOLUTION (ID, SUR_NAME, FIRST_NAME, PATRONYMIC)
                   SELECT R.ID, COALESCE(ARR[1], ''), COALESCE(ARR[2], ''), ARR[3]
                   FROM GJI_RESOLUTION R
                   JOIN FIO F ON F.ID = R.ID");

            this.Database.AddJoinedSubclassTable("GJI_TATARSTAN_PROTOCOL_MVD", "GJI_PROTOCOL_MVD", "GJI_TATARSTAN_PROTOCOL_MVD",
                new Column("SUR_NAME", DbType.String.WithSize(255)),
                new Column("FIRST_NAME", DbType.String.WithSize(255)),
                new Column("PATRONYMIC", DbType.String.WithSize(255)),
                new RefColumn("GJI_DICT_CITIZENSHIP_ID", "GJI_TATARSTAN_PROTOCOL_MVD_GJI_DICT_CITIZENSHIP_ID", "GJI_DICT_CITIZENSHIP", "ID"),
                new Column("CITIZENSHIP_TYPE", DbType.Int32),
                new Column("ADDITIONAL_INFO", DbType.String.WithSize(255)));

            this.Database.ExecuteNonQuery(
                $@"WITH FIO AS(
                        SELECT ID, REGEXP_SPLIT_TO_ARRAY(PHYSICAL_PERSON, E'\\s+') as ARR FROM GJI_PROTOCOL_MVD
                   )
                   INSERT INTO GJI_TATARSTAN_PROTOCOL_MVD (ID, SUR_NAME, FIRST_NAME, PATRONYMIC)
                   SELECT M.ID, COALESCE(ARR[1], ''), COALESCE(ARR[2], ''), ARR[3]
                   FROM GJI_PROTOCOL_MVD M
                   JOIN FIO F ON F.ID = M.ID");

            this.Database.AddJoinedSubclassTable("GJI_TATARSTAN_RESOLUTION_PAYFINE", "GJI_RESOLUTION_PAYFINE", "GJI_TATARSTAN_RESOLUTION_PAYFINE",
                new Column("ADMISSION_TYPE", DbType.Int32, ColumnProperty.NotNull, $"{(int)AdmissionType.Unselected}"));
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveTable("GJI_TATARSTAN_RESOLUTION");
            this.Database.RemoveTable("GJI_TAT_GIS_GMP_PATTERN_DICT");
            this.Database.RemoveTable("GJI_TATARSTAN_PROTOCOL_MVD");
        }
    }
}
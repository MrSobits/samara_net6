namespace Bars.GkhGji.Regions.Tatarstan.Migration._2022.Version_2022093000
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.Utils;

    [Migration("2022093000")]
    [MigrationDependsOn(typeof(Version_2022092900.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        private readonly SchemaQualifiedObjectName table = new SchemaQualifiedObjectName { Name = "GJI_TAT_PROTOCOL" };

        /// <inheritdoc />
        public override void Up()
        {
            this.Database.AddJoinedSubclassTable(this.table,
                "GJI_PROTOCOL",
                "GJI_TAT_PROTOCOL",
                new RefColumn("DOCUMENT_PLACE_ID", $"{this.table.Name}_DOCUMENT_PLACE_ID", "B4_FIAS_ADDRESS", "ID"),
                new Column("DATE_WRITE_OUT", DbType.Date),
                new Column("SURNAME", DbType.String),
                new Column("NAME", DbType.String),
                new Column("PATRONYMIC", DbType.String),
                new Column("BIRTH_DATE", DbType.Date),
                new Column("BIRTH_PLACE", DbType.String),
                new Column("FACT_ADDRESS", DbType.String),
                new RefColumn("GJI_DICT_CITIZENSHIP_ID", $"{this.table.Name}_CITIZENSHIP_ID", "GJI_DICT_CITIZENSHIP", "ID"),
                new Column("CITIZENSHIP_TYPE", DbType.Int32),
                new Column("SERIAL_AND_NUMBER", DbType.String),
                new Column("ISSUE_DATE", DbType.Date),
                new Column("ISSUING_AUTHORITY", DbType.String),
                new Column("COMPANY", DbType.String),
                new Column("SNILS", DbType.String));

            this.Database.ExecuteNonQuery($"insert into {this.table.Name} (id) select id from GJI_PROTOCOL");
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveTable(this.table.Name);
        }
    }
}
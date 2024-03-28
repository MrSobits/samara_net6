namespace Bars.GkhGji.Regions.Tatarstan.Migration._2022.Version_2022101200
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.Utils;

    [Migration("2022101200")]
    [MigrationDependsOn(typeof(Version_2022101100.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        private readonly SchemaQualifiedObjectName table = new SchemaQualifiedObjectName { Name = "GJI_MOTIVATED_PRESENTATION_APPEALCITS" };
        private readonly SchemaQualifiedObjectName annexTable = new SchemaQualifiedObjectName { Name = "GJI_MOTIVATED_PRESENTATION_APPEALCITS_ANNEX" };

        /// <inheritdoc />
        public override void Up()
        {
            this.Database.AddJoinedSubclassTable(this.table.Name,
                "GJI_DOCUMENT",
                this.table.Name + "_DOCUMENT",
                new RefColumn("APPEAL_CITS_ID",
                    ColumnProperty.NotNull,
                    this.table.Name + "_APPEAL_CITS",
                    "GJI_APPEAL_CITIZENS",
                    "ID"),
                new Column("PRESENTATION_TYPE", DbType.Int32),
                new RefColumn("OFFICIAL_ID",
                    this.table.Name + "_OFFICIAL",
                    "GKH_DICT_INSPECTOR",
                    "ID"),
                new Column("RESULT_TYPE", DbType.Int32));

            this.Database.AddEntityTable(this.annexTable.Name,
                new RefColumn("MOTIVATED_PRESENTATION_ID",
                    this.annexTable.Name + "_MOTIVATED_PRESENTATION",
                    "GJI_MOTIVATED_PRESENTATION_APPEALCITS",
                    "ID"),
                new Column("DOCUMENT_DATE", DbType.DateTime),
                new Column("NAME", DbType.String.WithSize(50)),
                new Column("DESCRIPTION", DbType.String.WithSize(255)),
                new FileColumn("FILE_ID", this.annexTable.Name + "_FILE"));
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveTable(this.annexTable.Name);
            this.Database.RemoveTable(this.table.Name);
        }
    }
}
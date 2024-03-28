namespace Bars.Gkh.Gis.Migrations._2021.Version_2021100100
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    [MigrationDependsOn(typeof(Version_2021012700.UpdateSchema))]
    [Migration("2021100100")]
    public class UpdateSchema : Migration
    {
        private SchemaQualifiedObjectName TableName => new SchemaQualifiedObjectName
        {
            Schema = "PUBLIC",
            Name = "PGMU_ADDRESSES"
        };

        /// <inheritdoc />
        public override void Up()
        {
            this.Database.AddTable(
                this.TableName,
                new Column("ID", DbType.Int64, ColumnProperty.PrimaryKeyWithIdentity),
                new Column("ERC_CODE", DbType.Int32, ColumnProperty.NotNull),
                new Column("POST_CODE", DbType.String, 6),
                new Column("TOWN", DbType.String, 40),
                new Column("DISTRICT", DbType.String, 40),
                new Column("STREET", DbType.String, 40),
                new Column("HOUSE", DbType.String, 40),
                new Column("BUILDING", DbType.String, 40),
                new Column("APARTMENT", DbType.String, 40),
                new Column("ROOM", DbType.String, 40));

            this.Database.ExecuteNonQuery($@"
                CREATE UNIQUE INDEX idx_unique_record ON {this.TableName}
                (
                	ERC_CODE,
                	lower(COALESCE(POST_CODE, '')),
                	lower(COALESCE(TOWN, '')),
                	lower(COALESCE(DISTRICT, '')),
                	lower(COALESCE(STREET, '')),
                	lower(COALESCE(HOUSE, '')),
                	lower(COALESCE(BUILDING, '')),
                	lower(COALESCE(APARTMENT, '')),
                	lower(COALESCE(ROOM, ''))
                );");

            this.Database.AddIndex("IDX_ERC", false, this.TableName, "ERC_CODE");
            this.Database.AddIndex("IDX_POST", false, this.TableName, "POST_CODE");
            this.Database.AddIndex("IDX_TOWN", false, this.TableName, "TOWN");
            this.Database.AddIndex("IDX_STREET", false, this.TableName, "STREET");
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveTable(this.TableName);
        }
    }
}
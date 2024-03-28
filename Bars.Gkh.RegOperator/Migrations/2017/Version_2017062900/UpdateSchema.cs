namespace Bars.Gkh.RegOperator.Migrations._2017.Version_2017062900
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.Utils;

    [Migration("2017062900")]
    [MigrationDependsOn(typeof(Version_2017062300.UpdateSchema))]
    [MigrationDependsOn(typeof(Version_2017062600.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc/>
        public override void Up()
        {
            this.Database.AddColumn("GKH_ADDRESS_MATCH", new Column("HOUSE_GUID", DbType.String, 36));

            var baseTableName = new SchemaQualifiedObjectName { Name = "CHES_NOT_MATCH_ACC_OWNER", Schema = "IMPORT" };

            this.Database.AddTable(baseTableName,
                new Column("ID", DbType.Int64, ColumnProperty.PrimaryKeyWithIdentity),
                new Column("NAME", DbType.String, 300),
                new Column("OWNER_TYPE", DbType.Int32, ColumnProperty.NotNull),
                new Column("PERS_ACC_NUM", DbType.String, 20, ColumnProperty.NotNull));

            this.Database.AddRefColumn(baseTableName, new RefColumn("PERIOD_ID", "NOT_MATCH_ACC_OWNER_PERIOD_ID", "REGOP_PERIOD", "ID"));
            this.Database.AddIndex("NOT_MATCH_ACC_OWNER_OWNER_TYPE", false, baseTableName, "OWNER_TYPE");
            this.Database.AddIndex("NOT_MATCH_ACC_OWNER_PERS_ACC_NUM", false, baseTableName, "PERS_ACC_NUM");

            this.Database.AddJoinedSubclassTable(
                new SchemaQualifiedObjectName { Name = "CHES_NOT_MATCH_IND_ACC_OWNER", Schema = "IMPORT" },
                baseTableName, 
                "NOT_MATCH_IND_ACC_OWNER",
                new Column("SURNAME", DbType.String, 100),
                new Column("FIRSTNAME", DbType.String, 100, ColumnProperty.NotNull),
                new Column("LASTNAME", DbType.String, 100),
                new Column("BIRTH_DATE", DbType.DateTime));

            this.Database.AddJoinedSubclassTable(
                new SchemaQualifiedObjectName { Name = "CHES_NOT_MATCH_LEGAL_ACC_OWNER", Schema = "IMPORT" },
                baseTableName,
                "NOT_MATCH_LEGAL_ACC_OWNER",
                new Column("INN", DbType.String, 100),
                new Column("KPP", DbType.String, 100));

            baseTableName = new SchemaQualifiedObjectName { Name = "CHES_MATCH_ACC_OWNER", Schema = "IMPORT" };

            this.Database.AddTable(baseTableName,
                new Column("ID", DbType.Int64, ColumnProperty.PrimaryKeyWithIdentity),
                new Column("NAME", DbType.String, 300),
                new Column("OWNER_TYPE", DbType.Int32, ColumnProperty.NotNull),
                new Column("PERS_ACC_NUM", DbType.String, 20, ColumnProperty.NotNull));

            this.Database.AddRefColumn(baseTableName, new RefColumn("OWNER_ID", "MATCH_ACC_OWNER_OWNER_ID", "REGOP_PERIOD", "ID"));
            this.Database.AddIndex("MATCH_ACC_OWNER_OWNER_TYPE", false, baseTableName, "OWNER_TYPE");
            this.Database.AddIndex("MATCH_ACC_OWNER_PERS_ACC_NUM", false, baseTableName, "PERS_ACC_NUM");

            this.Database.AddJoinedSubclassTable(
                new SchemaQualifiedObjectName { Name = "CHES_MATCH_IND_ACC_OWNER", Schema = "IMPORT" },
                baseTableName, 
                "MATCH_IND_ACC_OWNER",
                new Column("SURNAME", DbType.String, 100),
                new Column("FIRSTNAME", DbType.String, 100),
                new Column("LASTNAME", DbType.String, 100, ColumnProperty.NotNull),
                new Column("BIRTH_DATE", DbType.DateTime));

            this.Database.AddJoinedSubclassTable(
                new SchemaQualifiedObjectName { Name = "CHES_MATCH_LEGAL_ACC_OWNER", Schema = "IMPORT" },
                baseTableName,
                "MATCH_LEGAL_ACC_OWNER",
                new Column("INN", DbType.String, 100),
                new Column("KPP", DbType.String, 100));
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveTable(new SchemaQualifiedObjectName { Name = "CHES_MATCH_IND_ACC_OWNER", Schema = "IMPORT" });
            this.Database.RemoveTable(new SchemaQualifiedObjectName { Name = "CHES_MATCH_LEGAL_ACC_OWNER", Schema = "IMPORT" });
            this.Database.RemoveTable(new SchemaQualifiedObjectName { Name = "CHES_MATCH_ACC_OWNER", Schema = "IMPORT" });

            this.Database.RemoveTable(new SchemaQualifiedObjectName { Name = "CHES_NOT_MATCH_IND_ACC_OWNER", Schema = "IMPORT" });
            this.Database.RemoveTable(new SchemaQualifiedObjectName { Name = "CHES_NOT_MATCH_LEGAL_ACC_OWNER", Schema = "IMPORT" });
            this.Database.RemoveTable(new SchemaQualifiedObjectName { Name = "CHES_NOT_MATCH_ACC_OWNER", Schema = "IMPORT" });

            this.Database.RemoveColumn("GKH_ADDRESS_MATCH", "HOUSE_GUID");
        }
    }
}
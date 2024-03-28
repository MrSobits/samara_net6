namespace Bars.GkhGji.Migrations._2023.Version_2023033000
{
    using B4.Modules.Ecm7.Framework;
    using B4.Modules.NH.Migrations.DatabaseExtensions;
    using Gkh;
    using System.Data;

    [Migration("2023033000")]
    [MigrationDependsOn(typeof(_2022.Version_2022110201.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Накатить миграцию
        /// <summary>
        public override void Up()
        {
            Database.AddEntityTable(
           "GJI_DICT_DECISION_REASON_ERKNM",
           new Column("DOC_TYPE", DbType.Int16, ColumnProperty.NotNull, 10),
           new Column("NAME", DbType.String, 1500, ColumnProperty.NotNull),
           new Column("ERKNMID", DbType.String, 5),
           new Column("CODE", DbType.String, 50));
        }

        public override void Down()
        {
            Database.RemoveTable("GJI_DICT_DECISION_REASON_ERKNM");
        }
    }
}
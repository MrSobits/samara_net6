namespace Bars.GkhGji.Migrations._2022.Version_2022070600
{
    using B4.Modules.Ecm7.Framework;
    using B4.Modules.NH.Migrations.DatabaseExtensions;
    using Gkh;
    using System.Data;

    [Migration("2022070600")]
    [MigrationDependsOn(typeof(Version_2022062900.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Накатить миграцию
        /// </summary>
        public override void Up()
        {
            Database.AddEntityTable(
           "GJI_DICT_ERKNMM",
           new Column("CODE", DbType.Int32, 50, ColumnProperty.NotNull),
           new Column("NAME", DbType.String, 300),
           new Column("CODE_ERKNM", DbType.Int32, 50, ColumnProperty.NotNull),
           new Column("ENTITY_NAME", DbType.String, 300));

            Database.AddEntityTable(
           "GJI_DICT_REC_ERKNMM",
           new Column("CODE", DbType.Int32, 50, ColumnProperty.NotNull),
           new Column("NAME", DbType.String, 300),
           new Column("CODE_ERKNM", DbType.String),
           new Column("ENTITY_ID", DbType.Int64, 50, ColumnProperty.None),
           new Column("IDENT_ERKNM", DbType.String, 300),
           new Column("IDENT_SMEV", DbType.String, 300),
           new RefColumn("DICT_ERKNMM_ID", ColumnProperty.NotNull, "GJI_DICT_REC_ERKNMM_DICT_ERKNMM_ID", "GJI_DICT_ERKNMM", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("GJI_DICT_REC_ERKNMM");
            Database.RemoveTable("GJI_DICT_ERKNMM");
        }
    }
}
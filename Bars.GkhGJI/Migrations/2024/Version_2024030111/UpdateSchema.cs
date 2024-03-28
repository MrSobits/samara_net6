namespace Bars.GkhGji.Migrations._2024.Version_2024030111
{
    using B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2024030111")]
    [MigrationDependsOn(typeof(Version_2024030110.UpdateSchema))]
    /// Является Version_2019102900 из ядра
    public class UpdateSchema : Migration
    {
        /// <inheritdoc />
        public override void Up()
        {
            this.Database.ChangeColumn("GJI_DICT_INSPECTION_BASE_TYPE",
                new Column("CODE", DbType.String.WithSize(20), ColumnProperty.NotNull));

            this.Database.ChangeColumn("GJI_DICT_INSPECTION_BASE_TYPE",
                new Column("NAME", DbType.String.WithSize(2000), ColumnProperty.NotNull));

            this.Database.AddColumn("GJI_DICT_INSPECTION_BASE_TYPE",
                new Column("INSPECTION_KIND_ID", DbType.Int32, ColumnProperty.NotNull, defaultValue: 1));

            this.Database.AddColumn("GJI_DICT_INSPECTION_BASE_TYPE",
                new Column("VAL_SEND_ERP", DbType.Boolean, ColumnProperty.None, defaultValue: false));

        }

        /// <inheritdoc />
        public override void Down()
        {
            if (this.Database.ColumnExists("GJI_DICT_INSPECTION_BASE_TYPE", "CODE"))
            {
                this.Database.ExecuteNonQuery("alter table GJI_DICT_INSPECTION_BASE_TYPE alter column CODE type int4 using code::int;");
            }

            this.Database.ChangeColumn("GJI_DICT_INSPECTION_BASE_TYPE",
                new Column("NAME", DbType.String.WithSize(255), ColumnProperty.NotNull));

            this.Database.RemoveColumn("GJI_DICT_INSPECTION_BASE_TYPE", "INSPECTION_KIND_ID");

            this.Database.RemoveColumn("GJI_DICT_INSPECTION_BASE_TYPE", "VAL_SEND_ERP");
        }
    }
}
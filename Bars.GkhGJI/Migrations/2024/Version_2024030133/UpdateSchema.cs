namespace Bars.GkhGji.Migrations._2024.Version_2024030133
{
    using B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2024030133")]
    [MigrationDependsOn(typeof(Version_2024030132.UpdateSchema))]
    /// Является Version_2022093000 из ядра
    public class UpdateSchema : Migration
    {
        private readonly Column column = new Column("NEED_IN_SOPR", DbType.Boolean, ColumnProperty.NotNull, false);
        private readonly SchemaQualifiedObjectName table = new SchemaQualifiedObjectName { Schema = "public", Name = "GJI_DICT_STAT_SUB_SUBJECT" };

        /// <inheritdoc />
        public override void Up()
        {
            this.Database.AddColumn(this.table, this.column);
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveColumn(this.table, this.column.Name);
        }
    }
}
namespace Bars.GkhGji.Migrations._2024.Version_2024030136
{
    using B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.Enums;
    using System.Data;

    [Migration("2024030136")]
    [MigrationDependsOn(typeof(Version_2024030135.UpdateSchema))]
    /// Является Version_2022111100 из ядра
    public class UpdateSchema : Migration
    {
        private readonly SchemaQualifiedObjectName table = new SchemaQualifiedObjectName()
        {
            Name = "GJI_APPCIT_ANSWER"
        };

        private readonly Column newColumn = new Column("IS_SENT", DbType.Int16, ColumnProperty.None, (int)YesNoNotSet.NotSet);

        /// <inheritdoc />
        public override void Up()
        {
            this.Database.AddColumn(this.table, this.newColumn);
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveColumn(this.table, this.newColumn.Name);
        }
    }
}
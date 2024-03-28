namespace Bars.GkhGji.Migrations._2024.Version_2024030123
{
    using B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2024030123")]
    [MigrationDependsOn(typeof(Version_2024030122.UpdateSchema))]
    /// Является Version_2022041300 из ядра
    public class UpdateSchema : Migration
    {
        private SchemaQualifiedObjectName table = new SchemaQualifiedObjectName { Name = "GJI_DICT_EXPERT" };

        /// <inheritdoc />
        public override void Up()
        {
            this.Database.AddColumn(this.table, new Column("EXPERT_TYPE", DbType.Int32));
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveColumn(this.table, "EXPERT_TYPE");
        }
    }
}
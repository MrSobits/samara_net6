namespace Bars.Gkh.Migrations._2023.Version_2023050104
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2023050104")]

    [MigrationDependsOn(typeof(Version_2023050103.UpdateSchema))]

    /// Является Version_2018032100 из ядра
    public class UpdateSchema : Migration
    {
        /// <inheritdoc />
        public override void Up()
        {
            this.Database.AddColumn("GKH_DICT_WALL_MATERIAL", new Column("CODE", DbType.String));
            this.Database.AddColumn("GKH_DICT_ROOFING_MATERIAL", new Column("CODE", DbType.String));
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveColumn("GKH_DICT_WALL_MATERIAL", "CODE");
            this.Database.RemoveColumn("GKH_DICT_ROOFING_MATERIAL", "CODE");
        }
    }
}
namespace Bars.GkhCr.Migrations._2017.Version_2017032200
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    /// <summary>
    /// Миграция 2017032200
    /// </summary>
    [Migration("2017032200")]
    [MigrationDependsOn(typeof(Version_2017031200.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Накатить миграцию
        /// </summary>
        public override void Up()
        {
            this.Database.AddColumn("CR_OBJ_PROTOCOL", "DECISION_OMS", DbType.Boolean, ColumnProperty.NotNull, false);
        }

        /// <summary>
        /// Откатить миграцию
        /// </summary>
        public override void Down()
        {
            this.Database.RemoveColumn("CR_OBJ_PROTOCOL", "DECISION_OMS");
        }
    }
}
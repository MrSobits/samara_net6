namespace Bars.Gkh.Migrations._2016.Version_2016121200
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    /// <summary>
    /// Миграция 2016121200
    /// </summary>
    [Migration("2016121200")]
    [MigrationDependsOn(typeof(Bars.Gkh.Migrations._2016.Version_2016110300.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Вверх
        /// </summary>
        public override void Up()
        {
            this.Database.AddColumn("CLW_JUR_INSTITUTION", new Column("CODE", DbType.String, 50));
        }

        /// <summary>
        /// Вниз
        /// </summary>
        public override void Down()
        {
            this.Database.RemoveColumn("CLW_JUR_INSTITUTION", "CODE");
        }
    }
}
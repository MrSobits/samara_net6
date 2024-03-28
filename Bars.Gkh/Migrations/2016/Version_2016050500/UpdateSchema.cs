namespace Bars.Gkh.Migrations._2016.Version_2016050500
{
    using System.Data;
    using B4.Modules.Ecm7.Framework;
    using B4.Modules.NH.Migrations.DatabaseExtensions;

    /// <summary>
    /// Миграция конвертации запросов в дизайнера
    /// </summary>
    [Migration("2016050500")]
    [MigrationDependsOn(typeof(Version_2016033000.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Накатить миграцию
        /// </summary>
        public override void Up()
        {
            if (!this.Database.ColumnExists("GKH_MORG_JSKTSJ_CONTRACT", "INPUT_MDV_BEGIN_DATE"))
            {
                this.Database.AddColumn("GKH_MORG_JSKTSJ_CONTRACT", new Column("INPUT_MDV_BEGIN_DATE", DbType.Byte));
            }

            if (!this.Database.ColumnExists("GKH_MORG_JSKTSJ_CONTRACT", "INPUT_MDV_END_DATE"))
            {
                this.Database.AddColumn("GKH_MORG_JSKTSJ_CONTRACT", new Column("INPUT_MDV_END_DATE", DbType.Byte));
            }

            if (!this.Database.ColumnExists("GKH_MORG_JSKTSJ_CONTRACT", "DRAWING_PD_DATE"))
            {
                this.Database.AddColumn("GKH_MORG_JSKTSJ_CONTRACT", new Column("DRAWING_PD_DATE", DbType.Byte));
            }

            if (!this.Database.ColumnExists("GKH_MORG_JSKTSJ_CONTRACT", "CONTRACT_FOUNDATION"))
            {
                this.Database.AddColumn("GKH_MORG_JSKTSJ_CONTRACT", new Column("CONTRACT_FOUNDATION", DbType.Int32, ColumnProperty.NotNull, 10));
            }

            if (!this.Database.ColumnExists("GKH_MORG_JSKTSJ_CONTRACT", "TERMINATION_FILE"))
            {
                this.Database.AddRefColumn("GKH_MORG_JSKTSJ_CONTRACT", new RefColumn("TERMINATION_FILE", "MCJSKTSJ_TERM_FILE", "B4_FILE_INFO", "ID"));
            }

            if (!this.Database.ColumnExists("GKH_MORG_JSKTSJ_CONTRACT", "TERMINATION_DATE"))
            {
                this.Database.AddColumn("GKH_MORG_JSKTSJ_CONTRACT", new Column("TERMINATION_DATE", DbType.DateTime));
            }
        }

        /// <summary>
        /// Откатить миграцию
        /// </summary>
        public override void Down()
        {
            if (this.Database.ColumnExists("GKH_MORG_JSKTSJ_CONTRACT", "INPUT_MDV_BEGIN_DATE"))
            {
                this.Database.RemoveColumn("GKH_MORG_JSKTSJ_CONTRACT", "INPUT_MDV_BEGIN_DATE");
            }

            if (this.Database.ColumnExists("GKH_MORG_JSKTSJ_CONTRACT", "INPUT_MDV_END_DATE"))
            {
                this.Database.RemoveColumn("GKH_MORG_JSKTSJ_CONTRACT", "INPUT_MDV_END_DATE");
            }

            if (this.Database.ColumnExists("GKH_MORG_JSKTSJ_CONTRACT", "DRAWING_PD_DATE"))
            {
                this.Database.RemoveColumn("GKH_MORG_JSKTSJ_CONTRACT", "DRAWING_PD_DATE");
            }

            if (this.Database.ColumnExists("GKH_MORG_JSKTSJ_CONTRACT", "CONTRACT_FOUNDATION"))
            {
                this.Database.RemoveColumn("GKH_MORG_JSKTSJ_CONTRACT", "CONTRACT_FOUNDATION");
            }

            if (this.Database.ColumnExists("GKH_MORG_JSKTSJ_CONTRACT", "TERMINATION_FILE"))
            {
                this.Database.RemoveColumn("GKH_MORG_JSKTSJ_CONTRACT", "TERMINATION_FILE");
            }

            if (this.Database.ColumnExists("GKH_MORG_JSKTSJ_CONTRACT", "TERMINATION_DATE"))
            {
                this.Database.RemoveColumn("GKH_MORG_JSKTSJ_CONTRACT", "TERMINATION_DATE");
            }
        }
    }
}

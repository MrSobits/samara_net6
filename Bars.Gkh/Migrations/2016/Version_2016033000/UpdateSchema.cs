namespace Bars.Gkh.Migrations._2016.Version_2016033000
{
    using System.Data;
    using B4.Modules.Ecm7.Framework;
    using B4.Modules.NH.Migrations.DatabaseExtensions;

    /// <summary>
    /// Миграция конвертации запросов в дизайнера
    /// </summary>
    [Migration("2016033000")]
    [MigrationDependsOn(typeof(Version_2016030700.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Накатить миграцию
        /// </summary>
        public override void Up()
        {
            if (!this.Database.ColumnExists("GKH_MORG_CONTRACT_OWNERS", "TERMINATION_FILE"))
            {
                this.Database.AddRefColumn("GKH_MORG_CONTRACT_OWNERS", new RefColumn("TERMINATION_FILE", "MC_TERM_FILE", "B4_FILE_INFO", "ID"));
            }

            if (!this.Database.ColumnExists("GKH_MORG_CONTRACT_OWNERS", "TERMINATION_DATE"))
            {
                this.Database.AddColumn("GKH_MORG_CONTRACT_OWNERS", new Column("TERMINATION_DATE", DbType.DateTime));
            }

            if (!this.Database.ColumnExists("GKH_MORG_JSKTSJ_CONTRACT", "PROTOCOL_NUMBER"))
            {
                this.Database.AddColumn("GKH_MORG_JSKTSJ_CONTRACT", new Column("PROTOCOL_NUMBER", DbType.String));
            }

            if (!this.Database.ColumnExists("GKH_MORG_JSKTSJ_CONTRACT", "PROTOCOL_DATE"))
            {
                this.Database.AddColumn("GKH_MORG_JSKTSJ_CONTRACT", new Column("PROTOCOL_DATE", DbType.DateTime));
            }

            if (!this.Database.ColumnExists("GKH_MORG_JSKTSJ_CONTRACT", "PROTOCOL_FILE_INFO_ID"))
            {
                this.Database.AddRefColumn("GKH_MORG_JSKTSJ_CONTRACT", new RefColumn("PROTOCOL_FILE_INFO_ID", "PROT_JSKTSJ_TERM_FILE", "B4_FILE_INFO", "ID"));
            }
        }

        /// <summary>
        /// Откатить миграцию
        /// </summary>
        public override void Down()
        {
            if (this.Database.ColumnExists("GKH_MORG_CONTRACT_OWNERS", "TERMINATION_FILE"))
            {
                this.Database.RemoveColumn("GKH_MORG_CONTRACT_OWNERS", "TERMINATION_FILE");
            }

            if (this.Database.ColumnExists("GKH_MORG_CONTRACT_OWNERS", "TERMINATION_DATE"))
            {
                this.Database.RemoveColumn("GKH_MORG_CONTRACT_OWNERS", "TERMINATION_DATE");
            }

            if (this.Database.ColumnExists("GKH_MORG_JSKTSJ_CONTRACT", "PROTOCOL_NUMBER"))
            {
                this.Database.RemoveColumn("GKH_MORG_JSKTSJ_CONTRACT", "PROTOCOL_NUMBER");
            }

            if (this.Database.ColumnExists("GKH_MORG_JSKTSJ_CONTRACT", "PROTOCOL_DATE"))
            {
                this.Database.RemoveColumn("GKH_MORG_JSKTSJ_CONTRACT", "PROTOCOL_DATE");
            }

            if (this.Database.ColumnExists("GKH_MORG_JSKTSJ_CONTRACT", "PROTOCOL_FILE_INFO_ID"))
            {
                this.Database.RemoveColumn("GKH_MORG_JSKTSJ_CONTRACT", "PROTOCOL_FILE_INFO_ID");
            }
        }
    }
}
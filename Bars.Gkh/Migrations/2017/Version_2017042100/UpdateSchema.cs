namespace Bars.Gkh.Migrations._2017.Version_2017042100
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    /// <summary>
    /// Миграция 2017042100
    /// </summary>
    [Migration("2017042100")]
    [MigrationDependsOn(typeof(Version_2017032800.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc/>
        public override void Up()
        {
            this.Database.AddColumn("GKH_EMERGENCY_OBJECT", "RESETTLEMENT_DATE", DbType.DateTime);
            this.Database.AddColumn("GKH_EMERGENCY_OBJECT", "FACT_DEMOLITION_DATE", DbType.DateTime);
            this.Database.AddColumn("GKH_EMERGENCY_OBJECT", "FACT_RESETTLEMENT_DATE", DbType.DateTime);
            this.Database.AddColumn("GKH_EMERGENCY_OBJECT", "INHABITANT_NUMBER", DbType.Int32);

            this.Database.AddColumn("GKH_EMERGENCY_OBJECT", "EMG_DOCUMENT_NAME", DbType.String, 100);
            this.Database.AddColumn("GKH_EMERGENCY_OBJECT", "EMG_DOCUMENT_NUM", DbType.String, 50);
            this.Database.AddColumn("GKH_EMERGENCY_OBJECT", "EMG_DOCUMENT_DATE", DbType.DateTime);
            this.Database.AddRefColumn("GKH_EMERGENCY_OBJECT", new RefColumn("EMG_FILE_INFO_ID", "GKH_EMER_EMG_FILE", "B4_FILE_INFO", "ID"));
        }

        /// <inheritdoc/>
        public override void Down()
        {
            this.Database.RemoveColumn("GKH_EMERGENCY_OBJECT", "RESETTLEMENT_DATE");
            this.Database.RemoveColumn("GKH_EMERGENCY_OBJECT", "FACT_DEMOLITION_DATE");
            this.Database.RemoveColumn("GKH_EMERGENCY_OBJECT", "FACT_RESETTLEMENT_DATE");
            this.Database.RemoveColumn("GKH_EMERGENCY_OBJECT", "INHABITANT_NUMBER");
            this.Database.RemoveColumn("GKH_EMERGENCY_OBJECT", "EMG_DOCUMENT_NAME");
            this.Database.RemoveColumn("GKH_EMERGENCY_OBJECT", "EMG_DOCUMENT_NUM");
            this.Database.RemoveColumn("GKH_EMERGENCY_OBJECT", "EMG_DOCUMENT_DATE");
            this.Database.RemoveColumn("GKH_EMERGENCY_OBJECT", "EMG_FILE_INFO_ID");
        }
    }
}
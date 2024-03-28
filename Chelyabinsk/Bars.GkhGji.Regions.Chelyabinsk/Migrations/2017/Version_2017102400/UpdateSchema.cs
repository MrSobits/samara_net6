namespace Bars.GkhGji.Regions.Chelyabinsk.Migrations.Version_2017102400
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2017102400")]
    [MigrationDependsOn(typeof(Version_1.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            this.Database.AddEntityTable("GJI_CH_APPCIT_ADMONITION",
                new Column("APPCIT_ID", DbType.Int64),
                new Column("CONTRAGENT_ID", DbType.Int64),
                new Column("DOCUMENT_DATE", DbType.DateTime),
                new Column("PERFORMANCE_DATE", DbType.DateTime),
                new Column("PERFORMANCE_FACT_DATE", DbType.DateTime),
                new Column("DOCUMENT_NAME", DbType.String),
                new Column("DOCUMENT_NUM", DbType.String, 30),
                new Column("INSPECTOR_ID", DbType.Int64),
                new Column("ANSWERFILE_INFO_ID", DbType.Int64, 22),
                new Column("EXECUTOR_ID", DbType.Int64, 22),
            new Column("FILE_INFO_ID", DbType.Int64));

            Database.AddForeignKey("FK_GJI_CH_APPCIT_ADMON_FILE", "GJI_CH_APPCIT_ADMONITION", "FILE_INFO_ID", "B4_FILE_INFO", "ID");
            Database.AddForeignKey("FK_GJI_CH_ADMONITION_APPCIT", "GJI_CH_APPCIT_ADMONITION", "APPCIT_ID", "GJI_APPEAL_CITIZENS", "ID");
            Database.AddForeignKey("FK_GJI_CH_ADMONITION_CONTR", "GJI_CH_APPCIT_ADMONITION", "CONTRAGENT_ID", "GKH_CONTRAGENT", "ID");
            Database.AddForeignKey("FK_GJI_ADMONITION_INSPECTOR", "GJI_CH_APPCIT_ADMONITION", "INSPECTOR_ID", "GKH_DICT_INSPECTOR", "ID");

            this.Database.AddEntityTable("GJI_CH_APPCIT_ADMON_VIOLATION",
                new Column("PLANED_DATE", DbType.DateTime),
                new Column("FACT_DATE", DbType.DateTime),
                new Column("APPCIT_ADMONITION_ID", DbType.Int64),
                new Column("VIOLATION_ID", DbType.Int64));

            Database.AddForeignKey("FK_GJI_CHAAV_VIOL", "GJI_CH_APPCIT_ADMON_VIOLATION", "VIOLATION_ID", "GJI_DICT_VIOLATION", "ID");
            Database.AddForeignKey("FK_GJI_CHAAV_ADMONITION", "GJI_CH_APPCIT_ADMON_VIOLATION", "APPCIT_ADMONITION_ID", "GJI_CH_APPCIT_ADMONITION", "ID");

        }

        /// <summary>
        /// Откатить миграцию
        /// </summary>
        public override void Down()
        {
            Database.RemoveTable("GJI_CH_APPCIT_ADMON_VIOLATION");
            Database.RemoveTable("GJI_CH_APPCIT_ADMONITION");
        }
    }
}
namespace Bars.GkhGji.Regions.Tatarstan.Migration._2022.Version_2022042605
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2022042605")]
    [MigrationDependsOn(typeof(Version_2022042604.UpdateSchema))]
    public class UpdateSchema: Migration
    {
        public const string DisposalTableName = "GJI_TAT_DISPOSAL";
        public const string KnmReasonTableName = "GJI_KNM_REASON";
        /// <inheritdoc />
        public override void Up()
        {
            this.Database.AddColumn(DisposalTableName, new Column("INFORMATION_ABOUT_HARM", DbType.String));
            
            this.Database.AddEntityTable(KnmReasonTableName,
                new Column("DESCRIPTION",DbType.String),
                new RefColumn("DECISION_ID", ColumnProperty.NotNull, "KNM_REASON_DECISION",
                    "GJI_DECISION","ID"),
                new RefColumn("ERKNM_TYPE_DOCUMENTS_ID", ColumnProperty.NotNull, "KNM_REASON_DOCUMENTS_ERKNM",
                    "GJI_DICT_ERKNM_TYPE_DOCUMENT", "ID"),
                new RefColumn("FILE_ID", "KNM_REASON_FILE", 
                    "B4_FILE_INFO", "ID")
            );
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveColumn(DisposalTableName,"INFORMATION_ABOUT_HARM");
            this.Database.RemoveTable(KnmReasonTableName);
        }
    }
}
namespace Bars.GkhGji.Migrations._2021.Version_2021120600
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2021120600")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(Version_2021120300.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {

            Database.AddColumn("GJI_RESOLUTION_ROSPOTREBNADZOR_ANNEX", new Column("MESSAGE_CHECK", DbType.Int16, ColumnProperty.NotNull, (int)Bars.GkhGji.Enums.MessageCheck.NotSet));
            Database.AddColumn("GJI_ACTSURVEY_ANNEX", new Column("MESSAGE_CHECK", DbType.Int16, ColumnProperty.NotNull, (int)Bars.GkhGji.Enums.MessageCheck.NotSet));
            Database.AddColumn("GJI_RESOLUTION_ANNEX", new Column("MESSAGE_CHECK", DbType.Int16, ColumnProperty.NotNull, (int)Bars.GkhGji.Enums.MessageCheck.NotSet));
            Database.AddColumn("GJI_RESOLPROS_ANNEX", new Column("MESSAGE_CHECK", DbType.Int16, ColumnProperty.NotNull, (int)Bars.GkhGji.Enums.MessageCheck.NotSet));

            Database.AddColumn("GJI_PROTOCOLRSO_ANNEX", new Column("MESSAGE_CHECK", DbType.Int16, ColumnProperty.NotNull, (int)Bars.GkhGji.Enums.MessageCheck.NotSet));
            Database.AddColumn("GJI_PROT_MVD_ANNEX", new Column("MESSAGE_CHECK", DbType.Int16, ColumnProperty.NotNull, (int)Bars.GkhGji.Enums.MessageCheck.NotSet));
            Database.AddColumn("GJI_PROTOCOLMHC_ANNEX", new Column("MESSAGE_CHECK", DbType.Int16, ColumnProperty.NotNull, (int)Bars.GkhGji.Enums.MessageCheck.NotSet));
            Database.AddColumn("GJI_PROTOCOL_ANNEX", new Column("MESSAGE_CHECK", DbType.Int16, ColumnProperty.NotNull, (int)Bars.GkhGji.Enums.MessageCheck.NotSet));

            Database.AddColumn("GJI_PRESENTATION_ANNEX", new Column("MESSAGE_CHECK", DbType.Int16, ColumnProperty.NotNull, (int)Bars.GkhGji.Enums.MessageCheck.NotSet));
            Database.AddColumn("GJI_PRESCRIPTION_ANNEX", new Column("MESSAGE_CHECK", DbType.Int16, ColumnProperty.NotNull, (int)Bars.GkhGji.Enums.MessageCheck.NotSet));
            Database.AddColumn("GJI_DISPOSAL_ANNEX", new Column("MESSAGE_CHECK", DbType.Int16, ColumnProperty.NotNull, (int)Bars.GkhGji.Enums.MessageCheck.NotSet));
            Database.AddColumn("GJI_ACTCHECK_ANNEX", new Column("MESSAGE_CHECK", DbType.Int16, ColumnProperty.NotNull, (int)Bars.GkhGji.Enums.MessageCheck.NotSet));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_ACTCHECK_ANNEX", "MESSAGE_CHECK");
            Database.RemoveColumn("GJI_DISPOSAL_ANNEX", "MESSAGE_CHECK");
            Database.RemoveColumn("GJI_PRESCRIPTION_ANNEX", "MESSAGE_CHECK");
            Database.RemoveColumn("GJI_PRESENTATION_ANNEX", "MESSAGE_CHECK");
            Database.RemoveColumn("GJI_PROTOCOL_ANNEX", "MESSAGE_CHECK");
            Database.RemoveColumn("GJI_PROTOCOLMHC_ANNEX", "MESSAGE_CHECK");
            Database.RemoveColumn("GJI_PROT_MVD_ANNEX", "MESSAGE_CHECK");
            Database.RemoveColumn("GJI_PROTOCOLRSO_ANNEX", "MESSAGE_CHECK");
            Database.RemoveColumn("GJI_RESOLPROS_ANNEX", "MESSAGE_CHECK");
            Database.RemoveColumn("GJI_RESOLUTION_ANNEX", "MESSAGE_CHECK");
            Database.RemoveColumn("GJI_ACTSURVEY_ANNEX", "MESSAGE_CHECK");
            Database.RemoveColumn("GJI_RESOLUTION_ROSPOTREBNADZOR_ANNEX", "MESSAGE_CHECK");














        }
    }
}
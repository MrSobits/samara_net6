namespace Bars.GkhGji.Regions.Chelyabinsk.Migrations.Version_2022071900
{
    using B4.Modules.Ecm7.Framework;
    using B4.Modules.NH.Migrations.DatabaseExtensions;
    using Gkh;
    using System.Data;

    [Migration("2022071900")]
    [MigrationDependsOn(typeof(Version_2022070400.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Накатить миграцию
        /// </summary>
        public override void Up()
        {
            Database.AddColumn("GJI_CH_APPCIT_ADMONITION", new Column("PAYER_TYPE_APPCIT_ADMONITION", DbType.Int32, ColumnProperty.NotNull, 30));
            Database.AddColumn("GJI_CH_APPCIT_ADMONITION", new Column("INN_APPCIT_ADMONITION", DbType.String));
            Database.AddColumn("GJI_CH_APPCIT_ADMONITION", new Column("KPP_APPCIT_ADMONITION", DbType.String));
            Database.AddColumn("GJI_CH_APPCIT_ADMONITION", new Column("FIO_APPCIT_ADMONITION", DbType.String));
            Database.AddColumn("GJI_CH_APPCIT_ADMONITION", new Column("DOCUMENT_NUMB_FIZ_APPCIT_ADMONITION", DbType.String));
            Database.AddColumn("GJI_CH_APPCIT_ADMONITION", new Column("DOCUMENT_SERIAL_APPCIT_ADMONITION", DbType.String));
            Database.AddRefColumn("GJI_CH_APPCIT_ADMONITION",
                new RefColumn("PP_DOC_TYPE_APPCIT_ADMONITION_ID", ColumnProperty.None, "DOC_TYPE_ADMONITION_PHYSICAL_PERSON_DOC_TYPE", "GJI_DICT_PHYSICAL_PERSON_DOC_TYPE", "ID"));



        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_CH_APPCIT_ADMONITION", "PP_DOC_TYPE_APPCIT_ADMONITION_ID");
            Database.RemoveColumn("GJI_CH_APPCIT_ADMONITION", "DOCUMENT_SERIAL_APPCIT_ADMONITION");
            Database.RemoveColumn("GJI_CH_APPCIT_ADMONITION", "DOCUMENT_NUMB_FIZ_APPCIT_ADMONITION");
            Database.RemoveColumn("GJI_CH_APPCIT_ADMONITION", "FIO_APPCIT_ADMONITION");
            Database.RemoveColumn("GJI_CH_APPCIT_ADMONITION", "KPP_APPCIT_ADMONITION");
            Database.RemoveColumn("GJI_CH_APPCIT_ADMONITION", "INN_APPCIT_ADMONITION");
            Database.RemoveColumn("GJI_CH_APPCIT_ADMONITION", "PAYER_TYPE_APPCIT_ADMONITION");
        }
    }
}
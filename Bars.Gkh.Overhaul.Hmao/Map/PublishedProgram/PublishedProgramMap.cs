namespace Bars.Gkh.Overhaul.Hmao.Map
{
    using Bars.Gkh.Map;

    /// <summary>Маппинг для "Опубликованная программа"</summary>
    public class PublishedProgramMap : BaseImportableEntityMap<Entities.PublishedProgram>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public PublishedProgramMap()
            :
                base("Опубликованная программа", "OVRHL_PUBLISH_PRG")
        {
        }

        /// <summary>
        /// Мап
        /// </summary>
        protected override void Map()
        {
            this.Reference(x => x.ProgramVersion, "Ссылка на версию").Column("VERSION_ID").NotNull().Fetch();
            this.Reference(x => x.State, "Статус").Column("STATE_ID").Fetch();
            this.Property(x => x.EcpSigned, "Подписан ЭЦП").Column("ECP_SIGNED").DefaultValue(false).NotNull();
            this.Reference(x => x.FileSign, "Файл подписи").Column("FILE_ID").Fetch();
            this.Reference(x => x.FileXml, "Файл xml").Column("FILE_XML_ID").Fetch();
            this.Reference(x => x.FilePdf, "Файл pdf").Column("FILE_PDF_ID").Fetch();
            this.Reference(x => x.FileCertificate, "Файл сертификата").Column("FILE_CER_ID").Fetch();
            this.Property(x => x.SignDate, "Дата подписания").Column("SIGN_DATE");
            this.Property(x => x.PublishDate, "Дата опубликования").Column("PUBLISH_DATE");
            this.Property(x => x.TotalRoCount, "Количество домов в программе").Column("TOTAL_RO_COUNT");
            this.Property(x => x.IncludedRoCount, "Количество домов включеных в программу").Column("INCLUDED_RO_COUNT");
            this.Property(x => x.ExcludedRoCount, "Количество домов исключенных из программы").Column("EXCLUDED_RO_COUNT");
        }
    }
}
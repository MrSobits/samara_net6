namespace Bars.GkhGji.LogMap.Prescription
{
    using B4.Modules.NHibernateChangeLog;
    using Entities;

    public class PrescriptionArticleLawLogMap : AuditLogMap<PrescriptionArticleLaw>
    {
        public PrescriptionArticleLawLogMap()
        {
            this.Name("Статьи закона в предписании ГЖИ");
            this.Description(x => x.Prescription.DocumentNumber ?? string.Empty);

            this.MapProperty(x => x.ArticleLaw.Article, "Article", "Статья");
            this.MapProperty(x => x.ArticleLaw.Description, "Description", "Описание");
        }
    }
} 
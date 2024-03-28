namespace Bars.GkhGji.LogMap.Protocol
{
    using B4.Modules.NHibernateChangeLog;
    using Entities;

    public class ProtocolArticleLawLogMap : AuditLogMap<ProtocolArticleLaw>
    {
        public ProtocolArticleLawLogMap()
        {
            this.Name("Статьи закона в предписании ГЖИ");
            this.Description(x => x.Protocol.DocumentNumber ?? "");

            this.MapProperty(x => x.ArticleLaw.Article, "Article", "Статья");
            this.MapProperty(x => x.ArticleLaw.Description, "Description", "Описание");
        }
    }
}
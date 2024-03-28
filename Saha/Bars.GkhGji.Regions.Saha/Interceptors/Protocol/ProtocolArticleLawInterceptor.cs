namespace Bars.GkhGji.Regions.Saha.Interceptors
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    public class ProtocolArticleLawInterceptor : EmptyDomainInterceptor<ProtocolArticleLaw>
    {
        private const string articleLawCode19 = "1";

        public override IDataResult BeforeCreateAction(IDomainService<ProtocolArticleLaw> service, ProtocolArticleLaw entity)
        {
            /*
            Если для протокола созданного из предписания патаются создат ьтатью с кодом 19.5 то выдаем сообщение - По требованию 41181
            */

            var docChildrenDomain = Container.Resolve<IDomainService<DocumentGjiChildren>>();
            var artLawDomain = Container.Resolve<IDomainService<ArticleLawGji>>();

            try
            {
                if (artLawDomain.GetAll().Any(x => x.Id == entity.ArticleLaw.Id && x.Code == articleLawCode19) &&
                    docChildrenDomain.GetAll().Any(x => x.Children.Id == entity.Protocol.Id && x.Parent.TypeDocumentGji == TypeDocumentGji.Prescription))
                {
                    return
                        Failure(
                            "Добавление статьи 19.5 ч. 1 КОАП РФ невозможно. Протокол по данной статье необходимо создавать из акта проверки предписания");
                }

                return Success();
            }
            finally 
            {
                Container.Release(docChildrenDomain);
            }
        }
    }
}

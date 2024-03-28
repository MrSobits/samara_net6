namespace Bars.GkhGji.Regions.Tomsk.Interceptors
{
    using System.Linq;
    using System;
    using B4;
    using B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    using Entities;

    public class RequirementServiceInterceptor : EmptyDomainInterceptor<Requirement>
    {
        public IStateProvider StateProviderService { get; set; }

        public IDomainService<RequirementDocument> ReqDocService { get; set; }

        public IDomainService<DocumentGjiChildren> ChildrenService { get; set; }

        public override IDataResult BeforeCreateAction(IDomainService<Requirement> service, Requirement entity)
        {
            
            /*
            * - после формирования акта запретить формирование требований в приказе."Невозможно добавить требование, потому что уже сформирован акт проверки".
            */
            if (
                ChildrenService.GetAll()
                                .Any(
                                    x =>
                                    x.Parent.Id == entity.Document.Id
                                    && x.Parent.TypeDocumentGji == TypeDocumentGji.Disposal
                                    && x.Children.TypeDocumentGji == TypeDocumentGji.ActCheck))
            {
                return new BaseDataResult(false, "Невозможно добавить требование, так как уже сформирован акт проверки");
            }

            if (!entity.DocumentDate.HasValue)
            {
                entity.DocumentDate = DateTime.Now.Date;
            }

            // Перед сохранением меняем присваиваем статус Черновик к документу
            var stateProvider = Container.Resolve<IStateProvider>();
            stateProvider.SetDefaultState(entity);

            return Success();
        }

        public override IDataResult BeforeDeleteAction(IDomainService<Requirement> service, Requirement entity)
        {
            var reqDocService =  Container.Resolve<IDomainService<RequirementDocument>>();

            if (reqDocService.GetAll().Any(x => x.Document.Id == entity.Id))
            {
                return Failure("Требование имеет зависимые документы");
            }

            var reqArticleService = Container.Resolve<IDomainService<RequirementArticleLaw>>();

            reqArticleService.GetAll()
                .Where(x => x.Requirement.Id == entity.Id)
                .Select(x => x.Id)
                .ForEach(x => reqArticleService.Delete(x));

            var ids = ReqDocService.GetAll().Where(x => x.Requirement.Id == entity.Id).Select(x => x.Id).ToList();

            foreach (var id in ids)
            {
                ReqDocService.Delete(id);
            }

            return Success();
        }
    }
}
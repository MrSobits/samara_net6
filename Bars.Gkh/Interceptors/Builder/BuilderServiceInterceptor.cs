namespace Bars.Gkh.Interceptors
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;

    public class BuilderServiceInterceptor : EmptyDomainInterceptor<Builder>
    {
        public override IDataResult BeforeCreateAction(IDomainService<Builder> service, Builder entity)
        {
            var result = CheckEntity(entity);

            if (!result.Success)
            {
                return result;
            }

            return service.GetAll().Any(x => x.Contragent.Id == entity.Contragent.Id)
                ? Failure("Подрядная организация с таким контрагентом уже создана")
                : Success();
        }

        public override IDataResult BeforeUpdateAction(IDomainService<Builder> service, Builder entity)
        {
            var result = CheckEntity(entity);

            if (!result.Success)
            {
                return result;
            }

            return service.GetAll().Any(x => x.Contragent.Id == entity.Contragent.Id && x.Id != entity.Id)
                ? Failure("Подрядная организация с таким контрагентом уже создана")
                : Success();
        }

        public override IDataResult BeforeDeleteAction(IDomainService<Builder> service, Builder entity)
        {
            var buildDocService = Container.Resolve<IDomainService<BuilderDocument>>();
            var buildFeedService = Container.Resolve<IDomainService<BuilderFeedback>>();
            var buildLoanService = Container.Resolve<IDomainService<BuilderLoan>>();
            var buildProdBaseService = Container.Resolve<IDomainService<BuilderProductionBase>>();
            var buildSroInfoService = Container.Resolve<IDomainService<BuilderSroInfo>>();
            var buildTechnService = Container.Resolve<IDomainService<BuilderTechnique>>();
            var buildWorkService = Container.Resolve<IDomainService<BuilderWorkforce>>();
            var roCurrRepairService = Container.Resolve<IDomainService<RealityObjectCurentRepair>>();

            try
            {
                var buildDocList =
                    buildDocService.GetAll().Where(x => x.Builder.Id == entity.Id).Select(x => x.Id).ToArray();
                var buildFeedList =
                    buildFeedService.GetAll()
                        .Where(x => x.Builder.Id == entity.Id)
                        .Select(x => x.Id)
                        .ToArray();
                var buildLoanList =
                    buildLoanService.GetAll()
                        .Where(x => x.Builder.Id == entity.Id)
                        .Select(x => x.Id)
                        .ToArray();
                var buildProdBaseList =
                    buildProdBaseService.GetAll()
                        .Where(x => x.Builder.Id == entity.Id)
                        .Select(x => x.Id)
                        .ToArray();
                var buildSroInfoList =
                    buildSroInfoService.GetAll()
                        .Where(x => x.Builder.Id == entity.Id)
                        .Select(x => x.Id)
                        .ToArray();
                var buildTechnList =
                    buildTechnService.GetAll()
                        .Where(x => x.Builder.Id == entity.Id)
                        .Select(x => x.Id)
                        .ToArray();
                var buildWorkList =
                    buildWorkService.GetAll()
                        .Where(x => x.Builder.Id == entity.Id)
                        .Select(x => x.Id)
                        .ToArray();
                var roCurrRepairList =
                    roCurrRepairService.GetAll()
                        .Where(x => x.Builder.Id == entity.Id)
                        .Select(x => x.Id)
                        .ToArray();
                foreach (var value in buildDocList)
                {
                    buildDocService.Delete(value);
                }
                foreach (var value in buildFeedList)
                {
                    buildFeedService.Delete(value);
                }
                foreach (var value in buildLoanList)
                {
                    buildLoanService.Delete(value);
                }
                foreach (var value in buildProdBaseList)
                {
                    buildProdBaseService.Delete(value);
                }
                foreach (var value in buildSroInfoList)
                {
                    buildSroInfoService.Delete(value);
                }
                foreach (var value in buildTechnList)
                {
                    buildTechnService.Delete(value);
                }
                foreach (var value in buildWorkList)
                {
                    buildWorkService.Delete(value);
                }
                foreach (var value in roCurrRepairList)
                {
                    roCurrRepairService.Delete(value);
                }

                return Success();
            }
            catch (Exception)
            {
                return Failure("Не удалось удалить связанные записи");
            }
            finally
            {
                Container.Release(buildDocService);
                Container.Release(buildFeedService);
                Container.Release(buildLoanService);
                Container.Release(buildProdBaseService);
                Container.Release(buildSroInfoService);
                Container.Release(buildTechnService);
                Container.Release(buildWorkService);
                Container.Release(roCurrRepairService);
            }
        }

        private IDataResult CheckEntity(Builder entity)
        {
            if (entity.Contragent == null)
            {
                return Failure("Не заполнены обязательные поля: Контрагент");
            }

            if (entity.Description.IsNotEmpty() && entity.Description.Length > 500)
            {
                return Failure("Количество знаков в поле Описание не должно превышать 500 символов");
            }

            return Success();
        }
    }
}

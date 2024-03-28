using Bars.B4;
using Bars.B4.DataAccess;
using Bars.B4.IoC;
using Bars.B4.Utils;
using Bars.GkhGji.Regions.Tatarstan.Entities.Dict.KnmActions;
using System.Linq;

namespace Bars.GkhGji.Regions.Tatarstan.Interceptors.Dict
{
    public class KnmActionInterceptor : EmptyDomainInterceptor<KnmAction>
    {
        public override IDataResult BeforeDeleteAction(IDomainService<KnmAction> service, KnmAction entity)
        {
            var knmActionKnmTypeDomain = this.Container.ResolveDomain<KnmActionKnmType>();
            var knmActionControlTypeDomain = this.Container.ResolveDomain<KnmActionControlType>();
            var knmActionKindActionDomain = this.Container.ResolveDomain<KnmActionKindAction>();

            using (this.Container.Using(knmActionKnmTypeDomain, knmActionControlTypeDomain, knmActionKindActionDomain))
            {
                knmActionKnmTypeDomain.GetAll()
                    .Where(x => x.KnmAction.Id == entity.Id)
                    .Select(x => x.Id)
                    .ForEach(x => knmActionKnmTypeDomain.Delete(x));

                knmActionControlTypeDomain.GetAll()
                    .Where(x => x.KnmAction.Id == entity.Id)
                    .Select(x => x.Id)
                    .ForEach(x => knmActionControlTypeDomain.Delete(x));
                
                knmActionKindActionDomain.GetAll()
                    .Where(x => x.KnmAction.Id == entity.Id)
                    .Select(x => x.Id)
                    .ForEach(x => knmActionKindActionDomain.Delete(x));
            }

            return base.BeforeDeleteAction(service, entity);
        }

        public override IDataResult BeforeCreateAction(IDomainService<KnmAction> service, KnmAction entity)
            => CheckForDuplicates(service, entity);

        public override IDataResult BeforeUpdateAction(IDomainService<KnmAction> service, KnmAction entity)
            => CheckForDuplicates(service, entity);

        /// <summary>
        /// Проверка на уже существующие записи
        /// </summary>
        private IDataResult CheckForDuplicates(IDomainService<KnmAction> service, KnmAction entity)
            => service.GetAll().Any(x => x.ActCheckActionType == entity.ActCheckActionType && x.Id != entity.Id)
                ? this.Failure($"Запись с действием \"{entity.ActCheckActionType.GetDisplayName()}\" уже добавлена")
                : this.Success();
    }
}

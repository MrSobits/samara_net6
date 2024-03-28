using Bars.B4;
using Bars.B4.IoC;
using Bars.B4.Utils;
using Bars.GisIntegration.Base.Entities;
using System.Linq;

namespace Bars.GisIntegration.Base.Interceptors
{
    public class RisTaskInterceptor : UserEntityInterceptor<RisTask>
    {
        public override IDataResult BeforeDeleteAction(IDomainService<RisTask> service, RisTask entity)
        {
            var risTaskTriggerDomain = Container.Resolve<IDomainService<RisTaskTrigger>>();
            using (Container.Using(risTaskTriggerDomain))
            {
                risTaskTriggerDomain.GetAll()
                    .Where(x => x.Task == entity)
                    .ForEach(x => risTaskTriggerDomain.Delete(x.Id));
            }

            this.AdditionalActions(entity);

            return base.BeforeDeleteAction(service, entity);
        }

        /// <summary>
        /// Дополнительные действия перед удалением сущности (для переопределения в других модулях)
        /// </summary>
        /// <param name="entity">Сущность для удаления</param>
        public virtual void AdditionalActions(RisTask entity)
        {
        }
    }
}
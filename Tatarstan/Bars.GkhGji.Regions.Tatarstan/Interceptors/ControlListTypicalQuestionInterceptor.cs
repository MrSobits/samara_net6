namespace Bars.GkhGji.Regions.Tatarstan.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Dict;
    using Bars.GkhGji.Regions.Tatarstan.Entities.TatarstanControlList;

    public class ControlListTypicalQuestionInterceptor : EmptyDomainInterceptor<ControlListTypicalQuestion>
    {
        public override IDataResult BeforeDeleteAction(IDomainService<ControlListTypicalQuestion> service, ControlListTypicalQuestion entity)
        {
            var controlListQuestionService = this.Container.Resolve<IDomainService<TatarstanControlListQuestion>>();

            using (this.Container.Using(controlListQuestionService))
            {
                return controlListQuestionService.GetAll().Any(w => w.TypicalQuestion.Id == entity.Id)
                    ? this.Failure($"Вопрос <b>{entity.Question}</b> используется в проверочном листе")
                    : this.Success();
            }
        }
    }
}

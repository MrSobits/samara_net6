namespace Bars.GkhGji.Regions.Tatarstan.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Dict;
    using Bars.GkhGji.Regions.Tatarstan.Entities.TatarstanControlList;

    public class ControlListTypicalAnswerInterceptor : EmptyDomainInterceptor<ControlListTypicalAnswer>
    {
        public override IDataResult BeforeDeleteAction(IDomainService<ControlListTypicalAnswer> service, ControlListTypicalAnswer entity)
        {
            var controlListQuestionService = this.Container.Resolve<IDomainService<TatarstanControlListQuestion>>();

            using (this.Container.Using(controlListQuestionService))
            {
                return controlListQuestionService.GetAll().Any(w => w.TypicalAnswer.Id == entity.Id)
                    ? this.Failure($"Ответ <b>{entity.Answer}</b> используется в проверочном листе")
                    : this.Success();
            }
        }
    }
}

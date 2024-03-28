namespace Bars.GkhGji.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Entities;

    public class AnswerContentGjiInterceptor : EmptyDomainInterceptor<AnswerContentGji>
    {
        public override IDataResult BeforeDeleteAction(IDomainService<AnswerContentGji> service, AnswerContentGji entity)
        {
            if (Container.Resolve<IDomainService<AppealCitsAnswer>>().GetAll().Any(x => x.AnswerContent.Id == entity.Id))
            {
                return Failure("Существуют связанные записи в следующих таблицах: Ответы по обращению граждан;");
            }

            return this.Success();
        }
    }
}
namespace Bars.GkhGji.Regions.Tatarstan.DomainService.Impl
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Dict;
    using Castle.Windsor;

    /// <summary>
    /// Сервис работы с ControlListTypicalQuestion
    /// </summary>
    public class ControlListTypicalQuestionService : IControlListTypicalQuestionService
    {
        #region Поля
        private readonly IDomainService<ControlListTypicalQuestion> controlListTypicalQuestionDomain;
        private readonly IDomainService<MandatoryReqs> mandatoryReqDomain;
        private readonly IWindsorContainer container;
        #endregion

        public ControlListTypicalQuestionService(IDomainService<ControlListTypicalQuestion> controlListTypicalQuestionDomain, 
            IDomainService<MandatoryReqs> mandatoryReqDomain,
            IWindsorContainer container)
        {
            this.controlListTypicalQuestionDomain = controlListTypicalQuestionDomain;
            this.mandatoryReqDomain = mandatoryReqDomain;
            this.container = container;
        }

        public IDataResult UpdateControlListTypicalQuestion(BaseParams baseParams)
        {
            var addIds = baseParams.Params.GetAs<long[]>("addIds") ?? new long[0];
            var deleteIds = baseParams.Params.GetAs<long[]>("deleteIds") ?? new long[0];
            var mandatoryReqId = baseParams.Params.GetAsId("mandatoryReqId");

            if (mandatoryReqId == default(long) || !addIds.Any() && !deleteIds.Any())
            {
                return new BaseDataResult { Success = false, Message = "Некорректный идентификатор" };
            }

            using (container.Using(controlListTypicalQuestionDomain, mandatoryReqDomain))
            {
                var questions = controlListTypicalQuestionDomain.GetAll()
                    .Where(w => w.MandatoryRequirement.Id == mandatoryReqId || w.MandatoryRequirement == null)
                    .ToDictionary(d => d.Id);

                if (deleteIds.Any())
                {
                    foreach (var id in deleteIds)
                    {
                        var question = questions[id];
                        question.MandatoryRequirement = null;
                        controlListTypicalQuestionDomain.Update(question);
                    }
                }

                if (addIds.Any())
                {
                    var mandatory = mandatoryReqDomain.Get(mandatoryReqId);

                    foreach (var id in addIds.Distinct())
                    {
                        var question = questions[id];
                        question.MandatoryRequirement = mandatory;
                        controlListTypicalQuestionDomain.Update(question);
                    }
                }

                return new BaseDataResult();
            }
        }
    }
}

namespace Bars.Gkh.Overhaul.Tat.Interceptors
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Overhaul.Tat.Entities;

    public class SpecialAccountDecisionNoticeInterceptor : EmptyDomainInterceptor<SpecialAccountDecisionNotice>
    {
        public override IDataResult BeforeCreateAction(IDomainService<SpecialAccountDecisionNotice> service, SpecialAccountDecisionNotice entity)
        {
            // Перед сохранением проставляем начальный статус
            var stateProvider = this.Container.Resolve<IStateProvider>();
            stateProvider.SetDefaultState(entity);

            entity.NoticeDate = DateTime.Now;

            //проставляем порядковый номер
            entity.NoticeNum = this.GetMaxNoticeNum(service, entity) + 1;
            entity.NoticeNumber = $"{entity.SpecialAccountDecision.RealityObject.Municipality.Code}-{entity.NoticeNum}";

            return this.Success();
        }

        private int GetMaxNoticeNum(IDomainService<SpecialAccountDecisionNotice> service, SpecialAccountDecisionNotice entity)
        {
            var noticeNums = service.GetAll()
                .Where(x => x.SpecialAccountDecision.RealityObject.Municipality.Id
                    == entity.SpecialAccountDecision.RealityObject.Municipality.Id)
                .Select(x => x.NoticeNum)
                .ToList();

            return noticeNums.Any()
                ? noticeNums.Max()
                : 0;
        }
    }
}
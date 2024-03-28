namespace Bars.Gkh.Overhaul.Nso.Interceptors
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Overhaul.Nso.Entities;

    public class SpecialAccountDecisionNoticeInterceptor : EmptyDomainInterceptor<SpecialAccountDecisionNotice>
    {
        public override IDataResult BeforeCreateAction(IDomainService<SpecialAccountDecisionNotice> service, SpecialAccountDecisionNotice entity)
        {
            // Перед сохранением проставляем начальный статус
            var stateProvider = this.Container.Resolve<IStateProvider>();
            stateProvider.SetDefaultState(entity);

            entity.NoticeDate = DateTime.Now;

            //проставляем порядковый номер
            var noticeNums = service.GetAll()
                .Where(x => x.SpecialAccountDecision.RealityObject.Municipality.Id == entity.SpecialAccountDecision.RealityObject.Municipality.Id)
                .Where(x => x.NoticeDate == entity.NoticeDate)
                .Select(x => x.NoticeNum)
                .ToList();

            entity.NoticeNum = noticeNums.Any() ? noticeNums.Max() + 1 : 1;
            entity.NoticeNumber = string.Format(
                "{0}-{1}", entity.SpecialAccountDecision.RealityObject.Municipality.Code, entity.NoticeNum);

            return Success();
        }
    }
}
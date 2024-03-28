namespace Bars.GkhGji.Regions.Nso.DomainService
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Contracts.Enums;
    using Bars.GkhGji.Entities;

    using Castle.Windsor;

    public class NsoReminderService : INsoReminderService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult ListAppealCitsReminder(BaseParams baseParams)
        {
            var userManager = Container.Resolve<IGkhUserManager>();
            var servInsSub = Container.Resolve<IDomainService<InspectorSubscription>>();
            var servReminder = Container.Resolve<IDomainService<Reminder>>();

            try
            {
                var loadParams = baseParams.GetLoadParam();
                var activeOperator = userManager.GetActiveOperator();

                if (activeOperator == null || activeOperator.Inspector == null)
                {
                    return new ListDataResult(null, 0);
                }

                var data = servReminder.GetAll()
                    .Where(x => x.Actuality && x.TypeReminder == TypeReminder.Statement)
                    .Where(x => servInsSub.GetAll()
                                .Any(y => y.SignedInspector.Id == activeOperator.Inspector.Id)
                                ? servInsSub.GetAll()
                                 .Any(y => y.SignedInspector.Id == activeOperator.Inspector.Id && y.Inspector.Id == x.Inspector.Id)
                                : x.Inspector.Id == activeOperator.Inspector.Id
                           ) // Вообщем если у инспектора есть Подписки на других инспеткоров то показываем тех инспекторов на которых он подписал, Иначе показывам тольк опо своему Инспектору
                    .Select(x => new
                    {
                        x.Id,
                        AppealCits = x.AppealCits != null ? x.AppealCits.Id : 0,
                        x.Num,
                        x.CheckDate,
                        Inspector = x.Inspector.Fio,
                        AppealCorr = x.AppealCits.Correspondent,
                        AppealCorrAddress = x.AppealCits.CorrespondentAddress,
                        AppealDescription = x.AppealCits.Description
                    })
                    .OrderIf(loadParams.Order.Length == 0, true, x => x.CheckDate)
                    .Filter(loadParams, Container);

                var totalCount = data.Count();

                return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
            }
            catch (ValidationException exc)
            {
                return new BaseDataResult { Success = false, Message = exc.Message };
            }
            finally
            {
                Container.Release(userManager);
                Container.Release(servInsSub);
                Container.Release(servReminder);
            }
        }
    }
}
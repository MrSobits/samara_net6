namespace Bars.GkhGji.Regions.Tatarstan.Interceptors
{
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.GkhCalendar.Entities;
    using Bars.GkhCalendar.Enums;
    using Bars.GkhGji.Regions.Tatarstan.DomainService;

    public class DayInterceptor : EmptyDomainInterceptor<Day>
    {
        /// <inheritdoc />
        public override IDataResult BeforeUpdateAction(IDomainService<Day> service, Day entity)
        {
            var rapidResponseSystemDomainService = this.Container.Resolve<IRapidResponseSystemAppealService>();
            
            using (this.Container.Using(rapidResponseSystemDomainService))
            {
                // GKH-21041
                // По задаче нужно проверять изменилось значение типа дня(DayType) и в случае изменения производить перерасчёт ControlPeriod у сущности RapidResponseSystemAppealDetails
                // Для того чтобы сравнить старый и новый DayType нужно получить старое значения типа дня
                // Так как DomainService и все остальные способы работают с транзакциями, старые значения невозможно получить, так как при запуске транзакции данные сразу же обновлются в БД
                // Поэтому в данном случае использовался SQL зарос для получения значения из БД напрямую
                var oldDayType = (DayType)this.Container.Resolve<ISessionProvider>()
                    .GetCurrentSession()
                    .CreateSQLQuery($"select day_type from GKH_CALENDAR_DAY where id = {entity.Id}")
                    .UniqueResult<short>();

                if (entity.DayType != oldDayType)
                {
                    var baseParams = new BaseParams
                    {
                        Params = new DynamicDictionary
                        {
                            { "fromCalendar", true },
                            { "newDate", entity.DayDate }
                        }
                    };

                    rapidResponseSystemDomainService.UpdateSoprControlPeriod(baseParams);
                }
            }
            
            return base.BeforeUpdateAction(service, entity);
        }
    }
}
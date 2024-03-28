namespace Bars.GkhGji.Regions.BaseChelyabinsk.Interceptors.ActCheck
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Utils;
    using Bars.GkhCalendar.Entities;
    using Bars.GkhCalendar.Enums;
    using Bars.GkhGji.ConfigSections;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    /// <summary>
    /// Перехватчик для Дата и время проведения проверки
    /// </summary>
    public class ChelyabinskActCheckPeriodServiceInterceptor : EmptyDomainInterceptor<ActCheckPeriod>
    {
        /// <summary>
        /// Домен сервис для "Производственный календарь"
        /// </summary>
        public IDomainService<Day> ServiceDay { get; set; }

        /// <summary>
        /// Метод вызывается перед обновлением объекта
        /// </summary>
        /// <param name="service">Домен</param><param name="entity">Объект</param>
        /// <returns>
        /// Результат выполнения
        /// </returns>
        public override IDataResult BeforeUpdateAction(IDomainService<ActCheckPeriod> service, ActCheckPeriod entity)
        {
            return this.CheckDay(service, entity);
        }

        /// <summary>
        /// Метод вызывается перед созданием объекта
        /// </summary>
        /// <param name="service">Домен</param><param name="entity">Объект</param>
        /// <returns>
        /// Результат выполнения
        /// </returns>
        public override IDataResult BeforeCreateAction(IDomainService<ActCheckPeriod> service, ActCheckPeriod entity)
        {
            return this.CheckDay(service, entity);
        }

        private IDataResult CheckDay(IDomainService<ActCheckPeriod> service, ActCheckPeriod entity)
        {
            var config = this.Container.GetGkhConfig<HousingInspection>();
            if (config.SettingsOfTheDay.CheckDateType == CheckDateType.WithoutIndustrialCalendar)
            {
                return this.Success();
            }

            var date = entity.DateCheck.ToDateTime();
            var dayTypeQuery = this.ServiceDay.GetAll()
                .Where(x => x.DayDate == date)
                .Select(x => x.DayType);

            if (!dayTypeQuery.Any())
            {
                return this.Failure("Необходимо заполнить производственный календарь в системе");
            }

            var dayType = dayTypeQuery.First();
            if (dayType == DayType.Workday)
            {
                return this.Success();
            }

            return this.Failure("Введенная Дата проверки не относится к рабочим дням, выберите другую дату");
        }

        public override IDataResult AfterCreateAction(IDomainService<ActCheckPeriod> service, ActCheckPeriod entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.DocumentGji, entity.Id, entity.GetType(), GetPropertyValues(), entity.ActCheck.Id.ToString() + entity.DateCheck);
            }
            catch
            {

            }
            return this.Success();
        }

        public override IDataResult AfterUpdateAction(IDomainService<ActCheckPeriod> service, ActCheckPeriod entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.DocumentGji, entity.Id, entity.GetType(), GetPropertyValues(), entity.ActCheck.DocumentNumber + entity.DateCheck);
            }
            catch
            {

            }
            return this.Success();
        }

        public override IDataResult AfterDeleteAction(IDomainService<ActCheckPeriod> service, ActCheckPeriod entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(null, TypeEntityLogging.DocumentGji, entity.Id, entity.GetType(), GetPropertyValues(), entity.ActCheck.DocumentNumber + entity.DateCheck);
            }
            catch
            {

            }
            return this.Success();
        }

        private Dictionary<string, string> GetPropertyValues()
        {
            var result = new Dictionary<string, string>
            {
                { "ActCheck", "Акт проверки" },
                { "DateCheck", "Дата" },
                { "DateStart", "Дата начала" },
                { "DateEnd", "Дата окончания" }
            };
            return result;
        }
    }
}
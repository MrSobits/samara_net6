namespace Bars.GkhGji.Regions.BaseChelyabinsk.Interceptors.ActRemoval
{
    using System;
    using System.Collections.Generic;
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.ActRemoval;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Protocol;

    public class ActRemovalPeriodServiceInterceptor : EmptyDomainInterceptor<ActRemovalPeriod>
    {
		public override IDataResult BeforeCreateAction(IDomainService<ActRemovalPeriod> service, ActRemovalPeriod entity)
        {
			return this.CorrectDates(entity);
        }

		public override IDataResult BeforeUpdateAction(IDomainService<ActRemovalPeriod> service, ActRemovalPeriod entity)
		{
			return this.CorrectDates(entity);
		}

        public override IDataResult AfterCreateAction(IDomainService<ActRemovalPeriod> service, ActRemovalPeriod entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.DocumentGjiPeriod, entity.Id, entity.GetType(), GetPropertyValues(), entity.ActRemoval.Id.ToString() + " " + entity.DateCheck);
            }
            catch
            {

            }
            return this.Success();
        }

        public override IDataResult AfterUpdateAction(IDomainService<ActRemovalPeriod> service, ActRemovalPeriod entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.DocumentGjiPeriod, entity.Id, entity.GetType(), GetPropertyValues(), entity.ActRemoval.DocumentNumber + " " + entity.DateCheck);
            }
            catch
            {

            }
            return this.Success();
        }

        public override IDataResult AfterDeleteAction(IDomainService<ActRemovalPeriod> service, ActRemovalPeriod entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(null, TypeEntityLogging.DocumentGjiPeriod, entity.Id, entity.GetType(), GetPropertyValues(), entity.ActRemoval.DocumentNumber + " " + entity.DateCheck);
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
                { "ActRemoval", "Акт проверки предписания" },
                { "DateCheck", "Дата прверки" },
                { "DateStart", "Дата начала" },
                { "DateEnd", "Дата окончания" },
                { "TimeStart", "Время начала" },
                { "TimeEnd", "Время окончания" },
            };
            return result;
        }

        private IDataResult CorrectDates(ActRemovalPeriod entity)
		{
			// в момент создания берем переданное время начала проверки и время окончания
			// И формируем Дату начала и Дату окончания путем сопоставления с датой проверки DateCheck
			// То есть к дате проверки добавляем время начала получаем - Дату начала
			// К дате проверки добавляем время окончания и получаем Дату окончания
			var date = entity.DateCheck.ToDateTime();

			DateTime dateStart;
			DateTime.TryParse(entity.TimeStart, out dateStart);

			DateTime dateEnd;
			DateTime.TryParse(entity.TimeEnd, out dateEnd);

			entity.DateStart = new DateTime(date.Year, date.Month, date.Day, dateStart.Hour, dateStart.Minute, 0);
			entity.DateEnd = new DateTime(date.Year, date.Month, date.Day, dateEnd.Hour, dateEnd.Minute, 0);

			return this.Success();
		}
    }
}
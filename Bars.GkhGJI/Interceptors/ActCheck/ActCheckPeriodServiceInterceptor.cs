namespace Bars.GkhGji.Interceptors
{
    using System;
    using System.Collections.Generic;
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    public class ActCheckPeriodServiceInterceptor : EmptyDomainInterceptor<ActCheckPeriod>
    {
        public override IDataResult BeforeCreateAction(IDomainService<ActCheckPeriod> service, ActCheckPeriod entity)
        {
            // в момент создания берем переданное время начала проверки и время окончания
            // И формируем Дату начала и Дату окончания путем сопоставления с датой проверки DateCheck
            // То есть к дате проверки добавляем время начала получаем - Дату начала
            // К дате проверки добавляем время окончания и получаем Дату окончания
            var date = entity.DateCheck.ToDateTime();

            var dateStart = DateTime.Now;
            DateTime.TryParse(entity.TimeStart, out dateStart);

            var dateEnd = DateTime.Now;
            DateTime.TryParse(entity.TimeEnd, out dateEnd);

            entity.DateStart = new DateTime(
                date.Year, date.Month, date.Day, dateStart.Hour, dateStart.Minute, 0);
            entity.DateEnd = new DateTime(date.Year, date.Month, date.Day, dateEnd.Hour, dateEnd.Minute, 0);

            return this.Success();
        }

        public override IDataResult BeforeUpdateAction(IDomainService<ActCheckPeriod> service, ActCheckPeriod entity)
        {
            // в момент создания берем переданное время начала проверки и время окончания
            // И формируем Дату начала и Дату окончания путем сопоставления с датой проверки DateCheck
            // То есть к дате проверки добавляем время начала получаем - Дату начала
            // К дате проверки добавляем время окончания и получаем Дату окончания
            var date = entity.DateCheck.ToDateTime();

            var dateStart = DateTime.Now;
            DateTime.TryParse(entity.TimeStart, out dateStart);

            var dateEnd = DateTime.Now;
            DateTime.TryParse(entity.TimeEnd, out dateEnd);

            entity.DateStart = new DateTime(date.Year, date.Month, date.Day, dateStart.Hour, dateStart.Minute, 0);
            entity.DateEnd = new DateTime(date.Year, date.Month, date.Day, dateEnd.Hour, dateEnd.Minute, 0);

            return this.Success();
        }

        public override IDataResult AfterCreateAction(IDomainService<ActCheckPeriod> service, ActCheckPeriod entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.DocumentGjiPeriod, entity.Id, entity.GetType(), GetPropertyValues(), entity.ActCheck.Id.ToString() + " " + entity.DateCheck);
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
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.DocumentGjiPeriod, entity.Id, entity.GetType(), GetPropertyValues(), entity.ActCheck.DocumentNumber + " " + entity.DateCheck);
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
                logEntityHistoryService.UpdateLog(null, TypeEntityLogging.DocumentGjiPeriod, entity.Id, entity.GetType(), GetPropertyValues(), entity.ActCheck.DocumentNumber + " " + entity.DateCheck);
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
                { "DateCheck", "Дата проверки" },
                { "DateStart", "Дата начала" },
                { "DateEnd", "Дата окончания" },
                { "TimeStart", "Время начала" },
                { "TimeEnd", "Время окончания" },
            };
            return result;
        }
    }
}
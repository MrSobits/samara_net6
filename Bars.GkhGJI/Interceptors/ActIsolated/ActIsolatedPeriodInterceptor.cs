namespace Bars.GkhGji.Interceptors
{
    using System;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;

    public class ActIsolatedPeriodInterceptor : EmptyDomainInterceptor<ActIsolatedPeriod>
    {
        public override IDataResult BeforeCreateAction(IDomainService<ActIsolatedPeriod> service, ActIsolatedPeriod entity)
        {
            this.SetDates(entity);
            return base.BeforeCreateAction(service, entity);
        }

        public override IDataResult BeforeUpdateAction(IDomainService<ActIsolatedPeriod> service, ActIsolatedPeriod entity)
        {
            this.SetDates(entity);
            return base.BeforeUpdateAction(service, entity);
        }

        /// <summary>
        /// Метод для формирования Даты начала и Даты окончания путем сопоставления с датой проверки DateCheck
        /// К дате проверки добавляем время начала - получаем Дату начала
        /// К дате проверки добавляем время окончания - получаем Дату окончания
        /// </summary>
        /// <param name="entity"></param>
        private void SetDates(ActIsolatedPeriod entity)
        {
            var date = entity.DateCheck.ToDateTime();

            var dateStart = DateTime.Now;
            DateTime.TryParse(entity.TimeStart, out dateStart);

            var dateEnd = DateTime.Now;
            DateTime.TryParse(entity.TimeEnd, out dateEnd);

            entity.DateStart = new DateTime(date.Year, date.Month, date.Day, dateStart.Hour, dateStart.Minute, 0);
            entity.DateEnd = new DateTime(date.Year, date.Month, date.Day, dateEnd.Hour, dateEnd.Minute, 0);
        }
    }
}
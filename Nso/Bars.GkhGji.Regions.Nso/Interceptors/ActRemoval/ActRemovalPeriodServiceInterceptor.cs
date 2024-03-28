namespace Bars.GkhGji.Regions.Nso.Interceptors
{
	using System;
	using Bars.B4;
	using Bars.B4.Utils;
	using Bars.GkhGji.Regions.Nso.Entities;

	public class ActRemovalPeriodServiceInterceptor : EmptyDomainInterceptor<ActRemovalPeriod>
    {
		public override IDataResult BeforeCreateAction(IDomainService<ActRemovalPeriod> service, ActRemovalPeriod entity)
        {
			return CorrectDates(entity);
        }

		public override IDataResult BeforeUpdateAction(IDomainService<ActRemovalPeriod> service, ActRemovalPeriod entity)
		{
			return CorrectDates(entity);
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

			return Success();
		}
    }
}
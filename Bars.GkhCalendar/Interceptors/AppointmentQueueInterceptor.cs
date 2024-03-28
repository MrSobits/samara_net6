namespace Bars.GkhCalendar.Interceptors
{
    using Entities;
    using System;
    using Bars.B4;

    class AppointmentQueueInterceptor : EmptyDomainInterceptor<AppointmentQueue>
    {

        public override IDataResult BeforeCreateAction(IDomainService<AppointmentQueue> service, AppointmentQueue entity)
        {
            try
            {
                entity.ObjectCreateDate = DateTime.Now;
                entity.ObjectEditDate = DateTime.Now;
                entity.ObjectVersion = 1;

                return Success();
            }
            catch (Exception e)
            {
                return Failure($"Ошибка интерцептора BeforeCreateAction<AppointmentQueue>: {e.Message}");
            }
        }

        public override IDataResult BeforeUpdateAction(IDomainService<AppointmentQueue> service, AppointmentQueue entity)
        {
            try
            {
                entity.ObjectEditDate = DateTime.Now;
                return Success();
            }
            catch (Exception e)
            {
                return Failure($"Ошибка интерцептора BeforeUpdateAction<AppointmentQueue>: {e.Message}");
            }
        }
    }
}

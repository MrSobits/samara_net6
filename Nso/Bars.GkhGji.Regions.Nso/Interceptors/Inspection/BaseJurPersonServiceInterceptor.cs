namespace Bars.GkhGji.Regions.Nso.Interceptors
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Interceptors;

    public class BaseJurPersonServiceInterceptor : BaseJurPersonServiceInterceptor<BaseJurPerson>
    {
        public override IDataResult BeforeCreateAction(IDomainService<BaseJurPerson> service, BaseJurPerson entity)
        {
            string msg;
            if (!Validation(service, entity, out msg))
            {
                return this.Failure(msg);
            }

            return base.BeforeCreateAction(service, entity);
        }

        public override IDataResult BeforeUpdateAction(IDomainService<BaseJurPerson> service, BaseJurPerson entity)
        {
            string msg;
            if (!Validation(service, entity, out msg))
            {
                return this.Failure(msg);
            }

            return base.BeforeUpdateAction(service, entity);
        }

        public override IDataResult BeforeDeleteAction(IDomainService<BaseJurPerson> service, BaseJurPerson entity)
        {
            var jurPersonContragentDomain = Container.Resolve<IDomainService<BaseJurPersonContragent>>();

            try
            {
                var jurPersonContragents = jurPersonContragentDomain.GetAll().Where(x => x.BaseJurPerson.Id == entity.Id);

                foreach (var jurPersonContragent in jurPersonContragents)
                {
                    jurPersonContragentDomain.Delete(jurPersonContragent);
                }

                return base.BeforeDeleteAction(service, entity);
            }
            finally
            {
                Container.Release(jurPersonContragentDomain);
            }
        }

        /// <summary>
        /// Данный метод предназначен для проверки ДатыНачалаПроверки
        /// чтобы ДатаНачалапроверки не была больше чем ДопустимаяДата
        /// </summary>
        private bool Validation(IDomainService<BaseJurPerson> service, BaseJurPerson entity, out string message)
        {
            message = string.Empty;

            if (!entity.DateStart.HasValue)
            {
                // если нет даты то значит нечег опроверять
                return true;
            }

            if (entity.Contragent == null)
            {
                message = "Не обходимо выбрать юр. лицо";
                return false;
            }

            /*
                Шаг 1. Определить минимально допустимую дату (<Допустимая дата>)как более позднюю дату из дат:
                - <Дата последней плановой проверки> + 1 год;
                - <Дата начала осуществления деятельности> + 1 год.
                <Дата последней плановой проверки> - максимальное значение "Дата начала проверки" из выполненных (Факт проверки = Проведена) проверок "Плановая проверка юридического лица"
                <Дата начала осуществления деятельности> - брать "Дату начала деятельности" из уведомления о начале предпринимательской деятельности юрлица (Жилищная инспекция - Реестр уведомлений - Реестр уведомлений о начале)

                Шаг 2. Если <Дата начала проверки> в документе меньше <Допустимая дата> препятствовать сохранению данных и выдавать пользователю сообщение об ошибке: «Дата плановой проверки не может меньше <Допустимая дата>».
                Проверку выполнять при нажатии "Сохранить" (при создании и редактировании проверки)
             */

            var serviceBusinesactivity = Container.Resolve<IDomainService<BusinessActivity>>();

            try
            {
                var queryBaseJurPerson =
                service.GetAll()
                       .Where(x => x.Id != entity.Id && x.TypeFact == TypeFactInspection.Done && x.DateStart.HasValue && x.Contragent.Id == entity.Contragent.Id);

                var dateLastInspection = queryBaseJurPerson.Any() ? queryBaseJurPerson.Max(x => x.DateStart.Value).AddYears(1) : DateTime.MinValue;

                var queryBusinessActivity =
                    serviceBusinesactivity.GetAll()
                                          .Where(x => x.Contragent.Id == entity.Contragent.Id && x.DateBegin.HasValue);

                var dateMaxActivity = queryBusinessActivity.Any() ? queryBusinessActivity.Max(x => x.DateBegin.Value).AddYears(1) : DateTime.MinValue;

                var dateMax = dateLastInspection > dateMaxActivity ? dateLastInspection : dateMaxActivity;

                if (entity.DateStart < dateMax)
                {
                    message = string.Format("Дата плановой проверки не может меньше {0}", dateMax.ToShortDateString());
                    return false;
                }

                return true;
            }
            finally 
            {
                Container.Release(serviceBusinesactivity);
            }
            
        }

    }
}

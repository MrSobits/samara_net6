namespace Bars.GkhGji.Regions.Tyumen.Interceptors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Contracts.Reminder;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Interceptors;
    using Bars.GkhGji.NumberValidation;

    /// <summary>
    /// Такую пустышку навсякий смлучай нужно чтобы в регионах (Там где уже заменили или отнаследовались от этого класса) непопадало и можно было бы изменять методы как сущности Disposal
    /// </summary>
    public class DisposalServiceInterceptor : DisposalServiceInterceptor<Disposal>
    {
        // Внимание !! Код override нужно писать не в этом классе, а в DisposalServiceInterceptor<T>
    }

    /// <summary>
    /// Короче такой поворот событий делается для того чтобы в Модулях регионов  спомошью 
    /// SubClass расширять сущность Disposal + не переписывать код который регистрируется по сущности
    /// то есть в Disposal добавляеться поля, но интерцептор поскольку Generic просто наследуется  
    /// </summary>
    public class DisposalServiceInterceptor<T> : DocumentGjiInterceptor<T>
        where T : Disposal
    {
        public override IDataResult BeforeUpdateAction(IDomainService<T> service, T entity)
        {
            var servReminders = Container.Resolve<IDomainService<Reminder>>();
            var servDocumentGji = Container.Resolve<IDomainService<DocumentGji>>();
            var reminders = servReminders.GetAll().Where(x => x.DocumentGji.Id == entity.Id).ToList();

            var servDocumentGjiInspector = Container.Resolve<IDomainService<DocumentGjiInspector>>();
            var inspectors = servDocumentGjiInspector.GetAll()
                .Where(x => x.DocumentGji.Id == entity.Id).ToList();

            foreach (var inspector in inspectors)
            {
                var reminderInspector = reminders.Where(x => x.Inspector == inspector.Inspector).FirstOrDefault();
                if (reminderInspector == null)
                {
                    var newReminder = new Reminder
                    {
                        Actuality = true,
                        InspectionGji = entity.Inspection,
                        Inspector = inspector.Inspector,
                        CategoryReminder = Contracts.Enums.CategoryReminder.Licensing,
                        CheckDate = DateTime.Now.AddDays(5),
                        CheckingInspector = inspector.Inspector,
                        Guarantor = entity.IssuedDisposal,
                        DocumentGji = servDocumentGji.Get(entity.Id),
                        Contragent  = entity.Inspection.Contragent,
                        TypeReminder = Contracts.Enums.TypeReminder.DisposalPr,
                        Num = entity.DocumentNumber,
                        ObjectCreateDate = DateTime.Now,
                        ObjectEditDate = DateTime.Now,
                        ObjectVersion = 1

                    };
                    try
                    {
                        servReminders.Save(newReminder);
                    }
                    catch (Exception e)
                    { }
                }
            }

            return base.BeforeUpdateAction(service, entity);
        }

        public override IDataResult AfterCreateAction(IDomainService<T> service, T entity)
        {
           

            return base.BeforeCreateAction(service, entity);
        }

      
    }
}
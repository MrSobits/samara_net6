namespace Bars.GkhGji.Regions.Tyumen.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Tyumen.Entities;

    using Bars.B4.Utils;
    using System;

    public class ActCheckServiceInterceptor : GkhGji.Interceptors.ActCheckServiceInterceptor
    {
        public override IDataResult AfterUpdateAction(IDomainService<ActCheck> service, ActCheck entity)
        {

            var servReminders = Container.Resolve<IDomainService<Reminder>>();
            var servDocumentGji = Container.Resolve<IDomainService<DocumentGji>>();
            var reminders = servReminders.GetAll().Where(x => x.InspectionGji == entity.Inspection).ToList();

            var servDocumentGjiInspector = Container.Resolve<IDomainService<DocumentGjiInspector>>();
            var inspectors = servDocumentGjiInspector.GetAll()
                .Where(x => x.DocumentGji.Id == entity.Id).ToList();

            foreach (var inspector in inspectors)
            {
                var reminderInspector = reminders.Where(x => x.Inspector == inspector.Inspector).FirstOrDefault();
                if (reminderInspector != null)
                {
                    var servActCheckRealityObject = Container.Resolve<IDomainService<ActCheckRealityObject>>();

                    var actCheckRealityObject = servActCheckRealityObject.GetAll()
                        .Where(x => x.ActCheck == entity).FirstOrDefault();


                    if (actCheckRealityObject.HaveViolation == Gkh.Enums.YesNoNotSet.No)
                    {
                        reminderInspector.Actuality = false;
                    }

                    try
                    {
                        servReminders.Update(reminderInspector);
                    }
                    catch (Exception e)
                    {

                    }
                }
            }

            return this.Success();
         
            
        }
    }
}

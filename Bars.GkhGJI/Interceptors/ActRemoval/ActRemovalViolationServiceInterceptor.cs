namespace Bars.GkhGji.Interceptors
{
    using System.Linq;

    using B4;
    using Entities;

    public class ActRemovalViolationServiceInterceptor : EmptyDomainInterceptor<ActRemovalViolation>
    {
        public override IDataResult BeforeUpdateAction(IDomainService<ActRemovalViolation> service, ActRemovalViolation entity)
        {
            var datePlanRemovalStageMax = Container.Resolve<IDomainService<InspectionGjiViolStage>>().GetAll()
               .Where(x => x.InspectionViolation.Id == entity.InspectionViolation.Id)
               .Max(x => x.DatePlanRemoval);

            var dateDateFactRemovalStageMax = Container.Resolve<IDomainService<InspectionGjiViolStage>>().GetAll()
               .Where(x => x.InspectionViolation.Id == entity.InspectionViolation.Id)
               .Max(x => x.DateFactRemoval);

            // Перед изменением Нарушения акта проверки предписания (он же акт устранения нарушений)
            // Мы берем максимальную дату устранения Нарушения и проставляем в главный объект InspectionGJIViolation
            // Поскольку ActRemovalGJIViolation это только этап нарушения в котором проставляется дата устранения а само нарушение это InspectionGJIViolation
            var serviceViol = Container.Resolve<IDomainService<InspectionGjiViol>>();
            var viol = serviceViol.Load(entity.InspectionViolation.Id);
            if (!viol.DateFactRemoval.HasValue)
            {
                viol.DateFactRemoval = dateDateFactRemovalStageMax > entity.DateFactRemoval ? dateDateFactRemovalStageMax : entity.DateFactRemoval;
            }
            viol.DatePlanRemoval = datePlanRemovalStageMax > entity.DatePlanRemoval ? datePlanRemovalStageMax : entity.DatePlanRemoval;
            viol.SumAmountWorkRemoval = entity.SumAmountWorkRemoval;
 
            serviceViol.Update(viol);
            return Success();
        }

        public override IDataResult BeforeDeleteAction(IDomainService<ActRemovalViolation> service, ActRemovalViolation entity)
        {
            // Перед удалением нарушения акта проверки предписания (он же акт устранения нарушений)
            // Мы проверяем если Даты устранения равны значит мы можем затереть дату в главном нарушении проверки
            // такая проверка сделана потому что у данного нарушения может быть несколько записей в таблице ActRemovalGJIViolation
            // Но только та запись где даты совпадают и есть Источник
            var serviceViol = Container.Resolve<IDomainService<InspectionGjiViol>>();
            var viol = serviceViol.Load(entity.InspectionViolation.Id);
            if (viol.DateFactRemoval == entity.DateFactRemoval)
            {
                // Затираем дату устранения поскольку Этап устранения удаляется
                // И чтобы небыло несоответсвия что Этап удалился а дата устранения в главном нарушении осталась мы делаем так
                viol.DateFactRemoval = null;
                serviceViol.Update(viol);
            }

            return Success();
        }

        public override IDataResult AfterUpdateAction(IDomainService<ActRemovalViolation> service, ActRemovalViolation entity)
        {
            var serviceDocumentChildren = Container.Resolve<IDomainService<DocumentGjiChildren>>();

            var serviceProtViol = Container.Resolve<IDomainService<ProtocolViolation>>();

            var violProtocol = serviceProtViol.GetAll()
                .Where(y => serviceDocumentChildren.GetAll().Any(x => x.Children.Id == y.Document.Id && x.Parent.Id == entity.Document.Id)
                    && y.InspectionViolation.Id == entity.InspectionViolation.Id)
                .ToArray();

            foreach (var viol in violProtocol)
            {
                viol.DatePlanRemoval = entity.DatePlanRemoval;
                serviceProtViol.Update(viol);
            }

            return Success();
        }
    }
}
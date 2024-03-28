namespace Bars.GkhGji.Regions.Chelyabinsk.Interceptors
{
    using Bars.B4;
    using Bars.B4.Config;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.ActRemoval;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Enums.SMEV;
    using Entities;
    using Enums;
    using System;
    using System.Linq;

    class ERKNMInterceptor : EmptyDomainInterceptor<ERKNM>
    {
        public IGkhUserManager UserManager { get; set; }

        public IDomainService<ERKNMFile> GisGmpFileDomain { get; set; }

        public IDomainService<Inspector> InspectorDomain { get; set; }

        public IDomainService<ERKNMResultViolations> GISERPResultViolationsDomain { get; set; }

        public IDomainService<ActCheckPeriod> ActCheckPeriodDomain { get; set; }

        public IDomainService<ActRemovalPeriod> ActRemovalPeriodDomain { get; set; }

        public override IDataResult BeforeCreateAction(IDomainService<ERKNM> service, ERKNM entity)
        {
            entity.RequestDate = DateTime.Now;
            entity.NeedToUpdate = Gkh.Enums.YesNoNotSet.No;
            entity.ERPID = "";
            try
            {
//#if DEBUG
               //entity.Inspector = InspectorDomain.GetAll().First();
//#else
                Operator thisOperator = UserManager.GetActiveOperator();
                    if (thisOperator?.Inspector == null)
                        return Failure("Обмен информацией с ГИС ЕРКНМ доступен только сотрудникам ГЖИ");

                    entity.Inspector = thisOperator.Inspector;
//#endif
                //
                entity.RequestState = RequestState.Formed;
                return Success();
            }
            catch (Exception e)
            {
                return Failure($"Ошибка интерцептора BeforeCreateAction<ERKNM>: {e.Message}");
               

            }
        }

        public override IDataResult AfterCreateAction(IDomainService<ERKNM> service, ERKNM entity)
        {
          
            try
            {
                if (entity.Disposal != null)
                {
                    var inspViolations = this.Container.Resolve<IDomainService<InspectionGjiViol>>().GetAll()
                        .Where(x => x.Inspection == entity.Disposal.Inspection).ToList();
                    foreach (InspectionGjiViol viol in inspViolations)
                    {
                        ERKNMResultViolations newReault = new ERKNMResultViolations();
                        newReault.ObjectCreateDate = DateTime.Now;
                        newReault.ObjectEditDate = DateTime.Now;
                        newReault.ObjectVersion = 1;
                        newReault.ERKNM = entity;
                        newReault.VIOLATION_NOTE = viol.Violation.Name;
                        newReault.VIOLATION_ACT = viol.Violation.NormativeDocNames;                      
                        var inspViolationStages = this.Container.Resolve<IDomainService<InspectionGjiViolStage>>().GetAll()
                       .Where(x => x.InspectionViolation == viol).ToList();
                        foreach (InspectionGjiViolStage stage in inspViolationStages)
                        {
                            if (stage.Document != null && stage.Document.TypeDocumentGji == GkhGji.Enums.TypeDocumentGji.Prescription)
                            {
                                newReault.CODE = stage.Document.DocumentNumber + (stage.Document.DocumentDate.HasValue ? (" от " + stage.Document.DocumentDate.Value.ToShortDateString()) : "");
                                if (stage.Document.DocumentDate.HasValue)
                                    newReault.DATE_APPOINTMENT = stage.Document.DocumentDate.Value;
                                if (viol.DateFactRemoval != null)
                                {
                                    newReault.EXECUTION_NOTE = "Выполнено";
                                }
                                newReault.EXECUTION_DEADLINE = viol.DatePlanRemoval;
                            }
                        }
                        if (viol.DateFactRemoval != null)
                        {
                            newReault.VLAWSUIT_TYPE_ID = ERPVLawSuitType.LAWSUIT_SEC_VIII;
                        }
                        else
                        {
                            newReault.VLAWSUIT_TYPE_ID = ERPVLawSuitType.LAWSUIT_SEC_I;
                        }
                        GISERPResultViolationsDomain.Save(newReault);
                        //newReault.V_LAWSUIT = true;




                    }

                }

        
            }
            catch (Exception e)
            {

               
            }
            return base.AfterCreateAction(service, entity);
        }

       

        public override IDataResult BeforeDeleteAction(IDomainService<ERKNM> service, ERKNM entity)
        {
            try
            {
                //чистка приаттаченных файлов
                GisGmpFileDomain.GetAll()
               .Where(x => x.ERKNM.Id == entity.Id)
               .Select(x => x.Id)
               .ToList()
               .ForEach(x => GisGmpFileDomain.Delete(x));

                //чистка сведений о нарушениях
                GISERPResultViolationsDomain.GetAll()
               .Where(x => x.ERKNM.Id == entity.Id)
               .Select(x => x.Id)
               .ToList()
               .ForEach(x => GISERPResultViolationsDomain.Delete(x));

                return Success();
            }
            catch (Exception e)
            {
                return Failure($"Ошибка интерцептора BeforeDeleteAction<ERKNM>: {e.ToString()}");
            }
        }
 
       
    }
}

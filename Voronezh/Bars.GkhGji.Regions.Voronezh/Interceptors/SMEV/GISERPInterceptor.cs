namespace Bars.GkhGji.Regions.Voronezh.Interceptors
{
    using Bars.B4;
    using Bars.B4.Config;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.ActRemoval;
    using Entities;
    using Enums;
    using System;
    using System.Linq;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Enums.SMEV;

    class GISERPInterceptor : EmptyDomainInterceptor<GISERP>
    {
        public IGkhUserManager UserManager { get; set; }

        public IDomainService<GISERPFile> GisGmpFileDomain { get; set; }

        public IDomainService<Inspector> InspectorDomain { get; set; }

        public IDomainService<GISERPResultViolations> GISERPResultViolationsDomain { get; set; }

        public IDomainService<ActCheckPeriod> ActCheckPeriodDomain { get; set; }

        public IDomainService<ActRemovalPeriod> ActRemovalPeriodDomain { get; set; }

        public override IDataResult BeforeCreateAction(IDomainService<GISERP> service, GISERP entity)
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
                        return Failure("Обмен информацией с ГИС ЕРП доступен только сотрудникам ГЖИ");

                    entity.Inspector = thisOperator.Inspector;
//#endif
                //
                entity.RequestState = RequestState.Formed;
                return Success();
            }
            catch (Exception e)
            {
                return Failure($"Ошибка интерцептора BeforeCreateAction<GisErp>: {e.Message}");
               

            }
        }

        public override IDataResult AfterCreateAction(IDomainService<GISERP> service, GISERP entity)
        {
          
            try
            {
                if (entity.Disposal != null)
                {
                    var inspViolations = this.Container.Resolve<IDomainService<InspectionGjiViol>>().GetAll()
                        .Where(x => x.Inspection == entity.Disposal.Inspection).ToList();
                    foreach (InspectionGjiViol viol in inspViolations)
                    {
                        GISERPResultViolations newReault = new GISERPResultViolations();
                        newReault.ObjectCreateDate = DateTime.Now;
                        newReault.ObjectEditDate = DateTime.Now;
                        newReault.ObjectVersion = 1;
                        newReault.GISERP = entity;
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

        public override IDataResult BeforeUpdateAction(IDomainService<GISERP> service, GISERP entity)
        {
            try
            {
//#if DEBUG
//                entity.Inspector = InspectorDomain.GetAll().First();
//#else
                Operator thisOperator = UserManager.GetActiveOperator();
                if (thisOperator?.Inspector == null)
                {

                }
                else
                {
                  //entity.Inspector = thisOperator.Inspector;
                }
                if (entity.RequestState == RequestState.ResponseReceived && entity.GisErpRequestType == GisErpRequestType.Correction)
                {
                    entity.RequestState = RequestState.Formed;
                }
                if(entity.Disposal != null && !entity.ACT_DATE_CREATE.HasValue)
                {
                    var actfromDisposal = this.Container.Resolve<IDomainService<DocumentGjiChildren>>().GetAll()
                 .Where(x => x.Parent.Id == entity.Disposal.Id)
                 .Where(x => x.Children.TypeDocumentGji == TypeDocumentGji.ActCheck || x.Children.TypeDocumentGji == TypeDocumentGji.ActRemoval)
                 .Select(x => x.Children).FirstOrDefault();
                    if(actfromDisposal != null)
                    {
                        var actCheck = this.Container.Resolve<IDomainService<ActCheck>>().Get(actfromDisposal.Id);
                        if (actCheck != null)
                        {
                            entity.ACT_DATE_CREATE = actCheck.DocumentDate;
                            var witness = this.Container.Resolve<IDomainService<ActCheckWitness>>().GetAll()
                                .Where(x => x.ActCheck == actCheck).FirstOrDefault();
                            if (witness != null)
                            {
                                entity.REPRESENTATIVE_POSITION = witness.Position;
                                entity.REPRESENTATIVE_FULL_NAME = witness.Fio;
                            }
                            var period = ActCheckPeriodDomain.GetAll()
                             .Where(x => x.ActCheck == actCheck).FirstOrDefault();
                            if (period != null)
                            {
                                entity.START_DATE = period.DateStart;
                                if (period.DateStart != null && period.DateEnd != null)
                                {

                                    try
                                    {
                                        var convertedDate = period.DateEnd.Value.Subtract(period.DateStart.Value).Hours;
                                        entity.DURATION_HOURS = convertedDate;
                                    }
                                    catch { }
                                }

                            }

                            var viol = this.Container.Resolve<IDomainService<ActCheckRealityObject>>().GetAll()
                                .Where(x => x.ActCheck == actCheck).FirstOrDefault();
                            if (viol != null && viol.HaveViolation == YesNoNotSet.Yes)
                            {
                                entity.HasViolations = YesNoNotSet.Yes;
                            }

                        }
                        else
                        {
                            var actRemoval = this.Container.Resolve<IDomainService<ActRemoval>>().Get(actfromDisposal.Id);
                            if (actRemoval != null)
                            {
                                entity.ACT_DATE_CREATE = actRemoval.DocumentDate;
                                var witness = this.Container.Resolve<IDomainService<ActRemovalWitness>>().GetAll()
                                    .Where(x => x.ActRemoval == actRemoval).FirstOrDefault();
                                if (witness != null)
                                {
                                    entity.REPRESENTATIVE_POSITION = witness.Position;
                                    entity.REPRESENTATIVE_FULL_NAME = witness.Fio;
                                }
                                var period = ActRemovalPeriodDomain.GetAll()
                                 .Where(x => x.ActRemoval == actRemoval).FirstOrDefault();
                                if (period != null)
                                {
                                    entity.START_DATE = period.DateStart;
                                    if (period.DateStart != null && period.DateEnd != null)
                                    {

                                        try
                                        {
                                            var convertedDate = period.DateEnd.Value.Subtract(period.DateStart.Value).Hours;
                                            entity.DURATION_HOURS = convertedDate;
                                        }
                                        catch { }
                                    }

                                }
                             
                                if (actRemoval != null && actRemoval.TypeRemoval == YesNoNotSet.No)
                                {
                                    entity.HasViolations = YesNoNotSet.Yes;
                                }

                            }
                        }
                    }
                }
                var viols = GISERPResultViolationsDomain.GetAll()
                   .Where(x => x.GISERP == entity).Select(x => x.Id).ToList();
                if (viols.Count == 0 && entity.HasViolations == YesNoNotSet.Yes)
                {
                    try
                    {
                        if (entity.Disposal != null)
                        {
                            var inspViolations = this.Container.Resolve<IDomainService<InspectionGjiViol>>().GetAll()
                                .Where(x => x.Inspection == entity.Disposal.Inspection).ToList();
                            foreach (InspectionGjiViol viol in inspViolations)
                            {
                                GISERPResultViolations newReault = new GISERPResultViolations();
                                newReault.ObjectCreateDate = DateTime.Now;
                                newReault.ObjectEditDate = DateTime.Now;
                                newReault.ObjectVersion = 1;
                                newReault.GISERP = entity;
                                newReault.VIOLATION_NOTE = viol.Violation.Name;
                                newReault.VIOLATION_ACT = viol.Violation.NormativeDocNames;
                                var inspViolationStages = this.Container.Resolve<IDomainService<InspectionGjiViolStage>>().GetAll()
                               .Where(x => x.InspectionViolation == viol).ToList();
                                foreach (InspectionGjiViolStage stage in inspViolationStages)
                                {
                                    if (stage.Document != null && stage.Document.TypeDocumentGji == GkhGji.Enums.TypeDocumentGji.ActRemoval)
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
                                    if (stage.Document != null && stage.Document.TypeDocumentGji == GkhGji.Enums.TypeDocumentGji.Prescription)
                                    {
                                        newReault.CODE = stage.Document.DocumentNumber + (stage.Document.DocumentDate.HasValue ? (" от " + stage.Document.DocumentDate.Value.ToShortDateString()) : "");
                                        if (stage.Document.DocumentDate.HasValue)
                                            newReault.DATE_APPOINTMENT = stage.Document.DocumentDate.Value;
                                        if (viol.DateFactRemoval != null)
                                        {
                                            newReault.EXECUTION_NOTE = "Выполнено";
                                        }
                                        newReault.EXECUTION_DEADLINE = stage.DatePlanRemoval;
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
                }
                //#endif
                entity.ObjectEditDate = DateTime.Now;
                entity.RequestDate = DateTime.Now;
                return Success();
            }
            catch (Exception e)
            {
                return Failure($"Ошибка интерцептора BeforeUpdateAction<GisErp>: {e.Message}");
            }
        }

        public override IDataResult BeforeDeleteAction(IDomainService<GISERP> service, GISERP entity)
        {
            try
            {
                //чистка приаттаченных файлов
                GisGmpFileDomain.GetAll()
               .Where(x => x.GISERP.Id == entity.Id)
               .Select(x => x.Id)
               .ToList()
               .ForEach(x => GisGmpFileDomain.Delete(x));

                //чистка сведений о нарушениях
                GISERPResultViolationsDomain.GetAll()
               .Where(x => x.GISERP.Id == entity.Id)
               .Select(x => x.Id)
               .ToList()
               .ForEach(x => GISERPResultViolationsDomain.Delete(x));

                return Success();
            }
            catch (Exception e)
            {
                return Failure($"Ошибка интерцептора BeforeDeleteAction<GisErp>: {e.ToString()}");
            }
        }
 
       
    }
}

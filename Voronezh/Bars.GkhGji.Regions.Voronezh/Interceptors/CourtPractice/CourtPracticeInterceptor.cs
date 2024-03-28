namespace Bars.GkhGji.Regions.Voronezh
{
    using B4.Modules.States;
    using Bars.B4;
    using Bars.B4.Config;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;
    using Entities;
    using Enums;
    using System;
    using System.Linq;

    class CourtPracticeInterceptor : EmptyDomainInterceptor<CourtPractice>
    {
        public IGkhUserManager UserManager { get; set; }

        public IDomainService<Prescription> PrescriptionDomail { get; set; }
        public IDomainService<Resolution> ResolutionDomail { get; set; }
        public IDomainService<State> StateDomail { get; set; }

        public override IDataResult BeforeCreateAction(IDomainService<CourtPractice> service, CourtPractice entity)
        {
            try
            { 
                var servStateProvider = Container.Resolve<IStateProvider>();

                try
                {
                    servStateProvider.SetDefaultState(entity);                  
                }
                finally
                {
                    Container.Release(servStateProvider);
                }
                return Success();
            }
            catch (Exception e)
            {
                return Failure($"Ошибка интерцептора BeforeCreateAction<CourtPractice>: {e.Message}");
            }
        }
        public override IDataResult BeforeUpdateAction(IDomainService<CourtPractice> service, CourtPractice entity)
        {
            try
            {
                if (entity.DocumentGji != null)
                {
                    if (entity.DocumentGji.TypeDocumentGji == GkhGji.Enums.TypeDocumentGji.Prescription)
                    {
                        if (entity.CourtMeetingResult == CourtMeetingResult.CompletelySatisfied)
                        {
                            var prescription = PrescriptionDomail.Get(entity.DocumentGji.Id);
                            prescription.PrescriptionState = GkhGji.Enums.PrescriptionState.CancelledByCourt;
                            PrescriptionDomail.Update(prescription);
                        }
                    }
                    if (entity.DocumentGji.TypeDocumentGji == GkhGji.Enums.TypeDocumentGji.Resolution)
                    {
                        
                    }
                }
                if (entity.FormatHour.HasValue)
                {
                    if (entity.FormatMinute.HasValue)
                    {
                        entity.CourtMeetingTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, entity.FormatHour.Value, entity.FormatMinute.Value, DateTime.Now.Second);
                    }
                    else
                    {
                        entity.CourtMeetingTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, entity.FormatHour.Value, 0, 0);
                    }
                }
                return Success();
            }
            catch (Exception e)
            {
                return Failure($"Ошибка интерцептора BeforeCreateAction<CourtPractice>: {e.Message}");
            }
        }
    }
}

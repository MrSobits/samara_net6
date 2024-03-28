namespace Bars.GkhGji.Regions.Tomsk.ViewModel.Protocol
{
    using System;

    using Bars.B4;
    using Bars.GkhGji.Regions.Tomsk.Entities;

    public class TomskResolutionViewModel : Bars.GkhGji.DomainService.ResolutionViewModel<TomskResolution>
    {
        /* Если будет расширение конкретно для региона томск то писать переопределение суда */

        public override IDataResult Get(IDomainService<TomskResolution> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAs<long>("id", ignoreCase: true);
            var obj = domainService.Get(id);

            DateTime? time = null;
            if (obj.DocumentTime.HasValue)
            {
                time = new DateTime(1, 1, 1, obj.DocumentTime.Value.Hour, obj.DocumentTime.Value.Minute, 0);
            }

            return
                new BaseDataResult(
                    new
                        {
                            // TomskResolution
                            obj.Id,
                            obj.BecameLegal,
                            obj.Contragent,
                            obj.DateTransferSsp,
                            obj.DateWriteOut,
                            obj.DeliveryDate,
                            obj.Description,
                            obj.DocumentNumSsp,
                            obj.Executant,
                            obj.Municipality,
                            obj.Official,
                            obj.Paided,
                            obj.ParentDocumentsList,
                            obj.PenaltyAmount,
                            obj.PhysicalPerson,
                            obj.PhysicalPersonInfo,
                            obj.Sanction,
                            obj.SectorNumber,
                            obj.TypeInitiativeOrg,
                            obj.TypeTerminationBasement,

                            // Resolution
                            obj.DocumentDate,
                            obj.DocumentDateStr,
                            obj.DocumentNum,
                            obj.DocumentNumber,
                            obj.LiteralNum,
                            obj.DocumentSubNum,
                            obj.DocumentTime,
                            obj.DocumentYear,
                            obj.Inspection,
                            obj.Stage,
                            obj.State,
                            obj.TypeDocumentGji,
                            
                            // DocumentGji
                            obj.ExplanationText,
                            obj.FioAttend,
                            obj.HasPetition,
                            obj.PetitionText,
                            obj.PhysicalPersonGender,
                            obj.ResolutionText,

                            // Время (часы и минуты) составления документа
                            CreationTime = time != null ? time.Value.ToString("HH:mm") : null
                        });
        }
    }
}

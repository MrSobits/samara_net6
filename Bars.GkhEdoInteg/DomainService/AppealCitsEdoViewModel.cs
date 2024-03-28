namespace Bars.GkhEdoInteg.DomainService
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhEdoInteg.Entities;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Перекрывает AppealCitsViewModel из модуля Gji
    /// </summary>
    public class AppealCitsEdoViewModel : GkhGji.ViewModel.AppealCitsViewModel
    {
        public override IDataResult Get(IDomainService<AppealCits> domainService, BaseParams baseParams)
        {
            var appealCits = domainService.Get(baseParams.Params["id"].To<long>());

            var data =
                Container.Resolve<IDomainService<AppealCitsCompareEdo>>().GetAll()
                    .Where(x => x.AppealCits.Id == appealCits.Id)
                    .Select(x => new {x.IsEdo, x.DateActual, x.AddressEdo})
                    .FirstOrDefault();

            var value = new
            {
                appealCits.Id,
                appealCits.Number,
                appealCits.DocumentNumber,
                appealCits.NumberGji,
                appealCits.DateFrom,
                data?.DateActual,
                appealCits.CheckTime,
                appealCits.PreviousAppealCits,
                appealCits.KindStatement,
                appealCits.DescriptionLocationProblem,
                appealCits.FlatNum,
                appealCits.ManagingOrganization,
                appealCits.Year,
                appealCits.Status,

                appealCits.Correspondent,
                appealCits.CorrespondentAddress,
                appealCits.Email,
                appealCits.Phone,
                appealCits.QuestionsCount,
                appealCits.RedtapeFlag,
                appealCits.PreviousAppealCitExternalId,
                appealCits.Description,
                appealCits.File,
                appealCits.State,
                appealCits.TypeCorrespondent,
                appealCits.SuretyDate,
                appealCits.ExecuteDate,
                appealCits.Surety,
                appealCits.SuretyResolve,
                appealCits.Executant,
                appealCits.Tester,
                appealCits.ZonalInspection,
                appealCits.IsPrelimentaryCheck,
                appealCits.IsIdentityVerified,
                data?.AddressEdo,
                IsEdo = data?.IsEdo ?? false
            };

            return new BaseDataResult(value);
        }
    }
}
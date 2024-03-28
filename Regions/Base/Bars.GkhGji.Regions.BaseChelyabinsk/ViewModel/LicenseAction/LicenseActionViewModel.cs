namespace Bars.GkhGji.Regions.BaseChelyabinsk.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.FIAS;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities;

    public class LicenseActionViewModel : BaseViewModel<LicenseAction>
	{
		public override IDataResult List(IDomainService<LicenseAction> domainService, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);

            var data = domainService.GetAll()
                .Select(x => new
                {
                    x.Id,
                    x.ApplicantAgreement,
                    x.ApplicantEmail,
                    x.ApplicantEsiaId,
                    x.ApplicantFirstName,
                    x.ApplicantInn,
                    x.ApplicantLastName,
                    x.ApplicantMiddleName,
                    x.ApplicantOkved,
                    x.ApplicantPhone,
                    x.ApplicantSnils,
                    x.ApplicantType,
                    x.DocumentDate,
                    x.DocumentIssuer,
                    x.ObjectCreateDate,
                    x.DocumentName,
                    x.DocumentNumber,
                    x.DocumentSeries,
                    x.DocumentType,
                    x.FileInfo,
                    Contragent = x.Contragent.Name,
                    x.LicenseActionType,
                    x.LicenseDate,
                    x.LicenseNumber,
                    x.MiddleNameFl,
                    x.NameFl,
                    x.Position,
                    x.SurnameFl,
                    x.State,
                    FIO = string.Join(" ", x.ApplicantLastName, x.ApplicantFirstName, x.ApplicantMiddleName)
                })
				.Filter(loadParams, this.Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
	}
}
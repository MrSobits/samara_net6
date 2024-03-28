namespace Bars.Gkh1468.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh1468.DomainService;
    using Bars.Gkh1468.Entities;

    public class HouseProviderPassportViewModel : BaseProviderPassportViewModel<HouseProviderPassport>
    {
        private IHousePassportService _paspServ;
		public IUserIdentity UserIdentity { get; set; }
		public IAuthorizationService AuthorizationService { get; set; }

		public HouseProviderPassportViewModel(IFileManager fileManager)
            : base(fileManager)
        {
        }

        public IHousePassportService PaspService
        {
            get
            {
                return _paspServ ?? (_paspServ = Container.Resolve<IHousePassportService>());
            }
        }

        public override IDataResult Get(IDomainService<HouseProviderPassport> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAs<long>("id");
            var value = domainService.GetAll().Where(x => x.Id == id)
                            .Select(x => new
                            {
                                ContragentId = x.Contragent != null ? x.Contragent.Id : 0,
                                ContragentName = x.Contragent != null ? x.Contragent.Name : "",
                                x.ContragentType,
                                x.HouseType,
                                x.Id,
                                x.PassportStruct,
                                x.Pdf,
                                x.Percent,
                                RoId = x.RealityObject.Id,
                                x.RealityObject.Address,
                                x.ReportMonth,
                                x.ReportYear,
                                x.Certificate,
                                x.State,
                                MuId = x.RealityObject.Municipality.Id,
                                MuName = x.RealityObject.Municipality.Name
                            }).FirstOrDefault();

            return value == null ? new BaseDataResult() :
                new BaseDataResult(new
                {
                    Contragent = new { Id = value.ContragentId, Name = value.ContragentName },
                    value.ContragentType,
                    value.HouseType,
                    value.Id,
                    value.Pdf,
                    value.Percent,
                    RealityObject = new { Id = value.RoId, value.Address },
                    value.ReportMonth,
                    value.ReportYear,
                    value.Certificate,
                    value.State,
                    Municipality = new { Id = value.MuId, Name = value.MuName }
                });
        }

        public override IDataResult List(IDomainService<HouseProviderPassport> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var signyFilter = string.Empty;
            OrderField signyOrder = null;

            if (loadParams.CheckRuleExists("Signy"))
            {
                signyFilter = loadParams.GetRuleValue("Signy").ToStr();
                loadParams.DeleteRule("Signy");
            }

            var curOp = Container.Resolve<IGkhUserManager>().GetActiveOperator();
			var createByAllRo = AuthorizationService.Grant(UserIdentity, "Gkh1468.Passport.MyHouse.CreateByAllRo");

	        if (!createByAllRo)
	        {
		        if (curOp == null || curOp.Contragent == null)
		        {
			        return new ListDataResult();
		        }
	        }

	        // Если с клиента не пришла сортировка - то сортируем по году и месяцу
            if (loadParams.Order.Length == 0)
            {
                loadParams.Order = new[]
                                       {
                                           new OrderField { Asc = false, Name = "ReportYear" },
                                           new OrderField { Asc = false, Name = "ReportMonth" },
                                           new OrderField { Asc = false, Name = "RealityObject" }
                                       };
            }
            else if (loadParams.Order.Any(x => x.Name == "Signy"))
            {
                signyOrder = loadParams.Order.FirstOrDefault(x => x.Name == "Signy");
                loadParams.Order = loadParams.Order.Where(x => x.Name != "Signy").ToArray();
            }

			var contragentId = curOp != null && curOp.Contragent != null ? curOp.Contragent.Id : 0;

			var data =
                domainService.GetAll()
                             .Where(x => (createByAllRo && x.Contragent == null) || x.Contragent.Id == contragentId)
                             .Select(
                                 x =>
                                 new
                                     {
                                         x.Id,
                                         x.State,
                                         x.SignDate,
                                         RealityObject = x.RealityObject.Address,
                                         RealityObjectId = x.RealityObject.Id,
                                         x.Percent,
                                         x.ReportYear,
                                         x.ReportMonth,
                                         Contragent = x.Contragent.Name,   
                                         x.UserName,
                                         x.Certificate
                                     })
                             .ToList()
                             .AsQueryable()
                             .Order(loadParams)
                             .Filter(loadParams, Container);

            var result = data
                .Select(x => new
                {
                    x.Id,
                    x.State,
                    x.Contragent,
                    x.SignDate,
                    x.RealityObject,
                    x.RealityObjectId,
                    x.Percent,
                    x.ReportYear,
                    x.ReportMonth,
                    x.UserName,
                    Signy = GetSigny(x.Certificate),
                })
                .Where(x => signyFilter == string.Empty || (x.Signy != null && x.Signy.Contains(signyFilter)))
                .Paging(loadParams)
                .ToArray();

            if (signyOrder != null)
            {
                result = signyOrder.Asc ? result.OrderBy(x => x.Signy).ToArray() : result.OrderByDescending(x => x.Signy).ToArray();
            }

            return new ListDataResult(result, data.Count());
        }
    }
}
namespace Bars.GkhGji.ViewModel
{
    using System;
    using System.Linq;

    using B4;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;
    using Entities;

    public class AppealCitsRequestViewModel : BaseViewModel<AppealCitsRequest>
    {
        public IGkhUserManager UserManager { get; set; }
        public IDomainService<OperatorContragent> OperatorContragentDomain { get; set; }

        public override IDataResult List(IDomainService<AppealCitsRequest> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);
            var appealCitizensId = baseParams.Params.GetAs<long>("appealCitizensId");
            if (appealCitizensId > 0)
            {
                DateTime? nulldate = null;
                var data = domainService.GetAll()
                    .Where(x => x.AppealCits.Id == appealCitizensId)
                    .Select(x => new
                    {
                        x.Id,
                        x.DocumentName,
                        x.PerfomanceDate,//Срок исполнения                                  
                        x.DocumentNumber,//номер запроса                 
                        x.File,
                        AppealNumber =x.AppealCits.Number,// номер обращения 
                        AppealDate = x.AppealCits.DateFrom,// дата обращения  
                        x.DocumentDate, // дата запроса 
                        SendDate = x.SignedFile != null? x.ObjectEditDate: nulldate,//дата отправки запроса                      
                        Contragent = x.Contragent.Name,// Адресат                    
                        x.PerfomanceFactDate,// дата фактического исполнения запроса 
                        SenderInspector = x.SenderInspector != null ? x.SenderInspector.Fio : "",// Инспектор 
                        x.SignedFile,//Подписаный файл
                    })
                    .Filter(loadParams, Container);
                int totalCount = data.Count();
                return new ListDataResult(data.Order(loadParams).ToList(), totalCount);
            }
            else
            {
                var dateStart2 = baseParams.Params.GetAs("dateStart", new DateTime());
                var dateEnd2 = baseParams.Params.GetAs("dateEnd", new DateTime());
                Operator thisOperator = UserManager.GetActiveOperator();
                if (thisOperator?.Inspector == null)
                {
                    DateTime? nulldate = null;
                    var contragent = thisOperator.Contragent;
                    var contragentList = OperatorContragentDomain.GetAll()
                     .Where(x => x.Contragent != null)
                     .Where(x => x.Operator == thisOperator)
                     .Select(x => x.Contragent.Inn).Distinct().ToList();
                    if (contragent != null)
                    {
                        if (!contragentList.Contains(contragent.Inn))
                        {
                            contragentList.Add(contragent.Inn);
                        }
                    }
                    if (contragentList.Count == 0)
                    {
                        return null;
                    }
                    var data = domainService.GetAll()
                        .Where(x=> x.DocumentDate.HasValue && x.SignedFile != null)
                        .Where(x=> x.DocumentDate.Value >= dateStart2 && x.DocumentDate.Value <= dateEnd2)
                    //    .Where(x=> x.SignedFile != null)
                     .Where(x => x.Contragent != null)
                     .Where(x => contragentList.Contains(x.Contragent.Inn))
                     .Select(x => new
                     {
                         x.Id,
                         x.DocumentName,
                         x.PerfomanceDate,    // Срок исполнения                                  
                         x.DocumentNumber,    // номер запроса                 
                         x.File,
                         AppealNumber = x.AppealCits.Number, // номер обращения 
                         AppealDate =x.AppealCits.DateFrom,// дата обращения  
                         x.DocumentDate, // дата запроса 
                         SendDate = x.SignedFile != null ? x.ObjectEditDate : nulldate,//дата отправки запроса 
                         Contragent = x.Contragent.Name, // Адресат                   
                         x.PerfomanceFactDate,// дата фактического исполнения запроса 
                         SenderInspector = x.SenderInspector != null ? x.SenderInspector.Fio : "", // Инспектор 
                         x.SignedFile,// Подписаный файл
                     })
                     .Filter(loadParams, Container);
                    int totalCount = data.Count();
                    return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
                }
                else
                {
                    DateTime? nulldate = null;
                    var data = domainService.GetAll()
                  .Where(x => x.DocumentDate.HasValue && x.SignedFile != null)
                  .Select(x => new
                  {
                      x.Id,
                      x.DocumentName,
                      x.PerfomanceDate,    // Срок исполнения                                  
                      x.DocumentNumber,    // номер запроса                 
                      x.File,
                      AppealNumber = x.AppealCits.Number, // номер обращения 
                      AppealDate = x.AppealCits.DateFrom,// дата обращения   
                      x.DocumentDate, // дата запроса 
                      SendDate = x.SignedFile != null ? x.ObjectEditDate : nulldate,//дата отправки запроса 
                      Contragent = x.Contragent.Name, // Адресат             
                      x.PerfomanceFactDate,// дата фактического исполнения запроса 
                      SenderInspector = x.SenderInspector != null ? x.SenderInspector.Fio : "", // Инспектор 
                      x.SignedFile,// Подписаный файл
                  })
                  .Filter(loadParams, Container);
                    int totalCount = data.Count();
                    return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
                }
            }
        }
    }
}
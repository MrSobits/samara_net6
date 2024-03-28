using Bars.B4;
using Bars.B4.DataAccess;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.GkhGji.Entities;
using Bars.GkhGji.Regions.Voronezh.DomainService;
using Bars.GkhGji.Regions.Voronezh.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading;

namespace Bars.GkhGji.Regions.Voronezh.Tasks
{
    public class CheckERPNeedCorrectionTaskExecutor : ITaskExecutor
    {

        /// <summary>
        /// Идентификатор
        /// </summary>
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        public IRepository<GISERP> GISERPDomain { get; set; }

        public IDomainService<DocumentGji> DocumentGjiDomain { get; set; }

        public IDomainService<DocumentGjiChildren> DocumentGjiChildrenDomain { get; set; }

        /// <summary>
        /// Код задачи
        /// </summary>
        public string ExecutorCode { get; private set; }


        public IDataResult Execute(BaseParams @params, B4.Modules.Tasks.Common.Contracts.ExecutionContext ctx, IProgressIndicator indicator, CancellationToken ct)
        {           
            //отправка
            try
            {
                Dictionary<long, long> erpDict = new Dictionary<long, long>();

                GISERPDomain.GetAll()
                    .Where(x => x.ObjectCreateDate > DateTime.Now.AddMonths(-3))
                    .Where(x => x.GisErpRequestType == Enums.GisErpRequestType.Initialization)
                    .Where(x => !x.ACT_DATE_CREATE.HasValue).ToList()
                    .ForEach(obj =>
                    {
                        erpDict.Add(obj.Id, obj.Disposal.Id);
                    });
                foreach (KeyValuePair<long, long> key in erpDict)
                {
                    var disposalAct = DocumentGjiChildrenDomain.GetAll()
                        .Where(x => x.Parent.Id == key.Value)
                        .Where(x => x.Children != null)
                        .Where(x => x.Children.TypeDocumentGji == GkhGji.Enums.TypeDocumentGji.ActCheck || x.Children.TypeDocumentGji == GkhGji.Enums.TypeDocumentGji.ActRemoval || x.Children.TypeDocumentGji == GkhGji.Enums.TypeDocumentGji.ActSurvey)
                        .Where(x=> !x.Children.State.StartState)
                        .FirstOrDefault();
                    if (disposalAct != null)
                    {
                        var erp = GISERPDomain.Get(key.Key);
                        if (erp != null)
                        {
                            erp.GisErpRequestType = Enums.GisErpRequestType.NeedCorrection;
                            GISERPDomain.Update(erp);
                        }
                    }

                }
                return new BaseDataResult(true, "Реестр актуализирован");

            }
            catch (HttpRequestException e)
            {
                return new BaseDataResult(false, $"Ошибка : {e.InnerException}");
            }
        }

        
    }

}

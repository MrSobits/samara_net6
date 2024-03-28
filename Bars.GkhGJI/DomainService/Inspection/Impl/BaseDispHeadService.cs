namespace Bars.GkhGji.DomainService
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;

    using Castle.Windsor;

    public class BaseDispHeadService : IBaseDispHeadService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult GetInfo(BaseParams baseParams)
        {
            /*
             * метод нужен для получения списка домов управляющей организации для плановой инспекционной проверки
             * inspectionId - id проверки
             * условия выборки домов: жилой дом находится в разделе жилые дома управляющей организации и дата договора пересекается с датой плана проверки юр.лиц
             * realityObjIds - id домов которые можно выбирать вкачестве проверяемых через запятую
             * inspectorNames - ФИО инспекторов проверки
             * inspectorIds - ID инспекторов проверки
             */
            var inspectorsDomain = this.Container.Resolve<IDomainService<InspectionGjiInspector>>();

            try
            {
                var inspectorNames = string.Empty;
                var inspectorIds = string.Empty;

                if (baseParams.Params.ContainsKey("inspectionId"))
                {
                    var inspectionId = baseParams.Params["inspectionId"].ToLong();

                    var dataInspectors =
                        inspectorsDomain
                            .GetAll()
                            .Where(x => x.Inspection.Id == inspectionId)
                            .Select(x => new
                            {
                                x.Inspector.Id,
                                x.Inspector.Fio
                            })
                            .ToList();

                    foreach (var item in dataInspectors)
                    {
                        if (!string.IsNullOrEmpty(inspectorNames))
                        {
                            inspectorNames += ", ";
                        }

                        inspectorNames += item.Fio;

                        if (!string.IsNullOrEmpty(inspectorIds))
                        {
                            inspectorIds += ", ";
                        }

                        inspectorIds += item.Id.ToString();
                    }
                }

                return new BaseDataResult(new { inspectorNames, inspectorIds });
            }
            catch (ValidationException e)
            {
                return new BaseDataResult { Success = false, Message = e.Message };
            }
            finally
            {
                this.Container.Release(inspectorsDomain);
            }
        }
    }
}
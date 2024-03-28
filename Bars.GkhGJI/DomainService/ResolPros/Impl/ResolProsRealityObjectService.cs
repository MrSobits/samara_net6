namespace Bars.GkhGji.DomainService
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;

    using Castle.Windsor;

    public class ResolProsRealityObjectService : IResolProsRealityObjectService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult AddRealityObjects(BaseParams baseParams)
        {
            try
            {
                var documentId = baseParams.Params.ContainsKey("documentId") ? baseParams.Params["documentId"].ToLong() : 0;
                var objectIds = baseParams.Params.ContainsKey("objectIds") ? baseParams.Params["objectIds"].ToString() : "";

                if (!string.IsNullOrEmpty(objectIds))
                {
                    // в этом списке будут id домов которые уже связаны с этой инспекцией 
                    // (чтобы недобавлять несколько одинаковых домов)
                    var listObjects = new List<long>();

                    var serviceRealityObject = Container.Resolve<IDomainService<ResolProsRealityObject>>();

                    listObjects.AddRange(
                        serviceRealityObject.GetAll()
                            .Where(x => x.ResolPros.Id == documentId)
                            .Select(x => x.RealityObject.Id)
                            .Distinct()
                            .ToList());

                    foreach (var id in objectIds.Split(','))
                    {
                        var newId = id.ToLong();

                        // Если среди существующих домов данно инспекции уже есть такой дом то пролетаем мимо
                        if (listObjects.Contains(newId))
                            continue;

                        // Если такого дома еще нет то добалвяем
                        var newObj = new ResolProsRealityObject
                                         {
                                             ResolPros = new ResolPros { Id = documentId },
                                             RealityObject = new RealityObject { Id = newId }
                                         };

                        serviceRealityObject.Save(newObj);
                    }
                }

                return new BaseDataResult();
            }
            catch (ValidationException e)
            {
                return new BaseDataResult {Success = false, Message = e.Message};
            }
        }
    }
}
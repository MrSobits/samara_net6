namespace Bars.GkhGji.DomainService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;

    using Castle.Windsor;

    public class ProtocolMhcRealityObjectService : IProtocolMhcRealityObjectService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult AddRealityObjects(BaseParams baseParams)
        {
            var serviceRealityObject = Container.Resolve<IDomainService<ProtocolMhcRealityObject>>();

            try
            {
                var documentId = baseParams.Params.GetAs("documentId", 0L);
                var objectIds = baseParams.Params.GetAs("objectIds", new long[] { });

                var currentIds = serviceRealityObject.GetAll()
                                        .Where(x => x.ProtocolMhc.Id == documentId)
                                        .Select(x => x.RealityObject.Id)
                                        .Distinct()
                                        .ToList();

                var listToSave = new List<ProtocolMhcRealityObject>();

                foreach (var id in objectIds)
                {
                    
                    // Если среди существующих домов данно инспекции уже есть такой дом то пролетаем мимо
                    if (currentIds.Contains(id)) 
                        continue;

                    // Если такого дома еще нет то добалвяем
                    listToSave.Add(new ProtocolMhcRealityObject
                                        {
                                            ProtocolMhc = new ProtocolMhc { Id = documentId },
                                            RealityObject = new RealityObject { Id = id }
                                        });
                }

                if (listToSave.Any())
                {
                    using (var tr = Container.Resolve<IDataTransaction>())
                    {
                        try
                        {
                            listToSave.ForEach(serviceRealityObject.Save);
                            tr.Commit();
                        }
                        catch (Exception exc)
                        {
                            tr.Rollback();
                            throw exc;
                        }
                    }
                }
                
                return new BaseDataResult();
            }
            finally
            {
                Container.Release(serviceRealityObject);
            }
        }
    }
}
namespace Bars.GkhGji.Regions.BaseChelyabinsk.DomainService.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.ActRemoval;

    using Castle.Windsor;

    public class ActRemovalInspectedPartService : IActRemovalInspectedPartService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult AddInspectedParts(BaseParams baseParams)
        {
            try
            {
                var documentId = baseParams.Params.ContainsKey("documentId")
                                     ? baseParams.Params["documentId"].ToLong()
                                     : 0;
                var partIds = baseParams.Params.ContainsKey("partIds")
                                  ? baseParams.Params["partIds"].ToString().Split(',')
                                  : new string[] {};

                //в этом списке будут id инспектируемых частей, которые уже связаны с этим актом обследования
                //(чтобы не добавлять несколько одинаковых экспертов в одно и тоже распоряжение)
                var listIds = new List<long>();

                var serviceParts = this.Container.Resolve<IDomainService<ActRemovalInspectedPart>>();

                listIds.AddRange(
                    serviceParts.GetAll()
                        .Where(x => x.ActRemoval.Id == documentId)
                        .Select(x => x.InspectedPart.Id)
                        .Distinct()
                        .ToList());

                foreach (var id in partIds.Select(x => x.ToLong()))
                {
                    //Если среди существующих частей уже есть такая часть, то пролетаем мимо
                    if (listIds.Contains(id))
                        continue;

                    //Если такого эксперта еще нет то добалвяем
                    var newObj = new ActRemovalInspectedPart
                        {
							ActRemoval = new ActRemoval { Id = documentId },
                            InspectedPart = new InspectedPartGji {Id = id}
                        };

                    serviceParts.Save(newObj);
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
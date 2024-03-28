namespace Bars.GkhGji.DomainService
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;

    using Castle.Windsor;

    public class DisposalExpertService : IDisposalExpertService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult AddExperts(BaseParams baseParams)
        {
            try
            {
                var documentId = baseParams.Params.ContainsKey("documentId") ? baseParams.Params["documentId"].ToLong() : 0;
                var expertIds = baseParams.Params.ContainsKey("expertIds") ? baseParams.Params["expertIds"].ToString() : "";

                // в этом списке будут id экспертов, которые уже связаны с этим распоряжением
                // (чтобы недобавлять несколько одинаковых экспертов в одно и тоже распоряжение)
                var listIds = new List<long>();

                var serviceExperts = Container.Resolve<IDomainService<DisposalExpert>>();

                listIds.AddRange(serviceExperts.GetAll()
                                    .Where(x => x.Disposal.Id == documentId)
                                    .Select(x => x.Expert.Id)
                                    .Distinct()
                                    .ToList());

                foreach (var id in expertIds.Split(','))
                {
                    var newId = id.ToLong();

                    // Если среди существующих экспертов уже есть такой эксперт то пролетаем мимо
                    if (listIds.Contains(newId))
                        continue;

                    // Если такого эксперта еще нет то добавляем
                    var newObj = new DisposalExpert
                    {
                        Disposal = new Disposal { Id = documentId },
                        Expert = new ExpertGji { Id = newId }
                    };

                    serviceExperts.Save(newObj);
                }

                return new BaseDataResult();
            }
            catch (ValidationException e)
            {
                return new BaseDataResult { Success = false, Message = e.Message };
            }
        }
    }
}
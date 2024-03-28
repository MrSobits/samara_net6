namespace Bars.GkhGji.DomainService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.GkhGji.Entities;

    using Castle.Windsor;

    /// <summary>
    /// Сервис для "Инспектируемая часть в акте без взаимодействия"
    /// </summary>
    public class ActIsolatedInspectedPartService : IActIsolatedInspectedPartService
    {
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Добавить инспектируемые части
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Результат выполнения запроса</returns>
        public IDataResult AddInspectedParts(BaseParams baseParams)
        {
            try
            {
                var documentId = baseParams.Params.GetAs<long>("documentId");
                var partIds = baseParams.Params.GetAs<string>("partIds", string.Empty).ToLongArray();

                //в этом списке будут id инспектируемых частей, которые уже связаны с этим актом обследования
                //(чтобы не добавлять несколько одинаковых экспертов в одно и тоже распоряжение)
                var listIds = new List<long>();

                var serviceParts = this.Container.Resolve<IDomainService<ActIsolatedInspectedPart>>();

                listIds.AddRange(
                    serviceParts.GetAll()
                        .Where(x => x.ActIsolated.Id == documentId)
                        .Select(x => x.InspectedPart.Id)
                        .Distinct()
                        .ToList());

                foreach (var id in partIds.Select(x => x.ToLong()))
                {
                    //Если среди существующих частей уже есть такая часть, то пролетаем мимо
                    if (listIds.Contains(id))
                        continue;
                    
                    var newObj = new ActIsolatedInspectedPart
                    {
                        ActIsolated = new ActIsolated { Id = documentId},
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
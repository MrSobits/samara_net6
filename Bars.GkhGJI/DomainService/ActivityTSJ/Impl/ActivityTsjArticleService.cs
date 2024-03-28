namespace Bars.GkhGji.DomainService
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    using Castle.Windsor;

    public class ActivityTsjArticleService : IActivityTsjArticleService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult SaveParams(BaseParams baseParams)
        {
            /*
             * В данном методе сохраняем или обновляем пришедшие записи.
             * Если у записи есть id значит мы ее уже сохраняли и делаем обновление,
             * если id = 0 то это новая запись и сохраняем
             */

            try
            {
                var statuteId = baseParams.Params.ContainsKey("statuteId") ? baseParams.Params["statuteId"].ToLong() : 0;

                if (baseParams.Params.ContainsKey("modifiedRecordsJson"))
                {
                    // объявляем сервисы для послед работы с ними
                    var service = Container.Resolve<IDomainService<ActivityTsjArticle>>();
                    var serviceStatute = Container.Resolve<IDomainService<ActivityTsjStatute>>();

                    // сериализуем измененные записи в прокси класс
                    var articleProxyArray = baseParams.Params["modifiedRecordsJson"]
                        .As<List<object>>()
                        .Select(x => x.As<DynamicDictionary>().ReadClass<ArticleProxy>())
                        .ToList();

                    // бежим по массиву пришедших рекордов, которые мы сериализовали в прокси класс
                    foreach (var articleProxy in articleProxyArray)
                    {
                        if (articleProxy.Id > 0)
                        {
                            var articleStatute = service.Load(articleProxy.Id);
                            articleStatute.IsNone = articleProxy.IsNone;
                            articleStatute.Paragraph = articleProxy.Paragraph;
                            articleStatute.TypeState = articleProxy.TypeState;

                            service.Update(articleStatute);
                        }
                        else
                        {
                            var articleStatute = new ActivityTsjArticle
                                {
                                    ActivityTsjStatute = serviceStatute.Load(statuteId),
                                    ArticleTsj = new ArticleTsj {Id = articleProxy.ArticleTsjId},
                                    IsNone = articleProxy.IsNone,
                                    Paragraph = articleProxy.Paragraph,
                                    TypeState = articleProxy.TypeState
                                };

                            service.Save(articleStatute);
                        }
                    }
                }

                return new BaseDataResult();
            }
            catch (ValidationException e)
            {
                return new BaseDataResult { Success = false, Message = e.Message };
            }
        }

        // Прокси класс для десериализации JSON
        private class ArticleProxy
        {
            public long Id { get; set; }

            public long ArticleTsjId { get; set; }

            public bool IsNone { get; set; }

            public TypeState TypeState { get; set; }

            public string Paragraph { get; set; }
        }
    }
}
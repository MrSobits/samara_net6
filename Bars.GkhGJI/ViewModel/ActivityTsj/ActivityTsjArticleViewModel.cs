namespace Bars.GkhGji.ViewModel
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    public class ActivityTsjArticleViewModel : BaseViewModel<ActivityTsjArticle>
    {
        public override IDataResult List(IDomainService<ActivityTsjArticle> domainService, BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            /*
             *  Данный метод возвращает список всех статей из справочника. 
             *  Если данные статьи подвергались редактированию в инлайн гриде формы то 
             *  список добивается отредактированными полями (IsNone, Paragraph, TypeState).
             *  Сделано для того что бы хранить только данные по которым была добавлена
             *  инф-я (поля: Отсутствует, пункт устава, проверено), а отображать все записи из справочника
             */

            var statuteId = baseParams.Params.ContainsKey("statuteId")
                           ? baseParams.Params["statuteId"].ToLong()
                           : 0;

            // Список возвращаемых объектов
            var listCollection = new List<ActivityTsjArticleProxy>();

            // Формируем все статьи из справочника
            var articleTsjList = Container.Resolve<IDomainService<ArticleTsj>>().GetAll().ToList();

            // Получаем сохраненные данные по статьям
            var articleList = domainService.GetAll()
                .Where(x => x.ActivityTsjStatute.Id == statuteId)
                .Select(x => new
                {
                    x.Id,
                    ArticleTsjId = x.ArticleTsj.Id,
                    x.ArticleTsj.Name,
                    x.ArticleTsj.Code,
                    x.ArticleTsj.Group,
                    x.IsNone,
                    x.Paragraph,
                    x.TypeState
                })
                .ToList();

            // Для каждой статьи из справочника ищем данные по ее id в сущности, если есть добиваем ее ими
            foreach (var article in articleTsjList)
            {
                var activityTsjGjiArticle = articleList.FirstOrDefault(x => x.ArticleTsjId == article.Id);
                if (activityTsjGjiArticle != null)
                {
                    listCollection.Add(new ActivityTsjArticleProxy
                    {
                        Id = activityTsjGjiArticle.Id,
                        ArticleTsjId = article.Id,
                        Name = article.Name,
                        Code = article.Code,
                        Group = article.Group,
                        IsNone = activityTsjGjiArticle.IsNone,
                        Paragraph = activityTsjGjiArticle.Paragraph,
                        TypeState = activityTsjGjiArticle.TypeState
                    });
                }
                else
                {
                    listCollection.Add(new ActivityTsjArticleProxy
                    {
                        Id = article.Id,
                        ArticleTsjId = 0,
                        Name = article.Name,
                        Code = article.Code,
                        Group = article.Group,
                        IsNone = false,
                        Paragraph = string.Empty,
                        TypeState = TypeState.NotSet
                    });
                }
            }

            var dataListCollection = listCollection.AsQueryable().Filter(loadParam, Container);

            int totalCount = dataListCollection.Count();

            return new ListDataResult(dataListCollection.Order(loadParam).Paging(loadParam).ToList(), totalCount);
        }

        private class ActivityTsjArticleProxy
        {
            public long Id { get; set; }
                   
            public long ArticleTsjId { get; set; }

            public string Name { get; set; }

            public string Code { get; set; }

            public string Group { get; set; }

            public bool IsNone { get; set; }

            public string Paragraph { get; set; }

            public TypeState TypeState { get; set; }
        }
    }
}
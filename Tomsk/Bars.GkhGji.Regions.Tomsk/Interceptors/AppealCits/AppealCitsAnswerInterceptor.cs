namespace Bars.GkhGji.Regions.Tomsk.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Tomsk.Entities;

    /// <summary>
    /// Interceptor сущности Ответ по обращению
    /// </summary>
    public class AppealCitsAnswerInterceptor : GkhGji.Interceptors.AppealCitsAnswerInterceptor
    {
        /// <summary>
        /// Метод проверки перед удалением сущности. Перед удалением сущности удаляются связанные объекты
        /// </summary>
        /// <param name="service"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override IDataResult BeforeDeleteAction(IDomainService<AppealCitsAnswer> service, AppealCitsAnswer entity)
        {
            var result = base.BeforeDeleteAction(service, entity);

            if (!result.Success)
            {
                return result;
            }

            var appealCitsAnswerAddresseeDomain = Container.Resolve<IDomainService<AppealCitsAnswerAddressee>>();

            using (Container.Using(appealCitsAnswerAddresseeDomain))
            {
                appealCitsAnswerAddresseeDomain.GetAll()
                   .Where(x => x.Answer == entity)
                   .Select(x => x.Id)
                   .ForEach(x => appealCitsAnswerAddresseeDomain.Delete(x));
            }
            
            return this.Success();
        }

        /// <summary>
        /// Метод перед обновлением сущности. Проставляет дополнительные значения в связанные объекты
        /// </summary>
        /// <param name="service"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override IDataResult BeforeUpdateAction(IDomainService<AppealCitsAnswer> service, AppealCitsAnswer entity)
        {
            // Последнею дату из Ответов обращения сохранить в Срок исполнения поручителя (Дата ответа)
            // Сформированным ответом считается ответ, у которого в форме редактирования обращения на вкладке "Ответы" заполненны столбцы "Документ", "Дата документа", "Номер документа"
            var answersLastDate = service.GetAll()
                .Where(x => x.AppealCits.Id == entity.AppealCits.Id)
                .Where(x => x.DocumentNumber != "" && x.DocumentNumber != null)
                .Where(x => x.DocumentName != "" && x.DocumentName != null)
                .Where(x => x.DocumentDate.HasValue)
                .Select(x => x.DocumentDate)
                .OrderByDescending(x => x)
                .FirstOrDefault();

            if (answersLastDate != null)
            {
                entity.AppealCits.SuretyDate = answersLastDate;
            }
            
            return this.Success();
        }
    }
}
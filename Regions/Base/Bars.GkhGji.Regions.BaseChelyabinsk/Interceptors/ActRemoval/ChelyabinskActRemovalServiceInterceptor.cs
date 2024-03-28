namespace Bars.GkhGji.Regions.BaseChelyabinsk.Interceptors.ActRemoval
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Interceptors;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.ActCheck;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.ActRemoval;

    public class ChelyabinskActRemovalServiceInterceptor : ActRemovalServiceInterceptor<ChelyabinskActRemoval>
    {
		public override IDataResult BeforeDeleteAction(IDomainService<ChelyabinskActRemoval> service, ChelyabinskActRemoval entity)
		{
			var provDocService = this.Container.Resolve<IDomainService<ActRemovalProvidedDoc>>();

			try
			{
				var refFuncs = new List<Func<long, string>>
                {
                    id => provDocService.GetAll().Any(x => x.ActRemoval.Id == id) ? "Предоставленные документы" : null
                };

				var refs = refFuncs.Select(x => x(entity.Id)).Where(x => x != null).ToArray();

				var message = string.Empty;

				if (refs.Length > 0)
				{
					message = refs.Aggregate(message, (current, str) => current + string.Format(" {0}; ", str));
					message = string.Format("Существуют связанные записи в следующих таблицах: {0}", message);
					return this.Failure(message);
				}

				return base.BeforeDeleteAction(service, entity);
			}
			finally
			{
				this.Container.Release(provDocService);
			}
		}

        public override IDataResult AfterCreateAction(IDomainService<ChelyabinskActRemoval> service, ChelyabinskActRemoval entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.DocumentGji, entity.Id, entity.GetType(), GetPropertyValues(), entity.Id.ToString());
            }
            catch
            {

            }
            return this.Success();
        }

        public override IDataResult AfterUpdateAction(IDomainService<ChelyabinskActRemoval> service, ChelyabinskActRemoval entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.DocumentGji, entity.Id, entity.GetType(), GetPropertyValues(), entity.DocumentNumber);
            }
            catch
            {

            }
            return this.Success();
        }

        public override IDataResult AfterDeleteAction(IDomainService<ChelyabinskActRemoval> service, ChelyabinskActRemoval entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(null, TypeEntityLogging.DocumentGji, entity.Id, entity.GetType(), GetPropertyValues(), entity.DocumentNumber);
            }
            catch
            {

            }
            return this.Success();
        }

        private Dictionary<string, string> GetPropertyValues()
        {
            var result = new Dictionary<string, string>
            {
                { "Inspection", "Проверка ГЖИ" },
                { "Stage", "Этап проверки" },
                { "TypeDocumentGji", "Тип документа ГЖИ" },
                { "DocumentDate", "Дата документа" },
                { "DocumentNumber", "Номер документа" },
                { "DocumentSubNum", "Дополнительный номер документа (порядковый номер если документов одного типа несколько)" },
                { "State", "Статус" },
                { "TypeRemoval", "Признак устранено или неустранено нарушение" },
                { "Description", "Описание" },
                { "Area", "Площадь" },
                { "Flat", "Квартира" },
                { "AcquaintedWithDisposalCopy", "С копией приказа ознакомлен" },
                { "DocumentPlace", "Место составления" },
                { "DocumentTime", "Время составления акта" }
            };
            return result;
        }
    }
}
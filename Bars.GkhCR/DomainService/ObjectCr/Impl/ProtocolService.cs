namespace Bars.GkhCr.DomainService
{
	using B4.DataAccess;
	using Bars.B4;
	using Bars.B4.DomainService.BaseParams;
	using Bars.Gkh.Entities.Dicts.Multipurpose;
	using Bars.GkhCr.Entities;
	using Bars.GkhCr.Localizers;
	using Castle.Windsor;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Bars.B4.Utils;
	using Bars.Gkh.Config;
	using Bars.Gkh.ConfigSections.Cr;
	using Bars.Gkh.ConfigSections.Cr.Enums;
	using Bars.Gkh.Utils;

    /// <summary>
    /// Сервис протоколов Объекта КР
    /// </summary>
    public class ProtocolService : IProtocolService
    {
        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Вернуть актуальные даты
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат запроса</returns>
        public IDataResult GetDates(BaseParams baseParams)
        {
            var objectCrId = baseParams.Params.GetAs<long>("objectCrId", 0);
            var period =
                Container.Resolve<IDomainService<Entities.ObjectCr>>().GetAll()
                    .Where(x => x.Id == objectCrId)
                    .Select(x => new { x.ProgramCr.Period.DateStart, x.ProgramCr.Period.DateEnd })
                    .FirstOrDefault();

            return new BaseDataResult(new
                {
                    DateStart = period.DateStart.ToShortDateString(),
                    DateEnd = period.DateEnd.HasValue ? period.DateEnd.Value.ToShortDateString() : DateTime.MaxValue.ToShortDateString()
                });
        }

        /// <summary>
        /// Вернуть актуальные типы документов
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат запроса</returns>
        public IDataResult GetTypeDocumentCr(BaseParams baseParams)
        {
            var loadParams = baseParams.Params.Read<LoadParam>().Execute(Converter.ToLoadParam);
            
            var objectCrId = loadParams.Filter.GetAs<long>("objectCrId");
            var protocolId = loadParams.Filter.GetAs<long>("protocolId");

            var documents =
                Container.ResolveDomain<ProtocolCr>().GetAll()
                    .Where(x => x.ObjectCr.Id == objectCrId)
                    .Select(x => new { x.Id, x.TypeDocumentCr });

            var existTypeDocument = documents
                .AsEnumerable() 
                .GroupBy(x => x.TypeDocumentCr)
                .ToDictionary(x => x.Key.Key);

	        var allGlossaryItems = TypeDocumentCrLocalizer.GetAllGlossaryItems();
	        var dictTypeDocument = allGlossaryItems
		        .Where(x => !TypeDocumentCrLocalizer.IsDefault(x.Key))
		        .ToDictionary(x => x.Value.Id, x => x.Value);

			AddTypeDocumentCrItemByKey(allGlossaryItems, dictTypeDocument, TypeDocumentCrLocalizer.ActKey);

            var documentAddingOrder = Container.GetGkhConfig<GkhCrConfig>().General.DocumentAddingOrder;
            if (documentAddingOrder == DocumentAddingOrder.Use)
            {
                if (existTypeDocument.Count == 0)
                {
                    AddTypeDocumentCrItemByKey(allGlossaryItems, dictTypeDocument, TypeDocumentCrLocalizer.ProtocolFailureCrKey);
                    AddTypeDocumentCrItemByKey(allGlossaryItems, dictTypeDocument, TypeDocumentCrLocalizer.ProtocolNeedCrKey);
                    return new ListDataResult(
                        dictTypeDocument.Select(x => new {Id = x.Key, Name = x.Value.Value, Key = x.Value.Key}).ToList(), dictTypeDocument.Count);
                }

                GetActualDocumentTypes(existTypeDocument.Keys.ToList(), allGlossaryItems, dictTypeDocument);
            }
            else
            {
                AddTypeDocumentCrItemByKey(allGlossaryItems, dictTypeDocument, TypeDocumentCrLocalizer.ProtocolFailureCrKey);
                AddTypeDocumentCrItemByKey(allGlossaryItems, dictTypeDocument, TypeDocumentCrLocalizer.ProtocolNeedCrKey);
                AddTypeDocumentCrItemByKey(allGlossaryItems, dictTypeDocument, TypeDocumentCrLocalizer.ProtocolChangeCrKey);
                AddTypeDocumentCrItemByKey(allGlossaryItems, dictTypeDocument, TypeDocumentCrLocalizer.ActExpluatatinAfterCrKey);
                AddTypeDocumentCrItemByKey(allGlossaryItems, dictTypeDocument, TypeDocumentCrLocalizer.ProtocolCompleteCrKey);
                AddTypeDocumentCrItemByKey(allGlossaryItems, dictTypeDocument, TypeDocumentCrLocalizer.ActAuditDataExpenseKey);
            }

            //так как уже могут быть добавленные документы, то добавляем из в список типов доступных документов
            if (protocolId > 0)
            {
                var currentDocument = documents.FirstOrDefault(x => x.Id == protocolId);
				if (currentDocument != null && currentDocument.TypeDocumentCr != null)
                {
                    if (!dictTypeDocument.ContainsKey(currentDocument.TypeDocumentCr.Id))
                    {
						AddTypeDocumentCrItemByKey(allGlossaryItems, dictTypeDocument, currentDocument.TypeDocumentCr.Key);
                    }
                }
            }
            

            return new ListDataResult(dictTypeDocument.Select(x => new { Id = x.Key, Name = x.Value.Value, Key = x.Value.Key }).ToList(), dictTypeDocument.Count);
        }

        /// <summary>
        /// Добавить типы работ
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат запроса</returns>
        public IDataResult AddTypeWorks(BaseParams baseParams)
        {
            var protocolCrTypeWorkDomain = Container.ResolveDomain<ProtocolCrTypeWork>();
            var typeWorkDomain = Container.ResolveDomain<TypeWorkCr>();
            var protocolCrDomain = Container.ResolveDomain<ProtocolCr>();
            try
            {
                var protocolCrId = baseParams.Params.GetAs<long>("protocolCrId");
                var objectIds = baseParams.Params["objectIds"].ToStr().Split(',').Select(x => x.ToLong()).ToList();
                var exsistingTypeWorks = protocolCrTypeWorkDomain.GetAll().Where(x => x.Protocol.Id == protocolCrId).Select(x => x.TypeWork.Id).ToList();

                foreach (var id in objectIds.Where(x => !exsistingTypeWorks.Contains(x)))
                {
                    var newProtocolCrTypeWork = new ProtocolCrTypeWork
                    {
                        Protocol = protocolCrDomain.Load(protocolCrId),
                        TypeWork = typeWorkDomain.Load(id),
                    };
                    protocolCrTypeWorkDomain.Save(newProtocolCrTypeWork);
                }

                return new BaseDataResult { Success = true };
            }
            catch (ValidationException exc)
            {
                return new BaseDataResult { Success = false, Message = exc.Message };
            }
            finally
            {
                Container.Release(protocolCrTypeWorkDomain);
                Container.Release(typeWorkDomain);
                Container.Release(protocolCrDomain);
            }
        }

        /// <summary>
        /// Алгоритм добавления актуальных типов договоров
        /// </summary>
        /// <param name="existTypeDocument">Уже добавленные типы документов</param>
        /// <param name="allGlossaryItems">Данные из универсального справочника</param>
        /// <param name="dictTypeDocument">Результирующие типы документов</param>
        private void GetActualDocumentTypes(IList<string> existTypeDocument, Dictionary<string, MultipurposeGlossaryItem> allGlossaryItems,
            Dictionary<long, MultipurposeGlossaryItem> dictTypeDocument)
        {
            if (!existTypeDocument.Contains(TypeDocumentCrLocalizer.ProtocolChangeCrKey) &&
                !existTypeDocument.Contains(TypeDocumentCrLocalizer.ProtocolCompleteCrKey) &&
                !existTypeDocument.Contains(TypeDocumentCrLocalizer.ActExpluatatinAfterCrKey))
            {
                AddTypeDocumentCrItemByKey(allGlossaryItems, dictTypeDocument, TypeDocumentCrLocalizer.ProtocolNeedCrKey);
            }

            if (existTypeDocument.Contains(TypeDocumentCrLocalizer.ProtocolNeedCrKey))
            {
                if (!existTypeDocument.Contains(TypeDocumentCrLocalizer.ProtocolCompleteCrKey))
                {
                    AddTypeDocumentCrItemByKey(allGlossaryItems, dictTypeDocument, TypeDocumentCrLocalizer.ProtocolCompleteCrKey);
                }

                AddTypeDocumentCrItemByKey(allGlossaryItems, dictTypeDocument, TypeDocumentCrLocalizer.ProtocolChangeCrKey);
            }

            if (existTypeDocument.Contains(TypeDocumentCrLocalizer.ProtocolCompleteCrKey))
            {
                AddTypeDocumentCrItemByKey(allGlossaryItems, dictTypeDocument, TypeDocumentCrLocalizer.ActAuditDataExpenseKey);

                AddTypeDocumentCrItemByKey(allGlossaryItems, dictTypeDocument, TypeDocumentCrLocalizer.ActExpluatatinAfterCrKey);
            }
        }

		private void AddTypeDocumentCrItemByKey(Dictionary<string, MultipurposeGlossaryItem> cache, Dictionary<long, MultipurposeGlossaryItem> items, string key)
		{
			var item = TypeDocumentCrLocalizer.GetItemFromCacheByKey(cache, key);
			if (item != null)
			{
				items.Add(item.Id, item);
			}
		}
    }
}
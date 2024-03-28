namespace Bars.GkhCr.Localizers
{
	using Bars.B4.Application;
	using Bars.B4.DataAccess;
	using Bars.B4.IoC;
	using Bars.Gkh.Entities.Dicts.Multipurpose;
	using Castle.Windsor;
	using System.Collections.Generic;
	using System.Linq;

    /// <summary>
    /// Локализация по типам договоров
    /// </summary>
	public static class TypeDocumentCrLocalizer
	{
        /// <summary>
        /// Код справочника
        /// </summary>
		public const string GlossaryCode = "type_document_cr";

        /// <summary>
        /// Акт выполненных работ
        /// </summary>
        public const string ActKey = "Act";
        /// <summary>
        /// Протокол об отказе от КР
        /// </summary>
		public const string ProtocolFailureCrKey = "ProtocolFailureCr";
        /// <summary>
        /// Протокол о необходимости проведения КР
        /// </summary>
		public const string ProtocolNeedCrKey = "ProtocolNeedCr";
        /// <summary>
        /// Протокол о внесение изменений в КР
        /// </summary>
		public const string ProtocolChangeCrKey = "ProtocolChangeCr";
        /// <summary>
        /// Акт ввода в эксплуатацию дома после капремонта
        /// </summary>
		public const string ActExpluatatinAfterCrKey = "ActExpluatatinAfterCr";
        /// <summary>
        /// Протокол о завершении капремонта
        /// </summary>
		public const string ProtocolCompleteCrKey = "ProtocolCompleteCr";
        /// <summary>
        /// Акт сверки данных о расходах
        /// </summary>
		public const string ActAuditDataExpenseKey = "ActAuditDataExpense";

		private static readonly IWindsorContainer Container = ApplicationContext.Current.Container;

		private static readonly Dictionary<string, string> _items = new Dictionary<string, string>
        {
            { ActKey, "Акт выполненных работ" },
            { ProtocolFailureCrKey, "Протокол об отказе от КР" },
            { ProtocolNeedCrKey, "Протокол о необходимости проведения КР" },
            { ProtocolChangeCrKey, "Протокол о внесение изменений в КР" },
            { ActExpluatatinAfterCrKey, "Акт ввода в эксплуатацию дома после капремонта" },
            { ProtocolCompleteCrKey, "Протокол о завершении капремонта" },
            { ActAuditDataExpenseKey, "Акт сверки данных о расходах" }
        };

        /// <summary>
        /// Вернуть значения по умолчанию
        /// </summary>
        /// <returns></returns>
		public static Dictionary<string, string> GetDefaultItems()
		{
			return _items;
		}

        /// <summary>
        /// Тип документа по умолчанию
        /// </summary>
        /// <param name="code">Код типа документа</param>
        /// <returns>Флаг</returns>
		public static bool IsDefault(string code)
		{
			return _items.ContainsKey(code);
		}

        /// <summary>
        /// Вернуть все типы документов
        /// </summary>
        /// <returns>Словарь</returns>
		public static Dictionary<string, string> GetAllItems()
		{
			var glossaryDomain = Container.ResolveDomain<MultipurposeGlossary>();
			using (Container.Using(glossaryDomain))
			{
				var glossary = glossaryDomain.FirstOrDefault(x => x.Code == GlossaryCode);
				if (glossary == null)
				{
					return _items;
				}

				return glossary.Items.ToDictionary(x => x.Key, x => x.Value);
			}
		}

        /// <summary>
        /// Вернуть все типы документов
        /// </summary>
        /// <returns>Словарь типов документов тип <see cref="MultipurposeGlossaryItem"/></returns>
		public static Dictionary<string, MultipurposeGlossaryItem> GetAllGlossaryItems()
		{
			var glossaryItemService = Container.ResolveDomain<MultipurposeGlossaryItem>();
			using (Container.Using(glossaryItemService))
			{
				return glossaryItemService.GetAll()
					.Where(x => x.Glossary.Code == GlossaryCode)
					.Where(x => x.Value != null && x.Value != "")
					.ToDictionary(x => x.Key, x => x);
			}
		}

        /// <summary>
        /// Вернуть элемент из кэша
        /// </summary>
        /// <param name="cache">Кэш-хранилище</param>
        /// <param name="key">Ключ</param>
        /// <returns>Найденное значение или null</returns>
		public static MultipurposeGlossaryItem GetItemFromCacheByKey(Dictionary<string, MultipurposeGlossaryItem> cache, string key)
		{
			if (cache.ContainsKey(key))
			{
				return cache[key];
			}

			return null;
		}
	}
}
namespace Bars.GkhGji.Regions.Tomsk.DomainService.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using B4;

    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Tomsk.Enums;

    using Castle.Windsor;

    using TypeDocumentGji = Bars.GkhGji.Enums.TypeDocumentGji;

	/// <summary>
	/// Сервис для Тип требования
	/// </summary>
    public class TypeRequirementService : ITypeRequirementService
    {
		/// <summary>
		/// Контейнер
		/// </summary>
        public IWindsorContainer Container { get; set; }

		/// <summary>
		/// Домен сервис для Базовый документ ГЖИ
		/// </summary>
        public IDomainService<DocumentGji> DocumentDomain { get; set; }

		/// <summary>
		/// Домен сервис для Приказ
		/// </summary>
        public IDomainService<Disposal> DisposalDomain { get; set; }

		/// <summary>
		/// Получить элементы по документу
		/// </summary>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns>Результат выполнения запроса</returns>
        public IDataResult GetItemsByDoc(BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            var docId = loadParam.Filter.GetAs<long>("docId");
            var typeDoc = DocumentDomain.GetAll()
                .Where(x => x.Id == docId)
                .Select(x => x.TypeDocumentGji)
                .FirstOrDefault();

            var requirementDict = new Dictionary<int, string>();

            switch (typeDoc)
            {
                case TypeDocumentGji.Protocol:
                    {
                        requirementDict.Add((int)TypeRequirement.RequirementOnProtocol, TypeRequirement.RequirementOnProtocol.GetEnumMeta().Display);
                    }
                    break;

                case TypeDocumentGji.Disposal:
                    {
                        var typeDisposal = DisposalDomain.GetAll().Where(x => x.Id == docId).Select(x => x.TypeDisposal).FirstOrDefault();

                        if (typeDisposal == TypeDisposalGji.Base || typeDisposal == TypeDisposalGji.Licensing)
                        {
                            requirementDict.Add((int)TypeRequirement.RequirementOnCheck, TypeRequirement.RequirementOnCheck.GetEnumMeta().Display);
                            requirementDict.Add((int)TypeRequirement.RequirementOnInfoProvision, TypeRequirement.RequirementOnInfoProvision.GetEnumMeta().Display);
                        }
                        else if (typeDisposal == TypeDisposalGji.DocumentGji)
                        {
                            requirementDict.Add((int)TypeRequirement.RequirementOnPrescriptionCheck, TypeRequirement.RequirementOnPrescriptionCheck.GetEnumMeta().Display);
                            requirementDict.Add((int)TypeRequirement.RequirementOnInfoProvision, TypeRequirement.RequirementOnInfoProvision.GetEnumMeta().Display);
                        }
                    }
                    break;

                case TypeDocumentGji.AdministrativeCase:
                    {
                        requirementDict.Add((int)TypeRequirement.RequirementOnCheck, TypeRequirement.RequirementOnCheck.GetEnumMeta().Display);
                    }
                    break;
            }

            return new ListDataResult(requirementDict.Select(x => new { Value = x.Key, Display = x.Value }).ToList(), requirementDict.Count);
        }
    }
}

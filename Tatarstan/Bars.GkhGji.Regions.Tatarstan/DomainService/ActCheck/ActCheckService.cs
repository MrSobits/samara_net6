namespace Bars.GkhGji.Regions.Tatarstan.DomainService.ActCheck
{
    using System.Linq;

    using B4;

    using Bars.B4.IoC;
    using Bars.Gkh.Domain;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Tatarstan.Entities;
    
    /// <summary>
	/// Сервис для работы с Акт проверки
	/// </summary>
    public class ActCheckService : GkhGji.DomainService.ActCheckService
    {
	    /// <summary>
		/// Получить информацию
		/// </summary>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns>Результат выполнения запроса</returns>
        public override IDataResult GetInfo(BaseParams baseParams)
	    {
		    var documentId = baseParams.Params.GetAsId("documentId");
		    
		    var disposalDomain = this.Container.Resolve<IDomainService<TatarstanDisposal>>();
		    var docChildrenDomain = this.Container.Resolve<IDomainService<DocumentGjiChildren>>();
		    
		    using (this.Container.Using(docChildrenDomain, disposalDomain))
		    {
			    // получаем вид контроля
			    var parentId = docChildrenDomain.GetAll()
				    .Where(x => x.Children.Id == documentId)
				    .Select(x => x.Parent.Id)
				    .FirstOrDefault();

			    long controlTypeId = 0;
            
			    if (parentId > 0)
			    {
				    var disposal = disposalDomain.Get(parentId);
					controlTypeId = disposal?.ControlType?.Id ?? 0;
				}

			    var data = base.GetInfo(baseParams).Data as ActCheckInfo;
			    data.ControlTypeId = controlTypeId;

			    return new BaseDataResult(data);
		    }
	    }

	    /// <inheritdoc />
	    protected override string GetDataExportRegistrationName() => "Tat ActCheckDataExport";
    }
}
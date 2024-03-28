namespace Bars.GkhGji.ViewModel
{
    using System;
    using System.Linq;
    using B4;
    using Entities;
    using Enums;

    // пустышка на тот случай если от этого класса наследовались в других регионах
    public class DisposalViewModel : DisposalViewModel<Disposal>
    {
        // Внимание!! Код override писать в Generic калссе
    }

    // Genric Класс для тго чтобы сущность Disposal расширять через subclass В других регионах
    public class DisposalViewModel<T> : BaseViewModel<T> where T : Disposal
    {
        public override IDataResult Get(IDomainService<T> domainService, BaseParams baseParams)
        {
            var serviceDocumentChildren = Container.Resolve<IDomainService<DocumentGjiChildren>>();
            var serviceActCheck = Container.Resolve<IDomainService<ActCheck>>();

            try
            {
                var id = baseParams.Params.GetAs<long>("id");
                var obj = domainService.Get(id);

                // среди дочерних идентификаторов получаем либо ID общего акта проверки либо id акта проверки документа ГЖИ
                obj.ActCheckGeneralId = serviceActCheck.GetAll()
                    .Where(y => serviceDocumentChildren.GetAll()
                        .Any(x => x.Children.Id == y.Id
                            && x.Parent.Id == id
                            && x.Children.TypeDocumentGji == TypeDocumentGji.ActCheck))
                    .Where(x => (x.TypeActCheck == TypeActCheckGji.ActCheckGeneral || x.TypeActCheck == TypeActCheckGji.ActCheckDocumentGji))
                    .Select(x => x.Id)
                    .FirstOrDefault();

                if (obj.Inspection != null)
                {
                    // Для виджетов
                    obj.TypeBase = obj.Inspection.TypeBase;
                    obj.InspectionId = obj.Inspection.Id;
                }

                obj.HasChildrenActCheck = serviceDocumentChildren.GetAll().Count(x => x.Parent.Id == id && x.Children.TypeDocumentGji == TypeDocumentGji.ActCheck) > 0;

                return new BaseDataResult(obj);
            }
            finally
            {
                Container.Release(serviceDocumentChildren);
                Container.Release(serviceActCheck);
            }
        }
    }
}
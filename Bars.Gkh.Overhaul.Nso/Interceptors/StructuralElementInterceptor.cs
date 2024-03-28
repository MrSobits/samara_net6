namespace Bars.Gkh.Overhaul.Nso.Interceptors
{
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4;
    using Bars.Gkh.Overhaul.Nso.Entities;
    using Gkh.Entities.CommonEstateObject;
    using Gkh.Entities.RealEstateType;
    using Gkh.Utils;
    using Overhaul.Entities;

    public class StructuralElementInterceptor : EmptyDomainInterceptor<StructuralElement>
    {
        public IDomainService<RealityObjectStructuralElementInProgramm> Stage1DomainService { get; set; }

        public IDomainService<RealityObjectStructuralElement> RobjectSeDomainService { get; set; }

        public IDomainService<RealEstateTypeStructElement> RetSeDomainService { get; set; }

        public IDomainService<VersionRecordStage1> VersionStage1DomainService { get; set; }

        public override IDataResult BeforeDeleteAction(IDomainService<StructuralElement> service, StructuralElement entity)
        {
            var messages = new List<string>();

            if (Stage1DomainService.GetAll().Any(x => x.StructuralElement.StructuralElement.Id == entity.Id))
            {
                messages.Add("Первый этап долгосрочной программы");
            }

            if (RobjectSeDomainService.GetAll().Any(x => x.StructuralElement.Id == entity.Id))
            {
                messages.Add("Конструктивные элементы жилого дома");
            }

            if (RetSeDomainService.GetAll().Any(x => x.StructuralElement.Id == entity.Id))
            {
                messages.Add("Конструктивные элементы типов домов");
            }

            if (VersionStage1DomainService.GetAll().Any(x => x.StructuralElement.StructuralElement.Id == entity.Id))
            {
                messages.Add("Конструктивные элементы версии программы");
            }

            if (messages.Any())
            {
                return Failure(string.Format("Существуют связанные записи в следующих таблицах:<br>{0}<br>Удаление отменено.",
                                      messages.AggregateWithSeparator("; <br>")));
            }

            return Success();
        }
    }
}
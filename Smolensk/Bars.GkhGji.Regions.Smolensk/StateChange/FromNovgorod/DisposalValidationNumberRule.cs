using System;
using System.Globalization;
using System.Linq;
using Bars.B4;
using Bars.B4.Utils;
using Bars.Gkh.Entities;
using Bars.GkhGji.Contracts;
using Bars.GkhGji.Entities;
using Bars.GkhGji.Enums;

namespace Bars.GkhGji.Regions.Smolensk.StateChange
{
    public class DisposalValidationNumberRule : BaseDocValidationNumberRule
    {
        public IDisposalText DisposalText { get; set; }

        public override string Id { get { return "gji_disposal_validation_number"; } }

        public override string TypeId { get { return "gji_document_disp"; } }

        public override string Name 
        {
            get { return string.Format("Перенесено из ННовгорода - Проверка возможности формирования номера {0}", DisposalText.GenetiveCase.ToLower()); } 
        }

        public override string Description 
        {
            get { return string.Format("Перенесено из ННовгорода - Данное правило проверяет формирование номера {0} в соответствии с правилами", DisposalText.GenetiveCase.ToLower()); }
        }

        protected override void Action(DocumentGji document)
        {
            var serviceInspector = Container.Resolve<IDomainService<DocumentGjiInspector>>();
            var zonalInspectionService = Container.Resolve<IDomainService<ZonalInspectionInspector>>();
            var docResolutionProsecutorService = Container.Resolve<IDomainService<ResolPros>>();
            var docGjiService = Container.Resolve<IDomainService<DocumentGji>>();
            var zonInspMunicipalityService = Container.Resolve<IDomainService<ZonalInspectionMunicipality>>();

            var gjiNumber = string.Empty;

            try
            {
                document.DocumentYear =
                    document.DocumentDate.HasValue
                        ? document.DocumentDate.Value.Year
                        : 0;
                if (document.DocumentYear == 0)
                {
                    document.DocumentYear = DateTime.Now.Year;
                }

                var maxNum = docGjiService.GetAll()
                    .Where(x =>
                        x.DocumentYear == document.DocumentYear
                        && x.Id != document.Id
                        && (x.TypeDocumentGji == TypeDocumentGji.Disposal
                                || x.TypeDocumentGji == TypeDocumentGji.ResolutionProsecutor))
                    .Select(x => x.DocumentNum)
                    .Max();

                document.DocumentNum = maxNum.HasValue ? maxNum.Value + 1 : 1;

                // В зависимости от типа документа - индекс отдела получаем по-разному
                if (document.TypeDocumentGji == TypeDocumentGji.Disposal)
                {
                    var inspector = serviceInspector.GetAll().FirstOrDefault(x => x.DocumentGji.Id == document.Id);

                    if (inspector != null)
                    {
                        var zonalinspection =
                            zonalInspectionService.GetAll()
                                                  .FirstOrDefault(x => x.Inspector.Id == inspector.Inspector.Id);

                        if (zonalinspection != null)
                        {
                            gjiNumber = zonalinspection.ZonalInspection.IndexOfGji;
                        }
                    }
                }
                else
                {
                    var resolutionProsecutor =
                        docResolutionProsecutorService.GetAll().First(x => x.Id == document.Id);

                    var zonalInspection = zonInspMunicipalityService.GetAll()
                                                                    .FirstOrDefault(
                                                                        x =>
                                                                        x.Municipality ==
                                                                        resolutionProsecutor.Municipality)
                                                                    .With(x => x.ZonalInspection);
                    if (zonalInspection != null)
                    {
                        gjiNumber = zonalInspection.IndexOfGji;
                    }
                }

                if (gjiNumber.IsEmpty())
                {
                    throw new DocumentGjiStateChangeException("Не заполнен индекс ГЖИ у отдела жилищной инспекции");
                }

                var docNumbers =
                    docGjiService.GetAll()
                                 .Where(x => x.TypeDocumentGji == document.TypeDocumentGji)
                                 .Where(x => x.DocumentNumber.Length > 4)
                                 .AsEnumerable()
                                 .Where(
                                     x =>
                                     x.DocumentNumber.Substring(x.DocumentNumber.Length - 4, 4).ToInt() ==
                                     DateTime.Now.Year)
                                 .Where(x => x.DocumentNumber.StartsWith(gjiNumber))
                    // Номер расчитывается в пределах отдела ЗЖИ
                                 .Select(x => x.DocumentNumber.Split('-')[2])
                                 .ToArray();

                var maxNumber = 0;

                if (docNumbers.Any())
                {
                    var ordinalNumber =
                        docNumbers.Select(x => x.Substring(0, x.IndexOf("/", StringComparison.InvariantCulture)).ToInt())
                                  .ToList();

                    if (ordinalNumber.Any())
                    {
                        maxNumber =
                            ordinalNumber.Max();
                    }
                }

                maxNumber++;
                document.DocumentNumber = string.Format(
                    "{0}-{1}{2}/{3}",
                    gjiNumber,
                    maxNumber < 100 ? maxNumber.ToString("00") : maxNumber.ToString(CultureInfo.InvariantCulture),
                    document.TypeDocumentGji == TypeDocumentGji.ResolutionProsecutor ? "ПР" : string.Empty,
                    document.DocumentYear);
            }
            catch (Exception e)
            {
                if (e is DocumentGjiStateChangeException)
                {
                    throw;
                }

                throw new Exception("Не удалось установить номер документа");
            }
        }
    }
}
namespace Bars.Gkh.DomainService
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    
    using B4.Modules.Reports;

    using Bars.B4.Modules.Analytics.Reports.Entities;
    using Bars.B4.Utils;
    using Bars.Gkh.PassportProvider;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Serialization;

    using Castle.Windsor;

    /// <summary>
    /// Сервис для рабоыт с техническим паспортом
    /// </summary>
    public class TechPassportService : ITechPassportService
    {
        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <inheritdoc />
        public TehPassportValue GetValue(long realityObjectId, string formId, string cellCode)
        {
            // получаем провайдер реализующий необходимый паспорт
            var passport = this.Container.ResolveAll<IPassportProvider>()
                    .FirstOrDefault(x => x.Name == "Техпаспорт" && x.TypeDataSource == "xml");
            if (passport == null)
            {
                throw new Exception("Не найден провайдер технического паспорта");
            }

            var componentCodes = passport.GetComponentCodes(formId);

            return this.Container.Resolve<IDomainService<TehPassportValue>>()
                .GetAll()
                .FirstOrDefault(x => x.TehPassport.RealityObject.Id == realityObjectId && componentCodes.Contains(x.FormCode) && x.CellCode == cellCode);
        }

        public IDataResult GetForm(BaseParams baseParams)
        {
            try
            {
                // получаем провайдер реализующий необходимый паспорт
                var passport = this.Container.ResolveAll<IPassportProvider>().FirstOrDefault(x => x.Name == "Техпаспорт" && x.TypeDataSource == "xml");
                if (passport == null)
                {
                    throw new Exception("Не найден провайдер технического паспорта");
                }

                // получаем идентификатор формы и объекта недвижимости
                var formId = baseParams.Params["sectionId"].ToStr();
                var realityObjectId = baseParams.Params["realityObjectId"].ToLong();

                // получаем коды компонентов на форме
                var componentCodes = passport.GetComponentCodes(formId);

                // получаем все значения на форме
                var tehPassportValues = this.Container.Resolve<IDomainService<TehPassportValue>>().GetAll()
                    .Where(x => x.TehPassport.RealityObject.Id == realityObjectId && componentCodes.Contains(x.FormCode))
                    .ToArray();

                // заполняем данными словарь
                var data = new Dictionary<string, Dictionary<string, string>>();
                foreach (var tehPassportValue in tehPassportValues)
                {
                    var componentCode = tehPassportValue.FormCode;

                    if (data.ContainsKey(componentCode))
                    {
                        data[componentCode].Add(tehPassportValue.CellCode, tehPassportValue.Value);
                    }
                    else
                    {
                        data.Add(
                            componentCode,
                            new Dictionary<string, string> { { tehPassportValue.CellCode, tehPassportValue.Value } });
                    }
                }

                // клонируем описание и заполняем Cells значениями
                var form = (FormTechPassport)passport.GetFormWithData(formId, data);
                foreach (var comp in form.Components)
                {
                    comp.Rows = comp.Rows.ToList();
                    comp.Elements = comp.Elements.ToList();
                    comp.Columns = comp.Columns.OrderBy(x => x.Order).ToList();
                }

                // получаем описание элементов редактирования
                var editors = (List<EditorTechPassport>)passport.GetEditors(formId);

                return new BaseDataResult { Data = new { success = true, form, editors }, Success = true };
            }
            catch (ValidationException exc)
            {
                return new BaseDataResult
                    {
                        Success = false,
                        Message = exc.Message
                    };
            }
        }

        /// <inheritdoc />
        public IDataResult UpdateForm(BaseParams baseParams)
        {
            try
            {
                // получаем идентификатор формы и объекта недвижимости
                var formId = baseParams.Params["sectionId"].ToStr();
                var realityObjectId = baseParams.Params["realityObjectId"].ToLong();

                // получаем провайдер реализующий необходимый паспорт
                var passport = this.Container.ResolveAll<IPassportProvider>().FirstOrDefault(x => x.Name == "Техпаспорт" && x.TypeDataSource == "xml");
                if (passport == null)
                {
                    throw new Exception("Не найден провайдер технического паспорта");
                }

                var values = baseParams.Params["values"]
                    .As<List<object>>()
                    .Select(x => x.As<DynamicDictionary>())
                    .Select(x => x.ReadClass<SerializePassportValue>())
                    .ToList();

                passport.UpdateForm(realityObjectId, formId, values);
            }
            catch (ValidationException exc)
            {
                return new BaseDataResult
                {
                    Success = false,
                    Message = exc.Message
                };
            }

            return new BaseDataResult
                {
                    Success = true
                };
        }

        /// <inheritdoc />
        public IDataResult GetReportId(BaseParams baseParams)
        {
            try
            {
                var className = baseParams.Params["className"].ToStr();
                var reportObject = this.Container.Resolve<IDomainService<PrintForm>>().GetAll().FirstOrDefault(x => x.ClassName == className);

                if (reportObject != null)
                {
                    return new BaseDataResult
                    {
                        Success = true,
                        Data = new
                                   {
                                       ReportId = reportObject.Id,
                                   }
                    };
                }
            }
            catch (ValidationException exc)
            {
                return new BaseDataResult
                {
                    Success = false,
                    Message = exc.Message
                };
            }

            return new BaseDataResult
            {
                Success = true
            };
        }

        /// <inheritdoc />
        public IDataResult GetPrintFormResult(BaseParams baseParams)
        {
            var printFormObject = this.Container.Resolve<IRepository<PrintForm>>().Get(baseParams.Params.Get("id"));
            var printForm = this.Container.Resolve<IPrintForm>(printFormObject.ClassName);

            var rp = new ReportParams();

            printForm.SetUserParams(baseParams);
            printForm.PrepareReport(rp);
            var template = printForm.GetTemplate();

            IReportGenerator generator;
            if (printForm is IGeneratedPrintForm)
            {
                generator = printForm as IGeneratedPrintForm;
            }
            else
            {
                generator = this.Container.Resolve<IReportGenerator>("XlsIoGenerator");
            }

            var result = new MemoryStream();

            generator.Open(template);
            generator.Generate(result, rp);
            result.Seek(0, SeekOrigin.Begin);

            var reportName = string.Format("{0}.xls", printFormObject.Name);
            return new BaseDataResult(result) { Message = reportName };
        }
    }
}
namespace Bars.B4.Modules.Analytics.Reports.Domain
{
    using System;
    using System.Collections.Generic;

    using B4;
	using B4.DomainService.BaseParams;
	using B4.Modules.Pivot;
	using B4.Modules.Reports;
	using Castle.Windsor;
	using System.IO;
	using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.Analytics.Reports.Entities;
    using Bars.B4.Modules.Analytics.Reports.Entities.History;
	using Bars.B4.Modules.Analytics.Reports.Enums;
    using Bars.B4.Modules.FileStorage;

    /// <summary>
    /// The print form service.
    /// 
	/// Создан для того, чтобы переопределить поведение метода GetPrintFormResult,
	/// потому что для Стимула необходимо сначала вызвать Open, а затем Prepare.
	/// Для XlsIoGenerator на всякий случай оставлено прежнее поведение.
    /// </summary>
    public class PrintFormService : IPrintFormService
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<ReportHistory> ReportHistoryDomain { get; set; }

        public IUserInfoProvider UserInfoProvider { get; set; }

        /// <summary>
        /// The all print forms.
        /// </summary>
        /// <param name="baseParams"> The base params. </param>
        /// <returns> The <see cref="IDataResult"/>. </returns>
        public IDataResult AllPrintForms(BaseParams baseParams)
        {
            var authService = this.Container.Resolve<IAuthorizationService>();
            var userIdentity = this.Container.Resolve<IUserIdentity>();

            var pivotHandlers = this.Container.Kernel.GetAssignableHandlers(typeof(IPivotModel));
            var repository = this.Container.Resolve<IDomainService<PrintForm>>();
            var reportsToRelease = new List<IPrintForm>();

            try
            {
                var data = repository.GetAll()
                    .Select(x => new {x.Id, x.Name, CategoryName = x.Category.Name, x.ClassName, x.Description})
                    .AsEnumerable()
                    .Where(x => this.Container.Kernel.HasComponent(x.ClassName))
                    .Select(
                        x =>
                            new
                            {
                                x.Id,
                                x.Name,
                                Category = x.CategoryName,
                                x.ClassName,
                                x.Description,
                                PrintForm = this.Container.Resolve<IPrintForm>(x.ClassName)
                            })
                    .Where(x => string.IsNullOrEmpty(x.PrintForm.RequiredPermission) || authService.Grant(userIdentity, x.PrintForm.RequiredPermission));
                    
                 reportsToRelease.AddRange(data.Select(x => x.PrintForm));   
                    
                 var result = data   
                    .GroupBy(x => x.Category)
                    .Select(
                        x => new
                        {
                            Category = x.Key,
                            Reports = x.Select(
                                y => new
                                {
                                    y.Id,
                                    y.Name,
                                    y.Description,
                                    y.ClassName,
                                    IsPivot = pivotHandlers.Any(h => h.ComponentModel.Name == y.ClassName),
                                    y.PrintForm.ParamsController
                                })
                                .OrderBy(y => y.Name)
                                .ToList()
                        })
                    .OrderBy(x => x.Category)
                    .ToArray();

                return new BaseDataResult(result);
            }
            finally
            {
                this.Container.Release(repository);
                reportsToRelease.ForEach(
                    x =>
                    {
                        this.Container.Release(x);
                    });
            }
        }

        /// <summary>
        /// The list print forms classes.
        /// </summary>
        /// <param name="baseParams"> The base params. </param>
        /// <returns> The <see cref="IDataResult"/>. </returns>
        public IDataResult ListPrintFormsClasses(BaseParams baseParams)
        {
            var loadParams = baseParams.Params.Read<LoadParam>().Execute(Converter.ToLoadParam);
            
            var data = this.Container.Kernel
                .GetHandlers(typeof(IPrintForm))
                .AsQueryable()
                .Select(x => new
                {
                    Id = x.ComponentModel.Name,
                    Description = this.GetPrintFormDescription(x.ComponentModel.Name)
                });
            
            data = data.Filter(loadParams, this.Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToArray(), data.Count());
        }

        /// <summary>
        /// The get print form result.
        /// </summary>
        /// <param name="baseParams"> The base params. </param>
        /// <returns> The <see cref="IDataResult"/>. </returns>
        public IDataResult GetPrintFormResult(BaseParams baseParams)
        {
            var printFormDomainService = this.Container.Resolve<IDomainService<PrintForm>>();
            var fileManager = this.Container.Resolve<IFileManager>();

            using (var transaction = this.Container.Resolve<IDataTransaction>())
            {
                try
                {
                    var id = baseParams.Params.GetAs<long>(baseParams.Params.ContainsKey("reportId") ? "reportId" : "id");
                    var printFormObject = printFormDomainService.Get(id);

                    var printForm = this.Container.Resolve<IPrintForm>(printFormObject.ClassName);
                    try
                    {
                        Stream template;
                        IReportGenerator generator;
                        var rp = new ReportParams();

                        printForm.SetUserParams(baseParams);

                        //для отчетов на стимуле нужно сначала вызвать Open, а затем Prepare
                        if (printForm is IGeneratedPrintForm)
                        {
                            generator = printForm as IGeneratedPrintForm;

                            template = printForm.GetTemplate();
                            generator.Open(template);
                            printForm.PrepareReport(rp);
                        }
                        //для XlsIoGenerator на всякий случай поведение оставлено прежним
                        else
                        {
                            generator = this.Container.Resolve<IReportGenerator>("XlsIoGenerator");

                            printForm.PrepareReport(rp);
                            template = printForm.GetTemplate();
                            generator.Open(template);
                        }

                        var file = new MemoryStream();
                        generator.Generate(file, rp);
                        file.Seek(0, SeekOrigin.Begin);

                        var result = fileManager.SaveFile(file, this.GetFileName(printFormObject));

                        this.ReportHistoryDomain.Save(
                            new ReportHistory
                            {
                                ReportType = ReportType.PrintForm,
                                ReportId = id,
                                Date = DateTime.UtcNow,
                                Category = printFormObject.Category,
                                Name = printFormObject.Name,
                                User = this.UserInfoProvider?.GetActiveUser(),
                                File = result
                            });

                        transaction.Commit();

                        return new BaseDataResult(result.Id);
                    }
                    finally
                    {
                        this.Container.Release(printForm);
                    }
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw;
                }
                finally
                {
                    this.Container.Release(printFormDomainService);
                    this.Container.Release(fileManager);
                }
            }
        }

        /// <summary>
        /// The get print form description.
        /// </summary>
        /// <param name="className"> The class name. </param>
        /// <returns> The <see cref="string"/>. </returns>
        private string GetPrintFormDescription(string className)
        {
            var printForm = this.Container.Resolve<IPrintForm>(className);
            using (this.Container.Using(printForm))
            {
                return $"({printForm.GroupName}) {printForm.Desciption}";
            }
        }

        private string GetFileName(PrintForm printForm)
        {
            return $"{printForm.Name}.xlsx";
        }
    }
}
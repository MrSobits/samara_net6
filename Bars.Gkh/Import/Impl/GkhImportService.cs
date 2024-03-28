namespace Bars.Gkh.Import.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using B4;
    using B4.IoC;
    using B4.Modules.Tasks.Common.Contracts;
    using B4.Modules.Tasks.Common.Contracts.Result;
    using B4.Utils;
    using Bars.B4.Modules.Tasks.Common.Service;
    using Castle.Windsor;

    /// <inheritdoc />
    public class GkhImportService : IGkhImportService
    {
        public IWindsorContainer Container { get; set; }

        public ITaskManager TaskManager { get; set; }

        public List<GkhImportInfo> GetImportInfoList(BaseParams baseParams)
        {
            var codeImport = baseParams.Params.GetAs<string>("codeImport", ignoreCase: true);

            var imports = this.Container.ResolveAll<IGkhImport>();

            using (this.Container.Using(imports))
            {
                var importsData =  imports
                    .WhereIf(codeImport.IsNotEmpty(), x => x.CodeImport == codeImport)
                    .Select(x => new GkhImportInfo
                    {
                        Key = x.Key,
                        Name = x.Name,
                        PossibleFileExtensions = x.PossibleFileExtensions,
                        Dependencies = x.Dependencies,
                        PermissionName = x.PermissionName
                    })
                    .ToList();

                importsData.Add(new GkhImportInfo
                {
                    Key = "AmirsImport",
                    Name = "Импорт постановлений АМИРС",
                    PossibleFileExtensions = "xlsx",
                    Dependencies = null,
                    PermissionName = "Import.AmirsImport"
                });

                return importsData;
            }
        }

        /// <inheritdoc />
        public IDataResult Import(BaseParams baseParams)
        {
            return this.TaskManager.CreateTasks(this, baseParams);
        }

        /// <inheritdoc />
        public IDataResult MultiImport(BaseParams baseParams)
        {
            var files = baseParams.Files;

            if (files.IsEmpty())
            {
                throw new ValidationException("Нет файлов для импорта");
            }

            foreach (var fileData in files)
            {
                var taskParams = new BaseParams
                {
                    Params = baseParams.Params,
                    Files = new Dictionary<string, FileData>
                    {
                        { "FileImport", fileData.Value }
                    }
                };

                this.TaskManager.CreateTasks(this, taskParams);
            }

            return new BaseDataResult();
        }

        #region Implementation of ITaskProvider
        
        /// <inheritdoc />
        public CreateTasksResult CreateTasks(BaseParams @params)
        {
            var importId = @params.Params["importId"].ToStr();
            var userIdentity = this.Container.Resolve<IUserIdentity>();
            @params.Params["userId"] = userIdentity.UserId;

            if (importId.IsEmpty())
            {
                throw new Exception("Не найден параметр импорта");
            }

            var import = this.Container.Resolve<IGkhImport>(importId);

            using (this.Container.Using(import))
            {
                if (import == null)
                {
                    throw new NotImplementedException("Импорт не реализован. Код: {0}".FormatUsing(importId));
                }

                string[] importKeys = import.Dependencies;

                var dependency = new List<Dependency>
                {
                    new Dependency
                    {
                        Scope = DependencyScope.InsideExecutors,
                        Key = importId
                    }
                };

                if (importKeys.IsNotEmpty())
                {
                    dependency.AddRange(
                        importKeys.Select(
                            i => new Dependency
                            {
                                Scope = DependencyScope.InsideExecutors,
                                Key = i
                            }));
                }

                if (!import.Validate(@params, out var message))
                {
                    throw new ValidationException(message);
                }

                return new CreateTasksResult(
                    new[]
                    {
                        new TaskDescriptor(import.Name, importId, @params)
                        {
                            Dependencies = dependency.ToArray(),
                            Description = string.Join(", ", @params.Files
                                .Where(x => x.Value != null)
                                .Select(x => x.Value.FileName))
                        }
                    });
            }
        }

        public string TaskCode => "GkhImports";
        
        #endregion
    }
}
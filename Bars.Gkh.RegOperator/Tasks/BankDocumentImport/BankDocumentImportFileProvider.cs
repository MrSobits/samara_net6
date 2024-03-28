namespace Bars.Gkh.RegOperator.Tasks.BankDocumentImport
{
	using System.Collections.Generic;
	using System.Linq;

	using B4;
	using B4.IoC;
    using B4.Modules.Tasks.Common.Contracts;
    using B4.Modules.Tasks.Common.Contracts.Result;
    using B4.Modules.Tasks.Common.Service;

	using Bars.Gkh.Import;
	using Bars.Gkh.RegOperator.Imports;

	using Castle.Windsor;

	public class BankDocumentImportFileProvider : ITaskProvider
    {
        private readonly IWindsorContainer container;

        public BankDocumentImportFileProvider(IWindsorContainer container)
        {
            this.container = container;
        }

		public string TaskCode { get { return "BankDocumentImportFileProvider"; } }

		public CreateTasksResult CreateTasks(BaseParams baseParams)
	    {
		    var importFileId = baseParams.Params.GetAs<string>("importId");
		    var importArchiveId = typeof(BankArchiveImport).FullName;
            var importFiles = this.container.Resolve<IGkhImport>(BankDocumentImport.Id);
            var importArchives = this.container.Resolve<IGkhImport>(BankArchiveImport.Id);

            var userIdentity = this.container.Resolve<IUserIdentity>();
		    baseParams.Params["userId"] = userIdentity.UserId;

            using (this.container.Using(importFiles, importArchives))
            {
                var descriptors = new List<TaskDescriptor>();

                foreach (var file in baseParams.Files)
                {
                    var taskParams = new BaseParams
                    {
                        Params = baseParams.Params,
                        Files = new Dictionary<string, FileData> { { "FileImport", file.Value } }
                    };

                    
                    if (BankArchiveImport.IsSupportArchive(file.Value))
                    {
                        descriptors.Add(new TaskDescriptor(importArchives.Name, importArchiveId, taskParams));
                    }
                    else
                    {
                        descriptors.Add(new TaskDescriptor(importFiles.Name, importFileId, taskParams));
                    }

                }

                return new CreateTasksResult(descriptors.ToArray());
            }
        }
    }
}
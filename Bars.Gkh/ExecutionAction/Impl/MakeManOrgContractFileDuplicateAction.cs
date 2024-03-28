namespace Bars.Gkh.ExecutionAction.Impl
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Entities;

    using FileInfo = Bars.B4.Modules.FileStorage.FileInfo;

    public class MakeManOrgContractFileDuplicateAction : BaseExecutionAction
    {
        public override string Description => "Создать независимые файлы договоров (к ошибке 33777)";

        public override string Name => "Создать независимые файлы договоров (к ошибке 33777)";

        public override Func<IDataResult> Action => this.MakeManOrgContractFileDuplicates;

        protected FileInfo ReCreateFile(FileInfo fileInfo, IFileManager fileManager)
        {
            if (fileInfo == null)
            {
                return null;
            }

            var data = new byte[0];

            try
            {
                var fileInfoStream = fileManager.GetFile(fileInfo);

                data = new byte[fileInfoStream.Length];

                fileInfoStream.Seek(0, SeekOrigin.Begin);
                fileInfoStream.Read(data, 0, data.Length);
                fileInfoStream.Seek(0, SeekOrigin.Begin);
            }
            catch (FileNotFoundException)
            {
                return null;
            }

            var newFileInfo = fileManager.SaveFile(fileInfo.Name, fileInfo.Extention, data);

            return newFileInfo;
        }

        protected BaseDataResult MakeManOrgContractFileDuplicates()
        {
            var manOrgBaseContractService = this.Container.Resolve<IDomainService<ManOrgBaseContract>>();
            var fileManager = this.Container.Resolve<IFileManager>();
            var sessionProvider = this.Container.Resolve<ISessionProvider>();

            using (this.Container.Using(manOrgBaseContractService, fileManager, sessionProvider))
            {
                var fileGroupedContracts = manOrgBaseContractService.GetAll()
                    .Where(x => x.FileInfo != null)
                    .Select(x => new {x.FileInfo, ManOrgBaseContract = x})
                    .AsEnumerable()
                    .GroupBy(x => x.FileInfo)
                    .Where(x => x.Count() > 1)
                    .ToDictionary(x => x.Key, x => x.Select(y => y.ManOrgBaseContract).ToList());

                var listToUpdate = new List<ManOrgBaseContract>();

                try
                {
                    foreach (var pair in fileGroupedContracts)
                    {
                        var fileInfo = pair.Key;

                        pair.Value.RemoveAt(0); // Самый первый экземпляр файла оставляем на месте 
                        var contractsToReCreateFile = pair.Value;

                        foreach (var contractToReCreateFile in contractsToReCreateFile)
                        {
                            contractToReCreateFile.FileInfo = this.ReCreateFile(fileInfo, fileManager);
                            listToUpdate.Add(contractToReCreateFile);
                        }
                    }
                }
                catch
                {
                    return new BaseDataResult(false, "Возникла ошибка при выполнении действия");
                }

                sessionProvider.CloseCurrentSession();

                using (var session = sessionProvider.OpenStatelessSession())
                {
                    using (var transaction = session.BeginTransaction())
                    {
                        try
                        {
                            listToUpdate.ForEach(session.Update);
                            transaction.Commit();
                        }
                        catch
                        {
                            transaction.Rollback();
                            return new BaseDataResult(false, "Возникла ошибка при выполнении действия");
                        }
                    }
                }
            }

            return new BaseDataResult();
        }
    }
}
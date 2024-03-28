namespace Bars.GkhCr.Interceptors
{
    using System;
    using Bars.B4;
    using Entities;
    using System.Linq;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.DataAccess;
    using Bars.Gkh.FileManager;

    class CrFileRegisterInterceptor : EmptyDomainInterceptor<CrFileRegister>
    {
        public override IDataResult BeforeCreateAction(IDomainService<CrFileRegister> service, CrFileRegister entity)
        {
            var record = service.GetAll().FirstOrDefault(x => x.RealityObject.Id == entity.RealityObject.Id);

            if (record == null)
            {
                return Success();
            }
            else
            {
                return Failure($"Запись с адресом {record.RealityObject.Address} имеется в реестре.");
            }

        }

        public override IDataResult BeforeDeleteAction(IDomainService<CrFileRegister> service, CrFileRegister entity)
        {
            if (entity.File == null)
                return Success();

            var fileInfoDomain = Container.Resolve<IDomainService<FileInfo>>();
            var fileManager = this.Container.Resolve<IFileManager>();
            using (var transaction = Container.Resolve<IDataTransaction>())
            {
                try
                {
                    var file = fileInfoDomain.Get(entity.File.Id);
                    fileInfoDomain.Delete(file.Id);
                    var fullPath = $"{((FileSystemFileManager)fileManager).FilesDirectory.FullName}\\{file.ObjectCreateDate.Year}\\{file.ObjectCreateDate.Month}\\{file.Id}.{file.Extention}";
                    System.IO.File.Delete(fullPath);

                    transaction.Commit();
                    return Success();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    return Failure("Не удалось удалить файл.");
                }
                finally
                {
                    Container.Release(fileInfoDomain);
                }
            }
        }
    }
}

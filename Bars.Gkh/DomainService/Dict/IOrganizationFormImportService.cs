using System.IO;
using Bars.B4;
namespace Bars.Gkh.DomainService.Dict
{
    public interface IOrganizationFormImportService
    {
        IDataResult Import(FileData fileData);
    }
}

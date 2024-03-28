namespace Bars.GkhGji.DomainService
{
    using Bars.B4;
    using Bars.B4.Modules.FileStorage.DomainService;
    using Bars.GkhGji.Entities;

    // такую пустышку делаю чтобы в регионах заменять, но для этого надо чтобы именно она была зарегистрирована в основном модуле
    public class ResolutionDefinitionDomainService : ResolutionDefinitionDomainService<ResolutionDefinition>
    {
        // Внимание !! Код override нужно писать не в этом классе а в Generic
    }

    // Такую фигню делаю чтобы в модулях регионов расширять сущность ResolutionDefinition
    // FileStorageDomainService поставил потому что в регионах данная сущность рсширяется файлами 
    public class ResolutionDefinitionDomainService<T> : FileStorageDomainService<T>
        where T : ResolutionDefinition
    {
        
    }
}
namespace Bars.GkhGji.DomainService
{
    using Bars.B4;
    using Bars.B4.Modules.FileStorage.DomainService;
    using Bars.GkhGji.Entities;

    // такую пустышку делаю чтобы в регионах заменять, но для этого надо чтобы именно она была зарегистрирована в основном модуле
    public class ProtocolDefinitionDomainService : ProtocolDefinitionDomainService<ProtocolDefinition>
    {
        // Внимание !! Код override нужно писать не в этом классе а в Generic
    }

    // Такую фигню делаю чтобы в модулях регионов расширять сущность ProtocolDefinition
    // FileStorageDomainService делаю потмоучтов регионах сущностьдобавляются файлы, 
    // чтобы работало увсех и у регионов где есть файлы и где нет файлов
    public class ProtocolDefinitionDomainService<T> : FileStorageDomainService<T>
        where T : ProtocolDefinition
    {
        
    }
}
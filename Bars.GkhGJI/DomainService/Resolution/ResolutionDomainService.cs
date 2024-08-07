﻿namespace Bars.GkhGji.DomainService
{
    using Bars.B4;
    using Bars.GkhGji.Entities;

    // такую пустышку делаю чтобы в регионах заменять , но для этого надо чтобы именно она была зарегистрирована в основном модуле
    public class ResolutionDomainService : ResolutionDomainService<Resolution>
    {
        // Внимание !! Код override нужно писать не в этом классе а в Generic
    }

    // Такую фигню делаю чтобы в модулях регионов расширять сущность Resolution
    public class ResolutionDomainService<T> : BaseDomainService<T>
        where T : Resolution
    {
        
    }
}
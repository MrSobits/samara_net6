﻿namespace Bars.GkhGji.Regions.Habarovsk.DomainService
{
    using Entities;
    using System.Collections;

    using B4;
    
    public interface ICourtPracticeOperationsService
    {
        IDataResult AddCourtPracticeRealityObjects(BaseParams baseParams);

        IDataResult ListDocsForSelect(BaseParams baseParams);

        object GetDocInfo(BaseParams baseParams);
    }
}
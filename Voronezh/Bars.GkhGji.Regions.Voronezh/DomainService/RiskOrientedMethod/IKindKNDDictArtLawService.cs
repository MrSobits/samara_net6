﻿namespace Bars.GkhGji.Regions.Voronezh.DomainService
{
    using Entities;
    using System.Collections;

    using B4;
    
    public interface IKindKNDDictArtLawService
    {
        IDataResult GetListArticleLaw(BaseParams baseParams, bool isPaging, out int totalCount);
        IDataResult AddArticleLaw(BaseParams baseParams);
    }
}
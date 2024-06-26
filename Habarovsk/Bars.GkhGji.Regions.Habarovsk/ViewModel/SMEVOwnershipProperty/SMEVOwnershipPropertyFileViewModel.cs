﻿namespace Bars.GkhGji.Regions.Habarovsk.ViewModel
{
    using Entities;
    using B4;
    using B4.Utils;
    using System.Linq;
    using System;
    using Bars.GkhGji.Regions.Habarovsk.Entities.SMEVOwnershipProperty;

    public class SMEVOwnershipPropertyFileViewModel : BaseViewModel<SMEVOwnershipPropertyFile>
    {
        public override IDataResult List(IDomainService<SMEVOwnershipPropertyFile> domain, BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var id = loadParams.Filter.GetAs("SMEVOwnershipProperty", 0L);
            var showFiles = loadParams.Filter.GetAs("showSysFiles", false);

            var data = domain.GetAll()
            .Where(x => x.SMEVOwnershipProperty.Id == id)
            .WhereIf(!showFiles, x => x.FileInfo.Name.Contains("ID_"))
            .Select(x => new
            {
                x.Id,
                x.FileInfo,
                x.SMEVFileType
            })
            .AsQueryable()
            .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());



        }
    }
}
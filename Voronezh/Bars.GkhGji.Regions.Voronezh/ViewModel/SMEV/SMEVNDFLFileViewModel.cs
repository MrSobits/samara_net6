﻿namespace Bars.GkhGji.Regions.Voronezh.ViewModel
{
    using Entities;
    using B4;
    using B4.Utils;
    using System.Linq;
    using System;

    public class SMEVNDFLFileViewModel : BaseViewModel<SMEVNDFLFile>
    {
        public override IDataResult List(IDomainService<SMEVNDFLFile> domain, BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var id = loadParams.Filter.GetAs("smevNDFL", 0L);

            var data = domain.GetAll()
             .Where(x => x.SMEVNDFL.Id == id)
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
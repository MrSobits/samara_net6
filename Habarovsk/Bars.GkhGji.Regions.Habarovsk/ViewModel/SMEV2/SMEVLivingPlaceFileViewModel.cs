﻿namespace Bars.GkhGji.Regions.Habarovsk.ViewModel
{
    using Entities;
    using B4;
    using B4.Utils;
    using System.Linq;
    using System;

    public class SMEVLivingPlaceFileViewModel : BaseViewModel<SMEVLivingPlaceFile>
    {
        public override IDataResult List(IDomainService<SMEVLivingPlaceFile> domain, BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var id = loadParams.Filter.GetAs("SMEVLivingPlace", 0L);

            var data = domain.GetAll()
             .Where(x => x.SMEVLivingPlace.Id == id)
            .Select(x => new
            {
                x.Id,
                x.FileInfo,
                x.SMEVFileType
            })
            .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}
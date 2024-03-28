namespace Bars.Gkh.Services.Impl
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Services.DataContracts;
    using Bars.Gkh.Services.DataContracts.GetHousesPhotoInfo;
    using System;
    public partial class Service
    {
        /// <summary>
        /// Домен-сервис "Фото-архив жилого дома"
        /// </summary>
        public IDomainService<RealityObjectImage> RealityObjectImageDomainService { get; set; }

        /// <summary>
        /// Изображения жилых домов 
        /// </summary>
        /// <param name="houseIds">Id жилых домов</param>
        /// <returns></returns>
        public GetHousesPhotoInfoResponse GetHousesPhotoInfo(string houseIds)
        {
            var ids = houseIds.Split(',').ToList().ConvertAll(x => long.Parse(x.Trim()));

            if (!ids.Any())
            {
                return new GetHousesPhotoInfoResponse { Result = Result.DataNotFound };
            }

            var data = this.RealityObjectImageDomainService.GetAll()
                .Where(x => ids.Contains(x.RealityObject.Id))

                .Where(x => x.File != null)
                .Select(x =>
                new
                {
                    roId = x.RealityObject.Id,
                    x.File.Id,
                    x.DateImage,
                    x.Name,
                    x.ImagesGroup,
                    Period = x.Period != null ? $"{x.Period.DateStart.Date.ToShortDateString()}-{this.GetDate(x.Period.DateEnd) }" : null,
                    ViewWork = x.WorkCr != null? x.WorkCr.Name : null,
                    x.Description
                })
                .AsEnumerable()
                .GroupBy(x => x.roId)
                .Select(x => new HousePhoto { Id = x.Key, Photos = x.Select(
                    y => new Photo
                    {
                        Id = y.Id,
                        DatePhoto = y.DateImage?.ToShortDateString(),
                        Description = y.Description != "" ? y.Description : null,
                        ImageGroup = y.ImagesGroup.GetDisplayName(),
                        NamePhoto = y.Name,
                        Period = y.Period != null && y.Period != "-"? y.Period : null,
                        ViewWork = y.ViewWork
                    }).ToArray()
                })
                .ToArray();

            var result = data.Length == 0 ? Result.DataNotFound : Result.NoErrors;

            return new GetHousesPhotoInfoResponse { HousePhotos = data, Result = result };
        }

        private string GetDate(DateTime? date)
        {
            return date?.Date.ToShortDateString();
        }
    }
}
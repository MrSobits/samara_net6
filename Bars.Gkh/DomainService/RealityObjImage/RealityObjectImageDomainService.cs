namespace Bars.Gkh.DomainService.RealityObjImage
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;

    using Bars.B4;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Utils;

    public class RealityObjectImageDomainService : GkhFileStorageDomainService<RealityObjectImage>
    {
        /// <inheritdoc />
        public override IDataResult Save(BaseParams baseParams)
        {
            var saveParam = this.GetSaveParam(baseParams);
            var savedIds = new List<long>();

            this.InTransaction(() =>
            {
                foreach (var record in saveParam.Records)
                {
                    var value = record.AsObject();

                    foreach (var fileData in baseParams.Files)
                    {
                        this.ModifyFileData(fileData.Value);
                        this.SetFileInfoValue(value, fileData);
                    }
                    
                    this.SaveInternal(value);
                    savedIds.Add(value.Id);
                }
            });

            return new BaseDataResult(savedIds);
        }

        /// <inheritdoc />
        protected override void ModifyFileData(FileData fileData)
        {
            // уменьшаем image до 5Mb
            const int allowedFileSizeInByte = 5 * 1024 * 1024;
            var imageConverter = new ImageConverter();
            var image = (Image) imageConverter.ConvertFrom(fileData.Data);

            image.RotateWithMeta();

            var bitmap = new Bitmap(image);

            using (var memoryStream = new MemoryStream())
            {
                ImageResizeExtension.SaveTemporary(bitmap, memoryStream, 100, fileData.Extention);

                if (fileData.Data.Length > allowedFileSizeInByte)
                {
                    while (memoryStream.Length > allowedFileSizeInByte)
                    {
                        var scale = Math.Sqrt((double) allowedFileSizeInByte / (double) memoryStream.Length);
                        memoryStream.SetLength(0);
                        bitmap = ImageResizeExtension.ScaleImage(bitmap, scale);
                        ImageResizeExtension.SaveTemporary(bitmap, memoryStream, 100, fileData.Extention);
                    }
                }

                bitmap?.Dispose();

                fileData.Data = memoryStream.ToArray();
            }
        }
    }
}

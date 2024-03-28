namespace Bars.Gkh.Interceptors
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;

    public class RealityObjectImageInterceptor : EmptyDomainInterceptor<RealityObjectImage>
    {
        public override IDataResult BeforeCreateAction(IDomainService<RealityObjectImage> service, RealityObjectImage entity)
        {
            if (entity.ImagesGroup != ImagesGroup.Avatar)
            {
                return base.BeforeCreateAction(service, entity);
            }

            var avatarExist =
                service.GetAll()
                    .Any(x => x.RealityObject.Id == entity.RealityObject.Id && x.ImagesGroup == ImagesGroup.Avatar);

            if (avatarExist)
            {
                return Failure("Для данного дома уже существует фотография самого объекта(аватар)");
            }

            entity.Name = "Изображение жилого дома";
            entity.DateImage = DateTime.Now;
            return this.Success();
        }
    }
}

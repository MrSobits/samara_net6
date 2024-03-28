namespace Bars.Gkh.DomainService.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Text;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;

    using Castle.Windsor;
    
    /// <summary>
    /// Сервис предоставляет функционал сохранения и получения длинных полей.
    /// Требует наличия двух сущностей: родительской и хранящей поля.
    /// В хранящей сущности ожидается наличие ссылки на родительскую сущность
    /// и, как минимум, одного поля типа byte[]
    /// Пример использования:
    /// this.Container.Resolve&lt;IBlobPropertyService&lt;TEntity, TBlobEntity&gt;&gt;.Get(baseParams)
    /// На клиентской стороне за взаимодействие отвечает аспект GkhBlobText (gkhblobtextaspect)
    /// </summary>
    /// <typeparam name="TEntity">Родительская сущность</typeparam>
    /// <typeparam name="TBlobEntity">Сущность, хранящая длинные поля</typeparam>
    public class BlobPropertyService<TEntity, TBlobEntity> : IBlobPropertyService<TEntity, TBlobEntity>
        where TBlobEntity : PersistentObject, new() where TEntity : PersistentObject, new()
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<TBlobEntity> DomainService { get; set; }

        public IDataResult Get(BaseParams baseParams)
        {
            var parentId = baseParams.Params.GetAs("id", 0L, true);
            if (parentId == 0)
            {
                return new BaseDataResult(false, "Не передан идентификатор");
            }

            var field = baseParams.Params.GetAs<string>("field");
            if (string.IsNullOrEmpty(field))
            {
                return new BaseDataResult(false, "Не передано имя поля");
            }

            var entity = this.GetEntity(parentId);
            if (entity == null)
            {
                return new BaseDataResult(null);
            }

            var property = typeof(TBlobEntity).GetProperty(field);
            if (property == null || property.PropertyType != typeof(byte[]))
            {
                return new BaseDataResult(false, "Передано неверное имя поля, либо поле имеет неверный тип");
            }

            var value = Encoding.UTF8.GetString(property.GetValue(entity, null) as byte[] ?? new byte[0]);
            int previewLength;
            if (baseParams.Params.GetAs<bool>("previewOnly") && (previewLength = baseParams.Params.GetAs("previewLength", 0)) > 0)
            {
                var preview = value.Length > previewLength ? value.Substring(0, previewLength) : value;
                return new BaseDataResult(new Dictionary<string, string> { { "Id", parentId.ToString() }, { "preview", preview } });
            }

            return new BaseDataResult(new Dictionary<string, string> { { "Id", parentId.ToString() }, { field, value } });
        }

        public IDataResult Save(BaseParams baseParams)
        {
            var parentId = baseParams.Params.GetAs("id", 0L, true);
            if (parentId == 0)
            {
                return new BaseDataResult(false, "Не передан идентификатор");
            }

            var field = baseParams.Params.GetAs<string>("field");
            if (string.IsNullOrEmpty(field))
            {
                return new BaseDataResult(false, "Не передано имя поля");
            }

            var entity = this.GetEntity(parentId) ?? new TBlobEntity();

            var property = typeof(TBlobEntity).GetProperty(field);
            if (property == null || property.PropertyType != typeof(byte[]))
            {
                return new BaseDataResult(false, "Передано неверное имя поля");
            }

            var value = baseParams.Params.GetAs("value", string.Empty);
            property.SetValue(entity, Encoding.UTF8.GetBytes(value), null);

            if (entity.Id > 0)
            {
                this.DomainService.Update(entity);
            }
            else
            {
                var parentEntity = new TEntity { Id = parentId };
                var parentEntityProperty = this.GetParentEntityProperty();

                parentEntityProperty.SetValue(entity, parentEntity, null);

                this.DomainService.Save(entity);
            }

            var result = new Dictionary<string, string> { { "Id", parentId.ToString() }, { field, value } };
            var previewLength = baseParams.Params.GetAs("previewLength", 0);
            if (previewLength > 0)
            {
                var preview = value.Length > previewLength ? value.Substring(0, previewLength) : value;
                result.Add("preview", preview);

                var autoSavePreview = baseParams.Params.GetAs<bool>("autoSavePreview");
                if (autoSavePreview)
                {
                    this.SavePreview(baseParams, parentId, preview);
                }
            }

            return new BaseDataResult(result);
        }

        protected void SavePreview(BaseParams baseParams, long id, string preview)
        {
            var previewField = baseParams.Params.GetAs<string>("previewField");
            if (string.IsNullOrEmpty(previewField))
            {
                return;
            }

            var previewProperty = typeof(TEntity).GetProperty(previewField);
            if (previewProperty == null)
            {
                return;
            }

            var parentEntityDomain = this.Container.ResolveDomain<TEntity>();
            try
            {
                var parentEntity = parentEntityDomain.Get(id);
                if (parentEntity == null)
                {
                    return;
                }

                previewProperty.SetValue(parentEntity, preview, null);
                parentEntityDomain.Update(parentEntity);
            }
            finally
            {
                this.Container.Release(parentEntityDomain);
            }
        }

        protected PropertyInfo GetParentEntityProperty()
        {
            return typeof(TBlobEntity).GetProperties().FirstOrDefault(x => x.PropertyType == typeof(TEntity));
        }

        protected TBlobEntity GetEntity(long parentId)
        {
            var param = Expression.Parameter(typeof(TBlobEntity));
            var idExpr = Expression.MakeMemberAccess(Expression.MakeMemberAccess(param, this.GetParentEntityProperty()), typeof(PersistentObject).GetProperty("Id"));
            var equalExpr = Expression.Equal(idExpr, Expression.Constant(parentId));
            var selector = Expression.Lambda<Func<TBlobEntity, bool>>(equalExpr, param);

            return this.DomainService.GetAll().FirstOrDefault(selector);
        }
    }
}
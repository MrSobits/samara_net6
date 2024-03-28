namespace Bars.GkhGji.Regions.BaseChelyabinsk.DomainService.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Entities.Dict;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Prescription;

    using Castle.Windsor;

    /// <summary>
    /// Сервис для работы с направлениями деятельности по предписаниям
    /// </summary>
    public class PrescriptionActivityDirectionService : IPrescriptionActivityDirectionService
    {
        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Добавить направления деятельности
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат операции</returns>
        public IDataResult AddDirections(BaseParams baseParams)
        {
            var directDomain = this.Container.Resolve<IDomainService<PrescriptionActivityDirection>>();

            var listToSave = new List<PrescriptionActivityDirection>();

            try
            {
                var documentId = baseParams.Params.GetAs<long>("documentId");
                var directionIds = baseParams.Params.GetAs<string>("directionIds");

                // В этом словаре будут существующие записи
                // key - идентификатор 
                // value - объект
                var dict = directDomain.GetAll()
                        .Where(x => x.Prescription.Id == documentId)
                        .GroupBy(x => x.ActivityDirection.Id)
                        .AsEnumerable()
                        .ToDictionary(x => x.Key, y => y.Select(x => x.Id).FirstOrDefault());

                if (!string.IsNullOrEmpty(directionIds) && documentId > 0)
                {
                    
                    // По переданным id инспекторов если их нет в списке существующих, то добавляем
                    foreach (var id in directionIds.Split(','))
                    {
                        var newId = id.ToLong();

                        if (dict.ContainsKey(newId))
                        {
                            // Если с таким id уже есть в списке то удалем его из списка и просто пролетаем дальше
                            // без добавления в БД
                            dict.Remove(newId);
                            continue;
                        }

                        if (newId > 0)
                        {
                            var newObj = new PrescriptionActivityDirection
                            {
                                Prescription = new Prescription { Id = documentId },
                                ActivityDirection = new ActivityDirection { Id = newId }
                            };

                            listToSave.Add(newObj);
                        }

                    }

                    using (var transaction = this.Container.Resolve<IDataTransaction>())
                    {
                        try
                        {
                            foreach (var item in listToSave)
                            {
                                directDomain.Save(item);
                            }

                            //те записи котоыре остались в данном словаре уже ненужны их можно удалять
                            foreach (var keyValue in dict)
                            {
                                directDomain.Delete(keyValue.Value);
                            }

                            transaction.Commit();
                            return new BaseDataResult();
                        }
                        catch (ValidationException e)
                        {
                            transaction.Rollback();
                            return new BaseDataResult { Success = false, Message = e.Message };
                        }
                        catch
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }

                return new BaseDataResult();
            }
            catch (Exception exc)
            {
                throw exc;
            }
            finally
            {
                this.Container.Release(directDomain);
            }
        }

        /// <summary>
        /// Вернуть список текущих направлений деятельности по предписанию
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Список направлений</returns>
        public IDataResult ListDirections(BaseParams baseParams)
        {
            var directDomain = this.Container.Resolve<IDomainService<PrescriptionActivityDirection>>();

            try
            {
                var documentId = baseParams.Params.GetAs<long>("documentId");

                var data = directDomain.GetAll()
                    .Where(x => x.Prescription.Id == documentId)
                    .Select(x => x.ActivityDirection);

                int totalCount = data.Count();
                var result = data.ToArray();

                return new ListDataResult(result, totalCount);
            }
            catch (Exception e)
            {
                return new BaseDataResult(false, e.Message);
            }
            finally
            {
                this.Container.Release(directDomain);
            }
        }
    }
}
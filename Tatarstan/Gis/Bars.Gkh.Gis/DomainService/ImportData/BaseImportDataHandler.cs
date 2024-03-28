﻿namespace Bars.Gkh.Gis.DomainService.ImportData
 {
     using System.Collections.Generic;
     using Castle.Windsor;
     using DataResult;
     using Entities.Register.LoadedFileRegister;

     /// <summary>
     /// Обработчик импорта данных
     /// </summary>
     public abstract class BaseImportDataHandler
     {

         /// <summary>
         /// Контейнер
         /// </summary>
         public IWindsorContainer Container { get; set; }

         /// <summary>
         /// Множественный импорт данных
         /// </summary>
         /// <param name="loadedFile">Загруженные файлы с данными</param>
         /// <returns>Результат импорта</returns>
         public abstract IEnumerable<ImportDataResult> ImportData(IEnumerable<LoadedFileRegister> loadedFile);

         /// <summary>
         /// Импорт данных
         /// </summary>
         /// <param name="loadedFile">Загруженный файлы с данными</param>
         /// <returns>Результат импорта</returns>
         public abstract ImportDataResult ImportData(LoadedFileRegister loadedFile);
         }
 }
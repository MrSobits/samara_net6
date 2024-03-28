﻿namespace Bars.Gkh.Gis.DataResult
 {
     using B4;
     using System.Collections.Generic;

     /// <summary>
     /// Класс для итогов импорта
     /// </summary>
     public class ImportDataResult : IDataResult
     {
         /// <summary>
         /// Идентификатор загруженного файла
         /// </summary>
         public long? LoadedFileRegisterId { get; set; }

         /// <summary>
         /// Идентификаторы загруженного файла
         /// </summary>
         public List<long> LoadedFileRegisterIdList { get; set; }


         public bool Success { get; set; }

         /// <summary>
         /// Сообщение
         /// </summary>
         public string Message { get; set; }


         public object Data { get; set; }


         /// <summary>
         /// Ошибки импорта
         /// </summary>
         public string[] Errors { get; set; }

         /// <summary>
         /// Конструктор
         /// </summary>
         public ImportDataResult(long? loadedFileRegisterId)
         {
             LoadedFileRegisterId = loadedFileRegisterId;
             Success = true;
         }

         /// <summary>
         /// Конструктор
         /// </summary>
         public ImportDataResult(bool success, string message, long? loadedFileRegisterId)
         {
             LoadedFileRegisterId = loadedFileRegisterId;
             Success = success;
             Message = message;
         }

         /// <summary>
         /// Конструктор
         /// </summary>
         public ImportDataResult(bool success, string message, List<long> loadedFileRegisterId = null)
         {
             LoadedFileRegisterIdList = loadedFileRegisterId;
             Success = success;
             Message = message;
         }

         /// <summary>
         /// 
         /// </summary>
         public ImportDataResult(string[] errors, long? loadedFileRegisterId)
         {
             LoadedFileRegisterId = loadedFileRegisterId;
             Success = false;
             Errors = errors;
         }
     }
 }
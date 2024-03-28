namespace Bars.Gkh.RegOperator.DomainService.IndividualAccountOwner
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.RegOperator.Entities;

    public class IndividualAccountOwnerServiсe : BaseDomainService<IndividualAccountOwner>
    {
        /// <inheritdoc />
        public override IDataResult Save(BaseParams baseParams)
         {
             var statementService = this.Container.Resolve<IDomainService<IndividualAccountOwner>>();
             var fileManager = this.Container.Resolve<IFileManager>();

             var saveParam = baseParams
                 .Params
                 .Read<SaveParam<IndividualAccountOwner>>()
                 .Execute(container => B4.DomainService.BaseParams.Converter.ToSaveParam<IndividualAccountOwner>(container, true));
             try
             {
                 foreach (var record in saveParam.Records)
                 {
                     var value = record.AsObject();

                     var file = baseParams.Files.FirstOrDefault();

                     if (file.Value != null)
                     {
                        //value.FactAddrDoc = fileManager.SaveFile(file.Value);
                     }

                     if (value.Id == 0)
                     {
                         statementService.Save(value);
                     }
                     else
                     {
                         statementService.Update(value);
                     }
                 }
             }
             catch (Exception exc)
             {
                 return BaseDataResult.Error(exc.Message);
             }
             finally
             {
                 this.Container.Release(statementService);
                 this.Container.Release(fileManager);
             }

             return new BaseDataResult();
             
             
             // var values = new List<IndividualAccountOwner>();
            // foreach (SaveRecord<IndividualAccountOwner> record in this.GetSaveParam(baseParams, false).Records)
            // {
            //     IndividualAccountOwner obj = record.AsObject();
            //     //   this.SaveInternal(obj);
            //    // values.Add(obj);
            // }
            //
            // this.InTransaction((Action) (() =>
            //  {
            //      foreach (SaveRecord<IndividualAccountOwner> record in this.GetSaveParam(baseParams, false).Records)
            //      {
            //          IndividualAccountOwner obj = record.AsObject();
            //          this.SaveInternal(obj);
            //          values.Add(obj);
            //      }
            //  }));
            //  return (IDataResult) new SaveDataResult((object) values);
             
         }

        /// <inheritdoc />
        public override IDataResult Update(BaseParams baseParams)
        {
            List<IndividualAccountOwner> values = new List<IndividualAccountOwner>();
            this.InTransaction((Action) (() =>
            {
                foreach (SaveRecord<IndividualAccountOwner> record in this.GetSaveParam(baseParams, false).Records)
                {
                    var file = baseParams.Files["FactAddrDoc"];
                    var fileManager = this.Container.Resolve<IFileManager>();

                    FileInfo fileInfo = fileManager.SaveFile(file);
                    IndividualAccountOwner obj = record.AsObject();
                    //obj.FactAddrDoc = fileInfo;
                    this.UpdateInternal(obj);
                    values.Add(obj);
                }
            }));
            return (IDataResult) new BaseDataResult((object) values);
        }
    }
}
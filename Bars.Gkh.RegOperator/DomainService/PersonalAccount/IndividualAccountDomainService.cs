namespace Bars.Gkh.RegOperator.DomainService.PersonalAccount
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Bars.B4;
    using Bars.B4.DomainService.BaseParams;
    using Bars.B4.Modules.FIAS;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.FileStorage.DomainService;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Enums;

    public class IndividualAccountDomainService : FileStorageDomainService<IndividualAccountOwner>
    {
        /// <inheritdoc />
        public override IDataResult Save(BaseParams baseParams)
        {
            //var records = Converter.ToLoadParam(baseParams.Params);
            
            var indDomain = this.Container.Resolve<IDomainService<IndividualAccountOwner>>();
            var fiasDomain = this.Container.Resolve<IDomainService<FiasAddress>>();
            var realObjDomain = this.Container.Resolve<IDomainService<RealityObject>>();
            var roomDomain = this.Container.Resolve<IDomainService<Room>>();
            var fileManager = this.Container.Resolve<IFileManager>();

            var importFile =  baseParams.Files.FirstOrDefault().Value != null 
                ? fileManager.SaveFile(baseParams.Files.FirstOrDefault().Value) 
                : null;

            var owner = new IndividualAccountOwner();

            owner.FirstName = baseParams.Params.GetAs<string>("FirstName");
            owner.Surname = baseParams.Params.GetAs<string>("Surname");
            owner.SecondName = baseParams.Params.GetAs<string>("SecondName");
            
            owner.BirthPlace = baseParams.Params.GetAs<string>("BirthPlace");
            owner.IdentitySerial = baseParams.Params.GetAs<string>("IdentitySerial");
            owner.IdentityNumber = baseParams.Params.GetAs<string>("IdentityNumber");
            owner.AddressOutsideSubject = baseParams.Params.GetAs<string>("AddressOutsideSubject");
            owner.Email = baseParams.Params.GetAs<string>("Email");
            owner.DocumentIssuededOrg = baseParams.Params.GetAs<string>("DocumentIssuededOrg");


            if ( DateTime.TryParse(baseParams.Params.GetAs<string>("BirthDate"), out DateTime birDate))
            {
                owner.BirthDate = birDate;
            }
            
            if (DateTime.TryParse(baseParams.Params.GetAs<string>("DateDocumentIssuance"), out DateTime docIssue))
            {
                owner.DateDocumentIssuance = docIssue;
            }
            
            if (Enum.TryParse(baseParams.Params.GetAs<string>("IdentityType"),out IdentityType identityType))
            {
                owner.IdentityType = identityType;
            }
            
            if (Enum.TryParse(baseParams.Params.GetAs<string>("IdentityType"),out Gender gender))
            {
                owner.Gender = gender;
            }
            
            if (long.TryParse(baseParams.Params.GetAs<string>("RealityObject"),out long realObjId))
            {
                owner.RealityObject = realObjDomain.Get(realObjId);
            }
            
            //if (long.TryParse(baseParams.Params.GetAs<string>("FiasFactAddress"),out long fiasId))
            //{
                //owner.FiasFactAddress = fiasDomain.(baseParams.Params.GetAs<string>("FiasFactAddress"));
            //}
            
            if (long.TryParse(baseParams.Params.GetAs<string>("RegistrationAddress"),out long regAdr))
            {
                owner.RegistrationAddress = realObjDomain.Get(regAdr);
            }
            
            if (long.TryParse(baseParams.Params.GetAs<string>("RegistrationRoom"),out long regRoom))
            {
                owner.RegistrationRoom = roomDomain.Get(regRoom);
            }
            
            if (importFile != null) owner.FactAddrDoc = importFile;
            
            indDomain.Save(owner);
            
            return new SaveDataResult((object) owner);
        }

        /// <inheritdoc />
        public override IDataResult Update(BaseParams baseParams)
        {
            List<IndividualAccountOwner> values = new List<IndividualAccountOwner>();
            IndividualAccountOwner value = default(IndividualAccountOwner);
            InTransaction(delegate
            {
                SaveParam<IndividualAccountOwner> saveParam = GetSaveParam(baseParams);
                foreach (SaveRecord<IndividualAccountOwner> current in saveParam.Records)
                {
                    value = current.AsObject(FileProperties.Select((PropertyInfo x) => x.Name).ToArray());
                    Dictionary<string, FileInfo> dictionary = baseParams.Files.ToDictionary((KeyValuePair<string, FileData> fileData) => fileData.Key, (KeyValuePair<string, FileData> fileData) => SetFileInfoValue(value, fileData));
                    PropertyInfo[] fileProperties = FileProperties;
                    foreach (PropertyInfo propertyInfo in fileProperties)
                    {
                        if (current.Properties[propertyInfo.Name] == null && !dictionary.ContainsKey(propertyInfo.Name))
                        {
                            dictionary.Add(propertyInfo.Name, SetFileInfoValue(value, new KeyValuePair<string, FileData>(propertyInfo.Name, null)));
                        }
                    }
                    UpdateInternal(value);
                    foreach (FileInfo current2 in dictionary.Values.Where((FileInfo file) => file != null))
                    {
                        FileInfoService.Delete(current2.Id);
                    }
                    values.Add(value);
                }
            });
            return new BaseDataResult(values);
        }
    }
    
}
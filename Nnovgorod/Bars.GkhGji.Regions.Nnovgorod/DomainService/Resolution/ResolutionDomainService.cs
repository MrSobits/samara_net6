﻿namespace Bars.GkhGji.Regions.Nnovgorod.DomainService
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Nnovgorod.Entities;

    public class ResolutionDomainService : BaseDomainService<Resolution>
    {
        public IDomainService<DocumentGJIPhysPersonInfo> DocumentGjiPhysPersonInfoDomain { get; set; }

        public override IDataResult Update(BaseParams baseParams)
        {
            var values = new List<Resolution>();
            InTransaction(() =>
            {
                var saveParam = GetSaveParam(baseParams);
                foreach (var record in saveParam.Records)
                {
                    UpdatePhysPersonRequisites(record);
                    var value = record.AsObject();
                    UpdateInternal(value);
                    values.Add(value);
                }
            });

            return new BaseDataResult(values);
        }

        private void UpdatePhysPersonRequisites(SaveRecord<Resolution> protocol)
        {
            var info = this.DocumentGjiPhysPersonInfoDomain.GetAll()
                .FirstOrDefault(x => x.Document.Id == protocol.Entity.Id);

            var properties = protocol.NonObjectProperties;

            if (info == null)
            {
                info = new DocumentGJIPhysPersonInfo { Document = new DocumentGji { Id = protocol.Entity.Id } };

                var needToSave = false;

                if (properties.ContainsKey("PhysPersonAddress"))
                {
                    info.PhysPersonAddress = properties["PhysPersonAddress"].ToString();
                    needToSave = true;
                }

                if (properties.ContainsKey("PhysPersonBirthdayAndPlace"))
                {
                    info.PhysPersonBirthdayAndPlace = properties["PhysPersonBirthdayAndPlace"].ToString();
                    needToSave = true;
                }

                if (properties.ContainsKey("PhysPersonDocument"))
                {
                    info.PhysPersonDocument = properties["PhysPersonDocument"].ToString();
                    needToSave = true;
                }

                if (properties.ContainsKey("PhysPersonJob"))
                {
                    info.PhysPersonJob = properties["PhysPersonJob"].ToString();
                    needToSave = true;
                }

                if (properties.ContainsKey("PhysPersonPosition"))
                {
                    info.PhysPersonPosition = properties["PhysPersonPosition"].ToString();
                    needToSave = true;
                }

                if (properties.ContainsKey("PhysPersonMaritalStatus"))
                {
                    info.PhysPersonMaritalStatus = properties["PhysPersonMaritalStatus"].ToString();
                    needToSave = true;
                }

                if (properties.ContainsKey("PhysPersonSalary"))
                {
                    info.PhysPersonSalary = properties["PhysPersonSalary"].ToString();
                    needToSave = true;
                }

                if (needToSave)
                {
                    this.DocumentGjiPhysPersonInfoDomain.Save(info);
                }
            }
            else
            {
                info.PhysPersonAddress = properties.GetAs("PhysPersonAddress", (string)null) ?? info.PhysPersonAddress;
                info.PhysPersonBirthdayAndPlace = properties.GetAs("PhysPersonBirthdayAndPlace", (string)null) ?? info.PhysPersonBirthdayAndPlace;
                info.PhysPersonDocument = properties.GetAs("PhysPersonDocument", (string)null) ?? info.PhysPersonDocument;
                info.PhysPersonJob = properties.GetAs("PhysPersonJob", (string)null) ?? info.PhysPersonJob;
                info.PhysPersonMaritalStatus = properties.GetAs("PhysPersonMaritalStatus", (string)null) ?? info.PhysPersonMaritalStatus;
                info.PhysPersonPosition = properties.GetAs("PhysPersonPosition", (string)null) ?? info.PhysPersonPosition;
                info.PhysPersonSalary = properties.GetAs("PhysPersonSalary", (string)null) ?? info.PhysPersonSalary;

                this.DocumentGjiPhysPersonInfoDomain.Save(info);
            }
        }
    }
}
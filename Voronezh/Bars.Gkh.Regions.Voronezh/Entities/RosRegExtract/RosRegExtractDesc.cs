using Bars.B4.DataAccess;
using Bars.Gkh.Entities;
using Bars.Gkh.Enums;
using System;

namespace Bars.Gkh.Regions.Voronezh.Entities
{
    public class RosRegExtractDesc : PersistentObject
    {
        //Description
        public virtual string Desc_ID_Object { get; set; }
        public virtual string Desc_CadastralNumber { get; set; }
        public virtual string Desc_ObjectType { get; set; }
        public virtual string Desc_ObjectTypeText { get; set; }
        public virtual string Desc_ObjectTypeName { get; set; }
        public virtual string Desc_AssignationCode { get; set; }
        public virtual string Desc_AssignationCodeText { get; set; }
        public virtual string Desc_Area { get; set; }
        public virtual string Desc_AreaText { get; set; }
        public virtual string Desc_AreaUnit { get; set; }
        public virtual string Desc_Floor { get; set; }
        public virtual string Desc_ID_Address { get; set; }
        public virtual string Desc_AddressContent { get; set; }
        public virtual string Desc_RegionCode { get; set; }
        public virtual string Desc_RegionName { get; set; }
        public virtual string Desc_OKATO { get; set; }
        public virtual string Desc_KLADR { get; set; }
        public virtual string Desc_CityName { get; set; }
        public virtual string Desc_Urban_District { get; set; }
        public virtual string Desc_Locality { get; set; }
        public virtual string Desc_StreetName { get; set; }
        public virtual string Desc_Level1Name { get; set; }
        public virtual string Desc_Level2Name { get; set; }
        public virtual string Desc_ApartmentName { get; set; }

        /// <summary>
        /// Помещение
        /// </summary>
        public virtual Int64 Room_id { get; set; }

        /// <summary>
        /// Сопоставлен
        /// </summary>
        public virtual YesNoNotSet YesNoNotSet { get; set; }


        //Header fields
        public virtual string ExtractDate { get; set; }
        public virtual string ExtractNumber { get; set; }
        public virtual string HeadContent { get; set; }
        public virtual string Registrator { get; set; }
        public virtual string Appointment { get; set; }
        public virtual string NoShareHolding { get; set; }
        public virtual string RightAgainst { get; set; }
        public virtual string RightAssert { get; set; }
        public virtual string RightClaim { get; set; }
        public virtual string RightSteal { get; set; }
        //public virtual string XML { get; set; }
    }
}

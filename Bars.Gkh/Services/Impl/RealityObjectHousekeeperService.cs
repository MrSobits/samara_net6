namespace Bars.Gkh.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Castle.Windsor;
    using Bars.B4.Utils;
    using Bars.Gkh.Services.DataContracts.RealityObjectHousekeeper;
    using System.Security.Cryptography;
    using System.Text;
    using Bars.B4;
    using Bars.Gkh.Entities;
    using Bars.B4.Modules.Security;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Utils;  
    using Bars.B4.Application;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Services.ServiceContracts;

    /// <summary>
    /// Сервис сведений об обращениях граждан
    /// </summary>
    public partial class Service
    {
        public IDomainService<RealityObjectHousekeeper> HousekeeperDomain { get; set; }
      
        /// <summary>
        /// Импорт сведений об обращении граждан
        /// </summary>
        /// <returns></returns>
        public GetRealityObjectHousekeeperResponse GetHousekeepersList(string token)
        {
            if (!ValidateToken(token))
            {
                return new GetRealityObjectHousekeeperResponse
                {
                    RequestResult = RequestResult.IncorrectToken
                };
            }

            var housekeepers = new List<RealityObjectHousekeeperProxy>();

            var operatorsList = HousekeeperDomain.GetAll()
                .Select(x => new
                {
                    x.Id,
                    x.Login,
                    x.Password,
                    x.FIO,
                    x.IsActive,
                    RealityId = x.RealityObject.Id
                }).ToList();

            operatorsList.ForEach(obj =>
            {
                bool isActive = obj.IsActive == YesNoNotSet.Yes? true:false;
                housekeepers.Add(
                    new RealityObjectHousekeeperProxy
                    {
                        Id = obj.Id,
                        Login = obj.Login,
                        Password = obj.Password,
                        FIO = obj.FIO,
                        IsActive = isActive,
                        RealityId = obj.RealityId
                    });
            });


            GetRealityObjectHousekeeperResponse result = new GetRealityObjectHousekeeperResponse
            {
                RequestResult = RequestResult.NoErrors,
                Housekeepers = housekeepers.ToArray()
            };


            return result;
        }
        private string GetAccessToken()
        {
            var token = $"{DateTime.Now.ToString("dd.MM.yyyy")}_ANV_6966644";
            var hash = new MD5CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(token));
            string str = Convert.ToBase64String(hash);
            return Convert.ToBase64String(hash);
        }

        private bool ValidateToken(string check_token)
        {
            return this.GetAccessToken() == check_token;
        }
    }
}
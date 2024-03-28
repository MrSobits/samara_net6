using Bars.B4;
using Bars.B4.Utils;
using Bars.Gkh.Entities;
using System.Linq;

namespace Bars.Gkh.Interceptors
{
    public class RealityObjectHousekeeperInterceptor : EmptyDomainInterceptor<RealityObjectHousekeeper>
    {
        public override IDataResult BeforeCreateAction(IDomainService<RealityObjectHousekeeper> service, RealityObjectHousekeeper entity)
        {
            if (!string.IsNullOrEmpty(entity.NewPassword) && !string.IsNullOrEmpty(entity.Login))
            {
                var exists = CheckUnicalCreate(entity.Login, service);
                if (!exists)
                {
                    return Failure($"Старший по дому с логином {entity.Login} уже существует") ;
                }
                var result = CheckLogin(entity);
                if (result)
                {
                    entity.Password = MD5.GetHashString64(entity.NewPassword, HashType.MD5B2);
                    return Success();
                }
                else
                {
                    return Failure("Введенные логин и пароль не совпадают");
                }                
                
            }
            else
            {
                return Failure("Не указан логин и/или пароль");
            }
        }

        public override IDataResult BeforeUpdateAction(IDomainService<RealityObjectHousekeeper> service, RealityObjectHousekeeper entity)
        {
            if (!string.IsNullOrEmpty(entity.NewPassword))
            {
                var exists = CheckUnicalUpdate(entity.Login, service);
                if (!exists)
                {
                    return Failure($"Старший по дому с логином {entity.Login} уже существует");
                }
                var result = CheckLogin(entity);
                if (result)
                {
                    entity.Password = MD5.GetHashString64(entity.NewPassword, HashType.MD5B2);
                    return Success();
                }
                else
                {
                    return Failure("Введенные логин и пароль не совпадают");
                }

            }
            else if (!string.IsNullOrEmpty(entity.Login))
            {
                var exists = CheckUnicalUpdate(entity.Login, service);
                if (!exists)
                {
                    return Failure($"Старший по дому с логином {entity.Login} уже существует");
                }
                return Success();
            }
            else
            {
                return Failure("Не указан логин");
            }
        }

        private bool CheckUnicalUpdate(string login, IDomainService<RealityObjectHousekeeper> service)
        {
            if (!string.IsNullOrEmpty(login))
            {
                var exists = service.GetAll().Where(x => x.Login == login).ToList();
                if (exists != null && exists.Count > 1)
                {
                    return false;
                }
            }
            return true;
        }
        private bool CheckUnicalCreate(string login, IDomainService<RealityObjectHousekeeper> service)
        {
            if (!string.IsNullOrEmpty(login))
            {
                var exists = service.GetAll().Where(x => x.Login == login).ToList();
                if (exists != null && exists.Count > 0)
                {
                    return false;
                }
            }
            return true;
        }

        private bool CheckLogin(RealityObjectHousekeeper entity)
        {
            if (!string.IsNullOrEmpty(entity.NewPassword))
            {
                if (entity.NewPassword != entity.NewConfirmPassword)
                {
                    return false;
                }
            }

            return true;
        }
    }
}

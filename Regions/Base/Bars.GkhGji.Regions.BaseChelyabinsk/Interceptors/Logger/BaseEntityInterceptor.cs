namespace Bars.GkhGji.Regions.BaseChelyabinsk.Interceptors
{
    using Entities;
    using System;
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;
    using Enums.SMEV;
    using Bars.GkhGji.Entities;
    using System.Linq;
    using Bars.B4.DataAccess;

    class BaseEntityInterceptor : EmptyDomainInterceptor<BaseEntity>
    {
       public IGkhUserManager UserManager { get; set; }      

       public override IDataResult BeforeCreateAction(IDomainService<BaseEntity> service, BaseEntity entity)
       {
            try 
            {
                Operator thisOperator = UserManager.GetActiveOperator();
                if (thisOperator != null && thisOperator.Inspector != null)
                {
                 
                }
               

              
            }
            catch (Exception e)
            {
               
            }
            return Success();
       }

       public override IDataResult BeforeUpdateAction(IDomainService<BaseEntity> service, BaseEntity entity)
       {
            try
            {
                Operator thisOperator = UserManager.GetActiveOperator();
                if (thisOperator != null && thisOperator.Inspector != null)
                {

                }



            }
            catch (Exception e)
            {

            }
            return Success();
       }
    }
}

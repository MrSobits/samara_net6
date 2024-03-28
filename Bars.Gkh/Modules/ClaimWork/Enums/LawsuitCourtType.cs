using Bars.B4.Utils;

namespace Bars.Gkh.Modules.ClaimWork.Enums
{
    using System;

    /// <summary>
    /// Вид суда
    /// </summary>
    [Obsolete("Необходимо удалить")]
#warning Удалить
    public enum LawsuitCourtType
    {
        [Display("Не задано")]
        NotSet = 0,

        [Display("Арбитражный суд")]
        ArbitrationCourt = 10,

        [Display("Мировой суд")]
        WorldCourt = 20,

        [Display("Районный суд")]
        RaionCourt = 30, 
        
        [Display("Аппеляционный суд")]
        AppealCourt = 40,

        [Display("Касационный суд")]
        CasationCourt = 50


    }
}

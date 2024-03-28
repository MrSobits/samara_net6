// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApplicationInfo.cs" company="">
//   
// </copyright>
// <summary>
//   Информация о приложении
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bars.Gkh
{
    using System.Collections.Generic;

    using Bars.B4;

    /// <summary>
    /// Информация о приложении
    /// </summary>
    public class ApplicationInfo : IApplicationInfo
    {
        public string AppId
        {
            get
            {
                return "Bars.Gkh";
            }
        }
        
        public string Title
        {
            get
            {
                return "ЖКХ";
            }
        }
    }
}
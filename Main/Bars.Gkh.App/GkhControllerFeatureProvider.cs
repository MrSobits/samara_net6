namespace Bars.Gkh.App;

using System.Collections.Generic;
using System.Linq;

using Bars.B4.Application;
using Bars.B4.Controller.Provider;

using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;

public class GkhControllerFeatureProvider : IApplicationFeatureProvider<ControllerFeature>
{
    /// <inheritdoc />
    public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature)
    {
        var controllerProvider = ApplicationContext.Current.Container.Resolve<ControllerProvider>();
        
        var controllersToDelete = feature
            .Controllers
            .Where(controllerProvider.ReplacedTypes.Contains)
            .ToList();

        controllersToDelete.AddRange(feature
            .Controllers
            .Where(x => !controllerProvider.RegisteredTypes.Contains(x))
            .ToList());
        
        foreach (var controller in controllersToDelete)
        {
            feature.Controllers.Remove(controller);
        }
    }
}
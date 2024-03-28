Ext.define('B4.aspects.permission.nonresidentialplacement.State', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.nonresidentialplacementstateperm',

    permissions: [
        // Поле в шапке
        { name: 'GkhDi.DisinfoRealObj.NonResidentialPlacement.NonResidentialPlacement', applyTo: '#cbNonResidentialPlacement', selector: '#nonResidentialPlacementGridPanel' },

        // Нежилые помещения
        { name: 'GkhDi.DisinfoRealObj.NonResidentialPlacement.Add', applyTo: 'b4addbutton', selector: '#nonResidentialPlacementEditPanel' },
        { name: 'GkhDi.DisinfoRealObj.NonResidentialPlacement.Edit', applyTo: 'b4savebutton', selector: '#nonResidentialPlacementEditWindow' },
        {
            name: 'GkhDi.DisinfoRealObj.NonResidentialPlacement.Delete', applyTo: 'b4deletecolumn', selector: '#nonResidentialPlacementGrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        
        // Приборы учета
        { name: 'GkhDi.DisinfoRealObj.NonResidentialPlacement.Devices.Add', applyTo: 'b4addbutton', selector: '#nonResidentialPlacementMeteringDeviceGrid' },
        {
            name: 'GkhDi.DisinfoRealObj.NonResidentialPlacement.Devices.Delete', applyTo: 'b4deletecolumn', selector: '#nonResidentialPlacementMeteringDeviceGrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        { name: 'GkhDi.DisinfoRealObj.NonResidentialPlacement.Devices.Edit', applyTo: 'b4savebutton', selector: '#nonResidentialPlacementMeteringDeviceEditWindow' }
    ]
});
Ext.define('B4.aspects.permission.otherservice.State', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.otherservicestateperm',

    permissions: [
        // удаление, изменение
        {
            name: 'GkhDi.OtherService.Delete', applyTo: 'b4deletecolumn', selector: 'otherservicegrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        {
            name: 'GkhDi.OtherService.Edit', applyTo: 'b4editcolumn', selector: 'otherservicegrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
    ]
});
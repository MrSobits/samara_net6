Ext.define('B4.aspects.permission.dict.TemplateOtherService', {
    extend: 'B4.aspects.permission.GkhPermissionAspect',
    alias: 'widget.templateotherserviceperm',

    permissions: [
        { name: 'GkhDi.Dict.TemplateOtherService.Create', applyTo: 'b4addbutton', selector: 'templateotherservicegrid' },
        { name: 'GkhDi.Dict.TemplateOtherService.Edit', applyTo: 'b4savebutton', selector: 'templateotherserviceeditwindow' },
        {
            name: 'GkhDi.Dict.TemplateOtherService.Delete', applyTo: 'b4deletecolumn', selector: 'templateotherservicegrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        }
    ]
});
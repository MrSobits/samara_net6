Ext.define('B4.aspects.permission.dict.TemplateService', {
    extend: 'B4.aspects.permission.GkhPermissionAspect',
    alias: 'widget.templateserviceperm',

    permissions: [
        { name: 'GkhDi.Dict.TemplateService.Create', applyTo: 'b4addbutton', selector: '#templateServiceGrid' },
        { name: 'GkhDi.Dict.TemplateService.Edit', applyTo: 'b4savebutton', selector: '#templateServiceEditWindow' },
        { name: 'GkhDi.Dict.TemplateService.Delete', applyTo: 'b4deletecolumn', selector: '#templateServiceGrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        }
    ]
});
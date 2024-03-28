Ext.define('B4.aspects.permission.controldate.MunicipalityLimitDate', {
    extend: 'B4.aspects.permission.GkhPermissionAspect',
    alias: 'widget.municipalitylimitdateperm',

    init: function () {
        var me = this;

        me.permissions = [
            { name: 'GkhCr.ControlDate.MunicipalityLimitDate.Create', applyTo: 'b4addbutton', selector: 'controldatemunicipalitylimitdategrid' },
            {
                name: 'GkhCr.ControlDate.MunicipalityLimitDate.Edit', applyTo: 'b4editcolumn', selector: 'controldatemunicipalitylimitdategrid',
                applyBy: function (component, allowed) {
                    if (allowed) component.show();
                    else component.hide();
                }
            },
            {
                name: 'GkhCr.ControlDate.MunicipalityLimitDate.Delete', applyTo: 'b4deletecolumn', selector: 'controldatemunicipalitylimitdategrid',
                applyBy: function (component, allowed) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        ];

        this.callParent(arguments);
    }
});
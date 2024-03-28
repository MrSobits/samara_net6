Ext.define('B4.aspects.permission.adminresp.State', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.adminrespstateperm',

    permissions: [
        { name: 'GkhDi.Disinfo.AdminResp.AddFromGji', applyTo: '#loadGjiResolutionButton', selector: '#adminRespEditPanel' },
        { name: 'GkhDi.Disinfo.AdminResp.AdminRespField', applyTo: '#cbAdminResponsibility', selector: '#adminRespGridPanel' },
        { name: 'GkhDi.Disinfo.AdminResp.Add', applyTo: '#addAdminRespButton', selector: '#adminRespEditPanel' },
        { name: 'GkhDi.Disinfo.AdminResp.Edit', applyTo: 'b4savebutton', selector: '#adminRespEditWindow' },
        {
            name: 'GkhDi.Disinfo.AdminResp.Delete', applyTo: 'b4deletecolumn', selector: '#adminRespGrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        }
    ]
});
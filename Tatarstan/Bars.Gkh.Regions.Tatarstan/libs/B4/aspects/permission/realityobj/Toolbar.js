Ext.define('B4.aspects.permission.realityobj.Toolbar', {
    extend: 'B4.aspects.permission.GkhPermissionAspect',
    alias: 'widget.realityobjtoolbarpermissionaspect',

    permissions: [
        {
            name: 'Gkh.RealityObject.Field.View.SendTechPassport_View', applyTo: 'button[itemId=btnSendTp]', selector: 'realityobjtoolbar',
            applyBy: function (component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        }
    ]
});
Ext.define('B4.aspects.permission.dict.MeteringDevice', {
    extend: 'B4.aspects.permission.GkhPermissionAspect',
    alias: 'widget.meteringdevicedictperm',

    permissions: [
        { name: 'Gkh.Dictionaries.MeteringDevice.Create', applyTo: 'b4addbutton', selector: '#MeteringDeviceGrid' },
        { name: 'Gkh.Dictionaries.MeteringDevice.Edit', applyTo: 'b4savebutton', selector: '#meteringDeviceEditWindow' },
        { name: 'Gkh.Dictionaries.MeteringDevice.Delete', applyTo: 'b4deletecolumn', selector: '#MeteringDeviceGrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        }
    ]
});
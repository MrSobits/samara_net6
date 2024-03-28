Ext.define('B4.aspects.permission.realityobj.MeteringDevice', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.realityobjmeteringdeviceperm',

    /*
    * name - имя пермишена в дереве, 
    * applyTo - селектор контрола, к которому применяется пермишен, 
    * selector - селектор формы, на которой находится контрол
    */
    permissions: [
        {
            name: 'Gkh.RealityObject.Register.MeteringDevice.Create',
            applyTo: 'b4addbutton',
            selector: 'realityobjMeteringDeviceGrid'
        },
        {
            name: 'Gkh.RealityObject.Register.MeteringDevice.Edit',
            applyTo: 'b4savebutton',
            selector: 'realityobjmetdeviceeditwindow'
        },
        {
            name: 'Gkh.RealityObject.Register.MeteringDevice.Delete',
            applyTo: 'b4deletecolumn', selector: 'realityobjMeteringDeviceGrid',
            applyBy: function (component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        },
        {
            name: 'Gkh.RealityObject.Register.MeteringDevice.View',
            applyTo: 'b4savebutton',
            selector: 'realityobjmetdeviceeditwindow'
        },
        {
            name: 'Gkh.RealityObject.Register.MeteringDevice.SerialNumber_Edit',
            applyTo: 'textarea[name=SerialNumber]',
            selector: 'realityobjmetdeviceeditwindow'
        },
        {
            name: 'Gkh.RealityObject.Register.MeteringDevice.AddingReadingsManually_Edit',
            applyTo: 'b4combobox[name=AddingReadingsManually]',
            selector: 'realityobjmetdeviceeditwindow'
        },
        {
            name: 'Gkh.RealityObject.Register.MeteringDevice.NecessityOfVerificationWhileExpluatation_Edit',
            applyTo: 'b4combobox[name=NecessityOfVerificationWhileExpluatation]',
            selector: 'realityobjmetdeviceeditwindow'
        },
        {
            name: 'Gkh.RealityObject.Register.MeteringDevice.PersonalAccountNum_Edit',
            applyTo: 'textarea[name=PersonalAccountNum]',
            selector: 'realityobjmetdeviceeditwindow'
        },
        {
            name: 'Gkh.RealityObject.Register.MeteringDevice.DateFirstVerification_Edit',
            applyTo: 'datefield[name=DateFirstVerification]',
            selector: 'realityobjmetdeviceeditwindow'
        }
    ]
});
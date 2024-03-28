Ext.define('B4.aspects.permission.realityobj.housingcommunalservice.MeteringDeviceValue', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.meteringdevicevalue',

    permissions: [
    /*
    * name - имя пермишена в дереве, 
    * applyTo - селектор контрола, к которому применяется пермишен, 
    * selector - селектор формы, на которой находится контрол
    */
    //основной грид и панель редактирования жилого дома
        {
            name: 'Gkh.RealityObject.Register.HousingComminalService.MeteringDeviceValue.Edit',
            applyTo: 'b4addbutton',
            selector: 'hsemeteringdevicevaluegrid'
        },
        {
            name: 'Gkh.RealityObject.Register.HousingComminalService.MeteringDeviceValue.Edit',
            applyTo: 'b4savebutton',
            selector: 'hsemeteringdevicevalueeditwindow'
        },
        {
            name: 'Gkh.RealityObject.Register.HousingComminalService.MeteringDeviceValue.Delete',
            applyTo: 'b4deletecolumn',
            selector: 'hsemeteringdevicevaluegrid',
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
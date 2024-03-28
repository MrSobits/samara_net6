Ext.define('B4.aspects.permission.realityobj.housingcommunalservice.Account', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.accountperm',

    permissions: [
    /*
    * name - имя пермишена в дереве, 
    * applyTo - селектор контрола, к которому применяется пермишен, 
    * selector - селектор формы, на которой находится контрол
    */
    //основной грид и панель редактирования жилого дома
        { name: 'Gkh.RealityObject.Register.HousingComminalService.Account.Create', applyTo: 'b4addbutton', selector: 'hseaccountgrid' },
        { name: 'Gkh.RealityObject.Register.HousingComminalService.Account.Edit', applyTo: 'b4savebutton', selector: 'hseaccounteditwindow' },
        { name: 'Gkh.RealityObject.Register.HousingComminalService.Account.Delete', applyTo: 'b4deletecolumn', selector: 'hseaccountgrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        }
    ]
});
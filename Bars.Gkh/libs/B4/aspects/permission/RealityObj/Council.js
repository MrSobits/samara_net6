Ext.define('B4.aspects.permission.realityobj.Council', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.realityobjcouncilperm',

    /*
    * name - имя пермишена в дереве, 
    * applyTo - селектор контрола, к которому применяется пермишен, 
    * selector - селектор формы, на которой находится контрол
    */
    permissions: [
        { name: 'Gkh.RealityObject.Register.Councillors.Create', applyTo: 'b4addbutton', selector: '#councillorsGrid' },

        { name: 'Gkh.RealityObject.Register.Councillors.Edit', applyTo: '#councilSaveButton', selector: '#councillorsGrid' },
        { name: 'Gkh.RealityObject.Register.Councillors.Delete', applyTo: 'b4deletecolumn', selector: '#councillorsGrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        { name: 'Gkh.RealityObject.Register.Councillors.Edit', applyTo: 'b4savebutton', selector: '#councilApartmentHouseEditPanel' }
    ]
});
Ext.define('B4.aspects.permission.ROMCategory', {
    extend: 'B4.aspects.permission.GkhPermissionAspect',
    alias: 'widget.romcategoryperm',

    permissions: [
    /*
    * name - имя пермишена в дереве, 
    * applyTo - селектор контрола, к которому применяется пермишен, 
    * selector - селектор формы, на которой находится контрол
    */
        { name: 'GkhGji.RiskOrientedMethod.ROMCategory.Create', applyTo: 'b4addbutton', selector: '#romcategorygrid' },
        { name: 'GkhGji.RiskOrientedMethod.ROMCategory.Edit', applyTo: 'b4savebutton', selector: '#romcategoryeditwindow' },
        { name: 'GkhGji.RiskOrientedMethod.ROMCategory.Delete', applyTo: 'b4deletecolumn', selector: '#romcategorygrid',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        }
    
    ]
});
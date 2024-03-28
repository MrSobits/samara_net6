Ext.define('B4.aspects.permission.ROMCategoryState', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.romcategorystateperm',

    permissions: [
        /*
        * name - имя пермишена в дереве, 
        * applyTo - селектор контрола, к которому применяется пермишен, 
        * selector - селектор формы, на которой находится контрол
        */

        //поля панели редактирования
        {
               name: 'GkhGji.RiskOrientedMethod.ROMCategory.Field.Vp_Edit', applyTo: '#dfVp', selector: '#romcategoryeditwindow',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.setReadOnly(false);
                    else component.setReadOnly(true);
                }
            }
        },
        {
            name: 'GkhGji.RiskOrientedMethod.ROMCategory.Field.Vp_View', applyTo: '#dfVp', selector: '#romcategoryeditwindow',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },
      
       
    ]
});
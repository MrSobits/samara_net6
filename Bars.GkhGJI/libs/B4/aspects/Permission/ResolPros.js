Ext.define('B4.aspects.permission.ResolPros', {
    extend: 'B4.aspects.permission.GkhPermissionAspect',
    alias: 'widget.resolprosperm',

    permissions: [
    /*
    * name - имя пермишена в дереве, 
    * applyTo - селектор контрола, к которому применяется пермишен, 
    * selector - селектор формы, на которой находится контрол
    */
        { name: 'GkhGji.DocumentsGji.ResolPros.Create', applyTo: 'b4addbutton', selector: '#resolProsGrid' },
        { name: 'GkhGji.DocumentsGji.ResolPros.Edit', applyTo: 'b4savebutton', selector: '#resolProsEditPanel' },
        { name: 'GkhGji.DocumentsGji.ResolPros.Delete', applyTo: 'b4deletecolumn', selector: '#resolProsGrid',
        applyBy: function (component, allowed) {
            if (component) {
                if (allowed) component.show();
                else component.hide();
            }
        }
        }
    
    ]
});
Ext.define('B4.aspects.permission.dict.PlanInsCheck', {
    extend: 'B4.aspects.permission.GkhPermissionAspect',
    alias: 'widget.planinscheckperm',

    permissions: [
    /*
    * name - имя пермишена в дереве, 
    * applyTo - селектор контрола, к которому применяется пермишен, 
    * selector - селектор формы, на которой находится контрол
    */
    //основной грид и панель редактирования жилого дома
        { name: 'GkhGji.Dict.PlanInsCheck.Create', applyTo: 'b4addbutton', selector: '#planInsCheckGrid' },
        { name: 'GkhGji.Dict.PlanInsCheck.Edit', applyTo: 'b4savebutton', selector: '#planInsCheckEditWindow' },
        { name: 'GkhGji.Dict.PlanInsCheck.Delete', applyTo: 'b4deletecolumn', selector: '#planInsCheckGrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        }
    ]
});
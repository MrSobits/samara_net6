Ext.define('B4.aspects.permission.manorg.Documentation', {
    extend: 'B4.aspects.permission.GkhPermissionAspect',
    alias: 'widget.manorgdocperm',

    permissions: [
    /*
    * name - имя пермишена в дереве, 
    * applyTo - селектор контрола, к которому применяется пермишен, 
    * selector - селектор формы, на которой находится контрол
    */
        { name: 'Gkh.Orgs.Managing.Register.Documentation.Create', applyTo: 'b4addbutton', selector: '#manorgDocumentationGrid' },
        { name: 'Gkh.Orgs.Managing.Register.Documentation.Edit', applyTo: 'b4savebutton', selector: '#manorgDocumentationEditWindow' },
        {
            name: 'Gkh.Orgs.Managing.Register.Documentation.Delete', applyTo: 'b4deletecolumn', selector: '#manorgDocumentationGrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        }
    ]
});
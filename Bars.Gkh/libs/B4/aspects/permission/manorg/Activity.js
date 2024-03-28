Ext.define('B4.aspects.permission.manorg.Activity', {
    extend: 'B4.aspects.permission.GkhPermissionAspect',
    alias: 'widget.manorgactivityperm',

    permissions: [
    /*
    * name - имя пермишена в дереве, 
    * applyTo - селектор контрола, к которому применяется пермишен, 
    * selector - селектор формы, на которой находится контрол
    */
    { name: 'Gkh.Orgs.Managing.Register.Activity.Edit', applyTo: 'b4savebutton', selector: '#manorgActivityPanel' }
    ]
});
Ext.define('B4.aspects.permission.manorg.WorkMode', {
    extend: 'B4.aspects.permission.GkhPermissionAspect',
    alias: 'widget.manorgworkmodeperm',

    permissions: [
    /*
    * name - имя пермишена в дереве, 
    * applyTo - селектор контрола, к которому применяется пермишен, 
    * selector - селектор формы, на которой находится контрол
    */
        { name: 'Gkh.Orgs.Managing.Register.WorkMode.Edit', applyTo: 'b4savebutton', selector: '#workModeGrid' },
        { name: 'Gkh.Orgs.Managing.Register.WorkMode.Edit', applyTo: 'b4savebutton', selector: '#receptionCitizensGrid' },
        { name: 'Gkh.Orgs.Managing.Register.WorkMode.Edit', applyTo: 'b4savebutton', selector: '#dispatcherWorkGrid' }
    ]
});
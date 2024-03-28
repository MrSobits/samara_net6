Ext.define('B4.aspects.permission.manorg.CommonService', {
    extend: 'B4.aspects.permission.GkhPermissionAspect',
    alias: 'widget.manorgacommonserviceperm',

    permissions: [
    /*
    * name - имя пермишена в дереве, 
    * applyTo - селектор контрола, к которому применяется пермишен, 
    * selector - селектор формы, на которой находится контрол
    */
        { name: 'Gkh.Orgs.Managing.Register.Service.Common.Create', applyTo: 'b4addbutton', selector: '#manorgCommunalServiceGrid' },
        { name: 'Gkh.Orgs.Managing.Register.Service.Common.Edit', applyTo: 'b4savebutton', selector: '#manorgCommunalServiceEditWindow' },
        {
            name: 'Gkh.Orgs.Managing.Register.Service.Common.Delete', applyTo: 'b4deletecolumn', selector: '#manorgCommunalServiceGrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        }
    ]
});
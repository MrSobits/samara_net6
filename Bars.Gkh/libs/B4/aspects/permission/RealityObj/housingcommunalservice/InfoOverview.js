Ext.define('B4.aspects.permission.realityobj.housingcommunalservice.InfoOverview', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.infooverviewperm',

    permissions: [
    /*
    * name - имя пермишена в дереве, 
    * applyTo - селектор контрола, к которому применяется пермишен, 
    * selector - селектор формы, на которой находится контрол
    */
    //основной грид и панель редактирования жилого дома
        { name: 'Gkh.RealityObject.Register.HousingComminalService.InfoOverview.Create', applyTo: 'b4addbutton', selector: 'hseoverallbalancegrid' },
        { name: 'Gkh.RealityObject.Register.HousingComminalService.InfoOverview.Edit', applyTo: 'b4savebutton', selector: 'hseoverallbalancegrid' },
        { name: 'Gkh.RealityObject.Register.HousingComminalService.InfoOverview.Delete', applyTo: 'b4deletecolumn', selector: 'hseoverallbalancegrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        { name: 'Gkh.RealityObject.Register.HousingComminalService.InfoOverview.Edit', applyTo: 'b4savebutton', selector: 'hseinfoovervieweditpanel' }
        
    
    ]
});
Ext.define('B4.aspects.permission.dict.HeatSeasonPeriod', {
    extend: 'B4.aspects.permission.GkhPermissionAspect',
    alias: 'widget.heatseasonperiodperm',

    permissions: [
    /*
    * name - имя пермишена в дереве, 
    * applyTo - селектор контрола, к которому применяется пермишен, 
    * selector - селектор формы, на которой находится контрол
    */
    //основной грид и панель редактирования жилого дома
        { name: 'GkhGji.Dict.HeatSeasonPeriod.Create', applyTo: 'b4addbutton', selector: '#heatSeasonPeriodGrid' },
        { name: 'GkhGji.Dict.HeatSeasonPeriod.Edit', applyTo: 'b4savebutton', selector: '#heatSeasonPeriodEditWindow' },
        { name: 'GkhGji.Dict.HeatSeasonPeriod.Delete', applyTo: 'b4deletecolumn', selector: '#heatSeasonPeriodGrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        }
    ]
});
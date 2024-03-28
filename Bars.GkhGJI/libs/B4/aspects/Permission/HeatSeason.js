Ext.define('B4.aspects.permission.HeatSeason', {
    extend: 'B4.aspects.permission.GkhPermissionAspect',
    alias: 'widget.heatseasonperm',

    permissions: [
    /*
    * name - имя пермишена в дереве, 
    * applyTo - селектор контрола, к которому применяется пермишен, 
    * selector - селектор формы, на которой находится контрол
    */
        { name: 'GkhGji.HeatSeason.Create', applyTo: 'b4addbutton', selector: '#heatSeasonGrid' },
        { name: 'GkhGji.HeatSeason.Edit', applyTo: 'b4savebutton', selector: '#heatSeasonEditWindow' },
        { name: 'GkhGji.HeatSeason.Delete', applyTo: 'b4deletecolumn', selector: '#heatSeasonGrid',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },

        { name: 'GkhGji.HeatSeason.Register.Document.Create', applyTo: 'b4addbutton', selector: '#heatSeasonDocGrid' },
        { name: 'GkhGji.HeatSeason.Register.Document.Edit', applyTo: 'b4savebutton', selector: '#heatSeasonDocEditWindow' },
        { name: 'GkhGji.HeatSeason.Register.Document.Delete', applyTo: 'b4deletecolumn', selector: '#heatSeasonDocGrid',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },

        { name: 'GkhGji.HeatSeason.Register.Inspection.Create', applyTo: 'b4addbutton', selector: '#heatSeasonInspectionGrid' },
        { name: 'GkhGji.HeatSeason.Register.Inspection.Edit', applyTo: 'b4savebutton', selector: '#activityTsjEditWindow' }
    ]
});
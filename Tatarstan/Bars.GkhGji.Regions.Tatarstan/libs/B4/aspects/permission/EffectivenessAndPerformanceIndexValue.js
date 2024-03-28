Ext.define('B4.aspects.permission.EffectivenessAndPerformanceIndexValue', {
    extend: 'B4.aspects.permission.GkhPermissionAspect',
    alias: 'widget.effectivenessandperformanceindexvalueperm',

    permissions: [
        { name: 'GkhGji.EffectivenessAndPerformanceIndexValue.Create', applyTo: 'b4addbutton', selector: 'effectivenessandperformanceindexvaluegrid' },
        { name: 'GkhGji.EffectivenessAndPerformanceIndexValue.SendToTor', applyTo: 'button[name=sendToTorButton]', selector: 'effectivenessandperformanceindexvaluegrid' },
        {
            name: 'GkhGji.EffectivenessAndPerformanceIndexValue.Delete', applyTo: 'b4deletecolumn', selector: 'effectivenessandperformanceindexvaluegrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        }
    ]
});
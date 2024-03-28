Ext.define('B4.aspects.fieldrequirement.RealityObjectOutdoor', {
    extend: 'B4.aspects.FieldRequirementAspect',
    alias: 'widget.realityobjectoutdoorfieldrequirement',

    init: function () {
        this.requirements = [
            { name: 'Gkh.RealityObjectOutdoor.Field.Area', applyTo: '[name=Area]', selector: 'realityobjectoutdooreditpanel' },
            { name: 'Gkh.RealityObjectOutdoor.Field.AsphaltArea', applyTo: '[name=AsphaltArea]', selector: 'realityobjectoutdooreditpanel' },
            { name: 'Gkh.RealityObjectOutdoor.Field.Description', applyTo: '[name=Description]', selector: 'realityobjectoutdooreditpanel' },
            { name: 'Gkh.RealityObjectOutdoor.Field.RepairPlanYear', applyTo: '[name=RepairPlanYear]', selector: 'realityobjectoutdooreditpanel' }
        ];

        this.callParent(arguments);
    }
});
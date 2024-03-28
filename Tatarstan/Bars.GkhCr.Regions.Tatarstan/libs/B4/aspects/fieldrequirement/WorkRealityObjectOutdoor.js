Ext.define('B4.aspects.fieldrequirement.WorkRealityObjectOutdoor', {
    extend: 'B4.aspects.FieldRequirementAspect',
    alias: 'widget.workrealityobjectoutdoorfieldrequirement',

    init: function () {
        this.requirements = [
            { name: 'GkhCr.Dict.WorkRealityObjectOutdoor.Field.Code', applyTo: '[name=Code]', selector: 'workrealityobjectoutdoorwindow' },
            { name: 'GkhCr.Dict.WorkRealityObjectOutdoor.Field.UnitMeasure', applyTo: '[name=UnitMeasure]', selector: 'workrealityobjectoutdoorwindow' }
        ];

        this.callParent(arguments);
    }
});
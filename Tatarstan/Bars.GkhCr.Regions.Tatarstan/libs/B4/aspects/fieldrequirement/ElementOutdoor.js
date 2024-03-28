Ext.define('B4.aspects.fieldrequirement.ElementOutdoor', {
    extend: 'B4.aspects.FieldRequirementAspect',
    alias: 'widget.elementoutdoorfieldrequirement',

    init: function () {
        this.requirements = [
            { name: 'GkhCr.Dict.ElementOutdoor.Field.Code', applyTo: '[name=Code]', selector: 'elementoutdoorwindow' },
            { name: 'GkhCr.Dict.ElementOutdoor.Field.UnitMeasure', applyTo: '[name=UnitMeasure]', selector: 'elementoutdoorwindow' }
        ];

        this.callParent(arguments);
    }
});
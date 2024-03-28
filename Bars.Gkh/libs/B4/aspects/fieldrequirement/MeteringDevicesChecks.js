Ext.define('B4.aspects.fieldrequirement.MeteringDevicesChecks', {
    extend: 'B4.aspects.FieldRequirementAspect',
    alias: 'widget.meteringdeviceschecksfieldrequirement',
    
    init: function() {
        this.requirements = [
            { name: 'Gkh.RealityObject.MeteringDevicesChecks.Fields.MeteringDevice_Rqrd', applyTo: 'b4selectfield[name=MeteringDevice]', selector: 'meteringdeviceschecksEditWindow' },
            { name: 'Gkh.RealityObject.MeteringDevicesChecks.Fields.ControlReading_Rqrd', applyTo: '[name=ControlReading]', selector: 'meteringdeviceschecksEditWindow' },
            { name: 'Gkh.RealityObject.MeteringDevicesChecks.Fields.RemovalControlReadingDate_Rqrd', applyTo: '[name=RemovalControlReadingDate]', selector: 'meteringdeviceschecksEditWindow' },
            { name: 'Gkh.RealityObject.MeteringDevicesChecks.Fields.StartDateCheck_Rqrd', applyTo: '[name=StartDateCheck]', selector: 'meteringdeviceschecksEditWindow' },
            { name: 'Gkh.RealityObject.MeteringDevicesChecks.Fields.StartValue_Rqrd', applyTo: '[name=StartValue]', selector: 'meteringdeviceschecksEditWindow' },
            { name: 'Gkh.RealityObject.MeteringDevicesChecks.Fields.EndDateCheck_Rqrd', applyTo: '[name=EndDateCheck]', selector: 'meteringdeviceschecksEditWindow' },
            { name: 'Gkh.RealityObject.MeteringDevicesChecks.Fields.EndValue_Rqrd', applyTo: '[name=EndValue]', selector: 'meteringdeviceschecksEditWindow' },
            { name: 'Gkh.RealityObject.MeteringDevicesChecks.Fields.IntervalVerification_Rqrd', applyTo: '[name=IntervalVerification]', selector: 'meteringdeviceschecksEditWindow' }
        ];

        this.callParent(arguments);
    }
});
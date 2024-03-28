Ext.define('B4.aspects.permission.realityobj.MeteringDevicesChecksPermission', {
    extend: 'B4.aspects.permission.GkhPermissionAspect',
    alias: 'widget.meteringdeviceschecksperm',

    permissions: [
        { name: 'Gkh.RealityObject.MeteringDevicesChecks.Create', applyTo: 'b4addbutton', selector: 'meteringDevicesChecksGrid' },
        { name: 'Gkh.RealityObject.MeteringDevicesChecks.Edit', applyTo: 'b4editcolumn', selector: 'meteringDevicesChecksGrid' },
        { name: 'Gkh.RealityObject.MeteringDevicesChecks.Delete', applyTo: 'b4deletecolumn', selector: 'meteringDevicesChecksGrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        { name: 'Gkh.RealityObject.MeteringDevicesChecks.Fields.MeteringDevice_View', applyTo: 'b4selectfield[name=MeteringDevice]', selector: 'meteringdeviceschecksEditWindow' },
        { name: 'Gkh.RealityObject.MeteringDevicesChecks.Fields.ControlReading_View', applyTo: '[name=ControlReading]', selector: 'meteringdeviceschecksEditWindow' },
        { name: 'Gkh.RealityObject.MeteringDevicesChecks.Fields.RemovalControlReadingDate_View', applyTo: '[name=RemovalControlReadingDate]', selector: 'meteringdeviceschecksEditWindow' },
        { name: 'Gkh.RealityObject.MeteringDevicesChecks.Fields.StartDateCheck_View', applyTo: '[name=StartDateCheck]', selector: 'meteringdeviceschecksEditWindow' },
        { name: 'Gkh.RealityObject.MeteringDevicesChecks.Fields.StartValue_View', applyTo: '[name=StartValue]', selector: 'meteringdeviceschecksEditWindow' },
        { name: 'Gkh.RealityObject.MeteringDevicesChecks.Fields.EndDateCheck_View', applyTo: '[name=EndDateCheck]', selector: 'meteringdeviceschecksEditWindow' },
        { name: 'Gkh.RealityObject.MeteringDevicesChecks.Fields.EndValue_View', applyTo: '[name=EndValue]', selector: 'meteringdeviceschecksEditWindow' },
        { name: 'Gkh.RealityObject.MeteringDevicesChecks.Fields.MarkMeteringDevice_View', applyTo: '[name=MarkMeteringDevice]', selector: 'meteringdeviceschecksEditWindow' },
        { name: 'Gkh.RealityObject.MeteringDevicesChecks.Fields.IntervalVerification_View', applyTo: '[name=IntervalVerification]', selector: 'meteringdeviceschecksEditWindow' },
        { name: 'Gkh.RealityObject.MeteringDevicesChecks.Fields.NextDateCheck_View', applyTo: '[name=NextDateCheck]', selector: 'meteringdeviceschecksEditWindow' },
    ]
});
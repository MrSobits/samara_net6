Ext.define('B4.model.realityobj.MeteringDevicesChecks', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'MeteringDevicesChecks'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'MeteringDevice' },
        { name: 'RealityObject' },
        { name: 'ControlReading' },
        { name: 'RemovalControlReadingDate' },
        { name: 'StartDateCheck' },
        { name: 'StartValue' },
        { name: 'EndDateCheck' },
        { name: 'EndValue' },
        { name: 'MarkMeteringDevice' },
        { name: 'IntervalVerification' },
        { name: 'NextDateCheck' },
        { name: 'PersonalAccountNum' }
    ]
});
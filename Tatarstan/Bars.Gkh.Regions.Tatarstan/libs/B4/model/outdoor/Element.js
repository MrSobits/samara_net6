Ext.define('B4.model.outdoor.Element', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'RealityObjectOutdoorElementOutdoor'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Group' },
        { name: 'Element' },
        { name: 'UnitMeasure' },
        { name: 'Volume' },
        { name: 'Condition' },
        { name: 'Outdoor' }
    ]
});
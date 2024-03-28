Ext.define('B4.model.gisrealestate.IndicatorServiceComparison', {
    extend: 'B4.base.Model',
    proxy: {
        type: 'b4proxy',
        controllerName: 'IndicatorServiceComparison'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Service' },
        { name: 'ServiceName' },
        { name: 'GisTypeIndicator' }
        //{ name: 'TypeIndicatorList' }
    ]
});
Ext.define('B4.model.riskorientedmethod.KindKNDDict', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'KindKNDDict'
    },
    fields: [
        { name: 'KindKND'},
        { name: 'DateFrom' },
        { name: 'DateTo' },
    ]
});
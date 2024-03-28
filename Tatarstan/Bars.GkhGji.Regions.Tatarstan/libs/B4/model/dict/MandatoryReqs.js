Ext.define('B4.model.dict.MandatoryReqs', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'MandatoryReqs'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'TorId' },
        { name: 'MandratoryReqName' },
        { name: 'MandratoryReqContent' },
        { name: 'StartDateMandatory' },
        { name: 'EndDateMandatory' },
        { name: 'NpaFullName' },
    ]
});
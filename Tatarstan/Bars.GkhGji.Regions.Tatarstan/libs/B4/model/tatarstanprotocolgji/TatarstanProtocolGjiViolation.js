Ext.define('B4.model.tatarstanprotocolgji.TatarstanProtocolGjiViolation', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'TatarstanProtocolGjiViolation'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ViolationGji' },
        { name: 'NormativeDoc' }
    ]
});
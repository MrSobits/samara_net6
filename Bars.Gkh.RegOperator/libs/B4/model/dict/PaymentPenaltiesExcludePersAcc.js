Ext.define('B4.model.dict.PaymentPenaltiesExcludePersAcc', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'PaymentPenaltiesExcludePersAcc'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'PaymentPenalties' },
        { name: 'PersonalAccount' },
        { name: 'Address' },
        { name: 'Municipality' },
        { name: 'PersonalAccountNum' }
    ]
});
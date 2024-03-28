Ext.define('B4.model.GeneralStateHistory', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'GeneralStateHistory'
    },
    fields: [
        { name: 'ChangeDate', type: 'date', dateFormat: 'Y-m-d\TH:i:s', useNull: true },
        { name: 'StartState'},
        { name: 'FinalState'},
        { name: 'UserName', useNull: true },
        { name: 'UserLogin', useNull: true },
        { name: 'Id'}
    ]
});
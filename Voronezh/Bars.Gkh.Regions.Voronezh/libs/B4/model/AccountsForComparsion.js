Ext.define('B4.model.AccountsForComparsion', {
    extend: 'B4.base.Model',
    idProperty: 'Id',

    proxy: {
        type: 'b4proxy',
        controllerName: 'DataAreaOwnerMerger',
        listAction: 'ListAccountsForComparsion',
        timeout: 100000 // 5 ����� ��� ����������
    },
    fields: [
        { name: 'Id' },
        { name: 'Address' },
        { name: 'Municipality' },
        { name: 'OwnerName' },
        { name: 'CnumRoom' },
        { name: 'AreaRoom' },
        { name: 'AreaShare' }
    ]
});
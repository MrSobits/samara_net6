Ext.define('B4.model.dict.GkuTarifGji', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    requires: [
        'B4.enums.ServiceKindGku'
    ],
    proxy: {
        type: 'b4proxy',
        controllerName: 'GkuTariffGji'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Service', defaultValue: null },
        { name: 'ResourceOrg', defaultValue: null },
        { name: 'ManOrg', defaultValue: null },
        { name: 'ServiceKind', defaultValue: 10 },
        { name: 'Municipality' },
        { name: 'Contragent' },
        { name: 'TarifRso' },
        { name: 'TarifMo' },
        { name: 'NormativeValue' },
        { name: 'DateStart' },
        { name: 'DateEnd' },
        { name: 'PurchasePrice' },
        { name: 'PurchaseVolume' },
        { name: 'NormativeActInfo' }
    ]
});
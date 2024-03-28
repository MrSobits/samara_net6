/**
* Модель "Договор переадчи управления"
*/
Ext.define('B4.model.manorg.contract.Transfer', {
    extend: 'B4.model.manorg.contract.Base',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ManOrgContractTransfer'
    },
    fields: [
        { name: 'ManOrgJskTsj', defaultValue: null },
        { name: 'JskTsjContractId' },
        { name: 'ManOrgJskTsjName' },
        { name: 'RealityObjectName' },
        { name: 'DocumentDate' },
        { name: 'ContragentName' },
        { name: 'InputMeteringDeviceValuesBeginDate' },
        { name: 'InputMeteringDeviceValuesEndDate' },
        { name: 'DrawingPaymentDocumentDate' },
        { name: 'ThisMonthPaymentDocDate', defaultValue: false },
        { name: 'ProtocolNumber' },
        { name: 'ProtocolDate' },
        { name: 'ProtocolFileInfo', defaultValue: null },
        { name: 'ContractFoundation', defaultValue: 10 },
        { name: 'TerminationDate' },
        { name: 'TerminationFile', defaultValue: null },
        { name: 'PaymentAmount' },
        { name: 'SetPaymentsFoundation', defaultValue: null },
        { name: 'PaymentProtocolFile', defaultValue: null },
        { name: 'PaymentProtocolDescription' }
    ]
});
Ext.define('B4.model.manorg.contract.Owners', {
    extend: 'B4.model.manorg.contract.Base',
    requires: ['B4.enums.ManOrgContractOwnersFoundation'],
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ManOrgContractOwners'
    },
    fields: [
        { name: 'ContractFoundation', defaultValue: 20 },
        { name: 'InputMeteringDeviceValuesBeginDate' },
        { name: 'IsLastDayMeteringDeviceValuesBeginDate' },
        { name: 'InputMeteringDeviceValuesEndDate' },
        { name: 'IsLastDayMeteringDeviceValuesEndDate' },
        { name: 'DrawingPaymentDocumentDate' },
        { name: 'IsLastDayDrawingPaymentDocument' },
        { name: 'ThisMonthPaymentDocDate' },
        { name: 'ProtocolNumber' },
        { name: 'ProtocolDate' },
        { name: 'ProtocolFileInfo', defaultValue: null },
        { name: 'TerminationDate' },
        { name: 'TerminationFile', defaultValue: null },
        { name: 'PaymentAmount' },
        { name: 'SetPaymentsFoundation' },
        { name: 'PaymentProtocolFile', defaultValue: null },
        { name: 'PaymentProtocolDescription' },
        { name: 'RevocationReason' },
        { name: 'DateLicenceRegister' },
        { name: 'DateLicenceDelete' },
        { name: 'RegisterReason' },
        { name: 'DeleteReason' },
        { name: 'OwnersSignedContractFile' }
    ]
});
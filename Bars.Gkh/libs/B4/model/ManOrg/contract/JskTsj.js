Ext.define('B4.model.manorg.contract.JskTsj', {
    extend: 'B4.model.manorg.contract.Base',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ManOrgJskTsjContract'
    },
    fields: [
        { name: 'ManOrgTransferredManagementName' },
        { name: 'DocumentName' },
        { name: 'DocumentDate' },
        { name: 'ProtocolNumber' },
        { name: 'ProtocolDate' },
        { name: 'ProtocolFileInfo', defaultValue: null },
        { name: 'InputMeteringDeviceValuesBeginDate' },
        { name: 'IsLastDayMeteringDeviceValuesBeginDate' },
        { name: 'InputMeteringDeviceValuesEndDate' },
        { name: 'IsLastDayMeteringDeviceValuesEndDate' },
        { name: 'DrawingPaymentDocumentDate' },
        { name: 'IsLastDayDrawingPaymentDocument' },
        { name: 'ThisMonthPaymentDocDate' },
        { name: 'ContractFoundation', defaultValue: 10 },
        { name: 'TerminationDate' },
        { name: 'TerminationFile', defaultValue: null },
        { name: 'CompanyReqiredPaymentAmount' },
        { name: 'CompanyPaymentProtocolFile', defaultValue: null },
        { name: 'CompanyPaymentProtocolDescription' },
        { name: 'ReqiredPaymentAmount' },
        { name: 'PaymentProtocolFile', defaultValue: null },
        { name: 'PaymentProtocolDescription' }
    ]
});
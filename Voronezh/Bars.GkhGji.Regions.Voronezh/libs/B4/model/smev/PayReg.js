Ext.define('B4.model.smev.PayReg', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'PayReg'
    },
    fields: [
        { name: 'Amount' },
        { name: 'Kbk' },
        { name: 'OKTMO' },
        { name: 'PaymentDate' },
        { name: 'PaymentId' },
        { name: 'Purpose' },
        { name: 'SupplierBillID' },
        { name: 'PaymentOrg' },
        { name: 'PaymentOrgDescr' },
        { name: 'PayerId' }, 
        { name: 'GisGmp' }, 
        { name: 'PayerAccount' },
        { name: 'PayerName' },
        { name: 'BdiStatus' },
        { name: 'BdiPaytReason' },
        { name: 'BdiTaxPeriod' },
        { name: 'BdiTaxDocNumber' },
        { name: 'BdiTaxDocDate' },
        { name: 'AccDocDate' },
        { name: 'AccDocNo' },
        { name: 'Status' },
        { name: 'GisGmpUIN' },
        { name: 'GisGmpId' },
        { name: 'Reconcile', defaultValue: 20 },
        { name: 'GusGmpUIN' }, 
        { name: 'IsGisGmpConnected' }
    ]
});
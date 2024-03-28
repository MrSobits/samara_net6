Ext.define('B4.model.smev.GisGmp', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'GisGmp'
    },
    fields: [
        { name: 'ReqId' },
        { name: 'BillDate' },
        { name: 'CalcDate' },
        { name: 'Inspector' },
        { name: 'RequestState', defaultValue: 0 },
        { name: 'BillFor'},
        { name: 'DocumentNumber' },
        { name: 'DocumentSerial' },
        { name: 'PhysicalPersonDocType' },
        { name: 'Answer' },
        { name: 'ReconcileAnswer' },
        { name: 'Protocol' },
        { name: 'UIN' },
        { name: 'GisGmpChargeType', defaultValue: 1 },
        { name: 'Reason' },
        { name: 'MessageId' },
        { name: 'IdentifierType' },
        { name: 'INN' },
        { name: 'IsRF' },
        { name: 'KBK'},
        //{ name: 'KBK', defaultValue: '07811690040110000140' },
        { name: 'KIO' },
        { name: 'KPP' },
        { name: 'OKTMO', defaultValue: '75701000' },
        { name: 'PayerType' },
        { name: 'PaymentType' },
        { name: 'Status' },
        { name: 'TaxDocDate' },
        { name: 'TaxDocNumber' },
        { name: 'GisGmpPaymentsType', defaultValue: 1 },        
        { name: 'TaxPeriod' },
        //
        { name: 'PaymentKBK' },
        { name: 'GetPaymentsStartDate' },
        { name: 'GetPaymentsEndDate' },
        //
        { name: 'TotalAmount' },
        { name: 'PaymentsAmount' },
        { name: 'AltPayerIdentifier' },
        { name: 'GisGmpPaymentsKind', defaultValue: 90 },
        { name: 'GISGMPPayerStatus' },
        { name: 'TypeLicenseRequest', defaultValue: 0},
        { name: 'LicenseReissuance' },
        { name: 'ManOrgLicenseRequest' },

        { name: 'DocNumDate' },

        { name: 'PayerName' },
        { name: 'LegalAct' },
        { name: 'Reconciled' },

        { name: 'SMEVStage' }
    ]
});
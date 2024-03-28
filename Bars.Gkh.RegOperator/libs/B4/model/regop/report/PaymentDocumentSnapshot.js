Ext.define('B4.model.regop.report.PaymentDocumentSnapshot',
    {
        extend: 'B4.base.Model',
        proxy:
        {
            type: 'b4proxy',
            controllerName: 'PaymentDocumentSnapshot',
            timeout: 2 * 60 * 1000
        },

        fields: [
            { name: 'HolderId' },
            { name: 'HolderType' },
            { name: 'OwnerType' },
            { name: 'PaymentDocumentType' },
            { name: 'PaymentState' },
            { name: 'Period' },
            { name: 'Data' },
            { name: 'DocNumber' },
            { name: 'DocDate' },
            { name: 'Payer' },
            { name: 'Municipality' },
            { name: 'Settlement' },
            { name: 'Address' },
            { name: 'PaymentReceiverAccount' },
            { name: 'DeliveryAgent' },
            { name: 'TotalCharge' },
            { name: 'IsBase' },
            { name: 'AccountCount' },
            { name: 'SendingEmailState' },
            { name: 'ObjectCreateDate' },
            { name: 'ObjectEditDate' },
            { name: 'ObjectVersion' },
            { name: 'HasEmail' },
            { name: 'OwnerInn' }
        ]
    });
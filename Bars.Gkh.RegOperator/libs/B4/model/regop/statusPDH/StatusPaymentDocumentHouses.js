Ext.define('B4.model.regop.statusPDH.StatusPaymentDocumentHouses',
    {
        extend: 'B4.base.Model',
        proxy:
        {
            type: 'b4proxy',
            controllerName: 'StatusPaymentDocumentHouses',
            listAction: 'GetStatusPaymentDocumentHouses'
        },

        fields: [
            { name: 'Account' },
            { name: 'Name' },
            { name: 'Address' },
            { name: 'State' }
        ]
    });
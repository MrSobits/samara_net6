Ext.define('B4.model.regop.UnacceptedPaymentPacket', {
    extend: 'B4.base.Model',

    fields: [
        { name: 'Id' },
        { name: 'CreateDate' },
        { name: 'Description' },
        { name: 'State' },
        { name: 'Sum' },
        { name: 'DistributePenalty' }
    ],

    proxy: {
        type: 'b4proxy',
        controllerName: 'UnacceptedPaymentPacket'
    }
});
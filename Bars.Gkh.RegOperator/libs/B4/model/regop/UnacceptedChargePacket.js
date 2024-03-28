Ext.define('B4.model.regop.UnacceptedChargePacket', {
    extend: 'B4.base.Model',

    fields: [
        { name: 'Id' },
        { name: 'CreateDate' },
        { name: 'Description' },
        { name: 'PacketState' },
        { name: 'UserName' }
    ],
    proxy: {
        type: 'b4proxy',
        controllerName: 'UnacceptedChargePacket'
    }
});
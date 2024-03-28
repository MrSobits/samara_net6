Ext.define('B4.model.dict.OwnerProtocolTypeDecision', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'OwnerProtocolTypeDecision'
    },
    fields: [
      { name: 'Name' },
      { name: 'OwnerProtocolType' }
   ]
});
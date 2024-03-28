Ext.define('B4.model.dict.OwnerProtocolType', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'OwnerProtocolType'
    },
    fields: [
      { name: 'Name' },
      { name: 'Code' },
      { name: 'Description' }
   ]
});
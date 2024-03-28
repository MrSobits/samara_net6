Ext.define('B4.model.dict.FiasOktmo', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'FiasOktmo'
    },
    fields: [
         { name: 'Id', useNull: true },
         { name: 'Municipality' },
         { name: 'MoOkato' },
         { name: 'MoOktmo' },
         { name: 'OffName' },
         { name: 'ShortName' },
         { name: 'OKATO' },
         { name: 'OKTMO' },
         { name: 'FiasGuid' }
    ]
});
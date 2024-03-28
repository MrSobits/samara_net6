Ext.define('B4.model.integrations.gis.Method', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'GisIntegration',
        listAction: 'MethodList',
        listeners: {
            exception: function (proxy, response, operation) {

                var result = Ext.decode(response.responseText);

                if (result.success === false) {
                    Ext.Msg.alert('Ошибка загрузки списка методов', result.message || 'Ошибка загрузки списка методов');
                }
            }
        }
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Order' },
        { name: 'Name' },
        { name: 'Description' },
        { name: 'Type' },
        { name: 'NeedSign' },
        { name: 'Dependencies' }
    ]
});
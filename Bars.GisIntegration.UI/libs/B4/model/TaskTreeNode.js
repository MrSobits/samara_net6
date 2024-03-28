Ext.define('B4.model.TaskTreeNode', {
    extend: 'Ext.data.Model',
    idgen: {
        type: 'sequential',
        prefix: 'ID_'
    },
    fields: [
        'Id',
        'Type',
        'Name',
        'Author',
        'StartTime',
        'EndTime',
        'State',
        'Percent',
        'Message',
        'Result',
        'Protocol',
        'ResultLog'
    ],
    proxy: {
        url: 'TaskTree/GetTaskTreeNodes',
        type: 'ajax',
        reader: {
            type: 'json'
        },
        listeners: {
            exception: function (proxy, response, operation) {

                var result = Ext.decode(response.responseText);

                if (result.success === false) {
                    Ext.Msg.alert('Ошибка загрузки задач', result.message || 'Ошибка загрузки задач');
                }
            }
        }
    }
});
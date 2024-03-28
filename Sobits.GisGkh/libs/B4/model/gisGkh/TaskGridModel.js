Ext.define('B4.model.gisGkh.TaskGridModel', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'GisGkhRequests'
    },
    fields: [
        { name: 'MessageGUID' },
        { name: 'RequesterMessageGUID' },
        { name: 'ObjectCreateDate' },
        { name: 'OperatorName' },
        //{ name: 'ReqDate' },
        { name: 'RequestState' },
        { name: 'TypeRequest' },
        { name: 'Answer', },
        //{ name: 'IsExport', },
        { name: 'ReqFile', defaultValue: null },
        { name: 'RespFile', defaultValue: null },
        { name: 'LogFile', defaultValue: null }
    ]
     
    //proxy: {
    //    url: 'GisGkhIntegration/GetTaskList',
    //    type: 'ajax',
    //    reader: {
    //        type: 'json'
    //    },
    //    listeners: {
    //        exception: function (proxy, response, operation) {

    //            var result = Ext.decode(response.responseText);

    //            if (result.success === false) {
    //                Ext.Msg.alert('Ошибка загрузки задач', result.message || 'Ошибка загрузки задач');
    //            }
    //        }
    //    }
    //}
});
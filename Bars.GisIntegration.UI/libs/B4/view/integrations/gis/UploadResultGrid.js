Ext.define('B4.view.integrations.gis.UploadResultGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.uploadresultgrid',
    requires: [
        'B4.enums.ObjectValidateState',
        'B4.ux.grid.column.Enum'
    ],
    columnLines: true,
    uploadAttachmentsResult: undefined,
    triggerId: 0,

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.UploadResult', {
                //параметры для экспорта
                lastOptions: {
                    params: {
                        triggerId: me.triggerId
                    }
                }
            });

        if (me.uploadAttachmentsResult && me.uploadAttachmentsResult.length !== 0) {
            store.loadData(me.uploadAttachmentsResult);
        }

        Ext.applyIf(me, {
            store: store,
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    flex: 2,
                    text: 'Идентификатор',
                    dataIndex: 'Id'
                },
                {
                    xtype: 'gridcolumn',
                    flex: 3,
                    text: 'Наименование',
                    dataIndex: 'Name'
                },
                {
                    xtype: 'gridcolumn',
                    flex: 3,
                    text: 'Описание',
                    dataIndex: 'Description'
                },
                {
                    xtype: 'b4enumcolumn',
                    enumName: 'B4.enums.ObjectValidateState',
                    flex: 3,
                    text: 'Статус',
                    dataIndex: 'State'
                },
                {
                    xtype: 'gridcolumn',
                    flex: 7,
                    text: 'Сообщение',
                    dataIndex: 'Message'
                }
            ]
        });

        me.callParent(arguments);
    }
});

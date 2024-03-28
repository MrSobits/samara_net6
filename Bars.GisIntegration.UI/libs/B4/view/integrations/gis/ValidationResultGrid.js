Ext.define('B4.view.integrations.gis.ValidationResultGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.validationresultgrid',
    requires: [
        'B4.enums.ObjectValidateState',
        'B4.ux.grid.column.Enum'
    ],
    columnLines: true,
    validationResult: undefined,
    triggerId: 0,

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.ValidationResult', {
                //параметры для экспорта
                lastOptions: {
                    params: {
                        triggerId: me.triggerId
                    }
                }
            });

        if (me.validationResult && me.validationResult.length !== 0) {
            store.loadData(me.validationResult);
        }

        Ext.applyIf(me, {
            store: store,
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    flex: 2,
                    align: 'center',
                    text: 'Идентификатор',
                    dataIndex: 'Id'
                },
                {
                    xtype: 'gridcolumn',
                    flex: 3,
                    align: 'center',
                    text: 'Объект',
                    dataIndex: 'Description'
                },
                {
                    xtype: 'b4enumcolumn',
                    enumName: 'B4.enums.ObjectValidateState',
                    flex: 3,
                    align: 'center',
                    text: 'Статус',
                    dataIndex: 'State'
                },
                {
                    xtype: 'gridcolumn',
                    flex: 7,
                    align: 'center',
                    text: 'Сообщение',
                    dataIndex: 'Message'
                }
            ]
        });

        me.callParent(arguments);
    }
});
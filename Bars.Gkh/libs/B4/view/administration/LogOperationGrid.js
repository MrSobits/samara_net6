Ext.define('B4.view.administration.LogOperationGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Update',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.form.ComboBox',
        'B4.ux.grid.toolbar.Paging',
        'B4.store.administration.LogOperation',
        'B4.ux.grid.column.Enum',
        'B4.enums.LogOperationType'
    ],

    title: 'Логи операций',
    alias: 'widget.operationloggrid',
    closable: true,

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.administration.LogOperation');

        Ext.applyIf(me, {
            store: store,
            columnLines: true,
            columns: [
                {
                    xtype: 'datecolumn',
                    dataIndex: 'StartDate',
                    width: 125,
                    text: 'Время старта',
                    format: 'd.m.Y H:i:s',
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'EndDate',
                    width: 125,
                    text: 'Время окончания',
                    format: 'd.m.Y H:i:s',
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'b4enumcolumn',
                    dataIndex: 'OperationType',
                    text: 'Тип операции ',
                    enumName: 'B4.enums.LogOperationType',
                    width: 150,
                    filter: true
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Comment',
                    text: 'Комментарий ',
                    flex: 1,
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'User',
                    width: 120,
                    text: 'Пользователь ',
                    filter: {
                        xtype: 'textfield',
                        flex: 1
                    }
                }, 
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'LogFile',
                    width: 100,
                    text: 'Файл лога',
                    renderer: function (v) {
                        return v ? ('<a href="' + B4.Url.action('/FileUpload/Download?id=' + v.Id) + '" target="_blank" style="color: black">Скачать</a>') : '';
                    }
                }
                //{
                //    xtype: 'gridcolumn',
                //    dataIndex: 'LogFile',
                //    width: 100,
                //    text: 'Файл лога',
                //    renderer: function (value) {
                //        var fileId = value.Id,
                //            url;
                //        if (fileId > 0) {
                //            url = B4.Url.content(Ext.String.format('{0}/{1}?id={2}', 'FileUpload', 'Download', fileId));
                //            return '<a href="' + url + '" target="_blank" style="color: black">Скачать</a>';
                //        }
                //        return '';
                //    }
                //}
            ],
            plugins: [
                Ext.create('B4.ux.grid.plugin.HeaderFilters')
            ],
            viewConfig: {
                loadMask: true
            },
            dockedItems: [
                {
                    xtype: 'b4pagingtoolbar',
                    displayInfo: true,
                    store: store,
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    }
});
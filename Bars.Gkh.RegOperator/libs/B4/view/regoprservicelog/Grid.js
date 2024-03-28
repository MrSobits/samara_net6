Ext.define('B4.view.regoprservicelog.Grid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Update',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.form.ComboBox',
        'B4.ux.grid.toolbar.Paging',
        'B4.store.RegopServiceLog',
        'B4.enums.RegopServiceMethodType'
    ],

    title: 'Логи обращений к сервисам',
    alias: 'widget.regoprserviceloggrid',
    closable: true,

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.RegopServiceLog');

        Ext.applyIf(me, {
            store: store,
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'CashPayCenterName',
                    flex: 1,
                    text: 'Наименование РКЦ',
                    filter: {
                        xtype: 'textfield',
                        flex: 1
                    }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateExecute',
                    width: 130,
                    text: 'Время',
                    format: 'd.m.Y H:i:s',
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'FileNum',
                    flex: 1,
                    text: 'Номер файла',
                    filter: {
                        xtype: 'textfield',
                        flex: 1
                    }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'FileDate',
                    width: 100,
                    text: 'Дата файла',
                    format: 'd.m.Y',
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'MethodType',
                    width: 250,
                    text: 'Метод',
                    filter: {
                        xtype: 'b4combobox',
                        items: B4.enums.RegopServiceMethodType.getItemsWithEmpty([null, '-']),
                        editable: false,
                        operand: CondExpr.operands.eq,
                        valueField: 'Value',
                        displayField: 'Display'
                    },
                    renderer: function (val) { return B4.enums.RegopServiceMethodType.displayRenderer(val); }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Status',
                    flex: 1,
                    text: 'Статус ',
                    width: 100,
                    renderer: function (value) {
                        if (value) {
                            return 'Успешно';
                        }
                        
                        return 'Безуспешно';
                    }

                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'File',
                    flex: 1,
                    text: 'Файл лога',
                    renderer: function (value) {
                        var fileId = value.Id;
                        if (fileId > 0) {
                            var url = B4.Url.content(Ext.String.format('{0}/{1}?id={2}', 'FileUpload', 'Download', fileId));
                            return '<a href="' + url + '" target="_blank" style="color: black">Скачать</a>';
                        }
                        return '';
                    }
                }

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
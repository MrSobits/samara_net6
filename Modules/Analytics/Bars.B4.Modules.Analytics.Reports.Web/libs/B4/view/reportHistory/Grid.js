Ext.define('B4.view.reportHistory.Grid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.reporthistorygrid',

    requires: [
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.button.Update',
        'B4.ux.button.Delete',
        'B4.store.ReportHistory'
    ],

    title: 'Журнал отчетов',
    closable: false,

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.ReportHistory');

        me.relayEvents(store, ['beforeload'], 'store.');

        Ext.applyIf(me, {
            selModel: Ext.create('Ext.selection.CheckboxModel', {
                mode: 'MULTI'
            }),
            store: store,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Id',
                    flex: .4,
                    text: 'Номер',
                    filter: {
                         xtype: 'numberfield', operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'Date',
                    flex: .4,
                    text: 'Дата и время',
                    format: 'd.m.Y H:i:s',
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Category',
                    flex: 1,
                    text: 'Категория',
                    filter: {
                        xtype: 'b4combobox',
                        operand: CondExpr.operands.eq,
                        storeAutoLoad: false,
                        hideLabel: true,
                        editable: false,
                        valueField: 'Name',
                        emptyItem: { Name: '-' },
                        url: '/PrintFormCategory/List',
                        listeners: {
                            storebeforeload: function (field, store, options) {
                                options.params.usePaging = false;
                            }
                        }
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Name',
                    text: 'Наименование',
                    flex: 1,
                    filter: {
                         xtype: 'textfield'
                    }
                },
                {
                    xtype: 'actioncolumn',
                    iconCls: 'icon-zoom',
                    flex: .4,
                    text: 'Параметры',
                    handler: function (view, rowIndex, colIndex, item, e, record) {
                        me.fireEvent('rowaction', me, 'showParams', record);
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'File',
                    flex: 0.4,
                    text: 'Файл',
                    renderer: function (v) {
                        return v ? ('<a href="' + B4.Url.action('/FileUpload/Download?id=' + v.Id) + '" target="_blank" style="color: black">Скачать</a>') : '';
                    }
                },
                {
                    xtype: 'actioncolumn',
                    text: 'Сформировать повторно',
                    flex: .4,
                    iconCls: 'icon-arrow-right',
                    handler: function (view, rowIndex, colIndex, item, e, record) {
                        me.fireEvent('rowaction', me, 'reprint', record);
                    }
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            items: [
                                {
                                    xtype: 'b4updatebutton',
                                    handler: function () {
                                        var grid = this.up('b4grid');
                                        grid.fireEvent('gridaction', grid, 'update');
                                    }
                                },
                                {
                                    xtype: 'b4deletebutton',
                                    text: 'Удалить',
                                    handler: function () {
                                        var grid = this.up('b4grid');
                                        grid.fireEvent('gridaction', grid, 'delete');
                                    }
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'b4pagingtoolbar',
                    displayInfo: true,
                    store: store,
                    dock: 'bottom'
                }
            ],

            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
            viewConfig: {
                loadMask: true
            }
        });

        me.callParent(arguments);
    }
});
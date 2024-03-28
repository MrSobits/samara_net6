Ext.define('B4.view.appealcits.PreliminaryCheckGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.column.Enum',
        'B4.enums.PreliminaryCheckResult',
        'B4.ux.grid.toolbar.Paging'
    ],

    alias: 'widget.preliminaryCheckGrid',
    store: 'PreliminaryCheck',

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Inspector',
                    flex: 2,
                    text: 'Сотрудник'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Contragent',
                    width: 100,
                    flex: 2,
                    text: 'Контрагент'
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'CheckDate',
                    width: 100,
                    text: 'Дата',
                    format: 'd.m.Y',
                    renderer: function (v) {
                        if (Date.parse(v, 'd.m.Y') == Date.parse('01.01.0001', 'd.m.Y') || Date.parse(v, 'd.m.Y') == Date.parse('01.01.3000', 'd.m.Y')) {
                            v = undefined;
                        }
                        return Ext.util.Format.date(v, 'd.m.Y');
                    }
                },
                {
                    xtype: 'b4enumcolumn',
                    enumName: 'B4.enums.PreliminaryCheckResult',
                    dataIndex: 'PreliminaryCheckResult',
                    text: 'Результат',
                    flex: 1,
                    filter: true,
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Result',
                    flex: 1,
                    text: 'Примечание'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'FileInfo',
                    width: 100,
                    text: 'Файл',
                    renderer: function (v) {
                        return v ? ('<a href="' + B4.Url.action('/FileUpload/Download?id=' + v.Id) + '" target="_blank" style="color: black">Скачать PDF</a>') : '';
                    }
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
                }
            ],
            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
            viewConfig: {
                loadMask: true
            },
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            items: [
                                {
                                    xtype: 'b4addbutton'
                                },
                                {
                                    xtype: 'b4updatebutton'
                                }
                            ]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});
Ext.define('B4.view.appealcits.AppealCitsAdmonitionGrid', {
    extend: 'B4.ux.grid.Panel',
    
    alias: 'widget.appealCitsAdmonitionGrid',

    requires: [
        'B4.ux.button.Add',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.view.button.Sign',
        'B4.enums.KindKNDGJI',
        'B4.ux.grid.column.Enum'
    ],

    title: 'Предостережения',
    store: 'appealcits.AppealCitsAdmonition',
    itemId: 'appealCitsAdmonitionGrid',

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'b4enumcolumn',
                    enumName: 'B4.enums.KindKNDGJI',
                    dataIndex: 'KindKNDGJI',
                    flex: 1,
                    filter: true,
                    text: 'Вид контроля'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Contragent',
                    flex: 1,
                    text: 'Контрагент'
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'PerfomanceDate',
                    flex: 1,
                    text: 'Срок исполнения',
                    format: 'd.m.Y'
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'PerfomanceFactDate',
                    flex: 1,
                    text: 'Дата факт. исполнения',
                    format: 'd.m.Y'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'File',
                    width: 100,
                    text: 'Файл',
                    renderer: function (v) {
                        return v ? ('<a href="' + B4.Url.action('/FileUpload/Download?id=' + v.Id) + '" target="_blank" style="color: black">Скачать</a>') : '';
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'SignedFile',
                    width: 100,
                    text: 'Подписанный файл',
                    renderer: function (v) {
                        return v ? ('<a href="' + B4.Url.action('/FileUpload/Download?id=' + v.Id) + '" target="_blank" style="color: black">Скачать</a>') : '';
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Signature',
                    width: 100,
                    text: 'Подпись (sig)',
                    renderer: function (v) {
                        return v ? ('<a href="' + B4.Url.action('/FileUpload/Download?id=' + v.Id) + '" target="_blank" style="color: black">Скачать</a>') : '';
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'AnswerFile',
                    width: 100,
                    text: 'Файл ответа',
                    renderer: function (v) {
                        return v ? ('<a href="' + B4.Url.action('/FileUpload/Download?id=' + v.Id) + '" target="_blank" style="color: black">Скачать</a>') : '';
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'SignedAnswerFile',
                    width: 100,
                    text: 'Подписанный файл ответа',
                    renderer: function (v) {
                        return v ? ('<a href="' + B4.Url.action('/FileUpload/Download?id=' + v.Id) + '" target="_blank" style="color: black">Скачать</a>') : '';
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'AnswerSignature',
                    width: 100,
                    text: 'Подпись (sig)',
                    renderer: function (v) {
                        return v ? ('<a href="' + B4.Url.action('/FileUpload/Download?id=' + v.Id) + '" target="_blank" style="color: black">Скачать</a>') : '';
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
                            columns: 3,
                            items: [
                                {
                                    xtype: 'b4addbutton'
                                },
                                {
                                    xtype: 'b4updatebutton'
                                },
                                {
                                    xtype: 'b4signbutton',
                                    disabled: true
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'b4pagingtoolbar',
                    displayInfo: true,
                    store: this.store,
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    }
});
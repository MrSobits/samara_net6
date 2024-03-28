Ext.define('B4.view.appealcits.AnswerGrid', {
    extend: 'B4.ux.grid.Panel',
    
    alias: 'widget.appealcitsAnswerGrid',

    requires: [
        'B4.Url',
        'B4.ux.button.Add',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.column.YesNo',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.column.Enum',
        'B4.enums.TypeAppealAnswer',
        'B4.ux.grid.toolbar.Paging',
        'B4.view.button.Sign'
    ],

    title: 'Ответы',
    store: 'appealcits.Answer',
    itemId: 'appealCitsAnswerGrid',

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
                    xtype: 'b4gridstatecolumn',
                    dataIndex: 'State',
                    text: 'Статус',
                    width: 100,
                    processEvent: function (type, view, cell, recordIndex, cellIndex, e) {
                        if (type == 'click' && e.target.localName == 'img') {
                            var record = view.getStore().getAt(recordIndex);
                            view.ownerCt.fireEvent('cellclickaction', view.ownerCt, e, 'statechange', record);
                        }
                    },
                    scope: this
                },
                {
                    xtype: 'b4enumcolumn',
                    enumName: 'B4.enums.TypeAppealAnswer',
                    dataIndex: 'TypeAppealAnswer',
                    text: 'Тип документа',
                    flex: 1,
                    filter: true,
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'DocumentName',
                    flex: 1,
                    text: 'Документ'
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DocumentDate',
                    flex: 1,
                    text: 'Дата документа',
                    format: 'd.m.Y'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'DocumentNumber',
                    flex: 1,
                    text: 'Номер документа'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Addressee',
                    flex: 1,
                    text: 'Адресат'
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
                    text: 'Загрузка в ЕАИС "Обращения граждан"',
                    width: 220,
                    columns: [
                        {
                            xtype: 'yesnocolumn',
                            dataIndex: 'IsUploaded',
                            flex: 1,
                            text: 'Загружен успешно'
                        },
                        {
                            xtype: 'actioncolumn',
                            dataIndex: 'AdditionalInfo',
                            text: 'Протокол',
                            width: 50,
                            align: 'center',
                            icon: B4.Url.content('content/img/icons/information.png'),
                            handler: function (gridView, rowIndex, colIndex, el, e, rec) {
                                var me = this,
                                    grid = me.up('grid'),
                                    content = rec.get(me.dataIndex);

                                if (content) {
                                    Ext.Msg.show({
                                        title: 'Протокол загрузки',
                                        msg: content,
                                        buttons: Ext.Msg.OK
                                    });
                                } else {
                                    Ext.Msg.alert('Ошибка', 'Отсутствует протокол загрузки');
                                }
                            }
                        },
                    ]
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
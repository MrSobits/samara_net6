Ext.define('B4.view.gisGkh.TaskGrid', {
    //extend: 'Ext.tree.Panel',
    extend: 'B4.ux.grid.Panel', 
    alias: 'widget.gisgkhtaskgrid',

    requires: [
        //'B4.view.wizard.preparedata.Wizard',
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.column.Enum',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.enums.GisGkhRequestState',
        'B4.enums.YesNo',
        'B4.enums.GisGkhTypeRequest',
    ],

    //region: 'west',
    //rootVisible: false,
    ////animate: true,
    //autoScroll: true,
    //useArrows: true,
    //loadMask: true,
    //viewConfig: {
    //    loadMask: true
    //},

    //filterByStartTime: true,
    store: 'gisGkh.TaskGridStore',
    title: 'Выполнение задач',
    closable: true,
    enableColumnHide: true,
    

    initComponent: function () {
        var me = this;
        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                //{
                //    xtype: 'b4editcolumn',
                //    scope: me
                //},
                {
                    xtype: 'datecolumn',
                    dataIndex: 'ObjectCreateDate',
                    flex: 1,
                    text: 'Дата создания запроса',
                    format: 'd.m.Y',
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq,
                        format: 'd.m.Y H:i:s'
                    }, 
                },
                //{
                //    xtype: 'gridcolumn',
                //    dataIndex: 'RequesterMessageGUID',
                //    flex: 1,
                //    text: 'RequesterMessageGUID',
                //    filter: {
                //        xtype: 'textfield',
                //    },
                //},
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'OperatorName',
                    flex: 1,
                    text: 'Оператор',
                    filter: {
                        xtype: 'textfield',
                    },
                },
                //{
                //    xtype: 'datecolumn',
                //    dataIndex: 'ReqDate',
                //    flex: 1,
                //    text: 'ReqDate',
                //    format: 'd.m.Y',
                //    filter: {
                //        xtype: 'datefield',
                //        operand: CondExpr.operands.eq,
                //        format: 'd.m.Y H:i:s'
                //    },
                //},
                {
                    xtype: 'b4enumcolumn',
                    enumName: 'B4.enums.GisGkhTypeRequest',
                    dataIndex: 'TypeRequest',
                    text: 'Тип запроса',
                    flex: 1,
                    filter: true,
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'MessageGUID',
                    flex: 1,
                    text: 'Идентификатор запроса',
                    filter: {
                        xtype: 'textfield',
                    },
                },
                {
                    xtype: 'b4enumcolumn',
                    enumName: 'B4.enums.GisGkhRequestState',
                    dataIndex: 'RequestState',
                    text: 'Статус запроса',
                    flex: 1,
                    filter: true,
                },
                //{
                //    xtype: 'b4enumcolumn',
                //    enumName: 'B4.enums.YesNo',
                //    dataIndex: 'IsExport',
                //    text: 'Экспорт из ГИС ЖКХ',
                //    flex: 1,
                //    filter: true,
                //},
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ReqFile',
                    width: 100,
                    text: 'Запрос',
                    renderer: function (v) {
                        return v ? ('<a href="' + B4.Url.action('/FileUpload/Download?id=' + v) + '" target="_blank" style="color: black">Скачать</a>') : '';
                    }
                },
                {
                    xtype: 'actioncolumn',
                    text: 'Подписать',
                    action: 'signRequest',
                    width: 150,
                    items: [{
                        tooltip: 'Подписать запрос',
                        iconCls: 'icon-fill-button',
                        icon: B4.Url.content('content/img/btnSign.png')
                    }]
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'RespFile',
                    width: 100,
                    text: 'Ответ',
                    renderer: function (v) {
                        return v ? ('<a href="' + B4.Url.action('/FileUpload/Download?id=' + v) + '" target="_blank" style="color: black">Скачать</a>') : '';
                    }
                },
                //{
                //    xtype: 'actioncolumn',
                //    text: 'Отправить',
                //    action: 'sendRequest',
                //    width: 150,
                //    items: [{
                //        tooltip: 'Отправить запрос',
                //        iconCls: 'icon-fill-button',
                //        icon: B4.Url.content('content/img/btnSend.png')
                //    }]
                //},
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Answer',
                    flex: 1,
                    text: 'Ответ ГИС ЖКХ',
                    filter: {
                        xtype: 'textfield',
                    },
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'LogFile',
                    width: 100,
                    text: 'Лог запроса',
                    renderer: function (v) {
                        return v ? ('<a href="' + B4.Url.action('/FileUpload/Download?id=' + v) + '" target="_blank" style="color: black">Скачать</a>') : '';
                    }
                }
            ],
            plugins: [
                Ext.create('B4.ux.grid.plugin.HeaderFilters'),
                //{
                //    ptype: 'filterbar',
                //    renderHidden: false,
                //    showShowHideButton: true,
                //    showClearAllButton: true,
                //    pluginId: 'headerFilter'
                //},
            ],
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
                            columns: 4,
                            items: [
                                {
                                    xtype: 'b4addbutton'
                                },
                                {
                                    xtype: 'b4updatebutton'
                                },
                                //{
                                //    xtype: 'button',
                                //    text: 'Получить справочники',
                                //    tooltip: 'Получить справочники',
                                //    iconCls: 'icon-accept',
                                //    width: 150,
                                //    itemId: 'btnGetDictionaries'
                                //},
                                {
                                    xtype: 'button',
                                    text: 'Подписать и отправить',
                                    tooltip: 'Подписать и отправить запросы',
                                    iconCls: 'icon-accept',
                                    width: 170,
                                    itemId: 'btnSignRequests'
                                },
                                {
                                    xtype: 'button',
                                    text: 'Получить ответы',
                                    tooltip: 'Получить ответы',
                                    iconCls: 'icon-accept',
                                    width: 150,
                                    itemId: 'btnCheckAnswers'
                                },
                                //{
                                //    xtype: 'button',
                                //    text: 'История запросов',
                                //    tooltip: 'Посмотреть историю запросов',
                                //    iconCls: 'icon-accept',
                                //    width: 150,
                                //    itemId: 'btnGetPaymentsHistory'
                                //}
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

Ext.define('B4.view.egsointegration.EgsoIntegrationGrid', {
    extend: 'B4.ux.grid.Panel',

    alias: 'widget.egsointegrationgrid',
    title: 'Интеграция с ЕГСО ОВ',

    closable: true,


    requires: [
        'B4.ux.grid.Panel',
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Enum',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.store.egso.EgsoIntegration',
        'B4.enums.EgsoTaskType',
        'B4.enums.EgsoTaskStateType',
        'B4.ux.grid.column.Edit',
    ],

    initComponent: function() {
        var me = this,
            store = Ext.create('B4.store.egso.EgsoIntegration');

        Ext.applyIf(me, {
            store: store,
            columnLines: true,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    icon: 'content/img/icons/magnifier.png',
                    tooltip: 'Просмотр задачи',
                    scope: me
                },
                {
                    xtype: 'b4enumcolumn',
                    dataIndex: 'TaskType',
                    flex: 1,
                    text: 'Задача',
                    enumName: 'B4.enums.EgsoTaskType',
                    filter: true
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'ObjectCreateDate',
                    flex: 1,
                    text: 'Время создания',
                    format: 'd.m.Y H:i:s',
                    width: 180,
                    filter: {
                        xtype: 'datefield',
                        format: 'd.m.Y'
                    }
                    
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'EndDate',
                    flex: 1,
                    text: 'Время выполнения',
                    format: 'd.m.Y H:i:s',
                    width: 180,
                    filter: {
                        xtype: 'datefield',
                        format: 'd.m.Y'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    text: 'Пользователь',
                    dataIndex: 'User',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'b4enumcolumn',
                    text: 'Статус',
                    dataIndex: 'StateType',
                    flex: 1,
                    enumName: 'B4.enums.EgsoTaskStateType',
                    filter: true
                },
                {
                    xtype: 'actioncolumn',
                    dataIndex: 'LogId',
                    tooltip: 'Скачать лог',
                    icon: 'content/img/icons/package_down.png',
                    handler: function (gridView, rowIndex, colIndex, el, e, rec) {
                        var param = { id: rec.get('LogId') };

                        var urlParams = Ext.urlEncode(param);

                        var newUrl = Ext.urlAppend('/FileUpload/Download?' + urlParams, '_dc=' + (new Date().getTime()));
                        newUrl = B4.Url.action(newUrl);

                        Ext.DomHelper.append(document.body, {
                            tag: 'iframe',
                            id: 'downloadIframe',
                            frameBorder: 0,
                            width: 0,
                            height: 0,
                            css: 'display:none;visibility:hidden;height:0px;',
                            src: newUrl
                        });
                    },
                    width: 25
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
                },
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
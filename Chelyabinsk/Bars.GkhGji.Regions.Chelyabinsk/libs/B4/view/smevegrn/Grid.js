Ext.define('B4.view.smevegrn.Grid', {
    extend: 'B4.ux.grid.Panel',
    
    alias: 'widget.smevegrngrid',

    requires: [
        'B4.ux.button.Add',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.enums.KindKND',
        'B4.enums.RequestState',
        'B4.enums.RequestType'
    ],

    title: 'Реестр запросов в ЕГРН',
    store: 'smev.SMEVEGRN',
  //  itemId: 'sSTUExportTaskGrid',
    closable: true,

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
                    xtype: 'gridcolumn',
                    dataIndex: 'ReqId',
                    flex: 0.5,
                    text: 'Номер запроса'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'MessageId',
                    flex: 1,
                    text: 'Номер в СМЭВ',
                    filter: {
                        xtype: 'textfield',
                    },
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'RequestType',
                    text: 'Тип запроса',
                    flex: 1,
                    renderer: function (val) {
                        return B4.enums.RequestType.displayRenderer(val);
                    }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'RequestDate',
                    flex: 0.5,
                    text: 'Дата запроса',
                    format: 'd.m.Y'
                },
               {
                   xtype: 'gridcolumn',
                   dataIndex: 'Inspector',
                   flex: 1,
                   text: 'Инспектор'
               },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Declarant',
                    flex: 1,
                    text: 'Cубъект запроса'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Municipality',
                    flex: 1,
                    text: 'Муниципальное образование',
                    filter: {
                        xtype: 'b4combobox',
                        operand: CondExpr.operands.eq,
                        storeAutoLoad: false,
                        hideLabel: true,
                        editable: false,
                        valueField: 'Name',
                        emptyItem: { Name: '-' },
                        url: '/Municipality/ListWithoutPaging'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'RealityObject',
                    flex: 1,
                    text: 'Адрес',
                    filter: {
                        xtype: 'textfield',
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Room',
                    flex: 1,
                    text: 'Помещение',
                    filter: {
                        xtype: 'textfield',
                    }
                },                
               {
                   xtype: 'gridcolumn',
                   dataIndex: 'RequestState',
                   text: 'Состояние запроса',
                   flex: 1,
                   renderer: function (val) {
                       return B4.enums.RequestState.displayRenderer(val);
                   }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Answer',
                    flex: 1,
                    text: 'Ответ СМЭВ'
                },
                 {
                     xtype: 'actioncolumn',
                     text: 'Операция',
                     action: 'openpassport',
                     width: 150,
                     items: [{
                         tooltip: 'Отправить в СМЭВ',
                         iconCls: 'icon-fill-button',
                         icon: B4.Url.content('content/img/btnSend.png')
                     }]
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
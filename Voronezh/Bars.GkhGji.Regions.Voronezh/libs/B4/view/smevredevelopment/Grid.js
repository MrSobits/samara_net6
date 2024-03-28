Ext.define('B4.view.smevredevelopment.Grid', {
    extend: 'B4.ux.grid.Panel',
    
    alias: 'widget.smevredevelopmentgrid',

    requires: [
        'B4.ux.button.Add',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.enums.RequestState'
    ],

    title: 'Реестр запросов СГИО',
    store: 'smevredevelopment.SMEVRedevelopment',
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
                    xtype: 'datecolumn',
                    dataIndex: 'CalcDate',
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
                    dataIndex: 'Municipality',
                    flex: 1,
                    text: 'Муниципальное образование',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'RealityObject',
                    flex: 1,
                    text: 'Адрес'
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
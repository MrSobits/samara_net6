Ext.define('B4.view.boilerroom.Grid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.boilerroomgrid',

    requires: [
         'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.column.Enum',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.enums.YesNo',

        'B4.model.boilerroom.BoilerRoom'
    ],

    title: 'Котельные',
    closable: true,

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.base.Store', {
                model: 'B4.model.boilerroom.BoilerRoom'
            });

        Ext.apply(me, {
            store: store,
            columns: [
            {
                xtype: 'b4editcolumn',
                scope: me
            },
            {
                xtype: 'gridcolumn',
                dataIndex: 'Municipality',
                text: 'Муниципальное образование',
                flex: 1
            },
            {
                xtype: 'gridcolumn',
                dataIndex: 'Address',
                text: 'Адрес',
                flex: 1
            }, {
                xtype: 'b4enumcolumn',
                enumName: 'B4.enums.YesNo',
                filter: true,
                dataIndex: 'IsActive',
                text: 'Активна',
                flex: 1
            }, {
                xtype: 'b4enumcolumn',
                enumName: 'B4.enums.YesNo',
                filter: true,
                dataIndex: 'IsRunning',
                text: 'Запущена',
                flex: 1
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
                    store: store,
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    }
});
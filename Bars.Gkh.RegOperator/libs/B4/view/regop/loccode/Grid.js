Ext.define('B4.view.regop.loccode.Grid', {
    extend: 'B4.ux.grid.Panel',

    requires: [
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.plugin.HeaderFilters',

        'B4.ux.button.Update',
        'B4.ux.button.Add',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.column.Delete',
        'B4.store.regop.LocationCode'
    ],

    title: 'Коды населенных пунктов',

    alias: 'widget.loccodegrid',
    store: 'regop.LocationCode',
    closable: true,

    initComponent: function () {
        var me = this;

        Ext.apply(me, {
            columns: [
                {
                    xtype: 'b4editcolumn'
                },
               {
                   text: 'Район',
                   dataIndex: 'FiasLevel1',
                   flex: 1
               },
               {
                   text: 'Муниципальное образование',
                   dataIndex: 'FiasLevel2',
                   flex: 1
               },
               {
                   text: 'Поселение',
                   dataIndex: 'FiasLevel3',
                   flex: 1
               },
               {
                   xtype: 'b4deletecolumn'
               }
            ],

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
                    store: me.store,
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
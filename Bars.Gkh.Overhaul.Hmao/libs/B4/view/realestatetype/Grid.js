Ext.define('B4.view.realestatetype.Grid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.realestatetypegrid',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',

        'B4.ux.grid.plugin.HeaderFilters',

        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',

        'B4.ux.grid.toolbar.Paging',
        'B4.store.RealEstateType'

    ],

    title: 'Типы домов',
    closable: true,

    store: 'RealEstateType',

    initComponent: function () {
        var me = this;

        Ext.apply(me, {
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Name',
                    flex: 1,
                    text: 'Наименование',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'IndCount',
                    flex: 1,
                    maxWidth: 150,
                    text: 'Количество показателей',
                    sortable: false,
                    filter: {
                        xtype: 'numberfield',
                        minValue: 0,
                        allowDecimals: false,
                        operand: CondExpr.operands.eq
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
                                { xtype: 'b4addbutton' },
                                { xtype: 'b4updatebutton' }
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
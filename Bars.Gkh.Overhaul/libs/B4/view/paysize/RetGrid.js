Ext.define('B4.view.paysize.RetGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.paysizeretgrid',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.store.Paysize'
    ],

    initComponent: function() {
        var me = this,
            store = Ext.create('B4.store.paysize.RealEstateType');

        Ext.applyIf(me, {
            store: store,
            columnLines: true,
            columns: [
                {
                    text: 'Тип дома',
                    dataIndex: 'Name',
                    flex: 1
                },
                {
                    dataIndex: 'Value',
                    text: 'Размер взноса',
                    flex: 0.5,
                    editor: {
                        xtype: 'numberfield',
                        decimalSeparator: ',',
                        minValue: 0,
                        allowDecimal: true
                    },
                    renderer: function (val) {
                        return val >= 0 ? Ext.util.Format.currency(val, null, 2) : '';
                    }
                }
            ],
            plugins: [
                Ext.create('B4.ux.grid.plugin.HeaderFilters'),
                Ext.create('Ext.grid.plugin.CellEditing', {
                    clicksToEdit: 1
                })
            ],
            viewConfig: {
                loadMask: true
            },
            dockedItems: [
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
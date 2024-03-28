Ext.define('B4.view.informationoncontracts.Grid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.infcontractgrid',
    requires: [
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit'
    ],
    store: 'InformationOnContracts',
    itemId: 'informationOnContractsGrid',

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
                    dataIndex: 'Name',
                    flex: 1,
                    text: 'Наименование',
                    filter: { xtype: 'textfield' }
                },
                {
                    dataIndex: 'AddressName',
                    flex: 1,
                    text: 'Объект недвижимости',
                    filter: { xtype: 'textfield' }
                },                
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Cost',
                    width: 120,
                    text: 'Стоимость',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    },
                    renderer: function (val) {
                        if (!Ext.isEmpty(val) && val.toString().indexOf('.') != -1) {
                            return val.toString().replace('.', ',');
                        }
                        return val;
                    }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'From',
                    text: 'Дата',
                    format: 'd.m.Y',
                    width: 100,
                    filter: {
                        xtype: 'datefield',
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
Ext.define('B4.view.fuelinfo.FuelEnergyDebtInfoGrid', {

    extend: 'B4.ux.grid.Panel',

    requires: [
        'B4.ux.button.Save',
        'B4.ux.button.Update',
        'B4.view.Control.GkhDecimalField',

        'B4.store.fuelinfo.FuelEnergyDebtInfo'
    ],

    title: 'Раздел 4. Задолженность за ранее потребленные топливно-энергетические ресурсы (ТЭР) по состоянию на конец отчетного периода',

    alias: 'widget.fuelenergydebtinfogrid',

    cls: 'x-large-head',

    initComponent: function() {
        var me = this,
            store = Ext.create('B4.store.fuelinfo.FuelEnergyDebtInfo');

        me.relayEvents(store, ['beforeload'], 'store.');

        Ext.applyIf(me, {
            store: store,
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Mark',
                    text: 'Показатель',
                    width: 300
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'RowNumber',
                    text: '№ строки',
                    renderer: function (val) { return ('0' + val).slice(-2); },
                    width: 50
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Total',
                    text: 'Всего',
                    editor: { xtype: 'gkhdecimalfield' },
                    flex: 1
                }
            ],
            plugins: [
                Ext.create('Ext.grid.plugin.CellEditing', {
                    clicksToEdit: 1,
                    pluginId: 'cellEditing'
                })
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
                            columns: 3,
                            items: [
                                {
                                    xtype: 'b4updatebutton'
                                },
                                {
                                    xtype: 'b4savebutton'
                                }
                            ]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});
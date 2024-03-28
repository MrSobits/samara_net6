Ext.define('B4.view.housingfundmonitoring.InfoGrid', {

    extend: 'B4.ux.grid.Panel',

    requires: [
        'B4.ux.button.Save',
        'B4.ux.button.Update',
        'B4.view.Control.GkhDecimalField',
        'B4.view.Control.GkhButtonPrint',

        'B4.store.housingfundmonitoring.HousingFundMonitoringInfo'
    ],

    alias: 'widget.housingfundmonitoringinfogrid',

    cls: 'x-large-head',

    initComponent: function() {
        var me = this,
            store = Ext.create('B4.store.housingfundmonitoring.HousingFundMonitoringInfo');

        me.relayEvents(store, ['beforeload'], 'store.');

        Ext.applyIf(me, {
            store: store,
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'RowNumber',
                    text: '№ строки',
                    width: 100
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Mark',
                    text: 'Наименование показателя',
                    flex: 1
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'UnitMeasure',
                    text: 'Единица измерения',
                    width: 100
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Value',
                    text: 'Значение',
                    editor: { xtype: 'gkhdecimalfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'DataProvider',
                    text: 'Поставщик информации',
                    width: 300
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
                                },
                                {
                                    xtype: 'gkhbuttonprint',
                                    name: 'PrintButton',
                                    text: 'Экспорт'
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
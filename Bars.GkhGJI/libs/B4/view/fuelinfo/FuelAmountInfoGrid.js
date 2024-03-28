Ext.define('B4.view.fuelinfo.FuelAmountInfoGrid', {

    extend: 'B4.ux.grid.Panel',

    requires: [
        'B4.ux.button.Save',
        'B4.ux.button.Update',
        'B4.view.Control.GkhDecimalField',

        'B4.store.fuelinfo.FuelAmountInfo'
    ],

    title: 'Раздел 1. Поставка, расход и остатки топлива',

    alias: 'widget.fuelamountinfogrid',

    cls: 'x-large-head',

    initComponent: function() {
        var me = this,
            store = Ext.create('B4.store.fuelinfo.FuelAmountInfo');

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
                    text: 'Уголь, т',
                    columns: [
                         {
                             xtype: 'gridcolumn',
                             dataIndex: 'CoalTotal',
                             text: 'Всего',
                             editor: { xtype: 'gkhdecimalfield' }
                         },
                         {
                             xtype: 'gridcolumn',
                             dataIndex: 'CoalPrimary',
                             text: 'в том числе основное',
                             editor: { xtype: 'gkhdecimalfield' }
                         },
                         {
                             xtype: 'gridcolumn',
                             dataIndex: 'CoalReserve',
                             text: 'в том числе резервное',
                             editor: { xtype: 'gkhdecimalfield' }
                         }
                    ]
                },
                {
                    text: 'Дрова, м3',
                    columns: [
                         {
                             xtype: 'gridcolumn',
                             dataIndex: 'FirewoodTotal',
                             text: 'Всего',
                             editor: { xtype: 'gkhdecimalfield' }
                         },
                         {
                             xtype: 'gridcolumn',
                             dataIndex: 'FirewoodPrimary',
                             text: 'в том числе основное',
                             editor: { xtype: 'gkhdecimalfield' }
                         },
                         {
                             xtype: 'gridcolumn',
                             dataIndex: 'FirewoodReserve',
                             text: 'в том числе резервное',
                             editor: { xtype: 'gkhdecimalfield' }
                         }
                    ]
                },
                {
                    text: 'Мазут (темные нефтепродукты), т',
                    columns: [
                         {
                             xtype: 'gridcolumn',
                             dataIndex: 'MasutTotal',
                             text: 'Всего',
                             editor: { xtype: 'gkhdecimalfield' }
                         },
                         {
                             xtype: 'gridcolumn',
                             dataIndex: 'MasutPrimary',
                             text: 'в том числе основное',
                             editor: { xtype: 'gkhdecimalfield' }
                         },
                         {
                             xtype: 'gridcolumn',
                             dataIndex: 'MasutReserve',
                             text: 'в том числе резервное',
                             editor: { xtype: 'gkhdecimalfield' }
                         }
                    ]
                },
                {
                    text: 'Газ, м3',
                    columns: [
                         {
                             xtype: 'gridcolumn',
                             dataIndex: 'GasTotal',
                             text: 'Всего',
                             editor: { xtype: 'gkhdecimalfield' }
                         },
                         {
                             xtype: 'gridcolumn',
                             dataIndex: 'GasPrimary',
                             text: 'в том числе основное',
                             editor: { xtype: 'gkhdecimalfield' }
                         },
                         {
                             xtype: 'gridcolumn',
                             dataIndex: 'GasReserve',
                             text: 'в том числе резервное',
                             editor: { xtype: 'gkhdecimalfield' }
                         }
                    ]
                },
                {
                    text: 'Другие виды топлива, т усл. топлива',
                    columns: [
                         {
                             xtype: 'gridcolumn',
                             dataIndex: 'OtherTotal',
                             text: 'Всего',
                             editor: { xtype: 'gkhdecimalfield' }
                         },
                         {
                             xtype: 'gridcolumn',
                             dataIndex: 'OtherPrimary',
                             text: 'в том числе основное',
                             editor: { xtype: 'gkhdecimalfield' }
                         },
                         {
                             xtype: 'gridcolumn',
                             dataIndex: 'OtherReserve',
                             text: 'в том числе резервное',
                             editor: { xtype: 'gkhdecimalfield' }
                         }
                    ]
                },
                {
                    text: 'Торф, т',
                    columns: [
                         {
                             xtype: 'gridcolumn',
                             dataIndex: 'PeatTotal',
                             text: 'Всего',
                             editor: { xtype: 'gkhdecimalfield' }
                         },
                         {
                             xtype: 'gridcolumn',
                             dataIndex: 'PeatPrimary',
                             text: 'в том числе основное',
                             editor: { xtype: 'gkhdecimalfield' }
                         },
                         {
                             xtype: 'gridcolumn',
                             dataIndex: 'PeatReserve',
                             text: 'в том числе резервное',
                             editor: { xtype: 'gkhdecimalfield' }
                         }
                    ]
                },
                {
                    text: 'Сжиженный газ, т',
                    columns: [
                         {
                             xtype: 'gridcolumn',
                             dataIndex: 'LiquefiedGasTotal',
                             text: 'Всего',
                             editor: { xtype: 'gkhdecimalfield' }
                         },
                         {
                             xtype: 'gridcolumn',
                             dataIndex: 'LiquefiedGasPrimary',
                             text: 'в том числе основное',
                             editor: { xtype: 'gkhdecimalfield' }
                         },
                         {
                             xtype: 'gridcolumn',
                             dataIndex: 'LiquefiedGasReserve',
                             text: 'в том числе резервное',
                             editor: { xtype: 'gkhdecimalfield' }
                         }
                    ]
                },
                {
                    text: 'Древесные отходы, пеллеты, т усл. топлива',
                    columns: [
                         {
                             xtype: 'gridcolumn',
                             dataIndex: 'WoodWasteTotal',
                             text: 'Всего',
                             editor: { xtype: 'gkhdecimalfield' }
                         },
                         {
                             xtype: 'gridcolumn',
                             dataIndex: 'WoodWastePrimary',
                             text: 'в том числе основное',
                             editor: { xtype: 'gkhdecimalfield' }
                         },
                         {
                             xtype: 'gridcolumn',
                             dataIndex: 'WoodWasteReserve',
                             text: 'в том числе резервное',
                             editor: { xtype: 'gkhdecimalfield' }
                         }
                    ]
                },
                {
                    text: 'Дизельное топливо, т',
                    columns: [
                         {
                             xtype: 'gridcolumn',
                             dataIndex: 'DieselTotal',
                             text: 'Всего',
                             editor: { xtype: 'gkhdecimalfield' }
                         },
                         {
                             xtype: 'gridcolumn',
                             dataIndex: 'DieselPrimary',
                             text: 'в том числе основное',
                             editor: { xtype: 'gkhdecimalfield' }
                         },
                         {
                             xtype: 'gridcolumn',
                             dataIndex: 'DieselReserve',
                             text: 'в том числе резервное',
                             editor: { xtype: 'gkhdecimalfield' }
                         }
                    ]
                },
                {
                    text: 'Эл. энергия, тыс. квт',
                    columns: [
                         {
                             xtype: 'gridcolumn',
                             dataIndex: 'ElectricTotal',
                             text: 'Всего',
                             editor: { xtype: 'gkhdecimalfield' }
                         },
                         {
                             xtype: 'gridcolumn',
                             dataIndex: 'ElectricPrimary',
                             text: 'в том числе основное',
                             editor: { xtype: 'gkhdecimalfield' }
                         },
                         {
                             xtype: 'gridcolumn',
                             dataIndex: 'ElectricReserve',
                             text: 'в том числе резервное',
                             editor: { xtype: 'gkhdecimalfield' }
                         }
                    ]
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
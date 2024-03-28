Ext.define('B4.view.fuelinfo.FuelContractObligationInfoGrid', {

    extend: 'B4.ux.grid.Panel',

    requires: [
        'B4.ux.button.Save',
        'B4.ux.button.Update',
        'B4.view.Control.GkhDecimalField',

        'B4.store.fuelinfo.FuelContractObligationInfo'
    ],

    title: 'Раздел 3. Выполнение договорных обязательств по поставкам топлива',

    alias: 'widget.fuelcontractobligationinfogrid',

    cls: 'x-large-head',

    initComponent: function() {
        var me = this,
            store = Ext.create('B4.store.fuelinfo.FuelContractObligationInfo');

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
                             dataIndex: 'CoalDirectContract',
                             text: 'в том числе по прямым договорам',
                             editor: { xtype: 'gkhdecimalfield' }
                         },
                         {
                             xtype: 'gridcolumn',
                             dataIndex: 'CoalIntermediator',
                             text: 'в том числе через посредников',
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
                             dataIndex: 'FirewoodDirectContract',
                             text: 'в том числе по прямым договорам',
                             editor: { xtype: 'gkhdecimalfield' }
                         },
                         {
                             xtype: 'gridcolumn',
                             dataIndex: 'FirewoodIntermediator',
                             text: 'в том числе через посредников',
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
                             dataIndex: 'MasutDirectContract',
                             text: 'в том числе по прямым договорам',
                             editor: { xtype: 'gkhdecimalfield' }
                         },
                         {
                             xtype: 'gridcolumn',
                             dataIndex: 'MasutIntermediator',
                             text: 'в том числе через посредников',
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
                             dataIndex: 'GasDirectContract',
                             text: 'в том числе по прямым договорам',
                             editor: { xtype: 'gkhdecimalfield' }
                         },
                         {
                             xtype: 'gridcolumn',
                             dataIndex: 'GasIntermediator',
                             text: 'в том числе через посредников',
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
                             dataIndex: 'OtherDirectContract',
                             text: 'в том числе по прямым договорам',
                             editor: { xtype: 'gkhdecimalfield' }
                         },
                         {
                             xtype: 'gridcolumn',
                             dataIndex: 'OtherIntermediator',
                             text: 'в том числе через посредников',
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
                             dataIndex: 'PeatDirectContract',
                             text: 'в том числе по прямым договорам',
                             editor: { xtype: 'gkhdecimalfield' }
                         },
                         {
                             xtype: 'gridcolumn',
                             dataIndex: 'PeatIntermediator',
                             text: 'в том числе через посредников',
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
                             dataIndex: 'LiquefiedGasDirectContract',
                             text: 'в том числе по прямым договорам',
                             editor: { xtype: 'gkhdecimalfield' }
                         },
                         {
                             xtype: 'gridcolumn',
                             dataIndex: 'LiquefiedGasIntermediator',
                             text: 'в том числе через посредников',
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
                             dataIndex: 'WoodWasteDirectContract',
                             text: 'в том числе по прямым договорам',
                             editor: { xtype: 'gkhdecimalfield' }
                         },
                         {
                             xtype: 'gridcolumn',
                             dataIndex: 'WoodWasteIntermediator',
                             text: 'в том числе через посредников',
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
                             dataIndex: 'DieselDirectContract',
                             text: 'в том числе по прямым договорам',
                             editor: { xtype: 'gkhdecimalfield' }
                         },
                         {
                             xtype: 'gridcolumn',
                             dataIndex: 'DieselIntermediator',
                             text: 'в том числе через посредников',
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
                             dataIndex: 'ElectricDirectContract',
                             text: 'в том числе по прямым договорам',
                             editor: { xtype: 'gkhdecimalfield' }
                         },
                         {
                             xtype: 'gridcolumn',
                             dataIndex: 'ElectricIntermediator',
                             text: 'в том числе через посредников',
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
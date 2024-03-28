Ext.define('B4.view.claimwork.lawsuit.LawsuitReferenceCalculationGrid', {
    extend: 'B4.ux.grid.Panel',

    alias: 'widget.lawsuitreferencecalculationgrid',

    requires: [

        'B4.ux.button.Update',
        'B4.ux.button.Save',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.form.field.AreaShareField',
        'B4.store.LawsuitReferenceCalculation'
    ],

    title: 'Эталонные оплаты',
    closable: false,
    enableColumnHide: true,

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.LawsuitReferenceCalculation');

        Ext.applyIf(me, {
            store: store,
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    name: 'Name',
                    dataIndex: 'Name',
                    text: 'Период',
                    flex: 1
                },
                {
                    xtype: 'gridcolumn',
                    name: 'AreaShare',
                    dataIndex: 'AreaShare',
                    text: 'Доля собственности',
                    flex: 0.5,
                    editor: {
                        xtype: 'numberfield',
                        maxLength: 10
                    }
                },
                {
                    xtype: 'gridcolumn',
                    name: 'RoomArea',
                    dataIndex: 'RoomArea',
                    text: 'Площадь помещения',
                    flex: 0.5,
                    editor: {
                        xtype: 'numberfield',
                        maxLength: 10
                    }
                },
                {
                    xtype: 'gridcolumn',
                    name: 'BaseTariff',
                    dataIndex: 'BaseTariff',
                    text: 'Тариф',
                    flex: 0.5,
                    editor: {
                        xtype: 'numberfield',
                        maxLength: 10
                    }
                },
                {
                    xtype: 'gridcolumn',
                    name: 'TariffCharged',
                    dataIndex: 'TariffCharged',
                    text: 'Начислено за период',
                    flex: 0.5,
                    editor: {
                        xtype: 'numberfield',
                        maxLength: 10
                    }
                },
                {
                    xtype: 'gridcolumn',
                    name: 'TarifPayment',
                    dataIndex: 'TarifPayment',
                    text: 'Оплачено за период',
                    flex: 0.5,
                    editor: {
                        xtype: 'numberfield',
                        maxLength: 10
                    }
                },
                {
                    xtype: 'gridcolumn',
                    name: 'PaymentDate',
                    dataIndex: 'PaymentDate',
                    text: 'Дата платежа',
                    flex: 0.5
                },
                {
                    xtype: 'gridcolumn',
                    name: 'TarifDebt',
                    dataIndex: 'TarifDebt',
                    text: 'Задолженность за период',
                    flex: 0.5,
                    editor: {
                        xtype: 'numberfield',
                        maxLength: 10
                    }
                },
                {
                    xtype: 'gridcolumn',
                    name: 'Description',
                    dataIndex: 'Description',
                    text: 'Описание',
                    flex: 0.5
                }
            ],

            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            items: [

                                { xtype: 'b4updatebutton' },
                                { xtype: 'b4savebutton' }

                            ],
                        }
                    ]
                },
                {
                    xtype: 'b4pagingtoolbar',
                    displayInfo: true,
                    store: store,
                    dock: 'bottom'
                }
            ],
            plugins: [
                Ext.create('B4.ux.grid.plugin.HeaderFilters'),
                Ext.create('Ext.grid.plugin.CellEditing',
                    {
                        clicksToEdit: 1,
                        pluginId: 'cellEditing'
                    })
            ],
            viewConfig: {
                loadMask: true
            }

        });

        me.callParent(arguments);
    }
});


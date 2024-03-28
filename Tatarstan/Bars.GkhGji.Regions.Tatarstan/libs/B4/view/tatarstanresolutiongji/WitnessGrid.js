Ext.define('B4.view.tatarstanresolutiongji.WitnessGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.column.Enum',
        'B4.form.EnumCombo',
        'B4.enums.WitnessType',
        'B4.store.tatarstanprotocolgji.TatarstanProtocolGjiEyewitness'
    ],

    title: 'Сведения о свидетелях и потерпевших',
    alias: 'widget.tatarstanresolutiongjieyewitnessgrid',

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.tatarstanprotocolgji.TatarstanProtocolGjiEyewitness');

        me.relayEvents(store, ['beforeload'], 'eyewitnessstore.');

        Ext.applyIf(me, {
            columnLines: true,
            store: store,
            columns: [
                {
                    xtype: 'b4enumcolumn',
                    dataIndex: 'WitnessType',
                    text: 'Факт правонарушения удостоверяет',
                    enumName: 'B4.enums.WitnessType',
                    flex: 1,
                    filter: true,
                    editor: {
                        xtype: 'b4enumcombo',
                        enumName: 'B4.enums.WitnessType'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Fio',
                    flex: 1,
                    filter: { xtype: 'textfield' },
                    text: 'ФИО',
                    editor: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'FactAddress',
                    flex: 1,
                    filter: { xtype: 'textfield' },
                    text: 'Фактический адрес проживания',
                    editor: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Phone',
                    flex: 1,
                    filter: { xtype: 'textfield' },
                    text: 'Телефон',
                    editor: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
                }
            ],
            plugins: [
                Ext.create('B4.ux.grid.plugin.HeaderFilters'),
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
                                    xtype: 'b4addbutton'
                                },
                                {
                                    xtype: 'b4updatebutton'
                                },
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-accept',
                                    name: 'btnSave',
                                    text: 'Сохранить'
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'b4pagingtoolbar',
                    dock: 'bottom',
                    displayInfo: true,
                    store: store
                }
            ]
        });

        me.callParent(arguments);
    }
});
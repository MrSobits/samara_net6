﻿Ext.define('B4.view.actisolated.WitnessGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.actisolatedwitnessgrid',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.plugin.HeaderFilters'
    ],

    title: 'Лица, присутствующие при проверке',
    store: 'actisolated.Witness',

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Fio',
                    flex: 1,
                    text: 'ФИО',
                    editor: {
                        xtype: 'textfield',
                        maxLength: 300
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Position',
                    flex: 1,
                    text: 'Должность',
                    editor: {
                        xtype: 'textfield',
                        maxLength: 300
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'IsFamiliar',
                    width: 120,
                    text: 'С актом ознакомлен',
                    renderer: function (val) {
                        return val ? 'Да' : 'Нет';
                    },
                    editor: 'checkboxfield'
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
                            items: [
                                {
                                    xtype: 'b4addbutton'
                                },
                                {
                                    xtype: 'b4updatebutton'
                                },
                                {
                                    xtype: 'button',
                                    itemId: 'actIsolatedWitnessSaveButton',
                                    iconCls: 'icon-accept',
                                    text: 'Сохранить'
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
﻿Ext.define('B4.view.complaints.DecisionDictGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Edit',
        'B4.ux.button.Save',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.enums.CompleteReject',
        'B4.ux.grid.toolbar.Paging',
         'B4.form.ComboBox',
    ],
    alias: 'widget.complaintsdecdictgrid',
    title: 'Решения по жалобам',
    store: 'complaints.Decision',

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
                    dataIndex: 'Code',
                    flex: 1,
                    text: 'Код',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Name',
                    flex: 1,
                    text: 'Наименование',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'FullName',
                    flex: 1,
                    text: 'Значение в ТОРе',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                   {
                       xtype: 'gridcolumn',
                       dataIndex: 'CompleteReject',
                       flex: 0.5,
                       text: 'Тип',
                       renderer: function (val) {
                           return B4.enums.CompleteReject.displayRenderer(val);
                       },
                   },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
                }
            ],
            plugins: [
                Ext.create('B4.ux.grid.plugin.HeaderFilters'),
                   //Ext.create('Ext.grid.plugin.CellEditing', {
                   //    clicksToEdit: 1,
                   //    pluginId: 'cellEditing'
                   //})
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
                            columns: 2,
                            items: [
                                {
                                    xtype: 'b4addbutton'
                                },
                                {
                                    xtype: 'b4updatebutton'
                                }
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
﻿Ext.define('B4.view.dict.statsubsubjectgji.Grid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging'
    ],

    title: 'Подтематики',
    store: 'dict.StatSubsubjectGji',
    alias: 'widget.statSubsubjectGjiGrid',
    closable: true,

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
                    width: 100,
                    text: 'Код'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Name',
                    text: 'Наименование',
                    flex: 3
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'NeedInSopr',
                    flex: 1,
                    text: 'Учитывается в СОПР',
                    renderer: function (val) {
                        return val ? "Да" : "Нет";
                    },
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
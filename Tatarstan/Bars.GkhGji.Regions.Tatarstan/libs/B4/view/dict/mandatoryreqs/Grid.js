Ext.define('B4.view.dict.mandatoryreqs.Grid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',

        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',

        'B4.ux.grid.plugin.HeaderFilters',

        'B4.ux.grid.toolbar.Paging',
        'B4.TextValuesOverride'
    ],

    title: 'Обязательные требования',
    store: 'dict.MandatoryReqs',
    alias: 'widget.mandatoryreqsgrid',
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
                    dataIndex: 'MandratoryReqName',
                    flex: 1,
                    text: 'Наименование требования',
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'MandratoryReqContent',
                    flex: 1,
                    text: 'Содержание требования',
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'NpaFullName',
                    flex: 1,
                    text: 'Нормативно-правовой документ',
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
                                    name: 'sendToTorButton',
                                    iconCls: 'icon-table-go',
                                    text: 'Отправить в ТОР КНД',
                                    textAlign: 'left',
                                },
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
    },
});
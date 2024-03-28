Ext.define('B4.view.protocolgji.ViolationGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.column.Delete',
        'B4.ux.button.Add',
        'B4.ux.grid.toolbar.Paging'
    ],

    alias: 'widget.protocolgjiViolationGrid',
    store: 'protocolgji.Violation',
    itemId: 'protocolgjiViolationGrid',

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ViolationGji',
                    flex: 1,
                    text: 'Текст нарушения',
                    filter: { xtype: 'textfield' },
                    renderer: function(val, metaData) {
                        metaData.tdAttr = 'data-qtip="' + val + '"';
                        return val;
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'InspectionDescription',
                    flex: 1,
                    text: 'Подробнее',
                    filter: { xtype: 'textfield' },
                    renderer: function (val, metaData) {
                        metaData.tdAttr = 'data-qtip="' + val + '"';
                        return val;
                    }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DatePlanRemoval',
                    text: 'Срок устранения',
                    format: 'd.m.Y',
                    width: 100,
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq
                    },
                    itemId: 'cdfDatePlanRemoval'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Description',
                    text: 'Примечание',
                    flex: 1,
                    filter: { xtype: 'textfield' },
                    editor: {
                        xtype: 'textfield',
                        maxLength: 500
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
                            items: [
                                { xtype: 'b4addbutton' },
                                {
                                    xtype: 'button',
                                    itemId: 'protocolViolationSaveButton',
                                    iconCls: 'icon-accept',
                                    text: 'Сохранить'
                                },
                                {
                                    xtype: 'button',
                                    itemId: 'updateButton',
                                    iconCls: 'icon-arrow-refresh',
                                    text: 'Обновить'
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
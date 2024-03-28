Ext.define('B4.view.specaccowner.SpecialAccountOwnerGrid', {
    extend: 'B4.ux.grid.Panel',
    
    alias: 'widget.specaccownergrid',

    requires: [
        'B4.ux.button.Add',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.enums.OrgStateRole',
        'B4.enums.GroundsTermination'
    ],

    title: 'Реестр владельцев спецсчетов',
    store: 'specaccowner.SpecialAccountOwner',
  //  itemId: 'sSTUExportTaskGrid',
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
                    dataIndex: 'Contragent',
                    flex: 2,
                    text: 'Владелец спецсчета',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Inn',
                    flex: 1,
                    text: 'ИНН',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'OrgStateRole',
                    text: 'Статус организации',
                    flex: 1,
                    renderer: function (val) {
                        return B4.enums.OrgStateRole.displayRenderer(val);
                    },
                    filter: {
                        xtype: 'combobox',
                        store: B4.enums.OrgStateRole.getItemsWithEmpty([null, '-']),
                        operand: CondExpr.operands.eq,
                        editable: false
                    }
                },
                 {
                     xtype: 'gridcolumn',
                     dataIndex: 'ActivityGroundsTermination',
                     text: 'Основание прекращения деятельности',
                     flex: 1,
                     renderer: function (val) {
                         return B4.enums.GroundsTermination.displayRenderer(val);
                     },
                     filter: {
                         xtype: 'combobox',
                         store: B4.enums.GroundsTermination.getItemsWithEmpty([null, '-']),
                         operand: CondExpr.operands.eq,
                         editable: false
                     }
                 },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'ActivityDateEnd',
                    flex: 0.5,
                    text: 'Дата прекращения деятельности',
                    format: 'd.m.Y'
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
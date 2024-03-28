Ext.define('B4.view.publicservorg.ContractServiceGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.publicservorgcontractservicegrid',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'Ext.grid.DateColumn',

        'B4.enums.SchemeConnectionType',
        'B4.enums.HeatingSystemType'
    ],
    title: 'Предоставляемые услуги',
   
    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.publicservorg.RealObjPublicServiceOrgService');

        Ext.applyIf(me, {
            store: store,
            columnLines: true,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Service',
                    flex: 2,
                    text: 'Услуга',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'CommunalResource',
                    flex: 1,
                    text: 'Коммунальный ресурс',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'HeatingSystemType',
                    flex: 1,
                    text: 'Тип системы теплоснабжения',
                    renderer: function (val) { return B4.enums.HeatingSystemType.displayRenderer(val); },
                    filter: {
                        xtype: 'b4combobox',
                        items: B4.enums.HeatingSystemType.getItemsWithEmpty([null, '-']),
                        editable: false,
                        operand: CondExpr.operands.eq,
                        valueField: 'Value',
                        displayField: 'Display'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'SchemeConnectionType',
                    flex: 1,
                    text: 'Схема присоединения',
                    renderer: function (val) { return B4.enums.SchemeConnectionType.displayRenderer(val); },
                    filter: {
                        xtype: 'b4combobox',
                        items: B4.enums.SchemeConnectionType.getItemsWithEmpty([null, '-']),
                        editable: false,
                        operand: CondExpr.operands.eq,
                        valueField: 'Value',
                        displayField: 'Display'
                    }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'StartDate',
                    format: 'd.m.Y',
                    flex: 1,
                    text: 'Дата начала предоставления',
                    filter: { xtype: 'datefield' }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'EndDate',
                    format: 'd.m.Y',
                    flex: 1,
                    text: 'Дата окончания предоставления',
                    filter: { xtype: 'datefield' }
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
                            columns: 3,
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
                    store: store,
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    }
});

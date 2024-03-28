Ext.define('B4.view.manorg.RegistryGrid', {
    extend: 'B4.ux.grid.Panel',

    alias: 'widget.manorgreggrid',

    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',

        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.column.Enum',

        'B4.enums.TsjInfoType',

        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.plugin.HeaderFilters'
    ],
    title: 'Реестры',
    store: 'manorg.ManagingOrgRegistry',
    closable: true,

    initComponent: function () {
        var me = this;

        Ext.apply(me, {

            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'datecolumn',
                    text: 'Дата предоставления сведений',
                    flex: 1,
                    format: 'd.m.Y',
                    dataIndex: 'InfoDate',
                    filter: {
                        xtype: 'datefield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'b4enumcolumn',
                    filter: true,
                    flex: 1,
                    text: 'Тип сведений',
                    dataIndex: 'InfoType',
                    enumName: 'B4.enums.TsjInfoType'
                },
                {
                    xtype: 'gridcolumn',
                    text: 'Регистрационный номер',
                    flex: 1,
                    dataIndex: 'RegNumber',
                    filter: {
                        xtype: 'textfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'gridcolumn',
                    text: 'Дата внесения записи в ЕГРЮЛ',
                    flex: 1,
                    dataIndex: 'EgrulDate',
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'gridcolumn',
                    text: 'Документ',
                    flex: 1,
                    dataIndex: 'Doc'
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
                }
            ],

            viewConfig: {
                loadMask: true
            },
            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],

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
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'b4pagingtoolbar',
                    displayInfo: true,
                    store: me.store,
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    }
});
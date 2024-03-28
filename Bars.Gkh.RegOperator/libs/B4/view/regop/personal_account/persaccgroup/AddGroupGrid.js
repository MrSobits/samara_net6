Ext.define('B4.view.regop.personal_account.persaccgroup.AddGroupGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.paccountaddgroupgrid',

    requires: [
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.selection.CheckboxModel',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        
        'B4.form.SelectField',
        'B4.store.dict.PersAccGroup',
        'B4.ux.grid.filter.YesNo',
        'B4.enums.YesNo'
    ],

    initComponent: function() {
        var me = this;
        me.store = Ext.create('B4.store.dict.PersAccGroup');

        Ext.applyIf(me, {
            columnLines: true,
            selModel: Ext.create('B4.ux.grid.selection.CheckboxModel', {}),
            
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Name',
                    flex: 1,
                    text: 'Наименование группы',
                    editor: {
                        xtype: 'textfield',
                        maxLength: 100
                    },
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'IsSystem',
                    hidden: true,
                    text: 'Системная',
                    editor: {
                        xtype: 'textfield',
                        maxLength: 100
                    },
                    filter: {
                        xtype: 'b4combobox',
                        items: B4.enums.YesNo.getItemsWithEmpty([null, '-']),
                        editable: false,
                        operand: CondExpr.operands.eq,
                        valueField: 'Value',
                        displayField: 'Display'
                    }
                }
            ],
            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
            viewConfig: {
                loadMask: true
            },
            dockedItems: [
                {
                    xtype: 'b4pagingtoolbar',
                    displayInfo: true,
                    store: me.store,
                    dock: 'bottom'
                },
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            items: [
                                {
                                    xtype: 'button',
                                    itemId: 'btnAddToGroup',
                                    text: 'Включить в группу',
                                    iconCls: 'icon-accept'
                                },
                                {
                                    xtype: 'b4updatebutton',
                                    itemId: 'btnUpdateGroups',
                                    text: 'Обновить'
                                },
                                {
                                    xtype: 'b4addbutton',
                                    itemId: 'btnAddNewGroups',
                                    text: 'Добавить новую группу'
                                }
                            ]
                        },
                        { xtype: 'tbfill' },
                        {
                            xtype: 'buttongroup',
                            items: [{ xtype: 'b4closebutton' }]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});


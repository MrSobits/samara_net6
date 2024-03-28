Ext.define('B4.view.administration.operator.Grid', {
    extend: 'B4.ux.grid.Panel',
    
    alias: 'widget.admoperatorgrid',

    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.form.ComboBox',
        'B4.store.Role'
    ],

    store: 'administration.Operator',
    itemId: 'operatorGrid',

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
                    dataIndex: 'Municipality',
                    flex: 1,
                    text: 'Муниципальное образование',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Login',
                    flex: 1,
                    text: 'Логин',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Name',
                    flex: 1,
                    text: 'Имя пользователя',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ContragentName',
                    flex: 1,
                    text: 'Организация',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Role',
                    flex: 1,
                    text: 'Роль',
                    filter: {
                        xtype: 'b4combobox',
                        hideLabel: true,
                        editable: false,
                        emptyItem: { Name: '-' },
                        valueField: 'Name',
                        displayField: 'Name',
                        operand: CondExpr.operands.eq,
                        url: '/Role/RoleList',
                        queryMode: 'local',
                        triggerAction: 'all'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Phone',
                    flex: 1,
                    text: 'Телефон',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Inn',
                    flex: 1,
                    text: 'ИНН организации',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'booleancolumn',
                    falseText: '',
                    trueText: 'Да',
                    dataIndex: 'IsActive',
                    flex: 1,
                    text: 'Пользователь активен',
                    filter:
                    {
                        xtype: 'b4combobox',
                        items: [[null, '-'], [false, 'Нет'], [true, 'Да']],
                        editable: false,
                        operand: CondExpr.operands.eq,
                        valueField: 'Value',
                        displayField: 'Display'
                    }
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
Ext.define('B4.view.import.chesimport.IndAccountOwnerGrid', {
    extend: 'B4.ux.grid.Panel',

    requires: [
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.button.Update',

        'B4.form.ComboBox',
        'B4.form.GridStateColumn',

        'B4.enums.TypeHouse'
    ],

    alias: 'widget.chesimportindaccountownergrid',
    columnLines: true,
    title: 'Адреса в системе',

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.regop.owner.IndividualAccountOwner');

        Ext.applyIf(me, {
            store: store,
            selModel: Ext.create('Ext.selection.CheckboxModel', { mode: 'SINGLE' }),
            columns: [
                {
                    text: 'ФИО',
                    dataIndex: 'Name',
                    flex: 2,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'datecolumn',
                    text: 'Дата рождения',
                    dataIndex: 'BirthDate',
                    format: 'd.m.Y',
                    flex: 1,
                    filter: { xtype: 'datefield' }
                },
                {
                    text: 'Количество ЛС',
                    dataIndex: 'TotalAccountsCount',
                    width: 100,
                    filter: {
                        xtype: 'numberfield',
                        operand: CondExpr.operands.eq
                    }
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            items: [
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
                    view: me,
                    dock: 'bottom'
                }
            ],
            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
            viewConfig: {
                loadMask: true
            }
        });

        me.callParent(arguments);
    }
});

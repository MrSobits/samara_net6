Ext.define('B4.view.import.chesimport.LegalAccountOwnerGrid', {
    extend: 'B4.ux.grid.Panel',

    requires: [
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.button.Update',

        'B4.form.ComboBox',
        'B4.form.GridStateColumn',

        'B4.enums.TypeHouse'
    ],

    alias: 'widget.chesimportlegalaccountownergrid',
    columnLines: true,
    title: 'Адреса в системе',

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.regop.owner.LegalAccountOwner');

        Ext.applyIf(me, {
            store: store,
            selModel: Ext.create('Ext.selection.CheckboxModel', { mode: 'SINGLE' }),
            columns: [
                {
                    text: 'Наименование ЮЛ',
                    dataIndex: 'Name',
                    flex: 3,
                    filter: { xtype: 'textfield' }
                },
                {
                    text: 'ИНН',
                    dataIndex: 'Inn',
                    flex: 1,
                    filter: {
                        xtype: 'textfield',
                        filterName: 'Contragent.Inn'
                    }
                },
                {
                    text: 'КПП',
                    dataIndex: 'Kpp',
                    flex: 1,
                    filter: {
                        xtype: 'textfield',
                        filterName: 'Contragent.Kpp'
                    }
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

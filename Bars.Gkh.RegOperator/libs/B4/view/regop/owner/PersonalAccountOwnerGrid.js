Ext.define('B4.view.regop.owner.PersonalAccountOwnerGrid', {
    extend: 'B4.ux.grid.Panel',

    requires: [
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.plugin.HeaderFilters',

        'B4.form.ComboBox',

        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.button.Import',

        'B4.store.regop.owner.PersonalAccountOwner',

        'B4.enums.regop.PersonalAccountOwnerType'
    ],

    title: 'Реестр абонентов',

    alias: 'widget.paownergrid',
    store: 'regop.owner.PersonalAccountOwner',
    closable: true,

    enableColumnHide : true,

    initComponent: function () {
        var me = this;

        Ext.apply(me, {
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    text: 'Тип абонента',
                    dataIndex: 'OwnerType',
                    flex: 1,
                    renderer: function(val) {
                        switch (val) {
                            case 0:
                                return "Физическое лицо";
                            case 1:
                                return "Юридическое лицо";
                            default:
                                return "";
                        }
                    },
                    filter: {
                        xtype: 'b4combobox',
                        items: B4.enums.regop.PersonalAccountOwnerType.getItemsWithEmpty([null, '-']),
                        editable: false,
                        operand: CondExpr.operands.eq,
                        valueField: 'Value',
                        displayField: 'Display'
                    }
                },
                {
                    text: 'ФИО/Наименование ЮЛ',
                    dataIndex: 'Name',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    text: 'Количество ЛС',
                    dataIndex: 'AccountsCount',
                    flex: 1,
                    filter: {
                        xtype: 'numberfield',
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    text: 'Фактический адрес',
                    dataIndex: 'FiasFactAddress',
                    hidden: true,
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    itemId: 'toptoolbar',
                    items: [
                        {
                            xtype: 'buttongroup',
                            
                            columns: 4,
                            items: [
                                {
                                    xtype: 'b4updatebutton'
                                },
                                {
                                    xtype: 'b4addbutton',
                                    text: 'Добавить'
                                },
                                {
                                    xtype: 'b4importbutton',
                                    text: 'Импорт'
                                },
                                {
                                    xtype: 'button',
                                    text: 'Перейти ко всем абонентам',
                                    iconCls: 'icon-accept',
                                    itemId: 'btnClearAllFilters'
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
            ],
            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
            viewConfig: {
                loadMask: true
            }
        });

        me.callParent(arguments);
    }
});
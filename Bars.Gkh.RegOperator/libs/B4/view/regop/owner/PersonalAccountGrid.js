Ext.define('B4.view.regop.owner.PersonalAccountGrid', {
    extend: 'B4.ux.grid.Panel',

    requires: [
        'B4.grid.feature.Summary',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.form.ComboBox',
        'B4.ux.button.Add',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.form.GridStateColumn',
        'B4.store.regop.personal_account.BasePersonalAccount'
    ],

    title: 'Сведения о помещениях',

    alias: 'widget.paowneraccountgrid',

    enableColumnHide: true,

    features: [{
        ftype: 'b4_summary'
    }],

    initComponent: function() {
        var me = this,
            store = Ext.create('B4.store.regop.personal_account.BasePersonalAccount', { autoLoad: false, summary: true });

        Ext.applyIf(me, {
            store: store,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'b4gridstatecolumn',
                    dataIndex: 'State',
                    minWidth: 130,
                    maxWidth: 140,
                    menuText: 'Статус',
                    text: 'Статус',
                    filter: {
                        xtype: 'b4combobox',
                        url: '/State/GetListByType',
                        storeAutoLoad: false,
                        operand: CondExpr.operands.eq,
                        listeners: {
                            storebeforeload: function (field, store, options) {
                                options.params.typeId = 'gkh_regop_personal_account';
                            },
                            storeloaded: {
                                fn: function (field) {
                                    field.getStore().insert(0, { Id: null, Name: '-' });
                                }
                            }
                        }
                    },
                    sortable: false,
                    scope: this
                },
                {
                    text: 'Номер лицевого счета',
                    tooltoip: 'Номер лицевого счета',
                    dataIndex: 'PersonalAccountNum',
                    flex: 1,
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    text: 'Адрес',
                    dataIndex: 'RoomAddress',
                    flex: 1,
                    filter:
                    {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    text: 'Площадь',
                    tooltip: 'Площадь',
                    dataIndex: 'RealArea',
                    filter: {
                        xtype: 'numberfield',
                        operand: CondExpr.operands.eq,
                        hideTrigger: true
                    },
                    summaryType: 'sum'
                },
                {
                    text: 'Доля собственности',
                    tooltip: 'Доля собственности',
                    dataIndex: 'AreaShare',
                    hidden: true,
                    flex: 1,
                    filter: {
                        xtype: 'numberfield',
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    text: 'Размер взноса',
                    hidden: true,
                    flex: 1,
                    sortable: false,
                    dataIndex: 'Tariff',
                    filter: {
                        xtype: 'numberfield',
                        operand: CondExpr.operands.eq
                    }
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
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            items: [
                                {
                                    xtype: 'b4addbutton',
                                    text: 'Добавить'
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
            ],
            plugins: [
                Ext.create('B4.ux.grid.plugin.HeaderFilters')
            ],
            viewConfig: {
                loadMask: true
            }

        });
        me.callParent(arguments);

    }
});
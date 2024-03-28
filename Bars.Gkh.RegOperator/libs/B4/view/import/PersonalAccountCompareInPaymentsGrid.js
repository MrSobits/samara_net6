/**
* Форма ручного сопоставления записи из журнала предупреждений при импортах оплаты в закрытый период
*/
Ext.define('B4.view.import.PersonalAccountCompareInPaymentsGrid', {
    extend: 'B4.form.Window',
    alias: 'widget.compareinpaymentsgrid',
    itemId: 'compareinpaymentsgrid',
    title: 'Лицевые счета для сопоставления',
    trackResetOnLoad: true,
    modal: true,
    width: 800,
    minWidth: 800,
    minHeight: 500,
    height: 500,
    layout: {
        type: 'vbox',
        align: 'stretch'
    },

    mixins: ['B4.mixins.window.ModalMask'],

    requires: [
        'B4.enums.regop.PersonalAccountOwnerType',
        'B4.form.GridStateColumn',
        'Ext.ux.CheckColumn',
        'B4.ux.grid.toolbar.Paging',
        'B4.store.regop.personal_account.BasePersonalAccount'
    ],

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.regop.personal_account.BasePersonalAccount');

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'gridpanel',
                    cls: 'x-large-head',
                    store: store,
                    flex: 1,
                    plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
                    selModel: Ext.create('Ext.selection.CheckboxModel', {
                        mode: 'SINGLE'
                    }),
                    columns: [
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'PersonalAccountNum',
                            text: 'Номер',
                            width: 70,
                            filter: { xtype: 'textfield' }
                        },
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'RoomAddress',
                            text: 'Адрес',
                            flex: 1,
                            filter: { xtype: 'textfield' }
                        },
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'AccountOwner',
                            text: 'ФИО/Наименование абонента',
                            flex: 1,
                            filter: { xtype: 'textfield' }
                        },
                        {
                            text: 'Тип абонента',
                            dataIndex: 'OwnerType',
                            minWidth: 100,
                            maxWidth: 110,
                            filter: {
                                xtype: 'b4combobox',
                                items: B4.enums.regop.PersonalAccountOwnerType.getItemsWithEmpty([null, '-']),
                                editable: false,
                                operand: CondExpr.operands.eq,
                                valueField: 'Value',
                                displayField: 'Display'
                            },
                            renderer: function (val) { return B4.enums.regop.PersonalAccountOwnerType.displayRenderer(val); }
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
                            processEvent: function (type, view, cell, recordIndex, cellIndex, e) {
                                if (type == 'click' && e.target.localName == 'img') {
                                    var record = view.getStore().getAt(recordIndex);
                                    view.ownerCt.fireEvent('cellclickaction', view.ownerCt, e, 'statechange', record);
                                }
                            },
                            scope: this
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
                                            xtype: 'button',
                                            text: 'Подтвердить',
                                            iconCls: 'x-btn-icon icon-accept',
                                            textAlign: 'left',
                                            action: 'Accept',
                                            disabled: true // Разблокируется когда будет выбрана запись
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
                }
            ]
        });

        me.callParent(arguments);
    }
});
﻿Ext.define('B4.view.builder.AddWindow', {
    extend: 'B4.form.Window',

    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: { type: 'vbox', align: 'stretch' },
    width: 500,
    minWidth: 500,
    minHeight: 200,
    bodyPadding: 5,
    itemId: 'builderAddWindow',
    title: 'Подрядная организация',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.form.SelectField',
        'B4.form.ComboBox',
        'B4.store.Contragent',
        'B4.ux.button.Close',
        'B4.ux.button.Save'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 100,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'b4selectfield',
                    name: 'Contragent',
                    fieldLabel: 'Контрагент',
                    store: 'B4.store.Contragent',
                    itemId: 'sfContragent',
                    allowBlank: false,
                    editable: false,
                    columns: [
                        { text: 'Наименование', dataIndex: 'ShortName', flex: 1, filter: { xtype: 'textfield' } },
                        { text: 'Муниципальный район', dataIndex: 'Municipality', flex: 1,
                            filter: {
                                xtype: 'b4combobox',
                                operand: CondExpr.operands.eq,
                                storeAutoLoad: false,
                                hideLabel: true,
                                editable: false,
                                valueField: 'Name',
                                emptyItem: { Name: '-' },
                                url: '/Municipality/ListMoAreaWithoutPaging'
                            }
                        },
                        { text: 'ИНН', dataIndex: 'Inn', flex: 1, filter: { xtype: 'textfield' } }
                    ]
                },
                {
                    xtype: 'textarea',
                    name: 'Description',
                    fieldLabel: 'Описание',
                    maxLength: 500,
                    flex: 1
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
                                    xtype: 'b4savebutton'
                                }
                            ]
                        },
                        {
                            xtype: 'tbfill'
                        },
                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            items: [
                                {
                                    xtype: 'b4closebutton'
                                }
                            ]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});
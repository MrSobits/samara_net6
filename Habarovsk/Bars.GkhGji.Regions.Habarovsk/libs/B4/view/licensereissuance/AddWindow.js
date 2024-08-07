﻿Ext.define('B4.view.licensereissuance.AddWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.licensereissuanceaddwindow',
    
    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'form',
    width: 500,
    minWidth: 500,
    minHeight: 150,
    maxHeight: 150,
    bodyPadding: 5,
    itemId: 'licenseReissuanceAddWindow',
    title: 'Обращение за переоформлением лицензии',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.form.SelectField',
        'B4.store.manorglicense.ListManOrgWithLicense',
        'B4.ux.button.Close',
        'B4.ux.button.Save'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 170,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'b4selectfield',
                    name: 'Contragent',
                    fieldLabel: 'Лицензиат',
                    store: 'B4.store.manorglicense.ListManOrgWithLicense',
                    editable: false,
                    allowBlank: false,
                    columns: [
                        { text: 'Наименование', dataIndex: 'ShortName', flex: 1, filter: { xtype: 'textfield' } },
                        {
                            text: 'Муниципальное образование', dataIndex: 'Municipality', flex: 1,
                            filter: {
                                xtype: 'b4combobox',
                                operand: CondExpr.operands.eq,
                                storeAutoLoad: false,
                                hideLabel: true,
                                editable: false,
                                valueField: 'Name',
                                emptyItem: { Name: '-' },
                                url: '/Municipality/ListWithoutPaging'
                            }
                        },
                        { text: 'ИНН', dataIndex: 'Inn', flex: 1, filter: { xtype: 'textfield' } }
                    ]
                },
                {
                    xtype: 'datefield',
                    name: 'ReissuanceDate',
                    allowBlank: false,
                    fieldLabel: 'Дата обращения',
                    format: 'd.m.Y'
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
                                    xtype: 'b4savebutton'
                                }
                            ]
                        },
                        {
                            xtype: 'tbfill'
                        },
                        {
                            xtype: 'buttongroup',
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
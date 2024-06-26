﻿Ext.define('B4.view.publicservorg.EditPanel', {
    extend: 'Ext.form.Panel',
    alias: 'widget.publicservorgeditpanel',

    closable: true,
    width: 500,
    minWidth: 535,
    bodyPadding: 5,
    frame: true,
    title: 'Общие сведения',
    trackResetOnLoad: true,
    autoScroll: true,
    requires: [
        'B4.form.ComboBox',
        'B4.form.SelectField',
        'B4.store.Contragent',
        'B4.ux.button.Save',
        'B4.enums.OrgStateRole'
    ],
    
    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'fieldset',
                    anchor: '100%',
                    defaults: {
                        labelWidth: 150,
                        labelAlign: 'right'
                    },
                    title: 'Общие сведения',
                    items: [
                        {
                            xtype: 'b4selectfield',
                            store: 'B4.store.Contragent',
                            name: 'Contragent',
                            fieldLabel: 'Контрагент',
                            editable: false,
                            allowBlank: false,
                            anchor: '100%',
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
                            xtype: 'textareafield',
                            anchor: '100%',
                            name: 'Description',
                            fieldLabel: 'Описание'
                        }
                    ]
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
                            action: 'GoToContragent',
                            items: [
                                {
                                    xtype: 'button',
                                    text: 'Перейти к контрагенту',
                                    iconCls: 'icon-arrow-out',
                                    panel: me,
                                    handler: function () {
                                        var me = this,
                                            form = me.panel.getForm(),
                                            record = form.getRecord(),
                                            contragentId = record.get('Contragent') ? record.get('Contragent').Id : 0;

                                        if (contragentId) {
                                            Ext.History.add(Ext.String.format('contragentedit/{0}/', contragentId));
                                        }
                                    }
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

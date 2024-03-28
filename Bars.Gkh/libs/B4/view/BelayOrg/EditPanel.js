Ext.define('B4.view.belayorg.EditPanel', {
    extend: 'Ext.form.Panel',

    closable: true,
    layout: 'anchor',
    width: 500,
    bodyPadding: 5,
    itemId: 'belayOrgEditPanel',
    title: 'Общие сведения',
    trackResetOnLoad: true,
    autoScroll: true,
    frame: true,
    requires: [
        'B4.form.SelectField',
        'B4.store.Contragent',
        'B4.ux.button.Save',
        'B4.form.ComboBox',
        'B4.enums.OrgStateRole',
        'B4.enums.GroundsTermination'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'fieldset',
                    title: 'Общие сведения',
                    defaults: {
                        labelWidth: 100,
                        labelAlign: 'right',
                        anchor: '100%'
                    },
                    items: [
                        {
                            xtype: 'b4selectfield',
                            name: 'Contragent',
                            fieldLabel: 'Контрагент',
                            store: 'B4.store.Contragent',
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
                            ],
                            editable: false
                        },
                        {
                            xtype: 'combobox', editable: false,
                            fieldLabel: 'Статус',
                            store: B4.enums.OrgStateRole.getStore(),
                            displayField: 'Display',
                            valueField: 'Value',
                            name: 'OrgStateRole',
                            editable: false
                        },
                        {
                            xtype: 'textarea',
                            name: 'Description',
                            fieldLabel: 'Описание',
                            maxLength: 500
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    title: 'Деятельность',
                    defaults: {
                        labelWidth: 100,
                        labelAlign: 'right',
                        anchor: '100%'
                    },
                    items: [
                        {
                            xtype: 'datefield',
                            fieldLabel: 'Дата',
                            name: 'DateTermination',
                            format: 'd.m.Y',
                            maxWidth: 250
                        },
                        {
                            xtype: 'container',
                            padding: '0 0 5 0',
                            layout: 'hbox',
                            defaults: {
                                anchor: '100%',
                                labelAlign: 'right',
                                labelWidth: 100
                            },
                            items: [
                                {
                                    xtype: 'combobox', editable: false,
                                    name: 'ActivityGroundsTermination',
                                    itemId: 'cbActivityGroundsTermination',
                                    fieldLabel: 'Основание',
                                    store: B4.enums.GroundsTermination.getStore(),
                                    displayField: 'Display',
                                    valueField: 'Value',
                                    flex: 1,
                                    maxWidth: 500,
                                    editable: false
                                },
                                {
                                    xtype: 'label',
                                    itemId: 'lbActivityGroundsTerminationLabel',
                                    margin: '0 0 0 10'
                                }
                            ]
                        },
                        {
                            xtype: 'textarea',
                            name: 'ActivityDescription',
                            fieldLabel: 'Описание',
                            maxLength: 1000
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
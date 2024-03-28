Ext.define('B4.view.belaypolicy.EditPanel', {
    extend: 'Ext.form.Panel',
    requires: [
        'B4.form.SelectField',
        'B4.store.dict.BelayOrgKindActivity',
        'B4.store.BelayOrganization',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.enums.PolicyAction'
    ],
    closable: true,
    width: 500,
    minWidth: 535,
    bodyPadding: 5,
    itemId: 'belayPolicyEditPanel',
    title: 'Общие сведения',
    trackResetOnLoad: true,
    autoScroll: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                anchor: '100%',
                labelWidth: 200,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'b4selectfield',
                    name: 'BelayOrganization',
                    fieldLabel: 'Страховая организация',
                    store: 'B4.store.BelayOrganization',
                    allowBlank: false,
                    editable: false,
                    textProperty: 'ContragentName',
                    columns: [{ text: 'Наименование страховой организации', dataIndex: 'ContragentName', flex: 1}]
                },
                {
                    xtype: 'container',
                    layout:  'column',
                    items: [
                        {
                            xtype: 'container',
                            columnWidth: 0.5,
                            layout: 'anchor',
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'DocumentNumber',
                                    fieldLabel: 'Номер договора',
                                    anchor: '100%',
                                    labelWidth: 200,
                                    labelAlign: 'right',
                                    maxLength: 300
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            columnWidth: 0.5,
                            layout: 'anchor',
                            items: [
                                {
                                    xtype: 'datefield',
                                    name: 'DocumentDate',
                                    fieldLabel: 'Дата договора',
                                    format: 'd.m.Y',
                                    labelWidth: 220,
                                    labelAlign: 'right',
                                    anchor: '100%'
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'b4selectfield',
                    name: 'BelayOrgKindActivity',
                    fieldLabel: 'Вид страхуемой деятельности',
                    store: 'B4.store.dict.BelayOrgKindActivity',
                    allowBlank: false,
                    editable: false,
                    textProperty: 'Name'
                },
                {
                    xtype: 'container',
                    layout: 'column',
                    items: [
                        {
                            xtype: 'container',
                            columnWidth: 0.5,
                            layout: 'anchor',
                            items: [
                                {
                                    xtype: 'datefield',
                                    name: 'DocumentStartDate',
                                    fieldLabel: 'Дата начала действия договора',
                                    anchor: '100%',
                                    format: 'd.m.Y',
                                    labelWidth: 200,
                                    labelAlign: 'right'
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            columnWidth: 0.5,
                            layout: 'anchor',
                            items: [
                                {
                                    xtype: 'datefield',
                                    name: 'DocumentEndDate',
                                    fieldLabel: 'Дата окончания действия договора',
                                    anchor: '100%',
                                    labelAlign: 'right',
                                    format: 'd.m.Y',
                                    labelWidth: 220
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'numberfield',
                    anchor: '100%',
                    hideTrigger: true,
                    keyNavEnabled: false,
                    mouseWheelEnabled: false,
                    name: 'BelaySum',
                    fieldLabel: 'Страховая сумма'
                },
                {
                    xtype: 'fieldset',
                    defaults: {
                        labelWidth: 480,
                        anchor: '100%',
                        labelAlign: 'right',
                        hideTrigger: true,
                        keyNavEnabled: false,
                        mouseWheelEnabled: false
                    },
                    title: 'Гражданская ответственность',
                    items: [
                        {
                            xtype: 'numberfield',
                            name: 'LimitCivil',
                            fieldLabel: 'Общий лимит гражданской ответственности, руб.'
                        },
                        {
                            xtype: 'numberfield',
                            name: 'LimitCivilOne',
                            fieldLabel: 'Общий лимит гражданской ответственности (в отношении 1 пострадавшего), руб.'
                        },
                        {
                            xtype: 'numberfield',
                            name: 'LimitCivilInsured',
                            fieldLabel: 'Общий лимит гражданской ответственности (по страховому случаю), руб.'
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    defaults: {
                        labelWidth: 350,
                        anchor: '100%',
                        labelAlign: 'right'
                    },
                    title: 'Ответственность управляющей организации',
                    items: [
                        {
                            xtype: 'numberfield',
                            hideTrigger: true,
                            keyNavEnabled: false,
                            mouseWheelEnabled: false,
                            name: 'LimitManOrgHome',
                            fieldLabel: 'Лимит ответственности УО (в отношении дома), руб.',
                            labelWidth: 350
                        },
                        {
                            xtype: 'numberfield',
                            hideTrigger: true,
                            keyNavEnabled: false,
                            mouseWheelEnabled: false,
                            name: 'LimitManOrgInsured',
                            fieldLabel: 'Лимит ответственности УО (по страховому случаю), руб.',
                            labelWidth: 350
                        },
                        {
                            xtype: 'combobox', editable: false,
                            fieldLabel: 'Действие полиса',
                            store: B4.enums.PolicyAction.getStore(),
                            displayField: 'Display',
                            valueField: 'Value',
                            name: 'PolicyAction',
                            labelWidth: 200
                        },
                        {
                            xtype: 'textarea',
                            name: 'Cause',
                            fieldLabel: 'Причина',
                            labelWidth: 200,
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
                            columns: 2,
                            items: [
                                {
                                    xtype: 'b4savebutton'
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
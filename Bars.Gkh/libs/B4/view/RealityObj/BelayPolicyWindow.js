Ext.define('B4.view.realityobj.BelayPolicyWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.realityobjbelaypolicywindow',
    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'form',
    width: 700,
    bodyPadding: 5,
    title: 'Страховой полис',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.SelectField',
        'B4.view.belayorg.Grid',
        'B4.view.manorg.Grid',
        'B4.store.BelayOrganization',
        'B4.store.ManagingOrganization'
    ],

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelAlign: 'right',
                labelWidth: 200,
                anchor: '100%'
            },
            items: [
                {
                    xtype: 'b4selectfield',
                    name: 'BelayOrganization',
                    fieldLabel: 'Страховая организация',
                    readOnly: true,
                   

                    store: 'B4.store.BelayOrganization'
                },
                {
                    xtype: 'b4selectfield',
                    name: 'ManagingOrganization',
                    fieldLabel: 'Управляющая организация',
                    readOnly: true,
                    store: 'B4.store.ManagingOrganization',
                    columns: [{ text: 'Наименование', dataIndex: 'ContragentShortName', flex: 1 }]
                },
                {
                    xtype: 'container',
                    layout: { type: 'column' },
                    items: [
                        {
                            xtype: 'container',
                            columnWidth: 0.5,
                            layout: {
                                type: 'anchor'
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'DocumentNumber',
                                    fieldLabel: 'Номер договора',
                                    anchor: '100%',
                                    labelWidth: 200,
                                    readOnly: true
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            columnWidth: 0.5,
                            layout: {
                                type: 'anchor'
                            },
                            defaults: {
                                labelAlign: 'right',
                                anchor: '100%'
                            },
                            items: [
                                {
                                    xtype: 'datefield',
                                    name: 'DocumentDate',
                                    fieldLabel: 'Дата договора',
                                    anchor: '100%',
                                    format: 'd.m.Y',
                                    labelWidth: 220,
                                    readOnly: true
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'container',
                    layout: { type: 'column' },
                    items: [
                        {
                            xtype: 'container',
                            columnWidth: 0.5,
                            layout: {
                                type: 'anchor'
                            },
                            items: [
                                {
                                    xtype: 'datefield',
                                    name: 'DocumentStartDate',
                                    fieldLabel: 'Дата начала действия договора',
                                    anchor: '100%',
                                    format: 'd.m.Y',
                                    labelWidth: 200,
                                    readOnly: true
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            columnWidth: 0.5,
                            layout: {
                                type: 'anchor'
                            },
                            items: [
                                {
                                    xtype: 'datefield',
                                    name: 'DocumentEndDate',
                                    fieldLabel: 'Дата окончания действия договора',
                                    anchor: '100%',
                                    labelAlign: 'right',
                                    format: 'd.m.Y',
                                    labelWidth: 220,
                                    readOnly: true
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    defaults: {
                        labelWidth: 480,
                        labelAlign: 'right',
                        anchor: '100%'
                    },
                    title: 'Гражданская ответственность',
                    items: [
                        {
                            xtype: 'numberfield',
                            hideTrigger: true,
                            keyNavEnabled: false,
                            mouseWheelEnabled: false,
                            name: 'LimitCivilOne',
                            fieldLabel: 'Общий лимит гражданской ответственности (в отношении 1 пострадавшего), руб.',
                            readOnly: true
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    defaults: {
                        anchor: '100%',
                        labelAlign: 'right',
                        readOnly: true
                    },
                    title: 'Ответственность управляющей организации',
                    items: [
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
                            xtype: 'textfield',
                            name: 'PolicyAction',
                            fieldLabel: 'Действие полиса',
                            labelWidth: 200
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
                            columns: 1,
                            items: [
                                { xtype: 'b4closebutton' }
                            ]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});
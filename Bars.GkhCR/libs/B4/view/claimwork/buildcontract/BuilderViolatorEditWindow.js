Ext.define('B4.view.claimwork.buildcontract.BuilderViolatorEditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.builderviolatoreditwindow',
    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: { type: 'vbox', align: 'stretch' },
    width: 800,
    minWidth: 800,
    minHeight: 550,
    maxHeight: 550,
    bodyPadding: 5,
    title: 'Редактирование',
    closeAction: 'destroy',
    trackResetOnLoad: true,

    requires: [
        'B4.view.claimwork.buildcontract.BuilderViolatorViolGrid',
        'claimwork.BuilderViolatorViol',
        'B4.enums.BuildContractCreationType',
        'B4.ux.button.Close',
        'B4.ux.button.Save'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'container',
                    layout: { type: 'vbox', align: 'stretch' },
                    defaults: {
                        labelWidth: 170,
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            fieldLabel: 'Подрядчик',
                            name: 'Builder',
                            readOnly: true
                        },
                        {
                            xtype: 'textfield',
                            fieldLabel: 'ИНН',
                            name: 'Inn',
                            readOnly: true
                        },
                        {
                            xtype: 'container',
                            padding: '0 0 5 0',
                            anchor: '100%',
                            layout: 'hbox',
                            defaults: {
                                labelWidth: 170,
                                labelAlign: 'right',
                                flex: 1
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    fieldLabel: '№ договора',
                                    readOnly: true,
                                    name: 'DocumentNum'
                                },
                                {
                                    xtype: 'datefield',
                                    name: 'DocumentDateFrom',
                                    readOnly: true,
                                    fieldLabel: 'Дата договора',
                                    format: 'd.m.Y'
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            padding: '0 0 5 0',
                            anchor: '100%',
                            layout: 'hbox',
                            defaults: {
                                labelWidth: 170,
                                labelAlign: 'right',
                                flex: 1
                            },
                            items: [
                                {
                                    xtype: 'datefield',
                                    name: 'DateEndWork',
                                    readOnly: true,
                                    fieldLabel: 'Срок выполнения работ',
                                    format: 'd.m.Y'
                                },
                                {
                                    xtype: 'numberfield',
                                    name: 'CountDaysDelay',
                                    fieldLabel: 'Количество дней просрочки',
                                    hideTrigger: true,
                                    allowDecimals: false,
                                    readOnly: true,
                                    minValue: 0,
                                    negativeText: 'Значение не может быть отрицательным'
                                }
                            ]
                        },
                        {
                            xtype: 'combobox',
                            editable: false,
                            name: 'CreationType',
                            fieldLabel: 'Способ формирования',
                            displayField: 'Display',
                            readOnly: true,
                            store: B4.enums.BuildContractCreationType.getStore(),
                            valueField: 'Value',
                            allowBlank: false
                        }
                    ]
                },
                {
                    xtype: 'builderviolatorviolgrid',
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
                            items: [
                                {
                                    text: 'Договор',
                                    action: 'goContract',
                                    iconCls: 'icon-page-forward'
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
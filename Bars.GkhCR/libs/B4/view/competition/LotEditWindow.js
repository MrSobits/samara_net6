Ext.define('B4.view.competition.LotEditWindow', {
    extend: 'B4.form.Window',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'fit',
    width: 800,
    minWidth: 800,
    minHeight: 550,
    maxHeight: 550,
    alias: 'widget.competitionloteditwindow',
    title: 'Лот',
    closeAction: 'destroy',
    trackResetOnLoad: true,

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.FileField',
        'B4.view.competition.LotTypeWorkGrid',
        'B4.view.competition.LotBidGrid'
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
                    xtype: 'tabpanel',
                    layout: 'vbox',
                    flex: 1,
                    margins: -1,
                    border: false,
                    items: [
                        {
                            layout: { type: 'vbox', align: 'stretch' },
                            title: 'Основная информация',
                            border: false,
                            bodyPadding: 5,
                            margins: -1,
                            frame: true,
                            defaults: {
                                labelWidth: 170,
                                labelAlign: 'right'
                            },
                            items: [
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
                                            xtype: 'numberfield',
                                            name: 'LotNumber',
                                            fieldLabel: 'Номер лота',
                                            hideTrigger: true,
                                            allowDecimals: false,
                                            minValue: 0,
                                            allowBlank: false,
                                            negativeText: 'Значение не может быть отрицательным'
                                        },
                                        {
                                            xtype: 'numberfield',
                                            name: 'StartingPrice',
                                            fieldLabel: 'Начальная (максимальная) цена договора, руб',
                                            hideTrigger: true,
                                            allowDecimals: true,
                                            minValue: 0,
                                            allowBlank: false,
                                            negativeText: 'Значение не может быть отрицательным'
                                        }
                                    ]
                                },
                                {
                                    xtype: 'textarea',
                                    name: 'Subject',
                                    fieldLabel: 'Предмет договора',
                                    height: 50,
                                    maxLength: 500
                                },
                                {
                                    xtype: 'competitionlottypeworkgrid',
                                    margins: -1,
                                    flex: 1
                                }
                            ]
                        },
                        {
                            xtype: 'competitionlotbidgrid',
                            margins: -1,
                            flex: 1
                        },
                        {
                            layout: { type: 'vbox', align: 'stretch' },
                            title: 'Договор',
                            border: false,
                            bodyPadding: 5,
                            margins: -1,
                            frame: true,
                            defaults: {
                                labelWidth: 170,
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    fieldLabel: 'Участник',
                                    name: 'Winner',
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
                                            fieldLabel: 'Номер договора',
                                            name: 'ContractNumber'
                                        },
                                        {
                                            xtype: 'datefield',
                                            name: 'ContractDate',
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
                                            xtype: 'numberfield',
                                            name: 'ContractFactPrice',
                                            fieldLabel: 'Фактическая цена договора, руб',
                                            hideTrigger: true,
                                            allowDecimals: true,
                                            minValue: 0,
                                            negativeText: 'Значение не может быть отрицательным'
                                        },
                                        {
                                            xtype: 'component'
                                        }
                                    ]
                                },
                                {
                                    xtype: 'b4filefield',
                                    name: 'ContractFile',
                                    fieldLabel: 'Файл'
                                }
                            ]
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
                            items: [{ xtype: 'b4savebutton' }]
                        }, '->', {
                            xtype: 'buttongroup',
                            items: [{ xtype: 'b4closebutton' }]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});
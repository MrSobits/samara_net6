Ext.define('B4.view.contragent.BankEditWindow', {
    extend: 'B4.form.Window',

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.FileField',
        'B4.form.SelectField'
    ],

    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: { type: 'vbox', align: 'stretch' },
    width: 680,
    minWidth: 680,
    minHeight: 270,
    bodyPadding: 5,
    itemId: 'contragentBankEditWindow',
    title: 'Расчетные счет',
    closeAction: 'hide',
    trackResetOnLoad: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 100,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'textfield',
                    name: 'Name',
                    fieldLabel: 'Наименование',
                    allowBlank: false,
                    maxLength: 300
                },
                {
                    xtype: 'textfield',
                    name: 'CorrAccount',
                    fieldLabel: 'Корр. счет',
                    allowBlank: false,
                    maxLength: 50
                },
                {
                    xtype: 'textfield',
                    name: 'SettlementAccount',
                    fieldLabel: 'Расчетный счет',
                    allowBlank: false,
                    maxLength: 30
                },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    padding: '0 0 5 0',
                    defaults: {
                        xtype: 'textfield',
                        labelAlign: 'right',
                        labelWidth: 100,
                        maxLength: 50,
                        flex: 1
                    },
                    items: [
                        {
                            name: 'Bik',
                            fieldLabel: 'БИК',
                            allowBlank: false
                        },
                        {
                            name: 'Okpo',
                            fieldLabel: 'ОКПО'
                        },
                        {
                            name: 'Okonh',
                            fieldLabel: 'ОКОНХ'
                        }
                    ]
                },
                {
                    xtype: 'textarea',
                    name: 'Description',
                    fieldLabel: 'Описание',
                    maxLength: 1000,
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
                                { xtype: 'b4savebutton' }
                            ]
                        },
                        {
                            xtype: 'tbfill'
                        },
                        {
                            xtype: 'buttongroup',
                            columns: 2,
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
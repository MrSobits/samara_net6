Ext.define('B4.view.dict.paymentsizecr.EditWindow', {
    extend: 'B4.form.Window',

    //alias: 'widget.paymentsizecreditwindow',
    // Альберт, смотри - вот здесь без itemId никуда, нужно править код grideditwindowaspect
    itemId: 'paymentsSizeEditWindow',
    
    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
    
        'B4.form.ComboBox',

        'B4.ux.grid.Panel',
        'B4.enums.TypeIndicator',
        'B4.view.dict.paymentsizecr.MuPaySizeGrid'
    ],

    modal: true,
    layout: { type: 'vbox', align: 'stretch' },
    width: 630,
    minWidth: 600,
    bodyPadding: 5,
    title: 'Размер взноса на КР',

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'b4combobox',
                    editable: false,
                    name: 'TypeIndicator',
                    itemId: 'cbTypeIndicator',
                    store: B4.enums.TypeIndicator.getStore(),
                    fieldLabel: 'Показатель',
                    labelAlign: 'right',
                    labelWidth: 140,
                    displayField: 'Display',
                    valueField: 'Value',
                    allowBlank: false
                },
                {
                    xtype: 'numberfield',
                    name: 'PaymentSize',
                    itemId: 'tfPaymentSize',
                    fieldLabel: 'Значение показателя',
                    maxLength: 10,
                    enforceMaxLength: true,
                    hideTrigger: true,
                    labelAlign: 'right',
                    labelWidth: 140,
                    allowBlank: false
                },
                {
                    xtype: 'fieldset',
                    itemId: 'fsPeriod',
                    padding: 5,
                    title: 'Период действия',
                    layout: 'hbox',
                    defaults: {
                        xtype: 'datefield',
                        format: 'd.m.Y',
                        labelWidth: 135,
                        labelAlign: 'right',
                        flex: 1
                    },
                    items: [
                        {
                            name: 'DateStartPeriod',
                            itemId: 'dfDateStartPeriod',
                            fieldLabel: 'с',
                            allowBlank: false
                        },
                        {
                            name: 'DateEndPeriod',
                            itemId: 'dfDateEndPeriod',
                            fieldLabel: 'по'
                        }
                    ]
                },
                {
                    xtype: 'mupaysizecrpanel',
                    itemId: 'muPaySizeCrPanel',
                    height: 350,
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
                            columns: 1,
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
                            columns: 1,
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
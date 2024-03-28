Ext.define('B4.view.specialobjectcr.TypeWorkCrEditWindow', {
    extend: 'B4.form.Window',

    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    width: 600,
    minWidth: 400,
    maxWidth: 600,
    bodyPadding: 5,
    title: 'Вид работы',
    closeAction: 'destroy',

    alias: 'widget.typeworkspecialcreditwindow',

    requires: [
        'B4.view.Control.GkhDecimalField',
        'B4.form.SelectField',
        'B4.store.dict.Work',
        'B4.store.specialobjectcr.FinanceSourceForSelect',
        'B4.ux.button.Close',
        'B4.ux.button.Save'
    ],

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 190,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'b4selectfield',
                    name: 'FinanceSource',
                    fieldLabel: 'Разрез финансирования',
                    store: 'B4.store.specialobjectcr.FinanceSourceForSelect',
                    allowBlank: false
                },
                {
                    xtype: 'b4selectfield',
                    editable: false,
                    name: 'Work',
                    fieldLabel: 'Вид работы',
                    store: 'B4.store.dict.Work',
                    allowBlank: false
                },
                {
                    xtype: 'container',
                    padding: '5 0 5 0',
                    layout: 'hbox',
                    defaults: {
                        labelWidth: 190,
                        labelAlign: 'right',
                        allowBlank: false,
                        flex: 1
                    },
                    items: [
                        {
                            xtype: 'datefield',
                            name: 'DateStartWork',
                            fieldLabel: 'Начало выполнения работ'
                        },
                        {
                            xtype: 'datefield',
                            name: 'DateEndWork',
                            fieldLabel: 'Окончание выполнения работ'
                        }
                    ]
                },
                {
                    xtype: 'container',
                    padding: '5 0 5 0',
                    layout: 'hbox',
                    defaults: {
                        labelWidth: 190,
                        labelAlign: 'right',
                        flex: 1
                    },
                    items: [
                        {
                            xtype: 'gkhdecimalfield',
                            name: 'SumMaterialsRequirement',
                            fieldLabel: 'Потребность материалов (руб.)'
                        },
                        {
                            xtype: 'gkhdecimalfield',
                            name: 'Volume',
                            fieldLabel: 'Объем'
                        }
                    ]
                },
                {
                    xtype: 'container',
                    padding: '5 0 5 0',
                    layout: 'hbox',
                    defaults: {
                        labelWidth: 190,
                        labelAlign: 'right',
                        flex: 1
                    },
                    items: [
                        {
                            xtype: 'checkbox',
                            name: 'HasPsd',
                            fieldLabel: 'Наличие ПСД'
                        },
                        {
                            xtype: 'gkhdecimalfield',
                            name: 'Sum',
                            fieldLabel: 'Сумма (руб.)'
                        }
                    ]
                },
                {
                    xtype: 'textarea',
                    name: 'Description',
                    fieldLabel: 'Примечание',
                    maxLength: 2000,
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
                        { xtype: 'tbfill' },
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
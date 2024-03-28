Ext.define('B4.view.repairobject.repairwork.EditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.repairworkEditWindow',
    itemId: 'repairworkEditWindow',
    mixins: ['B4.mixins.window.ModalMask'],
    layout: { type: 'vbox', align: 'stretch' },
    width: 560,
    minWidth: 400,
    maxWidth: 600,
    bodyPadding: 5,

    title: 'Вид работы',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.view.Control.GkhDecimalField',
        'B4.form.SelectField',
        'B4.store.dict.WorkKindCurrentRepair',
        'B4.store.Builder',
        'B4.ux.button.Close',
        'B4.ux.button.Save'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 110,
                labelAlign: 'right',
                margin: '10 0 0 0',
                allowBlank: false
            },
            items: [
                {
                    xtype: 'b4selectfield',
                    editable: false,
                    name: 'Work',
                    itemId: 'sfWork',
                    fieldLabel: 'Вид работы',
                    store: 'B4.store.dict.WorkKindCurrentRepair',
                    disabled: true
                },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    defaults: {
                        labelWidth: 110,
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            readOnly: true,
                            name: 'UnitMeasure',
                            itemId: 'unitMeasure',
                            fieldLabel: 'Ед. изм.'
                        },
                        {
                            xtype: 'textfield',
                            readOnly: true,
                            name: 'TypeWork',
                            labelWidth: 150,
                            itemId: 'typeWork',
                            fieldLabel: 'Тип работы'
                        }
                    ]
                },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    defaults: {
                        labelWidth: 110,
                        labelAlign: 'right',
                        allowBlank: false
                    },
                    items: [
                        {
                            xtype: 'gkhdecimalfield',
                            name: 'Volume',
                            fieldLabel: 'Плановый объем',
                            itemId: 'dcfVolume'
                        },
                        {
                            xtype: 'gkhdecimalfield',
                            name: 'Sum',
                            labelWidth: 150,
                            fieldLabel: 'Плановая сумма (руб.)',
                            itemId: 'dcfSum'
                        }
                    ]
                },
                {
                    xtype: 'textfield',
                    allowBlank: false,
                    name: 'Builder',
                    fieldLabel: 'Подрядчик',
                    maxLength: 255,
                    enforceMaxLength: true
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
Ext.define('B4.view.specialobjectcr.ProgressExecutionWorkEditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.specialobjectcrprogressexecutionworkeditwin',
    mixins: ['B4.mixins.window.ModalMask'],
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    width: 700,
    minWidth: 600,
    maxWidth: 800,
    bodyPadding: 5,
    title: 'Ход выполнения работ',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires:
    [
        'B4.form.SelectField',
        'B4.store.dict.StageWorkCr',
        'B4.view.Control.GkhDecimalField',
        'B4.ux.button.Close',
        'B4.ux.button.Save'
    ],

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                anchor: '100%',
                labelWidth: 150
            },
            items: [
                {
                    xtype: 'container',
                    layout: { type: 'hbox' },
                    items: [
                        {
                            xtype: 'container',
                            layout: {
                                type: 'vbox',
                                align: 'stretch'
                            },
                            defaults: {
                                xtype: 'gkhdecimalfield',
                                labelWidth: 150,
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    name: 'VolumeOfCompletion',
                                    fieldLabel: 'Объем выполнения'
                                },
                                {
                                    name: 'CostSum',
                                    fieldLabel: 'Сумма расходов'
                                },
                                {
                                    name: 'PercentOfCompletion',
                                    fieldLabel: 'Процент выполнения',
                                    maxValue: 100
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            layout: {
                                type: 'vbox',
                                align: 'stretch'
                            },
                            defaults: {
                                labelWidth: 105,
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'b4selectfield',
                                    editable: false,
                                    name: 'StageWorkCr',
                                    fieldLabel: 'Этап работы',
                                    store: 'B4.store.dict.StageWorkCr'
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'ManufacturerName',
                                    fieldLabel: 'Производитель',
                                    maxLength: 2000
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
                            items: [
                                { xtype: 'b4savebutton' }
                            ]
                        },
                        { xtype: 'tbfill' },
                        {
                            xtype: 'buttongroup',
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
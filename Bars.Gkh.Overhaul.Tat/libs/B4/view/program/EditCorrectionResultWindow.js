Ext.define('B4.view.program.EditCorrectionResultWindow', {
    extend: 'B4.form.Window',

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.view.program.CorrectionResultDetailsGrid',
        'B4.store.program.CorrectionResult',
        'B4.view.program.CorrectionHistoryGrid'
    ],
    
    alias: 'widget.editcorrectionresultwin',
    title: 'Изменить номер очередности',
    modal: true,
    closeAction: 'hide',
    bodyPadding: '5 5 0 0',
    width: 700,
    height: 400,
    bodyStyle: Gkh.bodyStyle,

    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    closable: false,
    initComponent: function () {
        var me = this;

        Ext.apply(me, {
            items: [
                {
                    xtype: 'tabpanel',
                    border: false,
                    bodyStyle: Gkh.bodyStyle,
                    margins: -1,
                    flex: 1,
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    items: [
                        {
                            xtype: 'panel',
                            flex: 1,
                            padding: 2,
                            bodyStyle: Gkh.bodyStyle,
                            border: false,
                            title: 'Общая информация',
                            layout: {
                                type: 'vbox',
                                align: 'stretch'
                            },
                            defaults: {
                                labelAlign: 'right',
                                labelWidth: 150
                            },
                            items: [
                                {
                                    xtype: 'numberfield',
                                    hideTrigger: true,
                                    allowDecimals: false,
                                    minValue: 2014,
                                    maxValue: 2100,
                                    fieldLabel: 'Плановый год',
                                    readOnly: true,
                                    name: 'FirstPlanYear'
                                },
                                {
                                    xtype: 'numberfield',
                                    hideTrigger: true,
                                    allowDecimals: false,
                                    minValue: 2014,
                                    maxValue: 2100,
                                    fieldLabel: 'Скорректированный год',
                                    name: 'PlanYear'
                                },
                                {
                                    xtype: 'container',
                                    layout: {
                                        type: 'hbox',
                                        align: 'stretch'
                                    },
                                    defaults: {
                                        xtype: 'textfield',
                                        labelAlign: 'right',
                                        labelWidth: 150,
                                        flex: 1
                                    },
                                    items: [
                                        {
                                            name: 'IndexNumber',
                                            fieldLabel: 'Текущий номер',
                                            readOnly: true
                                        },
                                        {
                                            name: 'NewIndexNumber',
                                            fieldLabel: 'Новый номер',
                                            labelWidth: 100
                                        }]
                                },
                                {
                                    xtype: 'checkbox',
                                    fieldLabel: 'Фиксация скорректированного года',
                                    labelWidth: 225,
                                    name: 'FixedYear'
                                }
                            ]
                        },
                        {
                            xtype: 'correctionresultdetailsgrid',
                            flex: 1
                        },
                        {
                            xtype: 'progcorrecthistorygrid',
                            flex: 1
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
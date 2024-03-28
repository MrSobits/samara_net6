Ext.define('B4.view.econfeasibilitycalcresult.EditWindow', {
    extend: 'B4.form.Window',
    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'form',
    width: 1000,
    bodyPadding: 5,
    itemId: 'econfeasibilitycalcEditWindow',
    alias: 'widget.econfeasibilitycalcresultwindow',
    title: 'Детализация результата расчета целесообразности',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.ux.button.Close', 
        'B4.form.SelectField',
        'B4.view.econfeasibilitycalcresult.WorkGrid',
        'B4.model.RealityObject',
        'B4.model.dict.LivingSquareCost'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 200,
                labelAlign: 'right'
            },
            
            items: [

                {
                    xtype: 'b4selectfield',
                    name: 'RoId',
                    fieldLabel: 'Адрес дома',
                    model: 'B4.model.RealityObject',
                    textProperty: 'Address',
                    editable: 'false',
                    readOnly: 'true'                    
                },
                {
                    xtype: 'fieldset',
                    defaults: {
                        labelWidth: 200,
                        anchor: '100%',
                        labelAlign: 'right'
                    },
                    layout: {
                        type: 'hbox',
                        align: 'stretch'
                    },
                    title: 'Годы',
                    flex: 1,
                    items: [
                        {
                            xtype: 'numberfield',
                            name: 'YearStart',
                            fieldLabel: 'Начальный Год',
                            labelAlign: 'right',
                            labelWidth: 200,
                            anchor: '100%',
                            hideTrigger: true,
                            keyNavEnabled: false,
                            mouseWheelEnabled: false,
                            minValue: 0
                        },
                        {
                            xtype: 'numberfield',
                            name: 'YearEnd',
                            fieldLabel: 'Конечный Год',
                            labelAlign: 'right',
                            labelWidth: 200,
                            anchor: '100%',
                            hideTrigger: true,
                            keyNavEnabled: false,
                            mouseWheelEnabled: false,
                            minValue: 0
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    defaults: {
                        labelWidth: 200,
                        anchor: '100%',
                        labelAlign: 'right'
                    },
                    layout: {
                        type: 'hbox',
                        align: 'stretch'
                    },
                    
                    items: [
                        {
                            xtype: 'numberfield',
                            name: 'TotatRepairSumm',
                            fieldLabel: 'Стоимость ремонта дома',
                            labelAlign: 'right',
                            labelWidth: 200,
                            anchor: '100%',
                            hideTrigger: true,
                            keyNavEnabled: false,
                            mouseWheelEnabled: false,
                            decimalSeparator: ',',
                            minValue: 0
                        },
                        {
                            xtype: 'b4selectfield',
                            name: 'SquareCost',
                            fieldLabel: 'Средняя стоимость кв.м.',
                            model: 'B4.model.dict.LivingSquareCost', 
                            textProperty: 'Cost',
                            editable: false,
                            title: 'Средняя стоимость кв.м.',                            
                            readOnly: true
                            
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    defaults: {
                        labelWidth: 200,
                        anchor: '100%',
                        labelAlign: 'right'
                    },
                    layout: {
                        type: 'hbox',
                        align: 'stretch'
                    },
                    //title: 'Реквизиты проверки',
                    items: [
                        {
                            xtype: 'numberfield',
                            name: 'TotalSquareCost',
                            fieldLabel: 'Стоимость всех помещений',
                            labelAlign: 'right',
                            labelWidth: 200,
                            anchor: '100%',
                            hideTrigger: true,
                            keyNavEnabled: false,
                            mouseWheelEnabled: false,
                            decimalSeparator: ',',
                            minValue: 0
                        },
                        {
                            xtype: 'numberfield',
                            name: 'CostPercent',
                            fieldLabel: 'Процент',
                            labelAlign: 'right',
                            labelWidth: 200,
                            anchor: '100%',
                            hideTrigger: true,
                            keyNavEnabled: false,
                            mouseWheelEnabled: false,
                            decimalSeparator: ',',
                            minValue: 0
                        },
                    ]
                },
                {
                    xtype: 'tabpanel',
                    border: false,
                    flex: 1,
                    defaults: {
                        border: false
                    },
                    items: [
                        {
                            xtype: 'econfeasibilityworkgrid',
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
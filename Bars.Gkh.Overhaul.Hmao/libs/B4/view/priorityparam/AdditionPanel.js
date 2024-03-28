Ext.define('B4.view.priorityparam.AdditionPanel', {
    extend: 'Ext.form.Panel',

    layout: 'anchor',
    width: 500,
    bodyPadding: 5,
    alias: 'widget.priorityparamadditionpanel',
    trackResetOnLoad: true,
    autoScroll: true,
    frame: true,
    requires: [
        'B4.form.SelectField',
        'B4.ux.button.Save',
        'B4.enums.PriorityParamAdditionFactor',
        'B4.enums.PriorityParamFinalValue'
    ],

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'container',
                    padding: '0 0 5 0',
                    layout: 'hbox',
                    defaults: {
                        labelAlign: 'right',
                        labelWidth: 150,
                        flex: 1
                    },
                    items: [
                        {
                            xtype: 'combobox',
                            editable: false,
                            fieldLabel: 'Доп. множитель',
                            store: B4.enums.PriorityParamAdditionFactor.getStore(),
                            displayField: 'Display',
                            valueField: 'Value',
                            name: 'AdditionFactor'
                        },
                        {
                            name: 'FactorValue',
                            xtype: 'numberfield',
                            fieldLabel: 'Значение множителя',
                            itemId: 'FactorValue'
                        }
                    ]
                },
                {
                    xtype: 'container',
                    padding: '0 0 5 0',
                    layout: 'hbox',
                    defaults: {
                        xtype: 'textfield',
                        labelAlign: 'right',
                        labelWidth: 150
                    },
                    items: [
                        {
                            xtype: 'combobox',
                            editable: false,
                            fieldLabel: 'Конечное значение',
                            width: '50%',
                            store: B4.enums.PriorityParamFinalValue.getStore(),
                            displayField: 'Display',
                            valueField: 'Value',
                            name: 'FinalValue'
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
                                    xtype: 'b4savebutton'
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
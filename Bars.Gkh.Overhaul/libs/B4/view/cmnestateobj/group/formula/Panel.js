Ext.define('B4.view.cmnestateobj.group.formula.Panel', {
    extend: 'Ext.panel.Panel',

    alias: 'widget.structelgroupformulapanel',
    
    title: 'Формула',
    
    requires: [
         'B4.view.cmnestateobj.group.formula.ParamGrid'
    ],

    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    
    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'form',
                    bodyPadding: 5,
                    bodyStyle: Gkh.bodyStyle,
                    margin: -1,
                    defaults: {
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'FormulaName',
                            fieldLabel: 'Наименование',
                            anchor: '100%'
                        },
                        {
                            xtype: 'textarea',
                            name: 'FormulaDescription',
                            fieldLabel: 'Описание',
                            anchor: '100%'
                        },
                        {
                            xtype: 'textarea',
                            name: 'Formula',
                            fieldLabel: 'Формула расчета года ремонта',
                            anchor: '100%'
                        },
                        {
                            xtype: 'fieldcontainer',
                            layout: 'hbox',
                            items: [
                                {
                                    xtype: 'displayfield',
                                    name: 'FormulaMsg',
                                    style: 'text-align: right;',
                                    flex: 1
                                },
                                {
                                    xtype: 'splitter'
                                },
                                {
                                    xtype: 'button',
                                    text: 'Получить параметры',
                                    action: 'getparams'
                                },
                                {
                                    xtype: 'splitter'
                                },
                                {
                                    xtype: 'button',
                                    text: 'Проверить формулу',
                                    action: 'checkformula'
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'groupformulaparamsgrid',
                    flex: 1,
                    margin: -1
                }
            ]
        });

        me.callParent(arguments);
    }
});
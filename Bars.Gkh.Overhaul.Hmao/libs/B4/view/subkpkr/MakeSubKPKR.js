Ext.define('B4.view.subkpkr.MakeSubKPKR', {
    extend: 'B4.form.Window',
    alias: 'widget.versmakesubkpkrwin',
    mixins: ['B4.mixins.window.ModalMask'],
    layout: { type: 'vbox', align: 'stretch' },
    bodyPadding: 5,
    minWidth: 800,
    title: 'Создать подпрограмму КПКР из ДПКР',
    closable: false,
    requires: [
        'B4.view.Control.GkhTriggerField',
        'B4.view.subkpkr.MaxCostGrid',
        'B4.view.subkpkr.KEGrid',
        'B4.ux.button.Close',
    ],

    initComponent: function () {
        var me = this;
        
        Ext.applyIf(me, {
            defaults:
            {
                labelWidth: 200,
                labelAlign: 'right',
            },
            items: [
                {
                    xtype: 'panel',                    
                    border: true,
                    items: [
                        {
                            xtype: 'container',                            
                            layout: 'hbox',
                            defaults: {
                                labelAlign: 'right',
                                padding: '3 3 3 3',
                                flex: 1,
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    flex: 0.3,
                                    labelWidth: 200,
                                    name: 'CurrentCost',
                                    itemId: 'tfCurrentCost',
                                    fieldLabel: 'Выбранная стоимость',
                                    readOnly: true,
                                },
                                {
                                    xtype: 'textfield',
                                    flex: 0.3,
                                    name: 'CurrentLimit',
                                    itemId: 'tfCurrentLimit',
                                    fieldLabel: 'Лимит',
                                    readOnly: true,
                                },
                                {
                                    xtype: 'textfield',
                                    flex: 0.3,
                                    name: 'CurrentLeft',
                                    itemId: 'tfCurrentLeft',
                                    fieldLabel: 'Осталось',
                                    readOnly: true,
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    defaults: {
                        labelWidth: 200,
                        labelAlign: 'right',
                        flex: 1,
                    },
                    items: [
                        {
                            xtype: 'numberfield',
                            name: 'StartYear',
                            itemId: 'nfStartYear',
                            fieldLabel: 'Год начала:',
                            allowDecimals: true,
                            allowBlank: false,
                            minValue: 2014,
                            flex: 1,
                            maxValue: 3000
                        },
                        {
                            xtype: 'checkbox',
                            labelWidth: 350,
                            name: 'FirstYearWithoutWork',
                            itemId: 'cbFirstYearWithoutWork',
                            fieldLabel: 'В первый год КПКР нет работ',
                            allowBlank: false,
                            editable: true,
                        }  
                    ]
                },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    defaults: {
                        labelWidth: 200,
                        labelAlign: 'right',
                        flex: 1,
                    },
                    items: [
                        {
                            xtype: 'numberfield',
                            name: 'YearCount',
                            itemId: 'nfYearCount',
                            flex: 1,
                            fieldLabel: 'Количество лет для формирования:',
                            allowDecimals: true,
                            allowBlank: false,
                            minValue: 1,
                            maxValue: 255
                        },
                        {
                            xtype: 'checkbox',
                            labelWidth: 350,
                            name: 'FirstYearPSD',
                            itemId: 'cbFirstYearPSD',
                            fieldLabel: 'Формирование ПCД по всем годам в первом годе КПКР',
                            allowBlank: false,
                            editable: true,
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
                            xtype: 'subkpkrkkegrid',
                            flex: 1,
                        },
                        {
                            xtype: 'subkpkrmaxcostgrid',
                            flex: 1,
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
                                    xtype: 'button',
                                    text: 'Создать подпрограмму КПКР',
                                    tooltip: 'Создать подпрограмму КПКР',
                                    action: 'makeSubKPKR',
                                    iconCls: 'icon-accept'
                                },
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
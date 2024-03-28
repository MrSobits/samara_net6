Ext.define('B4.view.gasu.EditWindow', {
    extend: 'B4.form.Window',
    requires: [
        'B4.form.SelectField',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.view.gasu.FileInfoGrid',
        'B4.view.gasu.GASUDataGrid',
        'B4.enums.GasuMessageType'
     
    ],
    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'form',
    width: 1000,
    bodyPadding: 10,
    itemId: 'gasuEditWindow',
    title: 'Обмен данными с ГАС Управление',
    closeAction: 'hide',
    trackResetOnLoad: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'tabpanel',
                    border: false,
                    flex: 1,
                    defaults: {
                        border: false
                    },
                    items: [
                        {
                            layout: {
                                type: 'vbox',
                                align: 'stretch'
                            },
                            defaults: {
                                labelWidth: 150,
                                margin: '5 0 5 0',
                                align: 'stretch',
                                labelAlign: 'right'
                            },
                            bodyStyle: 'background: none repeat scroll 0 0 #DFE9F6',
                            title: 'Параметры запроса',
                            border: false,
                            bodyPadding: 10,
                            items: [
                                {
                                    xtype: 'container',
                                    layout: 'hbox',
                                    defaults: {
                                        xtype: 'combobox',
                                        margin: '0 0 5 0',
                                        labelWidth: 80,
                                        labelAlign: 'right',
                                    },
                                    items: [
                                        {
                                            xtype: 'button',
                                            text: 'Отправить сведения',
                                            tooltip: 'Отправить проверку',
                                            iconCls: 'icon-accept',
                                            width: 200,
                                            itemId: 'sendCalculateButton'
                                        },
                                        {
                                            xtype: 'button',
                                            text: 'Проверить ответ',
                                            tooltip: 'Проверить ответ',
                                            iconCls: 'icon-accept',
                                            width: 200,
                                            itemId: 'getCalculateStatusButton'
                                        }           
                                    ]
                                },
                                {
                                    xtype: 'combobox',
                                    name: 'GasuMessageType',
                                    fieldLabel: 'Тип запроса',
                                    displayField: 'Display',
                                    itemId: 'cbGasuMessageType',
                                    flex: 1,
                                    store: B4.enums.GasuMessageType.getStore(),
                                    valueField: 'Value',
                                    allowBlank: false,
                                    editable: false
                                },                               
                                {
                                    xtype: 'fieldset',
                                    defaults: {
                                        labelWidth: 150,
                                        anchor: '100%',
                                        labelAlign: 'right'
                                    },
                                    title: 'Реквизиты запроса',
                                    items: [                                     
                                      
                                        {
                                            xtype: 'container',
                                            layout: 'hbox',
                                            defaults: {
                                                xtype: 'datefield',
                                                //     margin: '10 0 5 0',
                                                labelWidth: 80,
                                                labelAlign: 'right',
                                            },
                                            items: [
                                                {
                                                    xtype: 'datefield',
                                                    flex: 1,
                                                    name: 'DateFrom',
                                                    itemId: 'tfDateFrom',
                                                    fieldLabel: 'Начало периода',
                                                    format: 'd.m.Y'
                                                },
                                                {
                                                    xtype: 'datefield',
                                                    flex: 1,
                                                    name: 'DateTo',
                                                    itemId: 'tfDateTo',
                                                    fieldLabel: 'Окончание периода',
                                                    format: 'd.m.Y'
                                                },                              
                                                                                     
                                            ]
                                        }
                                       
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
                                            xtype: 'gasudatagrid',
                                            flex: 1
                                        },
                                        {
                                            xtype: 'gasufileinfogrid',
                                            flex: 1
                                        }
                                    ]
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
                            columns: 2,
                            items: [
                                {
                                    xtype: 'b4savebutton'
                                }
                            ]
                        },
                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            items: [
                                {
                                    xtype: 'b4closebutton'
                                }
                            ]
                        },
                        {
                            xtype: 'textfield',
                            name: 'Answer',
                            itemId: 'dfAnswer',
                            fieldLabel: 'Ответ ГАСУ',
                            allowBlank: true,
                            disabled: false,
                            labelWidth: 100,
                            labelAlign: 'right',
                            editable: false,
                            flex: 0.7
                        }             
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});
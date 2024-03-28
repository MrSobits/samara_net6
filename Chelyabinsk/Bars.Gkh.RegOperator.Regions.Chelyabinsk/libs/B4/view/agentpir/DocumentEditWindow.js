Ext.define('B4.view.agentpir.DocumentEditWindow', {
    extend: 'B4.form.Window',
    itemId: 'agentPIRDocumentEditWindow',
    requires: [
        'B4.form.SelectField',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.FileField',
        'B4.enums.AgentPIRDocumentType',
        'B4.form.EnumCombo'
    ],
    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'form',
    width: 1000,
    bodyPadding: 10,
    title: 'Форма редактирования документа должника агент ПИР',
    closeAction: 'hide',
    trackResetOnLoad: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 100
            },
            items: [
                {
                    xtype: 'container',
                    layout: 'hbox',
                    defaults: {
                        xtype: 'combobox',
                        //margin: '5 0 5 0',
                        labelWidth: 100,
                        labelAlign: 'right',
                    },
                    items: [
                        {
                            flex: 1,
                            xtype: 'textfield',
                            labelAlign: 'right',
                            name: 'Number',
                            fieldLabel: 'Номер'
                        },
                        {
                            xtype: 'datefield',
                            labelAlign: 'right',
                            format: 'd.m.Y',
                            name: 'DocumentDate',
                            fieldLabel: 'Дата документа',
                            allowBlank: false
                        }
                    ]
                },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    defaults: {
                        xtype: 'combobox',
                        //margin: '5 0 5 0',
                        labelWidth: 100,
                        labelAlign: 'right',
                    },
                    items: [
                        {
                            flex: 1,
                            xtype: 'numberfield',
                            labelAlign: 'right',
                            name: 'DebtSum',
                            fieldLabel: 'Сумма основного долга'
                        },
                        {
                            flex: 1,
                            xtype: 'numberfield',
                            labelAlign: 'right',
                            name: 'PeniSum',
                            fieldLabel: 'Сумма долга по пени'
                        },
                        {
                            flex: 1,
                            xtype: 'numberfield',
                            labelAlign: 'right',
                            name: 'Duty',
                            fieldLabel: 'Сумма госпошлины'
                        },
                        {
                            flex: 1,
                            xtype: 'numberfield',
                            labelAlign: 'right',
                            name: 'Repaid',
                            fieldLabel: 'Сумма погашения',
                            disabled: true,
                            hideTrigger: true
                        }
                    ]
                },
                {
                    xtype: 'b4enumcombo',
                    fieldLabel: 'Тип документа',
                    enumName: 'B4.enums.AgentPIRDocumentType',
                    name: 'DocumentType'
                },
                {
                    xtype: 'b4filefield',
                    name: 'File',
                    fieldLabel: 'Файл документа'
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
                            columns: 2,
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
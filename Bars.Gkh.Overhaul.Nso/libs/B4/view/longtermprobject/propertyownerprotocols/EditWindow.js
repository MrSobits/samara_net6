Ext.define('B4.view.longtermprobject.propertyownerprotocols.EditWindow', {
    extend: 'B4.form.Window',

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.ComboBox',
        'B4.form.SelectField',
        'B4.form.FileField',
        'B4.form.ComboBox',
        'B4.enums.PropertyOwnerProtocolType'
    ],

    modal: true,
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    width: 600,
    minHeight: 275,
    bodyPadding: 5,
    itemId: 'propertyownerprotocolsEditWindow',
    title: 'Редактирование',

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 100,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'b4combobox',
                    name: 'TypeProtocol',
                    items: B4.enums.PropertyOwnerProtocolType.getItems(),
                    displayField: 'Display',
                    valueField: 'Value',
                    fieldLabel: 'Тип протокола',
                    editable: false
                },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    padding: '0 0 5 0',
                    defaults: {
                        labelAlign: 'right',
                        flex: 1,
                        labelWidth: 100,
                        allowBlank: false
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'DocumentNumber',
                            fieldLabel: 'Номер'
                        },
                        {
                            xtype: 'datefield',
                            format: 'd.m.Y',
                            name: 'DocumentDate',
                            fieldLabel: 'Дата'
                        }
                    ]
                },
                {
                    xtype: 'b4filefield',
                    name: 'DocumentFile',
                    fieldLabel: 'Файл',
                    itemId: 'ffDocumentFile',
                    allowBlank: false
                },
                {
                    xtype: 'textarea',
                    fieldLabel: 'Описание',
                    name: 'Description',
                    flex: 1
                },
                {
                    xtype: 'fieldset',
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    title: 'Количественные характеристики',
                    items: [
                        {
                            xtype: 'container',
                            padding: '0 0 5 0',
                            layout: 'hbox',
                            defaults: {
                                xtype: 'numberfield',
                                hideTrigger: true,
                                labelAlign: 'right',
                                flex: 1,
                                allowBlank: false,
                                keyNavEnabled: false,
                                mouseWheelEnabled: false,
                                decimalSeparator: ',',
                                minValue: 0
                            },
                            items: [
                                {
                                    labelWidth: 180,
                                    name: 'NumberOfVotes',
                                    fieldLabel: 'Количество голосов (кв.м.)'
                                },
                                {
                                    labelWidth: 220,
                                    name: 'TotalNumberOfVotes',
                                    fieldLabel: 'Общее количество голосов (кв.м.)'
                                }
                            ]
                        },
                        {
                            xtype: 'numberfield',
                            hideTrigger: true,
                            labelWidth: 180,
                            labelAlign: 'right',
                            name: 'PercentOfParticipating',
                            fieldLabel: 'Доля принявших участие (%)',
                            flex: .5,
                            allowBlank: false,
                            keyNavEnabled: false,
                            mouseWheelEnabled: false,
                            decimalSeparator: ',',
                            minValue: 0
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
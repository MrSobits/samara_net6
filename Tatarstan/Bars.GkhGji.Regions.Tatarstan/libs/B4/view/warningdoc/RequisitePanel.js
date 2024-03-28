Ext.define('B4.view.warningdoc.RequisitePanel', {
    extend: 'Ext.form.Panel',
    alias: 'widget.warningdocrequisitepanel',

    requires: [
        'B4.enums.YesNo',
        'B4.store.dict.Inspector',
        'B4.form.FileField',
        'B4.form.EnumCombo',
        'B4.ux.button.Add',
        'B4.ux.button.Update'
    ],

    title: 'Реквизиты',
    bodyStyle: Gkh.bodyStyle,
    bodyPadding: 8,

    initComponent: function() {
        var me = this,
            // два разных стора, чтобы подписка на события происходила корректно
            authorInspectorStore = Ext.create('B4.store.dict.Inspector'),
            executantInspectorStore = Ext.create('B4.store.dict.Inspector'),
            defaults = {
                labelWidth: 200,
                labelAlign: 'right'
            };

        Ext.apply(me, {
            layout: {
                type: 'vbox',
                align: 'stretch'
            },
            items: [
                {
                    xtype: 'container',
                    padding: '0 11',
                    layout: {
                        type: 'hbox'
                    },
                    items: [
                        {
                            xtype: 'container',
                            flex: 0.8,
                            layout: {
                                type: 'vbox',
                                align: 'stretch'
                            },
                            defaults: {
                                labelWidth: 200,
                                labelAlign: 'right',
                                readOnly: true,
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'BaseWarning',
                                    fieldLabel: 'Основание предостережения'
                                },
                                {
                                    xtype: 'b4filefield',
                                    name: 'File',
                                    fieldLabel: 'Документ основания',
                                    readOnly: false,
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'ErknmRegistrationNumber',
                                    fieldLabel: 'Учетный номер предостережения в ЕРКНМ'
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'ErknmId',
                                    fieldLabel: 'Идентификатор в ЕРКНМ'
                                },
                                {
                                    xtype: 'datefield',
                                    name: 'ErknmRegistrationDate',
                                    fieldLabel: 'Дата присвоения учетного номера / идентификатора ЕРКНМ'
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            flex: 1,
                            layout: {
                                type: 'vbox',
                                align: 'stretch'
                            },
                            defaults: {
                                labelWidth: 280,
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'datefield',
                                    format: 'd.m.Y',
                                    name: 'TakingDate',
                                    fieldLabel: 'Срок принятия мер о соблюдении требований',
                                    allowBlank: false
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'ResultText',
                                    fieldLabel: 'Результат предостережения',
                                    allowBlank: false
                                },
                                {
                                    xtype: 'container',
                                    layout: 'hbox',
                                    align: 'stretch',
                                    margin: '0 0 0 110',
                                    defaults: {
                                        flex: 1,
                                        labelWidth: 170,
                                        labelAlign: 'right'
                                    },
                                    items: [
                                        {
                                            xtype: 'datefield',
                                            name: 'ActionStartDate',
                                            fieldLabel: 'Дата начала</br>проведения мероприятия',
                                            margin: '0 20 0 0',
                                            allowBlank: false
                                        },
                                        {
                                            xtype: 'datefield',
                                            name: 'ActionEndDate',
                                            margin: '0 0 0 20',
                                            fieldLabel: 'Дата окончания</br>проведения мероприятия'
                                        },
                                    ]
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    defaults: defaults,
                    title: 'Должностные лица',
                    items: [
                        {
                            xtype: 'b4selectfield',
                            store: authorInspectorStore,
                            textProperty: 'Fio',
                            name: 'Autor',
                            flex: 1,
                            fieldLabel: 'ДЛ, вынесшее предостережение',
                            columns: [
                                { header: 'ФИО', xtype: 'gridcolumn', dataIndex: 'Fio', flex: 1, filter: { xtype: 'textfield' } },
                                { header: 'Должность', xtype: 'gridcolumn', dataIndex: 'Position', flex: 1, filter: { xtype: 'textfield' } }
                            ],
                            dockedItems: [
                               {
                                   xtype: 'b4pagingtoolbar',
                                   displayInfo: true,
                                   store: authorInspectorStore,
                                   dock: 'bottom'
                               }
                            ],
                            allowBlank: false,
                            editable: false
                        },
                        {
                            xtype: 'b4selectfield',
                            store: executantInspectorStore,
                            name: 'Executant',
                            flex: 1,
                            fieldLabel: 'Ответственный за исполнение',
                            textProperty: 'Fio',
                            columns: [
                                { header: 'ФИО', xtype: 'gridcolumn', dataIndex: 'Fio', flex: 1, filter: { xtype: 'textfield' } },
                                { header: 'Должность', xtype: 'gridcolumn', dataIndex: 'Position', flex: 1, filter: { xtype: 'textfield' } }
                            ],
                            dockedItems: [
                               {
                                   xtype: 'b4pagingtoolbar',
                                   displayInfo: true,
                                   store: executantInspectorStore,
                                   dock: 'bottom'
                               }
                            ],
                            allowBlank: false,
                            editable: false
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    title: 'Уведомление о направлении предостережения',
                    name: 'noticeFieldset',
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    defaults: defaults,
                    items: [
                        {
                            xtype: 'container',
                            layout: 'hbox',
                            items: [
                                {
                                    xtype: 'container',
                                    flex: 0.8,
                                    layout: {
                                        type: 'vbox',
                                        align: 'stretch'
                                    },
                                    defaults: defaults,
                                    items: [
                                        {
                                            xtype: 'datefield',
                                            format: 'd.m.Y',
                                            name: 'NcOutDate',
                                            fieldLabel: 'Дата'
                                        },
                                        {
                                            xtype: 'datefield',
                                            format: 'd.m.Y',
                                            name: 'NcOutDateLatter',
                                            fieldLabel: 'Дата исходящего письма'
                                        }
                                    ]
                                },
                                {
                                    xtype: 'container',
                                    padding: '0 0 0 10',
                                    flex: 1,
                                    layout: {
                                        type: 'vbox',
                                        align: 'stretch'
                                    },
                                    defaults: {
                                        labelWidth: 280,
                                        labelAlign: 'right'
                                    },
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            name: 'NcOutNum',
                                            fieldLabel: 'Номер документа'
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'NcOutNumLatter',
                                            fieldLabel: 'Номер исходящего письма'
                                        }
                                    ]
                                }
                            ]
                        },
                        {
                            xtype: 'combobox',
                            editable: false,
                            displayField: 'Display',
                            valueField: 'Value',
                            name: 'NcOutSent',
                            fieldLabel: 'Уведомление передано',
                            flex: 1,
                            store: B4.enums.YesNo.getStore()
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    title: 'Уведомление об устранении нарушений',
                    name: 'noticeFieldset',
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    defaults: defaults,
                    items: [
                        {
                            xtype: 'container',
                            layout: 'hbox',
                            items: [
                                {
                                    xtype: 'container',
                                    flex: 0.8,
                                    layout: {
                                        type: 'vbox',
                                        align: 'stretch'
                                    },
                                    defaults: defaults,
                                    items: [
                                        {
                                            xtype: 'datefield',
                                            format: 'd.m.Y',
                                            name: 'NcInDate',
                                            fieldLabel: 'Дата'
                                        },
                                        {
                                            xtype: 'datefield',
                                            format: 'd.m.Y',
                                            name: 'NcInDateLatter',
                                            fieldLabel: 'Дата исходящего письма'
                                        },
                                        {
                                            xtype: 'combobox',
                                            editable: false,
                                            displayField: 'Display',
                                            valueField: 'Value',
                                            name: 'NcInRecived',
                                            fieldLabel: 'Уведомление получено',
                                            store: B4.enums.YesNo.getStore()
                                        }
                                    ]
                                },
                                {
                                    xtype: 'container',
                                    padding: '0 0 0 10',
                                    flex: 1,
                                    layout: {
                                        type: 'vbox',
                                        align: 'stretch'
                                    },
                                    defaults: {
                                        labelWidth: 280,
                                        labelAlign: 'right'
                                    },
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            name: 'NcInNum',
                                            fieldLabel: 'Номер документа'
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'NcInNumLatter',
                                            fieldLabel: 'Номер исходящего письма'
                                        },
                                        {
                                            xtype: 'b4enumcombo',
                                            name: 'ObjectionReceived',
                                            enumName: 'B4.enums.YesNo',
                                            fieldLabel: 'Получено возражение'
                                        }
                                    ]
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
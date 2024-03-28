Ext.define('B4.view.repairobject.performedrepairworkact.EditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.performedrepairworkacteditwindow',
    mixins: ['B4.mixins.window.ModalMask'],
    layout: { type: 'vbox', align: 'stretch' },
    height: 265,
    minHeight: 265,
    width: 800,
    minWidth: 800,
    bodyPadding: 5,
    itemId: 'performedRepairWorkActEditWindow',
    title: 'Акт выполненных работ',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires:
    [
        'B4.form.SelectField',
        'B4.view.Control.GkhDecimalField',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.store.repairobject.RepairWork'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'container',
                    layout: 'hbox',
                    defaults: {
                        labelWidth: 160,
                        align: 'stretch',
                        flex: 1,
                        margins: '2 0 2 2',
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            fieldLabel: 'Объект',
                            xtype: 'textfield',
                            readOnly: true,
                            name: 'ObjectAddress',
                            itemId: 'objectAddress'
                        },
                        {
                            fieldLabel: 'Вид работы',
                            labelWidth: 90,
                            allowBlank: false,
                            xtype: 'b4selectfield',
                            editable: false,
                            name: 'RepairWork',
                            itemId: 'sfRepairWork',
                            store: 'B4.store.repairobject.RepairWork',
                            textProperty: 'WorkName',
                            columns: [
                                { text: 'Наименование', dataIndex: 'WorkName', flex: 1 },
                                { text: 'Подрядчик', dataIndex: 'BuilderName', flex: 1 },
                                { xtype: 'datecolumn', text: 'Дата начала', dataIndex: 'DateStart', format: 'd.m.Y', flex: 1 },
                                { xtype: 'datecolumn', text: 'Дата окончания', dataIndex: 'DateEnd', format: 'd.m.Y', flex: 1 }
                            ],
                            updateDisplayedText: function (data) {
                                var me = this,
                                    text;

                                if (Ext.isString(data)) {
                                    text = data;
                                }
                                else {
                                    text = data && data[me.textProperty] ? data[me.textProperty] : '';
                                    if (Ext.isEmpty(text)) {
                                        text = data && data['Work'] && data['Work']['Name']
                                            ? data['Work']['Name']
                                            : '';
                                    }
                                }

                                me.setRawValue.call(me, text);
                            }
                        }
                    ]
                },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    defaults: {
                        labelWidth: 160,
                        align: 'stretch',
                        flex: 1,
                        margins: '2 0 2 2',
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            fieldLabel: 'Фото выполненной работы',
                            xtype: 'b4filefield',
                            editable: false,
                            name: 'ObjectPhoto'
                        },
                        {
                            fieldLabel: 'Описание фото',
                            labelWidth: 90,
                            xtype: 'textarea',
                            rows: 2,
                            name: 'ObjectPhotoDescription',
                            itemId: 'objectPhotoDescription'
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    title: 'Акт выполненных работ',
                    layout: { type: 'vbox', align: 'stretch' },
                    items: [
                        {
                            xtype: 'container',
                            layout: 'hbox',
                            defaults: {
                                labelWidth: 160,
                                align: 'stretch',
                                flex: 1,
                                margins: '2 0 2 2',
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    fieldLabel: 'Дата',
                                    xtype: 'datefield',
                                    name: 'ActDate',
                                    format: 'd.m.Y'
                                },
                                {
                                    fieldLabel: 'Номер',
                                    labelWidth: 90,
                                    xtype: 'textfield',
                                    name: 'ActNumber',
                                    itemId: 'actNumber'
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            layout: 'hbox',
                            defaults: {
                                labelWidth: 160,
                                align: 'stretch',
                                flex: 1,
                                margins: '2 0 2 2',
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    fieldLabel: 'Объем выполненных работ',
                                    xtype: 'gkhdecimalfield',
                                    name: 'PerformedWorkVolume',
                                    allowBlank: false,
                                    minValue: 0,
                                    negativeText: 'Значение не может быть отрицательным'
                                },
                                {
                                    fieldLabel: 'Сумма',
                                    labelWidth: 90,
                                    xtype: 'gkhdecimalfield',
                                    allowBlank: false,
                                    name: 'ActSum',
                                    minValue: 0,
                                    negativeText: 'Значение не может быть отрицательным'
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            layout: 'hbox',
                            defaults: {
                                labelWidth: 160,
                                align: 'stretch',
                                flex: 1,
                                margins: '2 0 2 2',
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    fieldLabel: 'Файл',
                                    xtype: 'b4filefield',
                                    editable: false,
                                    name: 'ActFile'
                                },
                                {
                                    fieldLabel: 'Описание',
                                    labelWidth: 90,
                                    xtype: 'textarea',
                                    rows: 2,
                                    name: 'ActDescription',
                                    itemId: 'actDescription'
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
                                { xtype: 'b4savebutton' }
                            ]
                        },
                        { xtype: 'tbfill' },
                        {
                            xtype: 'buttongroup',
                            columns: 2,
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

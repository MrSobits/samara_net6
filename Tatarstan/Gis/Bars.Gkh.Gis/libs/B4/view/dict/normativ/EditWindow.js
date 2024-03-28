Ext.define('B4.view.dict.normativ.EditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.normativdicteditwin',
    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'form',
    width: 700,
    minWidth: 700,
    height: 275,
    minHeight: 275,
    bodyPadding: 5,
    title: 'Добавление / редактирование нормативного параметра',

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.FileField',
        'B4.form.SelectField',
        'B4.store.dict.Municipality',
        'B4.store.dict.Service',
        'B4.view.Control.GkhDecimalField'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 150,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'b4selectfield',
                    name: 'Municipality',
                    fieldLabel: 'Муниципальный район',
                    store: 'B4.store.dict.Municipality',
                    editable: false,
                    allowBlank: false,
                    padding: '5 0 5 0',
                    columns: [
                        { xtype: 'gridcolumn', header: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
                    ]
                },
                {
                    xtype: 'b4selectfield',
                    name: 'Service',
                    fieldLabel: 'Наименование услуги',
                    store: 'B4.store.dict.Service',
                    editable: false,
                    allowBlank: false,
                    columns: [
                        { xtype: 'gridcolumn', header: 'Наименование', dataIndex: 'Name', flex: 3, filter: { xtype: 'textfield' } },
                        {
                            xtype: 'gridcolumn', header: 'Код', dataIndex: 'Code', flex: 1,
                            filter: {
                                xtype: 'numberfield',
                                hideTrigger: true,
                                keyNavEnabled: false,
                                mouseWheelEnabled: false,
                                minValue: 1,
                                operand: CondExpr.operands.eq,
                                allowDecimals: false
                            }
                        }
                    ]
                },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    defaults: {
                        labelWidth: 150,
                        labelAlign: 'right',
                        flex: 1
                    },
                    items: [
                        {
                            xtype: 'gkhdecimalfield',
                            name: 'Value',
                            fieldLabel: 'Значение'
                        },
                        {
                            xtype: 'textfield',
                            name: 'Measure',
                            fieldLabel: 'Ед. измерения'
                        }
                    ]
                },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    defaults: {
                        labelWidth: 150,
                        labelAlign: 'right',
                        flex: 1
                    },
                    items: [
                        {
                            xtype: 'datefield',
                            name: 'DateStart',
                            fieldLabel: 'Начало действия',
                            allowBlank: false,
                            format: 'd.m.Y'
                        },
                        {
                            xtype: 'datefield',
                            name: 'DateEnd',
                            fieldLabel: 'Окончание',
                            format: 'd.m.Y'
                        }
                    ]
                },
                {
                    xtype: 'textfield',
                    name: 'Description',
                    fieldLabel: 'Описание',
                    maxLength: 500
                },
                {
                    xtype: 'textfield',
                    name: 'DocumentName',
                    fieldLabel: 'Наименование документа',
                    maxLength: 100
                },
                {
                    xtype: 'b4filefield',
                    name: 'DocumentFile',
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
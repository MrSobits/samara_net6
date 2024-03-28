Ext.define('B4.view.passport.StructEditor', {
    extend: 'Ext.Container',

    alias: 'widget.structeditor',

    requires: [
        'Ext.tree.Panel',
        'Ext.menu.Menu',
        'B4.ux.button.Save',
        'B4.view.passport.AttributeTreeGrid',
        'B4.view.passport.PartTreeGrid',
        'B4.view.passport.AttributeEditor'
    ],

    layout: {
        type: 'border',
        padding: 5
    },
    structId: null,

    closable: true,
    title: 'Редактор структуры паспорта 1468',

    initComponent: function () {
        var me = this;

        Ext.apply(me, {
            defaults: {
                split: true
            },
            items: [
                {
                    xtype: 'form',
                    frame: true,
                    region: 'north',
                    entity: 'PassportStruct',
                    defaults: {
                        labelAlign: 'right'
                    },
                    tbar: {
                        items: [
                            { xtype: 'b4savebutton' }
                        ]
                    },
                    items: [
                        {
                            xtype: 'container',
                            layout: {
                                type: 'hbox',
                                align: 'stretch'
                            },
                            items: [
                                {
                                    xtype: 'container',
                                    layout: {
                                        type: 'vbox',
                                        align: 'stretch'
                                    },
                                    items: [
                                        {
                                            xtype: 'hiddenfield',
                                            name: 'Id'
                                        },
                                        {
                                            fieldLabel: 'Наименование',
                                            xtype: 'textfield',
                                            name: 'Name',
                                            labelAlign: 'right',
                                            allowBlank: false
                                        },
                                        {
                                            fieldLabel: 'Тип паспорта',
                                            name: 'PassportType',
                                            xtype: 'combobox',
                                            labelAlign: 'right',
                                            editable: false,
                                            allowBlank: false,
                                            store: [
                                                [10, 'Паспорт МКД'],
                                                [20, 'Паспорт Жилого дома'],
                                                [30, 'Паспорт ОКИ']
                                            ]
                                        }
                                    ]
                                },
                                {
                                    xtype: 'container',
                                    flex: 1
                                },
                                {
                                    xtype: 'container',
                                    itemId: 'editorRestricted',
                                    layout: 'hbox',
                                    hidden: true,
                                    items: [
                                        {
                                            xtype: 'container',
                                            border: false,
                                            html: '<span class="im-info"></span> '
                                        },
                                        {
                                            xtype: 'container',
                                            width: 520,
                                            border: false,
                                            style: 'font: 12px tahoma,arial,helvetica,sans-serif; background: transparent; margin: 3px',
                                            html: 'Поля недоступны для редактирования, так как на основании структуры создан паспорт.<br>Для редактирования структуры необходимо удалить паспорта'
                                        }                                        
                                    ]
                                }
                            ]
                        },
                        {
                            xtype: 'fieldset',
                            title: 'Начало действия',
                            collapsible: false,
                            layout: {
                                type: 'hbox',
                                align: 'stretch'
                            },
                            defaults: {
                                labelAlign: 'right',
                                labelWidth: 90
                            },
                            items: [
                                {
                                    xtype: 'numberfield',
                                    name: 'ValidFromYear',
                                    fieldLabel: 'Год',
                                    allowDecimals: false,
                                    minValue: 2000,
                                    maxValue: 3000,
                                    allowBlank: false
                                },
                                {
                                    xtype: 'combobox',
                                    name: 'ValidFromMonth',
                                    fieldLabel: 'Месяц',
                                    editable: false,
                                    allowBlank: false,
                                    store: [
                                        [1, "Январь" ],
                                        [2, "Февраль" ],
                                        [3, "Март" ],
                                        [4, "Апрель" ],
                                        [5, "Май" ],
                                        [6, "Июнь" ],
                                        [7, "Июль" ],
                                        [8, "Август" ],
                                        [9, "Сентябрь" ],
                                        [10, "Октябрь" ],
                                        [11, "Ноябрь" ],
                                        [12, "Декабрь" ]
                                    ]
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'parttreegrid',
                    region: 'west'
                },
                {
                    xtype: 'container',
                    region: 'center',
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    items: [
                        {
                            xtype: 'form',
                            entity: 'Part',
                            disabled: true,
                            padding: '0 0 3 0',
                            layout: 'form',
                            frame: true,
                            tbar: {
                                items: [
                                    { xtype: 'b4savebutton' }
                                ]
                            },
                            defaults: {
                                margin: 10,
                                labelAlign: 'right',
                                labelWidth: 70
                            },
                            fieldDefaults: {
                                msgTarget: 'side',
                                labelWidth: 75
                            },
                            defaultType: 'textfield',
                            items: [
                                {
                                    fieldLabel: 'Название',
                                    name: 'Name'
                                },
                                {
                                    name: 'Id',
                                    xtype: 'hiddenfield'
                                },
                                {
                                    fieldLabel: 'Код',
                                    name: 'Code'
                                },
                                {
                                    xtype: 'combobox',
                                    name: 'Parent',
                                    displayField: 'Id',
                                    valueField: 'Id',
                                    visible: 'f'
                                },
                                {
                                    xtype: 'combobox',
                                    name: 'PassportStruct',
                                    displayField: 'Id',
                                    valueField: 'Id',
                                    visible: 'f'
                                },
                                {
                                    title: 'Заполняется',
                                    xtype: 'fieldset',
                                    hidden: true,
                                    layout: {
                                        type: 'hbox'
                                    },
                                    defaultType: 'checkbox',
                                    defaults: {
                                        labelAlign: 'right',
                                        labelWidth: 30,
                                        padding: '10 30 10 0'
                                    },
                                    items: [
                                        {
                                            name: 'Uo',
                                            fieldLabel: 'УО'
                                        },
                                        {
                                            name: 'Pku',
                                            fieldLabel: 'ПКУ'
                                        },
                                        {
                                            name: 'Pr',
                                            fieldLabel: 'ПР'
                                        }
                                    ]
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            flex: 1,
                            layout: {
                                type: 'hbox',
                                align: 'stretch'
                            },
                            items: [
                                {
                                    xtype: 'attrtreegrid',
                                    disabled: true,
                                    flex: 1
                                },
                                {
                                    xtype: 'attreditor',
                                    disabled: true,
                                    width: 300
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
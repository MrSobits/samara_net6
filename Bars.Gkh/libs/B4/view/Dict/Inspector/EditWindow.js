Ext.define('B4.view.dict.inspector.EditWindow', {
    extend: 'B4.form.Window',
    mixins: ['B4.mixins.window.ModalMask'],
    layout: { type: 'vbox', align: 'stretch' },
    width: 700,
    height: 500,
    bodyPadding: 5,
    itemId: 'inspectorEditWindow',
    title: 'Инспектор',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.form.SelectField',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.store.dict.ZonalInspection',
        'B4.view.dict.zonalinspection.Grid',
        'B4.view.Control.GkhTriggerField',
        'B4.TextValuesOverride'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                flex: 1
            },
            items: [
                {
                    xtype: 'tabpanel',
                    border: false,
                    margins: -1,
                    items: [
                        {
                            layout: 'anchor',
                            title: 'Основная информация', 
                            border: false,
                            bodyPadding: 5,
                            margins: -1,
                            frame: true,
                            defaults: {
                                anchor: '100%',
                                labelWidth: 150,
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'Code',
                                    fieldLabel: 'Код',
                                    allowBlank: false,
                                    maxLength: 300
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'Position',
                                    fieldLabel: 'Должность',
                                    maxLength: 300
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'Fio',
                                    fieldLabel: 'ФИО',
                                    allowBlank: false,
                                    maxLength: 300
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'ShortFio',
                                    fieldLabel: 'Фамилия И.О.',
                                    maxLength: 100
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'Phone',
                                    fieldLabel: 'Телефон',
                                    maxLength: 300
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'Email',
                                    fieldLabel: 'Электронная почта',
                                    maxLength: 100,
                                    regex: /^([\w\-\'\-]+)(\.[\w\'\-]+)*@([\w\-]+\.){1,5}([A-Za-z]){2,4}$/
                                },
                                {
                                    xtype: 'checkbox',
                                    name: 'IsHead',
                                    fieldLabel: 'Начальник'
                                },
                                {
                                    xtype: 'gkhtriggerfield',
                                    name: 'inspectorZoanalInsp',
                                    itemId: 'zonInspectorsTrigerField',
                                    fieldLabel: B4.TextValuesOverride.getText('Жилищная инспекция')
                                },
                                {
                                    xtype: 'textarea',
                                    name: 'Description',
                                    fieldLabel: 'Описание',
                                    maxLength: 500
                                },
                                {
                                    xtype: 'checkbox',
                                    name: 'Active',
                                    fieldLabel: 'Действует'
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'GisGkhGuid',
                                    fieldLabel: 'ГИС ЖКХ Guid',
                                    allowBlank: false,
                                    maxLength: 300
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'ERKNMPositionGuid',
                                    fieldLabel: 'ЕРКНМ Guid',
                                    allowBlank: false,
                                    maxLength: 300
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'ERKNMTitleSignerGuid',
                                    fieldLabel: 'Title Guid',
                                    allowBlank: false,
                                    maxLength: 300
                                }
                            ]
                        },
                        {
                            layout: 'anchor',
                            title: 'Падежи ФИО',
                            border: false,
                            bodyPadding: 5,
                            margins: -1,
                            frame: true,
                            defaults: {
                                labelWidth: 100,
                                anchor: '100%',
                                maxLength: 300
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'FioGenitive',
                                    fieldLabel: 'Родительный'
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'FioDative',
                                    fieldLabel: 'Дательный'
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'FioAccusative',
                                    fieldLabel: 'Винительный'
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'FioAblative',
                                    fieldLabel: 'Творительный'
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'FioPrepositional',
                                    fieldLabel: 'Предложный'
                                }
                            ]
                        },
                        {
                            layout: 'anchor',
                            title: 'Падежи Должность',
                            border: false,
                            bodyPadding: 5,
                            margins: -1,
                            frame: true,
                            defaults: {
                                labelWidth: 100,
                                anchor: '100%',
                                maxLength: 300
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'PositionGenitive',
                                    fieldLabel: 'Родительный'
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'PositionDative',
                                    fieldLabel: 'Дательный'
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'PositionAccusative',
                                    fieldLabel: 'Винительный'
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'PositionAblative',
                                    fieldLabel: 'Творительный'
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'PositionPrepositional',
                                    fieldLabel: 'Предложный'
                                }
                            ]
                        },
                        {
                            xtype: 'inspectorsubcriptiongrid',
                            flex: 1
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
Ext.define('B4.view.controlorg.editwindow.EditWindow', {
    extend: 'B4.form.Window',

    mixins: ['B4.mixins.window.ModalMask'],
    width: 1200,
    height: 750,
    bodyPadding: 5,
    title: 'Редактирование КНО',
    alias: 'widget.controlorgeditwindow',
    layout: 'border',

    requires: [
        'B4.ux.button.Close',
        'B4.view.controlorg.editwindow.ControlTypeRelationGrid',
        'B4.view.controlorg.editwindow.TatarstanZonalInspectionGrid'
    ],
    
    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'container',
                    layout: 'anchor',
                    region: 'north',
                    defaults: {
                        anchor: '100%',
                        labelAlign: 'right'
                    },
                    items:
                        [
                            {
                                xtype: 'fieldset',
                                defaults: {
                                    anchor: '100%',
                                    labelAlign: 'right',
                                    readOnly: true,
                                    labelWidth: 270
                                },
                                title: 'Общие сведения',
                                items: [
                                    {
                                        xtype: 'textfield',
                                        name: 'ContragentName',
                                        flex: 1,
                                        fieldLabel: 'Наименование',
                                        anchor: '100%'
                                    },
                                    {
                                        xtype: 'textfield',
                                        name: 'ContragentShortName',
                                        flex: 1,
                                        fieldLabel: 'Краткое наименование',
                                        anchor: '100%'
                                    },
                                    {
                                        xtype: 'textfield',
                                        name: 'ContragentOrganizationForm',
                                        flex: 1,
                                        fieldLabel: 'Организационно-правовая форма',
                                        anchor: '100%'
                                    },
                                    {
                                        xtype: 'textfield',
                                        name: 'ContragentParentOrgName',
                                        flex: 1,
                                        fieldLabel: 'Головная организация',
                                        anchor: '100%'
                                    },
                                ]
                            },
                            {
                                xtype: 'fieldset',
                                defaults: {
                                    anchor: '100%',
                                    labelAlign: 'right',
                                    readOnly: true,
                                    labelWidth: 270
                                },
                                title: 'Реквизиты',
                                items: [
                                    {
                                        xtype: 'container',
                                        layout: 'hbox',
                                        defaults: {
                                            anchor: '100%',
                                            labelAlign: 'right',
                                            readOnly: true,
                                            labelWidth: 270,
                                            margin: '0 0 5 0'
                                        },
                                        items: [
                                            {
                                                xtype: 'textfield',
                                                name: 'ContragentInn',
                                                flex: 1,
                                                fieldLabel: 'ИНН'
                                            },
                                            {
                                                xtype: 'textfield',
                                                name: 'ContragentKpp',
                                                flex: 1,
                                                fieldLabel: 'КПП'
                                            }
                                        ]
                                    },
                                    {
                                        xtype: 'textfield',
                                        name: 'ContragentJurAddress',
                                        flex: 1,
                                        fieldLabel: 'Юридический адрес',
                                        anchor: '100%'
                                    },
                                    {
                                        xtype: 'textfield',
                                        name: 'ContragentFactAddress',
                                        flex: 1,
                                        fieldLabel: 'Фактический адрес',
                                        anchor: '100%'
                                    },
                                    {
                                        xtype: 'textfield',
                                        name: 'ContragentPostAddress',
                                        flex: 1,
                                        fieldLabel: 'Почтовый адрес',
                                        anchor: '100%'
                                    },
                                    {
                                        xtype: 'textfield',
                                        name: 'ContragentOutsideSubjectAddress',
                                        flex: 1,
                                        fieldLabel: 'Адрес за пределами субъекта',
                                        anchor: '100%'
                                    }
                                ]
                            },
                            {
                                xtype: 'fieldset',
                                defaults: {
                                    anchor: '100%',
                                    labelAlign: 'right',
                                    readOnly: true,
                                    labelWidth: 270
                                },
                                title: 'Сведения о регистрации',
                                items: [
                                    {
                                        xtype: 'container',
                                        layout: 'hbox',
                                        defaults: {
                                            anchor: '100%',
                                            labelAlign: 'right',
                                            readOnly: true,
                                            labelWidth: 270,
                                            margin: '0 0 5 0'
                                        },
                                        items: [
                                            {
                                                xtype: 'textfield',
                                                name: 'ContragentOgrn',
                                                flex: 1,
                                                fieldLabel: 'ОГРН'
                                            },
                                            {
                                                xtype: 'datefield',
                                                name: 'ContragentDateRegistration',
                                                flex: 1,
                                                fieldLabel: 'Дата регистрации',
                                                format: 'd.m.Y'
                                            }
                                        ]
                                    },
                                    {
                                        xtype: 'textfield',
                                        name: 'ContragentOgrnRegistration',
                                        flex: 1,
                                        fieldLabel: 'Орган, принявший решение о регистрации',
                                        anchor: '100%'
                                    }
                                ]
                            },
                        ]
                },
                {
                    xtype: 'tabpanel',
                    region: 'center',
                    border: false,
                    margins: -1,
                    items: [
                        {
                            xtype: 'controltyperelationgrid',
                            margins: -1
                        },
                        {
                            xtype: 'tatarstanzonalinspectiongrid',
                            margins: -1
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
                                    xtype: 'b4closebutton',
                                    listeners: {
                                        click: {
                                            fn: function () {
                                                me.close();
                                            }
                                        }
                                    }
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

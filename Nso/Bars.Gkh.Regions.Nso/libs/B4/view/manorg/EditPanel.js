Ext.define('B4.view.manorg.EditPanel', {
    extend: 'Ext.form.Panel',

    requires: [
        'B4.form.ComboBox',
        'B4.store.Contragent',
        'B4.store.contragent.Contact',
        'B4.ux.button.Save',
        'B4.form.SelectField',

        'B4.enums.TypeManagementManOrg',
        'B4.enums.YesNoNotSet'
    ],

    itemId: 'manorgEditPanel',
    minWidth: 800,
    width: 800,
    autoScroll: true,
    bodyPadding: 5,
    closable: true,
    title: 'Общие сведения',
    trackResetOnLoad: true,
    frame: true,

    initComponent: function () {
        var me = this;

        me.initialConfig = Ext.apply({
            trackResetOnLoad: true
        }, me.initialConfig);

        Ext.applyIf(me, {
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    width: 1158,
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            items: [
                                {
                                    xtype: 'b4savebutton'
                                }
                            ]
                        }
                    ]
                }
            ],
            items: [
                {
                    xtype: 'fieldset',
                    title: 'Общие сведения',
                    defaults: {
                        labelAlign: 'right',
                        labelWidth: 190,
                        anchor: '100%'
                    },
                    items: [
                        {
                            xtype: 'b4selectfield',
                            store: 'B4.store.Contragent',
                            columns: [{ text: 'Наименование', dataIndex: 'ShortName', flex: 1, filter: { xtype: 'textfield' } }],
                            name: 'Contragent',
                            fieldLabel: 'Контрагент',
                            editable: false,
                            allowBlank: false
                        },
                        {
                            xtype: 'container',
                            layout: 'column',
                            items: [
                                {
                                    xtype: 'container',
                                    columnWidth: 0.5,
                                    padding: '0 5 0 0',
                                    defaults: {
                                        labelAlign: 'right',
                                        labelWidth: 190,
                                        anchor: '100%'
                                    },
                                    layout: 'anchor',
                                    items: [
                                        {
                                            xtype: 'combobox', editable: false,
                                            name: 'TypeManagement',
                                            fieldLabel: 'Тип управления',
                                            displayField: 'Display',
                                            store: B4.enums.TypeManagementManOrg.getStore(),
                                            valueField: 'Value',
                                            allowBlank: false
                                        }
                                    ]
                                },
                                {
                                    xtype: 'container',
                                    columnWidth: 0.5,
                                    padding: '0 0 0 5',
                                    defaults: {
                                        labelAlign: 'right',
                                        labelWidth: 190,
                                        anchor: '100%'
                                    },
                                    layout: 'anchor',
                                    items: [
                                        {
                                            xtype: 'numberfield',
                                            name: 'NumberEmployees',
                                            fieldLabel: 'Общая численность сотрудников',
                                            hideTrigger: true,
                                            keyNavEnabled: false,
                                            mouseWheelEnabled: false,
                                            allowDecimals: false,
                                            labelWidth: 200,
                                            nanText: '{value} не является числом'
                                        }
                                    ]
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            columnWidth: 0.22,
                            defaults: {
                                labelAlign: 'right',
                                anchor: '100%'
                            },
                            layout: {
                                type: 'hbox',
                                align: 'stretch'
                            },
                            items: [
                                {
                                    xtype: 'checkbox',
                                    name: 'OfficialSite731',
                                    itemId: 'cbOfficialSite731',
                                    fieldLabel: 'Официальный сайт для раскрытия инф. по 731 пост.',
                                    checked: false,
                                    labelWidth: 320,
                                    width: 350
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'OfficialSite',
                                    itemId: 'tfOfficialSite',
                                    disabled: true,
                                    allowBlank: false,
                                    labelWidth: 0,
                                    flex: 1,
                                    maxLength: 100,
                                    regex: /^(\S{2,256})(\.{1})([A-Za-zА-Яа-я0-9]{2,4})$/
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    title: 'Для рейтинга УО',
                    items: [
                        {
                            xtype: 'container',
                            layout: {
                                type: 'column'
                            },
                            items: [
                                {
                                    xtype: 'container',
                                    columnWidth: 0.5,
                                    defaults: {
                                        labelAlign: 'right',
                                        labelWidth: 190,
                                        anchor: '100%'
                                    },
                                    layout: {
                                        type: 'anchor'
                                    },
                                    items: [
                                        {
                                            xtype: 'combobox', editable: false,
                                            name: 'MemberRanking',
                                            fieldLabel: 'Участвует в рейтинге УК',
                                            displayField: 'Display',
                                            store: B4.enums.YesNoNotSet.getStore(),
                                            valueField: 'Value'
                                        },
                                        {
                                            xtype: 'numberfield',
                                            name: 'CountSrf',
                                            fieldLabel: 'Количество СРФ',
                                            hideTrigger: true,
                                            keyNavEnabled: false,
                                            mouseWheelEnabled: false
                                        },
                                        {
                                            xtype: 'numberfield',
                                            name: 'CountMo',
                                            fieldLabel: 'Количество МО',
                                            hideTrigger: true,
                                            keyNavEnabled: false,
                                            mouseWheelEnabled: false
                                        },
                                        {
                                            xtype: 'b4selectfield',
                                            store: 'B4.store.contragent.Contact',
                                            columns: [{ text: 'ФИО', dataIndex: 'FullName', flex: 1, filter: { xtype: 'textfield' } }],
                                            name: 'TsjHead',
                                            fieldLabel: 'Председатель ТСЖ',
                                            textProperty: 'FullName',
                                            editable: false,
                                            hidden: true
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'TsjHeadPhone',
                                            editable: false,
                                            fieldLabel: 'Телефон',
                                            hidden: true
                                        }
                                    ]
                                },
                                {
                                    xtype: 'container',
                                    columnWidth: 0.5,
                                    defaults: {
                                        labelAlign: 'right',
                                        labelWidth: 190,
                                        anchor: '100%'
                                    },
                                    layout: {
                                        type: 'anchor'
                                    },
                                    items: [
                                        {
                                            xtype: 'numberfield',
                                            name: 'CountOffices',
                                            fieldLabel: 'Количество офисов',
                                            hideTrigger: true,
                                            keyNavEnabled: false,
                                            mouseWheelEnabled: false
                                        },
                                        {
                                            xtype: 'numberfield',
                                            name: 'ShareSf',
                                            fieldLabel: 'Доля участия СФ(%)',
                                            hideTrigger: true,
                                            keyNavEnabled: false,
                                            mouseWheelEnabled: false
                                        },
                                        {
                                            xtype: 'numberfield',
                                            name: 'ShareMo',
                                            fieldLabel: 'Доля участия МО (%)',
                                            hideTrigger: true,
                                            keyNavEnabled: false,
                                            mouseWheelEnabled: false
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'TsjHeadEmail',
                                            editable: false,
                                            fieldLabel: 'Email',
                                            hidden: true
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'CaseNumber',
                                            editable: true,
                                            fieldLabel: '№ дела',
                                            hidden: true
                                        }
                                    ]
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'textareafield',
                    name: 'Description',
                    fieldLabel: 'Описание',
                    labelAlign: 'right',
                    labelWidth: 195,
                    anchor: '100%'
                }
            ]
        });

        me.callParent(arguments);
    }

});

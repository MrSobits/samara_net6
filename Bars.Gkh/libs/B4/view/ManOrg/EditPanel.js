Ext.define('B4.view.manorg.EditPanel', {
    extend: 'Ext.form.Panel',
    alias: 'widget.manorgEditPanel',

    requires: [
        'B4.form.ComboBox',
        'B4.store.Contragent',
        'B4.ux.button.Save',
        'B4.form.SelectField',
        'B4.form.FileField',
        'B4.form.FiasSelectAddress',
        'B4.enums.TypeManagementManOrg',
        'B4.enums.YesNoNotSet'
    ],

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
                            action: 'GoToContragent',
                            items: [
                                {
                                    xtype: 'button',
                                    text: 'Перейти к контрагенту',
                                    iconCls: 'icon-arrow-out',
                                    panel: me,
                                    handler: function() {
                                        var me = this,
                                            form = me.panel.getForm(),
                                            record = form.getRecord(),
                                            contragentId = record.get('Contragent') ? record.get('Contragent').Id : 0;

                                        if (contragentId) {
                                            Ext.History.add(Ext.String.format('contragentedit/{0}/', contragentId));
                                        }
                                    }
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
                            xtype: 'b4filefield',
                            labelWidth: 350,
                            fieldLabel: 'Устав товарищества собственников жилья или кооператива',
                            name: 'DispatchFile',
                            maxFileSize: 15728640,
                            possibleFileExtensions: 'odt,ods,odp,doc,docx,xls,xlsx,ppt,tif,tiff,pptx,txt,dat,jpg,jpeg,png,pdf,gif,rtf'
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
                                    fieldLabel: 'Официальный сайт для раскрытия инф. по 731 (988) ПП',
                                    checked: false,
                                    labelWidth: 350,
                                    width: 375
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

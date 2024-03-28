Ext.define('B4.view.protocolgji.RequisitePanel', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.protocolgjiRequisitePanel',
    
    requires: [
        'B4.store.Contragent',
        'B4.form.ComboBox',
        'B4.form.SelectField',
        'B4.view.Control.GkhTriggerField'
    ],
    border: false,
    autoScroll: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            layout: {
                type: 'vbox',
                align: 'stretch'
            },
            flex: 1,
            bodyStyle: Gkh.bodyStyle,
            title: 'Реквизиты',
            border: false,
            bodyPadding: 5,
            defaults: {
                labelWidth: 150,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'container',
                    layout: 'hbox',
                    padding: '0 0 5 0',
                    defaults: {
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'protocolBaseName',
                            itemId: 'protocolBaseNameTextField',
                            fieldLabel: 'Документ-основание',
                            readOnly: true,
                            labelWidth: 130,
                            flex: 1
                        },
                        {
                            xtype: 'checkbox',
                            name: 'ToCourt',
                            fieldLabel: 'Документы переданы в суд',
                            itemId: 'cbToCourt',
                            labelWidth: 170,
                            flex: 0.7
                        }
                    ]
                },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    padding: '0 0 5 0',
                    defaults: {
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            xtype: 'gkhtriggerfield',
                            name: 'protocolInspectors',
                            itemId: 'trigfInspector',
                            fieldLabel: 'Инспекторы',
                            allowBlank: false,
                            labelWidth: 130,
                            flex: 1
                        },
                        {
                            xtype: 'datefield',
                            name: 'DateToCourt',
                            fieldLabel: 'Дата передачи документов',
                            format: 'd.m.Y',
                            itemId: 'dfDateToCourt',
                            labelWidth: 170,
                            flex: 0.7
                        }
                    ]
                },
                {
                    xtype: 'textarea',
                    name: 'Description',
                    fieldLabel: 'Примечание',
                    labelWidth: 130,
                    itemId: 'taDescriptionProtocol',
                    maxLength: 2000
                },
                {
                    xtype: 'fieldset',
                    defaults: {
                        anchor: '100%',
                        labelWidth: 120
                    },
                    title: 'Документ выдан',
                    items: [
                        {
                            xtype: 'b4combobox',
                            itemId: 'cbExecutant',
                            name: 'Executant',
                            labelAlign: 'right',
                            allowBlank: false,
                            editable: false,
                            fieldLabel: 'Тип исполнителя',
                            fields: ['Id', 'Name', 'Code'],
                            url: '/ExecutantDocGji/List',
                            queryMode: 'local',
                            triggerAction: 'all'
                        },
                        {
                            xtype: 'container',
                            layout: 'hbox',
                            defaults: {
                                labelWidth: 120,
                                labelAlign: 'right',
                                flex: 1
                            },
                            items: [
                                {
                                    xtype: 'b4selectfield',
                                    store: 'B4.store.Contragent',
                                    textProperty: 'Name',
                                    name: 'Contragent',
                                    fieldLabel: 'Контрагент',
                                    labelAlign: 'right',
                                    itemId: 'sfContragent',
                                    disabled: true,
                                    editable: false,
                                    columns: [
                                        { text: 'Наименование', dataIndex: 'ShortName', flex: 1, filter: { xtype: 'textfield' } },
                                        {
                                            text: 'Муниципальное образование', dataIndex: 'Municipality', flex: 1,
                                            filter: {
                                                xtype: 'b4combobox',
                                                operand: CondExpr.operands.eq,
                                                storeAutoLoad: false,
                                                hideLabel: true,
                                                editable: false,
                                                valueField: 'Name',
                                                emptyItem: { Name: '-' },
                                                url: '/Municipality/ListWithoutPaging'
                                            }
                                        },
                                        { text: 'ИНН', dataIndex: 'Inn', flex: 1, filter: { xtype: 'textfield' } },
                                        { text: 'КПП', dataIndex: 'Kpp', flex: 1, filter: { xtype: 'textfield' } }
                                    ]
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'PhysicalPerson',
                                    fieldLabel: 'Физическое лицо',
                                    itemId: 'tfPhysPerson',
                                    maxLength: 300,
                                    disabled: true
                                }
                            ]
                        },
                        {
                            xtype: 'fieldset',
                            disabled: true,
                            defaults: {
                                anchor: '100%',
                                labelWidth: 230,
                                labelAlign: 'right',
                                xtype: 'textfield'
                            },
                            margin: '5 0 0 0',
                            title: 'Реквизиты физ. лица',
                            itemId: 'taPhysPersonInfo',
                            items: [
                                {
                                    name: 'PhysPersonAddress',
                                    fieldLabel: 'Адрес (место жительства, телефон)',
                                    itemId: 'tfPhysPersonAddress'
                                },
                                {
                                    name: 'PhysPersonJob',
                                    fieldLabel: 'Место работы',
                                    itemId: 'tfPhysPersonJob'
                                },
                                {
                                    name: 'PhysPersonPosition',
                                    fieldLabel: 'Должность',
                                    itemId: 'tfPhysPersonPosition'
                                },
                                {
                                    name: 'PhysPersonBirthdayAndPlace',
                                    fieldLabel: 'Дата, место рождения',
                                    itemId: 'tfPhysPersonBirthdayAndPlace'
                                },
                                {
                                    name: 'PhysPersonDocument',
                                    fieldLabel: 'Документ, удостоверяющий личность',
                                    itemId: 'tfPhysPersonDocument'
                                },
                                {
                                    xtype: 'container',
                                    layout: 'hbox',
                                    disabled: false,
                                    defaults: {
                                        labelAlign: 'right',
                                        xtype: 'textfield',
                                        flex: 1,
                                        disabled: true
                                    },
                                    items: [
                                        {
                                            name: 'PhysPersonSalary',
                                            fieldLabel: 'Заработная плата',
                                            itemId: 'tfPhysPersonSalary',
                                            labelWidth: 230
                                        },
                                        {
                                            name: 'PhysPersonMaritalStatus',
                                            fieldLabel: 'Семейное положение, кол-во иждивенцев',
                                            itemId: 'tfPhysPersonMaritalStatus',
                                            labelWidth: 260
                                        }
                                    ]
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    title: 'Уведомление о проверке',
                    items: [
                        {
                            xtype: 'container',
                            layout: 'hbox',
                            defaults: {
                                labelWidth: 120,
                                labelAlign: 'right',
                                anchor: '100%',
                                flex:1
                            },
                            items: [
                                {
                                    xtype: 'datefield',
                                    format: 'd.m.Y',
                                    name: 'NoticeDocDate',
                                    flex: 1,
                                    fieldLabel: 'Дата документа'
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'NoticeDocNumber',
                                    flex: 1,
                                    fieldLabel: 'Номер документа'
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
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
                labelWidth: 170,
                labelAlign: 'right'
            },
            autoScroll: true,
            items: [
                {
                    xtype: 'container',
                    layout: 'hbox',
                    padding: '0 0 5 0',
                    defaults: {
                        labelAlign: 'right',
                        labelWidth: 170
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'protocolBaseName',
                            itemId: 'protocolBaseNameTextField',
                            fieldLabel: 'Документ-основание',
                            readOnly: true,
                            flex: 1
                        },
                        {
                            xtype: 'checkbox',
                            name: 'ToCourt',
                            fieldLabel: 'Документы переданы в суд',
                            itemId: 'cbToCourt',
                            flex: 0.7
                        }
                    ]
                },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    padding: '0 0 5 0',
                    defaults: {
                        labelAlign: 'right',
                        labelWidth: 170
                    },
                    items: [
                        {
                            xtype: 'gkhtriggerfield',
                            name: 'protocolInspectors',
                            itemId: 'trigfInspector',
                            fieldLabel: 'Инспекторы',
                            allowBlank: false,
                            flex: 1
                        },
                        {
                            xtype: 'datefield',
                            name: 'DateToCourt',
                            fieldLabel: 'Дата передачи документов',
                            format: 'd.m.Y',
                            itemId: 'dfDateToCourt',
                            flex: 0.7
                        }
                    ]
                },
                {
                    xtype: 'textarea',
                    name: 'Description',
                    fieldLabel: 'Состав административного правонарушения',
                    itemId: 'taDescriptionProtocol',
                    maxLength: 2000
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
                            xtype: 'datefield',
                            name: 'FormatDate',
                            fieldLabel: 'Дата составления',
                            format: 'd.m.Y',
                            labelWidth: 170,
                            flex: 0.7
                        },
                        {
                            xtype: 'textfield',
                            name: 'NotifNumber',
                            fieldLabel: 'Номер уведомления о месте и времени составления',
                            maxLength: 100,
                            labelWidth: 350,
                            flex: 1
                        }
                    ]
                },
                {
                    xtype: 'textfield',
                    name: 'ProceedingsPlace',
                    fieldLabel: 'Место рассмотрения дела',
                    maxLength: 1000
                },
                {
                    xtype: 'textarea',
                    name: 'Remarks',
                    fieldLabel: 'Замечания со стороны нарушителя',
                    maxLength: 1000
                },
                {
                    xtype: 'fieldset',
                    defaults: {
                        anchor: '100%',
                        labelWidth: 160
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
                            xtype: 'b4selectfield',
                            store: 'B4.store.Contragent',
                            textProperty: 'ShortName',
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
                            xtype: 'container',
                            layout: 'hbox',
                            defaults: {
                                labelWidth: 160,
                                labelAlign: 'right',
                                disabled: true,
                                flex: 1
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'PhysicalPerson',
                                    fieldLabel: 'Физическое лицо',
                                    itemId: 'tfPhysPerson',
                                    maxLength: 300
                                },
                                {
                                    xtype: 'textarea',
                                    name: 'PhysicalPersonInfo',
                                    fieldLabel: 'Реквизиты физ. лица',
                                    itemId: 'taPhysPersonInfo',
                                    maxLength: 500,
                                    labelWidth: 130
                                }
                            ]
                        },
                        {
                            xtype: 'fieldset',
                            title: 'Реквизиты физ. лица',
                            visible: false,
                            itemId: 'personFields',
                            defaults: {
                                xtype: 'textfield',
                                labelWidth: 190,
                                anchor: '100%'
                            },
                            items: [
                                {
                                    fieldLabel: 'Адрес (место жительства, телефон)',
                                    name: 'PersonAddress'
                                },
                                {
                                    fieldLabel: 'Место работы',
                                    name: 'PersonJob'
                                },
                                {
                                    fieldLabel: 'Должность',
                                    name: 'PersonPosition'
                                },
                                {
                                    fieldLabel: 'Дата, место рождения',
                                    name: 'PersonBirthDatePlace'
                                },
                                {
                                    fieldLabel: 'Документ, удостоверяющий личность',
                                    name: 'PersonDoc'
                                },
                                {
                                    fieldLabel: 'Заработная плата',
                                    name: 'PersonSalary'
                                },
                                {
                                    fieldLabel: 'Семейное положение, кол-во иждивенцев',
                                    name: 'PersonRelationship'
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
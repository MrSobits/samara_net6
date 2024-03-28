Ext.define('B4.view.localgov.EditPanel', {
    extend: 'Ext.form.Panel',

    closable: true,
    width: 500,
    minWidth: 535,
    bodyPadding: 5,
    itemId: 'localGovEditPanel',
    title: 'Общие сведения',
    trackResetOnLoad: true,
    autoScroll: true,
    frame: true,
    requires: [
        'B4.form.SelectField',
        'B4.form.ComboBox',
        'B4.store.Contragent',
        'B4.ux.button.Save',
        'B4.store.dict.Municipality',
        'B4.view.Control.GkhTriggerField',
        
        'B4.enums.OrgStateRole'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'fieldset',
                    defaults: {
                        labelWidth: 190,
                        anchor: '100%',
                        labelAlign: 'right'
                    },
                    title: 'Общая информация',
                    items: [
                        {
                            xtype: 'b4selectfield',
                            name: 'Contragent',
                            fieldLabel: 'Контрагент',
                            store: 'B4.store.Contragent',
                            allowBlank: false,
                            columns: [
                                { text: 'Наименование', dataIndex: 'ShortName', flex: 1, filter: { xtype: 'textfield' } },
                                { text: 'Муниципальный район', dataIndex: 'Municipality', flex: 1,
                                    filter: {
                                        xtype: 'b4combobox',
                                        operand: CondExpr.operands.eq,
                                        storeAutoLoad: false,
                                        hideLabel: true,
                                        editable: false,
                                        valueField: 'Name',
                                        emptyItem: { Name: '-' },
                                        url: '/Municipality/ListMoAreaWithoutPaging'
                                    }
                                },
                                { text: 'ИНН', dataIndex: 'Inn', flex: 1, filter: { xtype: 'textfield' } }
                            ],
                            editable: false
                        },
                        {
                            xtype: 'gkhtriggerfield',
                            name: 'municipalityInspectors',
                            itemId: 'localgovMunicipalitiesTrigerField',
                            fieldLabel: 'Муниципальные образования',
                            editable: false
                        },
                        {
                            xtype: 'combobox', editable: false,
                            fieldLabel: 'Статус',
                            store: B4.enums.OrgStateRole.getStore(),
                            displayField: 'Display',
                            valueField: 'Value',
                            name: 'OrgStateRole',
                            editable: false
                        },
                        {
                            xtype: 'textfield',
                            name: 'NameDepartamentGkh',
                            fieldLabel: 'Наименование подразделения, ответственного за ЖКХ',
                            allowBlank: false,
                            maxLength: 300
                        },
                        {
                            xtype: 'textarea',
                            name: 'Description',
                            fieldLabel: 'Описание',
                            maxLength: 500
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    defaults: {
                        labelWidth: 190,
                        anchor: '100%',
                        labelAlign: 'right',
                        maxLength: 50
                    },
                    title: 'Контакты',
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'Email',
                            fieldLabel: 'E-mail'
                        },
                        {
                            xtype: 'textfield',
                            name: 'Phone',
                            fieldLabel: 'Телефон',
                            allowBlank: false
                        },
                        {
                            xtype: 'textfield',
                            name: 'OfficialSite',
                            fieldLabel: 'Официальный сайт',
                            allowBlank: false
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
                                    handler: function () {
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
            ]
        });

        me.callParent(arguments);
    }
});
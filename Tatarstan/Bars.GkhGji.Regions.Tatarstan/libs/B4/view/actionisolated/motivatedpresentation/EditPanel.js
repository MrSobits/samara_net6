Ext.define('B4.view.actionisolated.motivatedpresentation.EditPanel', {
    extend: 'Ext.form.Panel',

    requires: [
        'B4.view.actionisolated.motivatedpresentation.AnnexGrid',
        'B4.view.actionisolated.motivatedpresentation.InspectionInfoGrid',
        'B4.view.actionisolated.motivatedpresentation.ViolationGrid',
        'B4.view.GjiDocumentCreateButton',
        'B4.view.Control.GkhIntField',
        'B4.view.Control.GkhButtonPrint',
        'B4.view.Control.GkhTriggerField',
        'B4.form.SelectField',
        'B4.form.FiasSelectAddress',
        'B4.ux.button.Save',
        'B4.ux.grid.toolbar.Paging'
    ],

    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    itemId: 'motivatedPresentationEditPanel',
    title: 'Мотивированное представление',
    trackResetOnLoad: true,
    closable: true,

    initComponent: function () {
        var me = this,
            inspectorStore = Ext.create('B4.store.dict.Inspector');

        Ext.applyIf(me, {
            bodyStyle: Gkh.bodyStyle,
            items: [
                {
                    xtype: 'panel',
                    bodyStyle: Gkh.bodyStyle,
                    padding: '5 0 0 0',
                    defaults: {
                        padding: '0 0 5 0'
                    },
                    border: false,
                    items: [
                        {
                            xtype: 'container',
                            layout: 'hbox',
                            items: [
                                {
                                    xtype: 'container',
                                    layout: {
                                        type: 'vbox',
                                        align: 'stretch'
                                    },
                                    defaults: {
                                        labelAlign: 'right',
                                        labelWidth: 50,
                                        width: 200
                                    },
                                    items: [
                                        {
                                            xtype: 'datefield',
                                            name: 'DocumentDate',
                                            fieldLabel: 'Дата',
                                            format: 'd.m.Y',
                                            allowBlank: false
                                        },
                                        {
                                            xtype: 'gkhintfield',
                                            name: 'DocumentYear',
                                            itemId: 'nfDocumentYear',
                                            fieldLabel: 'Год',
                                            hideTrigger: true,
                                            editable: false
                                        }
                                    ]
                                },
                                {
                                    xtype: 'container',
                                    layout: {
                                        type: 'vbox',
                                        align: 'stretch'
                                    },
                                    defaults: {
                                        labelAlign: 'right',
                                        labelWidth: 140,
                                        width: 295
                                    },
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            name: 'DocumentNumber',
                                            fieldLabel: 'Номер документа',
                                            readOnly: true
                                        },
                                        {
                                            xtype: 'gkhintfield',
                                            name: 'DocumentNum',
                                            fieldLabel: 'Номер',
                                            hideTrigger: true,
                                            readOnly: true
                                        }
                                    ]
                                },
                                {
                                    xtype: 'b4fiasselectaddress',
                                    name: 'CreationPlace',
                                    fieldLabel: 'Место составления',
                                    flatIsVisible: false,
                                    allowBlank: false,
                                    labelAlign: 'right',
                                    labelWidth: 140,
                                    width: 500,
                                    fieldsRegex: {
                                        tfHousing: {
                                            regex: /^\d+$/,
                                            regexText: 'В это поле можно вводить только цифры'
                                        },
                                        tfBuilding: {
                                            regex: /^\d+$/,
                                            regexText: 'В это поле можно вводить только цифры'
                                        }
                                    }
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'tabpanel',
                    itemId: 'motivatedPresentationTabPanel',
                    border: false,
                    flex: 1,
                    items: [
                        {
                            xtype: 'panel',
                            autoScroll: true,
                            title: 'Реквизиты',
                            bodyStyle: Gkh.bodyStyle,
                            bodyPadding: 8,
                            layout: {
                                type: 'vbox',
                                align: 'stretch'
                            },
                            items: [
                                {
                                    xtype: 'fieldset',
                                    layout: {
                                        type: 'vbox',
                                        align: 'stretch'
                                    },
                                    title: 'Должностные лица',
                                    items: [
                                        {
                                            xtype: 'container',
                                            layout: {
                                                type: 'vbox',
                                                align: 'stretch'
                                            },
                                            defaults: {
                                                xtype: 'b4selectfield',
                                                store: inspectorStore,
                                                textProperty: 'Fio',
                                                editable: false,
                                                allowBlank: false,
                                                labelWidth: 200,
                                                labelAlign: 'right',
                                                columns: [
                                                    { header: 'ФИО', xtype: 'gridcolumn', dataIndex: 'Fio', flex: 1, filter: { xtype: 'textfield' } },
                                                    { header: 'Должность', xtype: 'gridcolumn', dataIndex: 'Position', flex: 1, filter: { xtype: 'textfield' } }
                                                ],
                                                dockedItems: [
                                                    {
                                                        xtype: 'b4pagingtoolbar',
                                                        displayInfo: true,
                                                        store: inspectorStore,
                                                        dock: 'bottom'
                                                    }
                                                ],
                                                flex: 1
                                            },
                                            flex: 2,
                                            items: [
                                                {
                                                    name: 'IssuedMotivatedPresentation',
                                                    fieldLabel: 'ДЛ, вынесшее мотивированное представление'
                                                },
                                                {
                                                    name: 'ResponsibleExecution',
                                                    fieldLabel: 'Ответственный за исполнение'
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'gkhtriggerfield',
                                            itemId: 'trigfInspectors',
                                            fieldLabel: 'Инспекторы',
                                            modalWindow: true,
                                            editable: false,
                                            allowBlank: false,
                                            labelWidth: 200,
                                            labelAlign: 'right',
                                            padding: '5 0 0 0',
                                            flex: 1
                                        }
                                    ]
                                },
                                {
                                    xtype: 'motivatedpresentationviolationgrid',
                                    height: 300
                                },
                                {
                                    xtype: 'motivatedpresentationinspectioninfogrid',
                                    height: 300
                                }
                            ]
                        },
                        {
                            xtype: 'motivatedpresentationactionisolatedannexgrid'
                        }
                    ]
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    itemId: 'documentGJIToolBar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            items: [
                                {
                                    xtype: 'b4savebutton'
                                },
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-accept',
                                    text: 'Отменить',
                                    textAlign: 'left',
                                    itemId: 'btnCancel'
                                },
                                {
                                    xtype: 'gjidocumentcreatebutton'
                                },
                                {
                                    xtype: 'gkhbuttonprint'
                                },
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-delete',
                                    text: 'Удалить',
                                    textAlign: 'left',
                                    itemId: 'btnDelete'
                                },
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-accept',
                                    text: 'Сформировать внеплановую проверку',
                                    textAlign: 'left',
                                    itemId: 'btnCreateUnscheduledInspection'
                                }
                            ]
                        },
                        {
                            xtype: 'tbfill'
                        },
                        {
                            xtype: 'buttongroup',
                            itemId: 'statusButtonGroup',
                            items: [
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-accept',
                                    itemId: 'btnState',
                                    text: 'Статус',
                                    menu: []
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
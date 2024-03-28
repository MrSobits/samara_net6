Ext.define('B4.view.actionisolated.actactionisolated.EditPanel', {
    extend: 'Ext.form.Panel',

    closable: true,
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    itemId: 'actActionIsolatedEditPanel',
    title: 'Акт',
    trackResetOnLoad: true,
    autoScroll: true,
    bodyStyle: Gkh.bodyStyle,
    requires: [
        'B4.form.ComboBox',
        'B4.form.SelectField',
        'B4.store.DocumentGji',
        'B4.ux.button.Save',
        'B4.ux.button.Add',
        'B4.form.EnumCombo',
        'B4.form.FiasSelectAddress',
        'B4.view.Control.GkhIntField',
        'B4.view.Control.GkhDecimalField',
        'B4.view.Control.GkhTriggerField',
        'B4.view.Control.GkhButtonPrint',
        'B4.view.actionisolated.actactionisolated.ResultPanel',
        'B4.view.actionisolated.actactionisolated.AnnexGrid',
        'B4.enums.YesNoNotSet',
        'B4.enums.TypeDocumentGji',
        'B4.enums.AcquaintState',
        'B4.DisposalTextValues',
        'B4.view.GjiDocumentCreateButton',
        'B4.view.actionisolated.actactionisolated.ActionGrid',
        'B4.view.actionisolated.actactionisolated.DefinitionPanel'
    ],

    initComponent: function () {
        var me = this;
        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'panel',
                    border: false,
                    bodyStyle: Gkh.bodyStyle,
                    defaults: {
                        xtype: 'container',
                        border: false,
                        layout: 'hbox',
                        shrinkWrap: true,
                        margin: 6
                    },
                    items: [
                        {
                            defaults: {
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'datefield',
                                    name: 'DocumentDate',
                                    itemId: 'dfDocumentDate',
                                    fieldLabel: 'Дата',
                                    format: 'd.m.Y',
                                    allowBlank: false,
                                    labelWidth: 50,
                                    width: 200
                                },
                                {
                                    xtype: 'textfield',
                                    itemId: 'tfDocumentNumber',
                                    name: 'DocumentNumber',
                                    fieldLabel: 'Номер документа',
                                    readOnly: true,
                                    labelWidth: 140,
                                    width: 295
                                }
                            ]
                        },
                        {
                            defaults: {
                                xtype: 'gkhintfield'
                            },
                            items: [
                                {
                                    name: 'DocumentYear',
                                    itemId: 'nfDocumentYear',
                                    fieldLabel: 'Год',
                                    labelWidth: 50,
                                    width: 200,
                                    hideTrigger: true
                                },
                                {
                                    name: 'DocumentNum',
                                    itemId: 'nfDocumentNum',
                                    fieldLabel: 'Номер',
                                    readOnly: true,
                                    labelWidth: 140,
                                    width: 295,
                                    hideTrigger: true
                                },
                                {
                                    name: 'LiteralNum',
                                    itemId: 'nfLiteralNum',
                                    fieldLabel: 'Буквенный подномер',
                                    xtype: 'textfield',
                                    labelAlign: 'right',
                                    labelWidth: 140,
                                    width: 295
                                },
                                {
                                    name: 'DocumentSubNum',
                                    itemId: 'nfDocumentSubNum',
                                    fieldLabel: 'Подномер',
                                    labelWidth: 140,
                                    width: 295,
                                    hideTrigger: true
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'tabpanel',
                    itemId: 'actActionIsolatedTabPanel',
                    border: false,
                    flex: 1,
                    defaults: {
                        border: false
                    },
                    items: [
                        {
                            layout: {
                                type: 'vbox',
                                align: 'stretch'
                            },
                            bodyStyle: Gkh.bodyStyle,
                            title: 'Реквизиты',
                            border: false,
                            bodyPadding: 10,
                            items: [
                                {
                                    xtype: 'container',
                                    layout:  'hbox',
                                    items: [
                                        {
                                            xtype: 'container',
                                            layout: {
                                                type: 'vbox',
                                                align: 'stretch'
                                            },
                                            flex: 4,
                                            defaults: {
                                                allowBlank: false,
                                                labelAlign: 'right'
                                            },
                                            items: [
                                                {
                                                    xtype: 'gkhtriggerfield',
                                                    name: 'actActionIsolatedInspectors',
                                                    itemId: 'trigfInspectors',
                                                    fieldLabel: 'Инспекторы', 
                                                    labelWidth: 110
                                                },
                                                {
                                                    xtype: 'container',
                                                    padding: '5 0 0 0',
                                                    layout: 'hbox',
                                                    align: 'stretch',
                                                    defaults: {
                                                        labelWidth: 180,
                                                        labelAlign: 'right',
                                                        allowBlank: false
                                                    },
                                                    items: [
                                                        {
                                                            xtype: 'b4fiasselectaddress',
                                                            name: 'DocumentPlaceFias',
                                                            fieldLabel: 'Место составления',
                                                            flex: 1,
                                                            labelWidth: 110,
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
                                                        },
                                                        {
                                                            xtype: 'timefield',
                                                            fieldLabel: 'Время составления акта',
                                                            format: 'H:i',
                                                            altFormats: 'H:i:s',
                                                            increment: 60,
                                                            submitFormat: 'Y-m-d H:i:s',
                                                            itemId: 'tfDocumentTime',
                                                            name: 'DocumentTime',
                                                        },
                                                    ]
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'container',
                                            layout: 'vbox',
                                            defaults: {
                                                labelWidth: 180,
                                                labelAlign: 'right',
                                                allowBlank: false
                                            },
                                            items: [

                                                {
                                                    xtype: 'gkhdecimalfield',
                                                    name: 'Area',
                                                    itemId: 'nfArea',
                                                    fieldLabel: 'Площадь',
                                                    allowBlank: true
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    name: 'Flat',
                                                    itemId: 'tfFlat',
                                                    padding: '5 0 0 0',
                                                    fieldLabel: 'Квартира',
                                                    maxLength: 10
                                                }
                                            ]
                                        }
                                    ]
                                },
                                {
                                    xtype: 'fieldset',
                                    title: 'Ознакомление с результатами мероприятия',
                                    name: 'AcquaintInfo',
                                    bodyPadding: 10,
                                    items: [
                                        {
                                            xtype: 'container',
                                            layout: {
                                                type: 'hbox',
                                                align: 'stretch'
                                            },
                                            defaults: {
                                                labelAlign: 'right'
                                            },
                                            items:[
                                                {
                                                    xtype: 'b4enumcombo',
                                                    name: 'AcquaintState',
                                                    fieldLabel: 'Статус ознакомления',
                                                    enumName: 'B4.enums.AcquaintState',
                                                    labelWidth: 250,
                                                    flex: 1
                                                },
                                                {
                                                    xtype: 'datefield',
                                                    name: 'AcquaintedDate',
                                                    fieldLabel: 'Дата ознакомления',
                                                    padding: '5 0 0 0',
                                                    labelWidth: 200,
                                                    flex: 1
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'container',
                                            layout: {
                                                type: 'hbox',
                                                align: 'stretch'
                                            },
                                            defaults: {
                                                labelWidth: 250,
                                                labelAlign: 'right'
                                            },
                                            items: [
                                                {
                                                    xtype: 'textfield',
                                                    name: 'RefusedToAcquaintPerson',
                                                    fieldLabel: 'ФИО должностного лица, отказавшегося от ознакомления с актом',
                                                    hidden: true,
                                                    flex: 1
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    name: 'AcquaintedPerson',
                                                    fieldLabel: 'ФИО должностного лица, ознакомившегося с актом',
                                                    hidden: true,
                                                    flex: 1
                                                }
                                            ]
                                        }
                                    ]
                                }
                            ]
                        },
                        {
                            xtype: 'actactionisolatedactiongrid',
                            hidden: true
                        },
                        {
                            xtype: 'actactionisolatedresultpanel',
                            title: 'Результаты мероприятия'
                        },
                        {
                            xtype: 'actactionisolateddefinitionpanel',
                            title: 'Определения'
                        },
                        {
                            xtype: 'actactionisolatedannexgrid',
                            bodyStyle: 'backrgound-color:transparent;'
                        }
                    ]
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    itemId: 'documentGjiToolBar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            itemId: 'mainButtonGroup',
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
                                    xtype: 'button',
                                    iconCls: 'icon-delete',
                                    text: 'Удалить',
                                    textAlign: 'left',
                                    itemId: 'btnDelete'
                                },
                                {
                                    xtype: 'gkhbuttonprint'
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
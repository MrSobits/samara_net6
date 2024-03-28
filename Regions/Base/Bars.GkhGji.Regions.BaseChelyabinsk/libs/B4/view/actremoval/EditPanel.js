Ext.define('B4.view.actremoval.EditPanel', {
    extend: 'Ext.form.Panel',

    closable: true,
    layout: { type: 'vbox', align: 'stretch' },
    itemId: 'actRemovalEditPanel',
    title: 'Акт проверки предписания',
    trackResetOnLoad: true,
    autoScroll: true,

    requires: [
        'B4.store.DocumentGji',
        'B4.ux.button.Save',
        'B4.ux.button.Add',
        'B4.form.ComboBox',
        'B4.form.SelectField',
        'B4.enums.YesNoNotSet',
        'B4.enums.TypeDocumentGji',
        'B4.DisposalTextValues',
        'B4.view.Control.GkhIntField',
        'B4.view.Control.GkhDecimalField',
        'B4.view.Control.GkhTriggerField',
        'B4.view.Control.GkhButtonPrint',
        'B4.view.GjiDocumentCreateButton',
        'B4.view.actremoval.WitnessGrid',
        'B4.view.actremoval.PeriodGrid',
        'B4.view.actremoval.InspectedPartGrid',
        'B4.view.actremoval.DefinitionGrid',
        'B4.view.actremoval.AnnexGrid',
        'B4.view.actremoval.ProvidedDocGrid',
        'B4.view.actremoval.ViolationGrid'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'panel',
                    overflowY: 'hidden',
                    overflowX: 'hidden',
                    id: 'actRemovalTopPanel',
                    split: false,
                    collapsible: false,
                    border: false,
                    bodyStyle: Gkh.bodyStyle,
                    defaults: {
                        labelWidth: 170,
                        border: false,
                        layout: 'hbox',
                        xtype: 'panel',
                        shrinkWrap: true
                    },
                    items: [
                        {
                            padding: '10px 15px 5px 15px',
                            bodyStyle: Gkh.bodyStyle,
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
                                    maxLength: 300,
                                    labelWidth: 140,
                                    width: 295
                                }
                            ]
                        },
                        {
                            padding: '0 15px 20px 15px',
                            bodyStyle: Gkh.bodyStyle,
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
                    itemId: 'actRemovalTabPanel',
                    border: false,
                    flex: 1,
                    bodyStyle: Gkh.bodyStyle,
                    autoScroll: true,
                    listeners: {
                        render: function (p) {
                            p.body.on('scroll', function (e) {
                                var elementDisposalTopPanel = Ext.getCmp('actRemovalTopPanel').body.dom;
                                elementDisposalTopPanel.scrollLeft = e.target.scrollLeft;
                                elementDisposalTopPanel.scrollTop = e.target.scrollTop;
                            }, p);
                        }
                    },
                    defaults: {
                        minWidth: 1100
                    },
                    items: [
                        {
                            layout: {
                                type: 'vbox',
                                align: 'stretch'
                            },
                            title: 'Реквизиты',
                            border: false,
                            bodyPadding: 10,
                            frame: true,
                            autoScroll: true,
                            items: [
                                {
                                    xtype: 'container',
                                    layout: 'hbox',
                                    defaults: {
                                        labelWidth: 180,
                                        allowBlank: false,
                                        labelAlign: 'right'
                                    },
                                    items: [
                                        {
                                            xtype: 'gkhtriggerfield',
                                            name: 'actRemovalInspectors',
                                            itemId: 'actRemovalInspectorsTrigerField',
                                            fieldLabel: 'Инспекторы',
                                            flex: 2
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'AcquaintedWithDisposalCopy',
                                            fieldLabel: 'С копией приказа ознакомлен',
                                            maxLength: 250,
                                            labelWidth: 125,
                                            flex: 1
                                        },
                                        {
                                            xtype: 'gkhdecimalfield',
                                            name: 'Area',
                                            itemId: 'nfAreaActRemoval',
                                            fieldLabel: 'Площадь'
                                        }
                                    ]
                                },
                                {
                                    xtype: 'container',
                                    padding: '5 0 0 0',
                                    layout: 'hbox',
                                    defaults: {
                                        labelWidth: 180,
                                        labelAlign: 'right'
                                    },
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            flex: 2,
                                            name: 'DocumentPlace',
                                            itemId: 'tfDocumentPlace',
                                            fieldLabel: 'Место составления'
                                        },
                                        {
                                            xtype: 'timefield',
                                            fieldLabel: 'Время составления акта',
                                            format: 'H:i',
                                            submitFormat: 'Y-m-d H:i:s',
                                            minValue: '0:00',
                                            maxValue: '23:00',
                                            itemId: 'tfDocumentTime',
                                            name: 'DocumentTime',
                                            flex: 1
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'Flat',
                                            itemId: 'nfFlatActRemoval',
                                            fieldLabel: 'Квартира',
                                            maxLength: 10,
                                            labelAlign: 'right'
                                        }
                                    ]
                                },
                                {
                                    xtype: 'actRemovalWitnessGrid',
                                    padding: '5 0 0 0',
                                    height: 200
                                },
                                {
                                    xtype: 'actRemovalPeriodGrid',
                                    padding: '5 0 0 0',
                                    height: 200
                                }
                            ]
                        },
                        {
                            itemId: 'actRemovalResultTab',
                            title: 'Результаты проверки предписания',
                            border: false,
                            layout: 'fit',
                            items: [
                                {
                                    xtype: 'panel',
                                    height: 230,
                                    layout: { type: 'vbox', align: 'stretch' },
                                    split: false,
                                    collapsible: false,
                                    border: false,
                                    bodyPadding: 5,
                                    frame: true,
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            itemId: 'actRemovalBaseTextField',
                                            name: 'BaseName',
                                            fieldLabel: 'Предписание',
                                            readOnly: true,
                                            labelWidth: 140,
                                            labelAlign: 'right'
                                        },
                                        {
                                            xtype: 'container',
                                            layout: 'hbox',
                                            padding: '5px 0 5px 0',
                                            defaults: {
                                                labelWidth: 140,
                                                labelAlign: 'right'
                                            },
                                            items: [
                                                {
                                                    xtype: 'b4combobox',
                                                    editable: false,
                                                    name: 'TypeRemoval',
                                                    itemId: 'cbTypeRemoval',
                                                    fieldLabel: 'Нарушения устранены',
                                                    displayField: 'Display',
                                                    items: B4.enums.YesNoNotSet.getItems(),
                                                    valueField: 'Value',
                                                    flex: 0.35
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'textarea',
                                            name: 'Description',
                                            fieldLabel: 'Описание',
                                            labelAlign: 'right',
                                            labelWidth: 140,
                                            itemId: 'taDescriptionActRemoval',
                                            flex: 1,
                                            maxLength: 2000
                                        },
                                        {
                                            xtype: 'container',
                                            layout: { type: 'hbox', align: 'stretch' },
                                            defaults: {
                                                labelWidth: 250,
                                                labelAlign: 'right',
                                                flex: 1
                                            },
                                            items: [
                                                {
                                                    xtype: 'textarea',
                                                    name: 'PersonViolationInfo',
                                                    fieldLabel: 'Сведения о лицах, допустивших нарушения',
                                                    maxLength: 1000
                                                },
                                                {
                                                    xtype: 'textarea',
                                                    padding: '0 0 0 5',
                                                    name: 'PersonViolationActionInfo',
                                                    fieldLabel: 'Сведения о том, что нарушения были допущены в результате виновных ' +
                                                        'действий (бездействия) должностных лиц и(или) работников проверяемого лица',
                                                    maxLength: 1000
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'actRemovalViolationGrid',
                                            border: false,
                                            flex: 1
                                        }
                                    ]
                                }
                            ]
                        },
                        {
                            xtype: 'actRemovalProvidedDocGrid',
                            bodyStyle: 'backrgound-color:transparent;'
                        },
                        {
                            xtype: 'actRemovalInspectedPartGrid',
                            bodyStyle: 'backrgound-color:transparent;'
                        },
                        {
                            xtype: 'actRemovalDefinitionGrid',
                            bodyStyle: 'backrgound-color:transparent;'
                        },
                        {
                            xtype: 'actRemovalAnnexGrid',
                            bodyStyle: 'backrgound-color:transparent;'
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
                                    xtype: 'button',
                                    iconCls: 'icon-accept',
                                    text: 'Объединить акты',
                                    textAlign: 'left',
                                    itemId: 'btnMerge'
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
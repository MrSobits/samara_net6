Ext.define('B4.view.finactivity.EditPanel', {
    extend: 'Ext.form.Panel',
    closable: true,
    bodyPadding: '5px 2x 0 2px',
    bodyStyle: Gkh.bodyStyle,
    trackResetOnLoad: true,
    autoScroll: true,
    title: 'Финансовая деятельность',
    itemId: 'finActivityEditPanel',
    layout: {
        type: 'vbox',
        align: 'stretch'
    },

    requires: [
        'B4.ux.button.Save',
        'B4.view.Control.GkhDecimalField',
        'B4.form.SelectField',
        'B4.store.dict.TaxSystem',
        'B4.form.FileField',
        'B4.view.finactivity.AuditGrid',
        'B4.view.finactivity.DocByYearGrid',
        'B4.view.finactivity.ManagRealityObjGrid',
        'B4.view.finactivity.RepairCategoryGrid',
        'B4.view.finactivity.RepairSourceGrid',
        'B4.view.finactivity.CommunalServiceGrid',
        'B4.view.finactivity.ManagCategoryGrid'
    ],
    
    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'tabpanel',
                    flex: 1,
                    itemId: 'tpFinActivity',
                    border: false,
                    margins: -1,
                    items: [
                        {
                            xtype: 'form',
                            layout: {
                                type: 'vbox',
                                align: 'stretch'
                            },
                            itemId: 'tpGeneralFinActivity',
                            title: 'Общие сведения',
                            autoScroll: true,
                            border: false,
                            bodyPadding: 2,
                            margins: -1,
                            frame: true,
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
                                                    xtype: 'button',
                                                    itemId: 'saveGeneralDataButton',
                                                    text: 'Сохранить',
                                                    tooltip: 'Сохранить',
                                                    iconCls: 'icon-accept'
                                                },
                                                {
                                                    xtype: 'button',
                                                    idProperty: 'helpButton',
                                                    text: 'Справка',
                                                    tooltip: 'Справка',
                                                    iconCls: 'icon-help'
                                                }
                                            ]
                                        }
                                    ]
                                }
                            ],
                            items: [
                                {
                                    xtype: 'container',
                                    padding: 2,
                                    style: 'font: 12px tahoma,arial,helvetica,sans-serif; background: transparent; margin: 3px; border: 1px solid lightblue;',
                                    html: '<span class="im-info""></span>  Все загружаемые документы должны быть в формате PDF.'
                                },
                                {
                                    xtype: 'container',
                                    layout: 'anchor',
                                    defaults: {
                                        labelWidth: 200,
                                        labelAlign: 'right',
                                        anchor: '100%'
                                    },
                                    items: [
                                        {
                                            xtype: 'b4selectfield',
                                            name: 'TaxSystem',
                                            itemId: 'sflTaxSystem',
                                            fieldLabel: 'Система налогообложения',
                                           

                                            store: 'B4.store.dict.TaxSystem',
                                            editable: false,
                                            allowBlank: false
                                        },
                                        {
                                            xtype: 'finauditgrid',
                                            height: 200
                                        },
                                        {
                                            xtype: 'fieldset',
                                            layout: {
                                                type: 'hbox'
                                            },
                                            align: 'stretch',
                                            title: 'Для рейтинга',
                                            items: [
                                                {
                                                    xtype: 'container',
                                                    layout: {
                                                        type: 'vbox',
                                                        align: 'stretch'
                                                    },
                                                    flex: 6,
                                                    defaults: {
                                                        labelWidth: 250,
                                                        labelAlign: 'right',
                                                        anchor: '100%'
                                                    },
                                                    items: [
                                                        {
                                                            xtype: 'gkhdecimalfield',
                                                            name: 'ValueBlankActive',
                                                            fieldLabel: 'Величина чистых активов (тыс. руб.)'
                                                        },
                                                        {
                                                            xtype: 'textarea',
                                                            name: 'Description',
                                                            fieldLabel: 'Описание',
                                                            maxLength: 2000
                                                        }
                                                    ]
                                                },
                                                {
                                                    xtype: 'container',
                                                    flex: 7,
                                                    margin: '0 0 0 10px',
                                                    layout: {
                                                        type: 'vbox',
                                                        align: 'stretch'
                                                    },                                                    
                                                    defaults: {
                                                        labelWidth: 400,
                                                        labelAlign: 'right'
                                                    },
                                                    items: [
                                                        {
                                                            xtype: 'gkhdecimalfield',
                                                            name: 'ClaimDamage',
                                                            fieldLabel: 'Иски по компенсации нанесенного ущерба (тыс. руб.)'
                                                        },
                                                        {
                                                            xtype: 'gkhdecimalfield',
                                                            name: 'FailureService',
                                                            fieldLabel: 'Иски по снижению платы в связи с неоказанием услуг (тыс. руб.)'
                                                        },
                                                        {
                                                            xtype: 'gkhdecimalfield',
                                                            name: 'NonDeliveryService',
                                                            fieldLabel: 'Иски по снижению платы в связи с недопоставкой услуг (тыс. руб.)'
                                                        }
                                                    ]
                                                }
                                            ]
                                        }
                                    ]
                                }
                            ]
                        },
                        {
                            title: 'Документы',
                            xtype: 'form',
                            layout: {
                                type: 'vbox',
                                align: 'stretch'
                            },                            
                            itemId: 'tpDocsFinActivity',
                            autoScroll: true,
                            border: false,
                            bodyPadding: 2,
                            frame: true,
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
                                                    xtype: 'button',
                                                    itemId: 'saveDocsButton',
                                                    text: 'Сохранить',
                                                    tooltip: 'Сохранить',
                                                    iconCls: 'icon-accept'
                                                },
                                                {
                                                    xtype: 'button',
                                                    itemId: 'helpDocsButton',
                                                    text: 'Справка',
                                                    tooltip: 'Справка',
                                                    iconCls: 'icon-help'
                                                }
                                            ]
                                        }
                                    ]
                                }
                            ],
                            items: [
                                {
                                    xtype: 'container',
                                    padding: 2,
                                    style: 'font: 12px tahoma,arial,helvetica,sans-serif; background: transparent; margin: 3px; border: 1px solid lightblue;',
                                    html: '<span class="im-info""></span>  Все загружаемые документы должны быть в формате PDF.'
                                },
                                {
                                    xtype: 'container',
                                    items: [
                                        {
                                            xtype: 'fieldset',
                                            layout: {
                                                type: 'vbox',
                                                align: 'stretch'
                                            },                                            
                                            defaults: {
                                                labelAlign: 'right',
                                                labelWidth: 300
                                            },
                                            title: 'Бухгалтерский баланс',
                                            items: [
                                                {
                                                    xtype: 'b4filefield',
                                                    possibleFileExtensions: 'pdf',
                                                    fieldLabel: 'Бухгалтерский баланс (для организаций на УСНО - налоговая декларация)',
                                                    name: 'BookkepingBalance'
                                                },
                                                {
                                                    xtype: 'b4filefield',
                                                    possibleFileExtensions: 'pdf',
                                                    fieldLabel: 'Приложение к бухгалтерскому балансу',
                                                    name: 'BookkepingBalanceAnnex'
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'component',
                                            itemId: 'cmpGridNotation',
                                            padding: '0 0 5 5',
                                            html: '<div>Необходимо загрузить следующие документы: <br/>' +
                                                'Сметы доходов и расходов:<br/>' +
                                                '- за отчетный период;<br/>' +
                                                '- за год, предшествующий отчетному периоду.<br/>' +
                                                'Отчет о выполнении сметы доходов и расходов<br/>' +
                                                '- за отчетный период;<br/>' +
                                                '- за год, предшествующий отчетному периоду.<br/>' +
                                                'Заключение ревизионной комиссии по результатам проверки годовой бухгалтерской (финансовой) отчетности<br/>' +
                                                '- за отчетный период;<br/>' +
                                                '- за год, предшествующий отчетному периоду;<br/>' +
                                                '- за 2 года, предшествующих отчетному периоду.</div>'
                                        },
                                        {
                                            xtype: 'findocbyyeargrid',
                                            height: 340
                                        }
                                    ]
                                }
                            ]
                        },
                        {
                            xtype: 'finmanagrogrid'
                        },
                        {
                            layout: {
                                type: 'vbox',
                                align: 'stretch'
                            },
                            title: 'Ремонт и обслуживание',
                            autoScroll: true,
                            border: false,
                            bodyPadding: 2,
                            margins: -1,
                            frame: true,
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
                                                    xtype: 'button',
                                                    itemId: 'saveRepairButton',
                                                    text: 'Сохранить',
                                                    tooltip: 'Сохранить',
                                                    iconCls: 'icon-accept'
                                                },
                                                {
                                                    xtype: 'button',
                                                    itemId: 'addRepairDataRealObjButton',
                                                    text: 'Заполнить сведениями по домам',
                                                    tooltip: 'Заполнить сведениями по домам',
                                                    iconCls: 'icon-arrow-in'
                                                }
                                            ]
                                        }
                                    ]
                                }
                            ],
                            items: [
                                {
                                    xtype: 'finrepaircateggrid'
                                },
                                {
                                    xtype: 'finrepairsourcegrid',
                                    margin: '10px 0 0 0'
                                }
                            ]
                        },
                        {
                            xtype: 'fincommunalservicegrid'
                        },
                        {
                            xtype: 'finmanagcatgrid'
                        }                        
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});

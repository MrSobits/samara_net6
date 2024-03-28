Ext.define('B4.view.objectcr.HousekeeperReportEditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.housekeeperreporteditwindow',
    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'form',
    width: 700,
    height: 500,
    minWidth: 600,
    maxWidth: 800,
    bodyPadding: 5,
    title: 'Отчет старшего по дому',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires:
    [        
        'B4.ux.button.Close',
        'B4.ux.button.Save',     
        'B4.view.objectcr.HousekeeperReportFileGrid'
    ],

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 100
            },
            items: [
                {
                    xtype: 'container',
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    items: [
                        {
                            xtype: 'container',
                            layout: {
                                type: 'hbox',
                                align: 'stretch'
                            },
                            defaults: {
                                labelWidth: 100,
                                margin: '5 0 5 0',
                                labelAlign: 'right',
                                editable: false
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'FIO',
                                    flex: 1,
                                    fieldLabel: 'ФИО',
                                    maxLength: 300
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'PhoneNumber',
                                    flex: 1,
                                    fieldLabel: 'Телефон',
                                    maxLength: 300
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
                                labelWidth: 100,
                                margin: '5 0 5 0',
                                labelAlign: 'right',
                                editable: false
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'ReportNumber',
                                    flex: 1,
                                    itemId: 'tfReportNumber',
                                    fieldLabel: 'Номер отчета',
                                    maxLength: 300
                                },
                                {
                                    xtype: 'datefield',
                                    name: 'ReportDate',
                                    flex: 1,
                                    itemId: 'dfReportDate',
                                    fieldLabel: 'Дата отчета',
                                    format: 'd.m.Y'
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
                                margin: '5 0 5 0',
                                labelWidth: 100,
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'textarea',
                                    flex:1,
                                    name: 'Description',
                                    itemId: 'taDescription',
                                    fieldLabel: 'Описание',
                                    maxLength: 5000
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
                                labelWidth: 100,
                                margin: '5 0 5 0',
                                labelAlign: 'right',
                                editable: false
                            },
                            items: [
                                {
                                    xtype: 'datefield',
                                    name: 'CheckDate',
                                    flex: 1,
                                    itemId: 'dfCheckDate',
                                    fieldLabel: 'Дата проверки',
                                    format: 'd.m.Y'
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'CheckTime',
                                    flex: 1,
                                    itemId: 'tfCheckTime',
                                    fieldLabel: 'Время проверки',
                                    maxLength: 50
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
                                margin: '5 0 5 0',
                                labelWidth: 100,
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'textarea',
                                    flex: 1,
                                    name: 'Answer',
                                    itemId: 'taAnswer',
                                    fieldLabel: 'Комментарий СК',
                                    maxLength: 1500
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
                                margin: '5 0 5 0',
                                labelWidth: 100,
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'checkbox',
                                    name: 'IsArranged',
                                    fieldLabel: 'Замечания устранены'
                                }
                            ]
                        }
                                     
                    ]
                },
                {
                    xtype: 'tabpanel',
                    enableTabScroll: true,
                    flex: 1,
                    items: [
                        { xtype: 'housekeeperreportfilegrid' },
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
                            items: [{ xtype: 'b4savebutton' }]
                        },
                        { xtype: 'tbfill' },
                        {
                            xtype: 'buttongroup',
                            items: [{ xtype: 'b4closebutton' }]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});
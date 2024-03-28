Ext.define('B4.view.protocol197.EditPanel', {
    extend: 'Ext.form.Panel',

    closable: true,
    layout: { type: 'vbox', align: 'stretch' },
    itemId: 'protocol197EditPanel',
    title: 'Протокол по ст.19.7 КоАП РФ',
    trackResetOnLoad: true,
    autoScroll: true,
    bodyStyle: Gkh.bodyStyle,
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Save',
        'B4.view.Control.GkhIntField',
        'B4.view.Control.GkhButtonPrint',
        'B4.view.protocol197.RequisitePanel',
        'B4.view.protocol197.ArticleLawGrid',
        'B4.view.protocol197.AnnexGrid',
        'B4.view.protocol197.ViolationEditPanel'
    ],
    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'panel',
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
                    shrinkWrap: true,
                    overflowY: 'hidden',
                    overflowX: 'hidden',
                    id: 'protocol197TopPanel',
                    padding: 5,
                    items: [
                        {
                            padding: '5px 15px 5px 15px',
                            bodyStyle: Gkh.bodyStyle,
                            defaults: {
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'datefield',
                                    name: 'DocumentDate',
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
                                    labelWidth: 130,
                                    maxLength: 300,
                                    width: 250
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'FormatPlace',
                                    fieldLabel: 'Место и время составления протокола:',
                                    format: 'd.m.Y',
                                    labelWidth: 240,
                                    width: 600,
                                    maxLength: 500
                                },
                                {
                                    xtype: 'numberfield',
                                    name: 'FormatHour',
                                    margin: '0 0 0 10',
                                    fieldLabel: '',
                                    labelWidth: 25,
                                    width: 45,
                                    maxValue: 23,
                                    minValue: 0
                                },
                                {
                                    xtype: 'label',
                                    text: ':',
                                    margin: '5'
                                },
                                {
                                    xtype: 'numberfield',
                                    name: 'FormatMinute',
                                    width: 45,
                                    maxValue: 59,
                                    minValue: 0
                                }

                            ]
                        },
                        {
                            padding: '5px 15px 5px 15px',
                            bodyStyle: Gkh.bodyStyle,
                            defaults: {
                                xtype: 'gkhintfield',
                                hideTrigger: true
                            }
                        }
                    ]
                },
                {
                    xtype: 'tabpanel',
                    border: false,
                    flex: 1,
                    itemId: 'protocol197TabPanel',
                    autoScroll: true,
                    defaults: {
                        minWidth: 1400
                    },
                    listeners: {
                        render: function (p) {
                            p.body.on('scroll', function (e) {
                                var elementDisposalTopPanel = Ext.getCmp('protocol197TopPanel').body.dom;
                                elementDisposalTopPanel.scrollLeft = e.target.scrollLeft;
                                elementDisposalTopPanel.scrollTop = e.target.scrollTop;
                            }, p);
                        }
                    },
                    items: [
                        {
                            xtype: 'protocol197RequisitePanel',
                            flex: 1
                        },
                        {
                            xtype: 'protocol197ViolationEditPanel',
                            flex: 1
                        },
                        {
                            xtype: 'protocol197ArticleLawGrid',
                            flex: 1
                        },
                        {
                            xtype: 'panel',
                            layout: {
                                type: 'vbox',
                                align: 'stretch'
                            },
                            bodyStyle: Gkh.bodyStyle,
                            title: 'Деятельность',
                            border: false,
                            bodyPadding: 5,
                            defaults: {
                                labelAlign: 'right',
                                labelWidth: 170
                            },
                            items: [
                                {
                                    xtype: 'gkhtriggerfield',
                                    name: 'protocol197Directions',
                                    itemId: 'protocol197DirectionsTrigerField',
                                    fieldLabel: 'Направление деятельности',
                                    allowBlank: true
                                },
                                {
                                    xtype: 'b4selectfield',
                                    store: 'B4.store.dict.NormativeDoc',
                                    fieldLabel: 'Правовое основание',
                                    name: 'NormativeDoc',
                                    itemId: 'sfNormativeDoc',
                                    hidden: true
                                }
                            ]
                        },
                        {
                            xtype: 'protocol197AnnexGrid',
                            flex: 1
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
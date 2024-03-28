Ext.define('B4.view.resolutionrospotrebnadzor.EditPanel', {
    extend: 'Ext.form.Panel',

    itemId: 'resolutionrospotrebnadzorEditPanel',
    title: 'Постановление Роспотребнадзора',
    trackResetOnLoad: true,
    autoScroll: true,
    closable: true,
    layout: {
        type: 'vbox',
        align: 'stretch'
    },

    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Save',
        'B4.form.ComboBox',
        'B4.form.SelectField',
        'B4.view.Control.GkhIntField',
        'B4.view.Control.GkhDecimalField',
        'B4.store.dict.ExecutantDocGji',
        'B4.store.dict.Municipality',
        'B4.store.dict.Inspector',
        'B4.store.dict.SanctionGji',
        'B4.store.Contragent',
        'B4.view.Control.GkhButtonPrint',
        'B4.view.resolutionrospotrebnadzor.RequisitePanel',
        'B4.view.resolutionrospotrebnadzor.DisputeGrid',
        'B4.view.resolutionrospotrebnadzor.DefinitionGrid',
        'B4.view.resolutionrospotrebnadzor.PayFineGrid',
        'B4.view.resolutionrospotrebnadzor.ViolationGrid',
        'B4.view.resolutionrospotrebnadzor.AnnexGrid',
        'B4.view.resolutionrospotrebnadzor.ArticleLawGrid',
        'B4.enums.TypeInitiativeOrgGji'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'panel',
                    autoScroll: true,
                    split: false,
                    collapsible: false,
                    border: false,
                    bodyStyle: Gkh.bodyStyle,
                    defaults: {
                        labelWidth: 170,
                        border: false,
                        xtype: 'panel',
                        layout: 'hbox',
                        shrinkWrap: true
                    },
                    items: [
                        {
                            padding: '10px 15px 5px 15px',
                            bodyStyle: Gkh.bodyStyle,
                            defaults: {
                                labelAlign: 'right',
                                allowBlank: false
                            },
                            items: [
                                {
                                    xtype: 'datefield',
                                    name: 'DocumentDate',
                                    fieldLabel: 'Дата',
                                    format: 'd.m.Y',
                                    allowBlank: false,
                                    labelWidth: 50,
                                    width: 200,
                                    itemId: 'documentDate'
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'DocumentNumber',
                                    itemId: 'tfDocumentNumber',
                                    fieldLabel: 'Номер документа',
                                    labelWidth: 140,
                                    maxlength: 255,
                                    width: 295,
                                    allowBlank: false
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
                                    fieldLabel: 'Год',
                                    itemId: 'nfDocumentYear',
                                    labelWidth: 50,
                                    maxlength: 4,
                                    width: 200,
                                    hideTrigger: true,
                                    allowBlank: true
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'tabpanel',
                    border: false,
                    flex: 1,
                    defaults: {
                        border: false
                    },
                    items: [
                        {
                            xtype: 'resolutionRospotrebnadzorRequisitePanel',
                            flex: 1
                        },
                        {
                            xtype: 'resolutionRospotrebnadzorDisputeGrid',
                            flex: 1
                        },
                        {
                            xtype: 'resolutionRospotrebnadzorDefinitionGrid',
                            flex: 1
                        },
                        {
                            xtype: 'resolutionRospotrebnadzorPayFineGrid',
                            flex: 1
                        },
                        {
                            xtype: 'resolutionRospotrebnadzorAnnexGrid',
                            flex: 1
                        },
                        {
                            xtype: 'resolutionRospotrebnadzorArticleLawGrid',
                            flex: 1
                        },
                        {
                            xtype: 'resolutionRospotrebnadzorViolationGrid',
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
                                }
                            ]
                        },
                        { xtype: 'tbfill' },
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
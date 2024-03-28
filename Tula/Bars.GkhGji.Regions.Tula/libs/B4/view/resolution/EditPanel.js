Ext.define('B4.view.resolution.EditPanel', {
    extend: 'Ext.form.Panel',

    closable: true,
    layout: { type: 'vbox', align: 'stretch' },
    itemId: 'resolutionEditPanel',
    title: 'Постановление',
    trackResetOnLoad: true,
    autoScroll: true,

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
        'B4.view.resolution.RequisitePanel',
        'B4.view.resolution.DisputeGrid',
        'B4.view.resolution.DefinitionGrid',
        'B4.view.resolution.PayFineGrid',
        'B4.view.resolution.AnnexGrid',
        'B4.enums.TypeInitiativeOrgGji',
        'B4.enums.YesNoNotSet',
        'B4.view.GjiDocumentCreateButton'
    ],

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'panel',
                    layout: 'form',
                    split: false,
                    collapsible: false,
                    border: false,
                    bodyStyle: 'background: none repeat scroll 0 0 #DFE9F6',
                    defaults: {
                        labelWidth: 170,
                        border: false,
                        xtype: 'container',
                        layout: 'hbox'
                    },
                    items: [
                        {
                            padding: '10px 15px 5px 15px',
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
                                    name: 'DocumentNumber',
                                    itemId: 'tfDocumentNumber',
                                    fieldLabel: 'Номер документа',
                                    labelWidth: 140,
                                    maxlength: 300,
                                    width: 295
                                }
                                
                            ]
                        },
                        {
                            padding: '0 15px 20px 15px',
                            defaults: {
                                xtype: 'gkhintfield'
                            },
                            items: [
                                {
                                    name: 'DocumentYear',
                                    fieldLabel: 'Год',
                                    itemId: 'nfDocumentYear',
                                    labelWidth: 50,
                                    width: 200,
                                    hideTrigger: true
                                },
                                {
                                    name: 'DocumentNum',
                                    fieldLabel: 'Номер',
                                    itemId: 'nfDocumentNum',
                                    labelWidth: 140,
                                    width: 295,
                                    hideTrigger: true
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
                    border: false,
                    flex: 1,
                    defaults: {
                        border: false
                    },
                    items: [
                        {
                            xtype: 'resolutionRequisitePanel',
                            flex: 1
                        },
                        {
                            xtype: 'resolutionDisputeGrid',
                            flex: 1
                        },
                        {
                            xtype: 'resolutionDefinitionGrid',
                            flex: 1
                        },
                        {
                            xtype: 'resolutionPayFineGrid',
                            flex: 1
                        },
                        {
                            xtype: 'resolutionAnnexGrid',
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
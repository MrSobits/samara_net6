Ext.define('B4.view.documentsgjiregister.NullInspEditWindow', {
    extend: 'B4.form.Window',

    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: { type: 'vbox', align: 'stretch' },
    width: 800,
    height: 410,
    minHeight: 410,
    maxHeight: 600,
    bodyPadding: 5,
    itemId: 'disposalNullInspEditWindow',
    title: '',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Save',
        'B4.form.SelectField',
        'B4.store.dict.Inspector',
        'B4.view.Control.GkhIntField',
        'B4.view.Control.GkhButtonPrint',
        'B4.view.Control.GkhTriggerField',
        'B4.view.documentsgjiregister.NullInspAnnexGrid',
        'B4.DisposalTextValues'
    ],

    initComponent: function () {
        var me = this;

        me.title = B4.DisposalTextValues.getSubjectiveCase() + ' вне инспекторской деятельности';

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 100
            },
            items: [
                {
                    xtype: 'container',
                    border: false,
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    items: [
                        {
                            xtype: 'panel',
                            split: false,
                            collapsible: false,
                            border: false,
                            bodyStyle: Gkh.bodyStyle,
                            defaults: {
                                border: false
                            },
                            items: [
                                {
                                    xtype: 'panel',
                                    padding: '0 0 5 0',
                                    anchor: '100%',
                                    bodyStyle: Gkh.bodyStyle,
                                    layout: {
                                        pack: 'start',
                                        type: 'hbox'
                                    },
                                    defaults: {
                                        flex: 1,
                                        labelWidth: 150,
                                        labelAlign: 'right'
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
                                            xtype: 'textfield',
                                            name: 'DocumentNumber',
                                            itemId: 'tfDocumentNumber',
                                            fieldLabel: 'Номер документа'
                                        }
                                    ]
                                },
                                {
                                    xtype: 'panel',
                                    padding: '0 0 5 0',
                                    anchor: '100%',
                                    bodyStyle: Gkh.bodyStyle,
                                    layout: {
                                        pack: 'start',
                                        type: 'hbox'
                                    },
                                    defaults: {
                                        xtype: 'gkhintfield',
                                        flex: 1,
                                        labelWidth: 150
                                    },
                                    items: [
                                        {
                                            name: 'DocumentYear',
                                            itemId: 'nfDocumentYear',
                                            fieldLabel: 'Год',
                                            hideTrigger: true
                                        },
                                        {
                                            name: 'DocumentNum',
                                            itemId: 'nfDocumentNum',
                                            fieldLabel: 'Номер',
                                            hideTrigger: true
                                        }
                                    ]
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'tabpanel',
                    layout: { type: 'vbox', align: 'stretch' },
                    flex: 1,
                    itemId: 'disposalTabPanel',
                    border: false,
                    items: [
                        {
                            layout: 'anchor',
                            title: 'Реквизиты',
                            border: false,
                            bodyPadding: 5,
                            margins: -1,
                            frame: true,
                            defaults: {
                                anchor: '100%',
                                labelAlign: 'right',
                                labelWidth: 200
                            },
                            items: [
                                {
                                    xtype: 'b4selectfield',
                                    store: 'B4.store.dict.Inspector',
                                    textProperty: 'Fio',
                                    name: 'IssuedDisposal',
                                    fieldLabel: 'ДЛ, вынесшее ' + B4.DisposalTextValues.getSubjectiveCase(),
                                    columns: [
                                        { header: 'ФИО', xtype: 'gridcolumn', dataIndex: 'Fio', flex: 1, filter: { xtype: 'textfield' } },
                                        { header: 'Должность', xtype: 'gridcolumn', dataIndex: 'Position', flex: 1, filter: { xtype: 'textfield' } }
                                    ],
                                    dockedItems: [
                                       {
                                           xtype: 'b4pagingtoolbar',
                                           displayInfo: true,
                                           store: 'B4.store.dict.Inspector',
                                           dock: 'bottom'
                                       }
                                    ],
                                    itemId: 'sflIssued',
                                    editable: false,
                                    allowBlank: false
                                },
                                {
                                    xtype: 'b4selectfield',
                                    store: 'B4.store.dict.Inspector',
                                    name: 'ResponsibleExecution',
                                    itemId: 'sflRespExec',
                                    fieldLabel: 'Ответственный за исполнение',
                                    textProperty: 'Fio',
                                    editable: false,
                                    columns: [
                                        { header: 'ФИО', xtype: 'gridcolumn', dataIndex: 'Fio', flex: 1, filter: { xtype: 'textfield' } },
                                        { header: 'Должность', xtype: 'gridcolumn', dataIndex: 'Position', flex: 1, filter: { xtype: 'textfield' } }
                                    ],
                                    dockedItems: [
                                       {
                                           xtype: 'b4pagingtoolbar',
                                           displayInfo: true,
                                           store: 'B4.store.dict.Inspector',
                                           dock: 'bottom'
                                       }
                                    ]
                                },
                                {
                                    xtype: 'textarea',
                                    dataIndex: 'Description',
                                    fieldLabel: 'Описание',
                                    maxLength: 500,
                                    itemId: 'taDescription'
                                }
                            ]
                        },
                        {
                            xtype: 'disposalnullinspannexgrid',
                            flex: 1
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
                            columns: 1,
                            items: [
                                {
                                    xtype: 'b4savebutton'
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
                        },
                        {
                            xtype: 'buttongroup',
                            columns: 1,
                            items: [
                                {
                                    xtype: 'b4closebutton'
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
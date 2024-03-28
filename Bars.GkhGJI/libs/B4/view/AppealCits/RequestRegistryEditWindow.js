Ext.define('B4.view.appealcits.RequestRegistryEditWindow', {
    extend: 'B4.form.Window',

    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: { type: 'vbox', align: 'stretch' },
    width: 600,
    minWidth: 520,
    minHeight: 310,
    height: 500,
    bodyPadding: 5,
    itemId: 'appealCitsRequestRegistryEditWindow',
    title: 'Форма редактирования запроса',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.form.FileField',
        'B4.form.SelectField',
        'B4.store.dict.CompetentOrgGji',
        'B4.view.appealcits.RequestAnswerRegistryGrid',
        'B4.ux.button.Close',
        'B4.ux.button.Save'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 170,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'b4selectfield',
                    store: 'B4.store.dict.CompetentOrgGji',
                    textProperty: 'Name',
                    name: 'CompetentOrg',
                    fieldLabel: 'Компетентная организация',
                    editable: false,
                    columns: [
                        { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1 },
                        { header: 'Код', xtype: 'gridcolumn', dataIndex: 'Code', flex: 1 }
                    ],
                    allowBlank: false
                },
                {
                    xtype: 'textfield',
                    name: 'DocumentName',
                    fieldLabel: 'Документ',
                    maxLength: 300
                },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    defaults: {
                        xtype: 'datefield',
                        labelWidth: 170,
                        labelAlign: 'right',
                        flex: 1
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'DocumentNumber',
                            fieldLabel: 'Номер',
                            maxLength: 300
                        },
                        {
                            xtype: 'datefield',
                            name: 'DocumentDate',
                            allowblank:false,
                            fieldLabel: 'Дата документа',
                            format: 'd.m.Y',
                            labelWidth: 150 
                        }
                    ]
                },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    padding: '5 0 5 0',
                    defaults: {
                        xtype: 'datefield',
                        labelWidth: 170,
                        labelAlign: 'right',
                        format: 'd.m.Y',
                        flex: 1
                    },
                    items: [
                        {
                            name: 'PerfomanceDate',
                            fieldLabel: 'Срок исполнения',
                            labelWidth: 170 
                        },
                        {
                            name: 'PerfomanceFactDate',
                            fieldLabel: 'Дата факт. исполнения',
                            labelWidth: 150
                        }
                    ]
                },
                {
                    xtype: 'b4filefield',
                    name: 'SignedFile',
                    fieldLabel: 'Файл',
                    fileController: 'action/FileTransport',
                    downloadAction: 'GetFileFromPrivateServer',
                    editable: false
                },
                {
                    xtype: 'textarea',
                    name: 'Description',
                    disabled: true,
                    fieldLabel: 'Описание',
                    maxLength: 500,
                    flex: 1
                },
                {
                    xtype: 'tabpanel',
                    itemId: 'requestRegistryAnswerTabPanel',
                    flex: 1,
                    border: false,
                    items: [
                        {
                            xtype: 'appcitrequestanswerreggrid',
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
                            columns: 2,
                            items: [
                                //{
                                //    xtype: 'b4savebutton'
                                //}
                            ]
                        },
                        {
                            xtype: 'tbfill'
                        },
                        {
                            xtype: 'buttongroup',
                            columns: 2,
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
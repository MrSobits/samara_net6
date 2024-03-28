Ext.define('B4.view.claimwork.LawSuitDebtWorkSSPDocumentEditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.lawsuitdebtworksspdoceditwindow',
    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: 'form',
    width: 700,
    minWidth: 700,
    minHeight: 270,
    maxHeight: 270,
    bodyPadding: 7,
    title: 'Документ',
    closeAction: 'destroy',
    trackResetOnLoad: true,

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.enums.ClaimWork.TypeLawsuitDocument',
        'B4.enums.ClaimWork.CollectDebtFrom',
        'B4.form.EnumCombo',
        'B4.form.FileField'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelAlign: 'right',
                labelWidth: 150
            },
            items: [
                {
                    xtype: 'b4enumcombo',
                    name: 'TypeLawsuitDocument',
                    fieldLabel: 'Документ',
                    allowBlank: false,
                    labelAlign: 'right',
                    enumName: 'B4.enums.ClaimWork.TypeLawsuitDocument'
                },
                {
                    xtype: 'container',
                    padding: '0 0 5 0',
                    layout: {
                        type: 'hbox',
                        align: 'stretch'
                    },
                    defaults: {
                        labelAlign: 'right',
                        labelWidth: 150,
                        flex: 1
                    },
                    items: [
                        {
                            xtype: 'b4enumcombo',
                            name: 'CollectDebtFrom',
                            fieldLabel: 'ИП в отношении',
                            enumName: 'B4.enums.ClaimWork.CollectDebtFrom',
                            labelAlign: 'right'
                        },
                        {
                            xtype: 'b4selectfield',
                            store: 'B4.store.dict.JurInstitution',
                            name: 'Rosp',
                            fieldLabel: 'Наименование РОСП',
                            labelAlign: 'right',
                            textProperty: 'ShortName',
                            columns: [
                                { dataIndex: 'Municipality', text: 'Муниципальное образование', flex: 1, filter: { xtype: 'textfield' } },
                                { dataIndex: 'ShortName', text: 'Краткое наименование', flex: 1, filter: { xtype: 'textfield' } }
                            ],
                            listeners: {
                                'beforeload': function (store, operation) {
                                    operation.params['type'] = 20;
                                }
                            },
                            editable: false
                        },
                    ]
                },
                {
                    xtype: 'container',
                    padding: '0 0 5 0',
                    layout: 'hbox',
                    defaults: {
                        labelAlign: 'right',
                        labelWidth: 150,
                        flex: 1
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'NumberString',
                            fieldLabel: 'Номер',
                            allowBlank: false,
                            maxLength: 50,
                            labelAlign: 'right'
                        },
                        {
                            xtype: 'datefield',
                            name: 'Date',
                            fieldLabel: 'Дата',
                            allowBlank: false,
                            format: 'd.m.Y'
                        }
                    ]
                },
                {
                    xtype: 'b4filefield',
                    name: 'File',
                    allowBlank: false,
                    fieldLabel: 'Файл'
                },
                {
                    xtype: 'textarea',
                    name: 'Note',
                    fieldLabel: 'Примечание',
                    height:70,
                    maxLength: 255
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            items: [
                                { xtype: 'b4savebutton' }
                            ]
                        },
                        { xtype: 'tbfill' },
                        {
                            xtype: 'buttongroup',
                            items: [
                                { xtype: 'b4closebutton' }
                            ]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});
Ext.define('B4.view.manorglicense.LicensePrescriptionEditWindow', {
    extend: 'B4.form.Window',

    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: { type: 'vbox', align: 'stretch' },
    width: 800,
    minWidth: 500,
    minHeight: 310,
    height: 310,
    bodyPadding: 5,
    itemId: 'licensePrescriptionEditWindow',
    title: 'Постановление',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.form.FileField',
        'B4.form.SelectField',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.store.dict.SanctionGji',
        'B4.enums.YesNoNotSet',
        'B4.store.dict.ArticleLawGji'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 100,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'container',
                    layout: {
                        type: 'hbox',
                        align: 'stretch'
                    },
                    defaults: {
                        padding: '0 5 5 0',
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'DocumentNumber',
                            labelAlign: 'right',
                            fieldLabel: 'Номер документа',
                            flex: 1
                        },
                        {
                            xtype: 'datefield',
                            name: 'DocumentDate',
                            fieldLabel: 'от',
                            format: 'd.m.Y',
                            flex: 0.5
                        },
                        {
                            xtype: 'datefield',
                            name: 'ActualDate',
                            fieldLabel: 'Вступило в силу',
                            format: 'd.m.Y',
                            flex: 0.5
                        }
                    ]
                },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    padding: '0 0 5 5',
                    defaults: {
                        labelAlign: 'right',
                        flex: 1
                    },
                    items: [
                        {
                            xtype: 'b4selectfield',
                            store: 'B4.store.dict.ArticleLawGji',
                            textProperty: 'Name',
                            name: 'ArticleLawGji',
                            fieldLabel: 'Статья закона',
                            editable: false,
                            columns: [
                                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1 },
                                { header: 'Код', xtype: 'gridcolumn', dataIndex: 'Code', flex: 1 }
                            ]
                        },
                        {
                            xtype: 'b4selectfield',
                            store: 'B4.store.dict.SanctionGji',
                            textProperty: 'Name',
                            name: 'SanctionGji',
                            fieldLabel: 'Вид санкции',
                            editable: false,
                            columns: [
                                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1 },
                                { header: 'Код', xtype: 'gridcolumn', dataIndex: 'Code', flex: 1 }
                            ]
                        },
                        {
                            xtype: 'gkhdecimalfield',
                            name: 'Penalty',
                            fieldLabel: 'Сумма штрафа',
                            itemId: 'nfPenaltyAmount'
                        }
                    ]
                },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    padding: '0 0 5 5',
                    defaults: {
                        labelAlign: 'right',
                        flex: 1
                    },
                    items: [
                        {
                            xtype: 'combobox',
                            name: 'YesNoNotSet',
                            fieldLabel: 'Оспорено',
                            itemId: 'cbTermination',
                            store: B4.enums.YesNoNotSet.getStore(),
                            valueField: 'Value',
                            displayField: 'Display',
                            flex: 1,
                            editable: false
                        },
                        {
                            xtype: 'b4filefield',
                            name: 'FileInfo',
                            fieldLabel: 'Файл',
                            flex: 2,
                            editable: false
                        },
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
                                {
                                    xtype: 'b4savebutton'
                                }
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
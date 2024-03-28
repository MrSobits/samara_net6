Ext.define('B4.view.prescription.CancelEditWindow', {
    extend: 'B4.form.Window',

    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    width: 500,
    minHeight: 300,
    bodyPadding: 5,
    itemId: 'prescriptionCancelEditWindow',
    title: 'Форма решения об отмене',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.form.SelectField',
        'B4.store.dict.Inspector',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.view.Control.GkhButtonPrint',
        
        'B4.enums.YesNoNotSet'
    ],

    /*
    Перекрывается в модуле GkhGji.Regions.Smolensk
    */
    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                allowBlank: false,
                labelWidth: 150,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'container',
                    layout: 'hbox',
                    padding: '0 0 5 0',
                    defaults: {
                        labelAlign: 'right',
                        labelWidth: 150,
                        allowBlank: false,
                        flex: 1
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'DocumentNum',
                            fieldLabel: 'Номер',
                            maxLength: 300
                        },
                        {
                            xtype: 'datefield',
                            name: 'DocumentDate',
                            fieldLabel: 'Дата',
                            format: 'd.m.Y',
                            labelWidth: 130
                        }
                    ]
                },
                {
                    xtype: 'b4selectfield',
                    store: 'B4.store.dict.Inspector',
                    textProperty: 'Fio',
                    name: 'IssuedCancel',
                    fieldLabel: 'ДЛ, вынесшее решение',
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
                    xtype: 'container',
                    layout: 'hbox',
                    padding: '0 0 5 0',
                    defaults: {
                        allowBlank: false,
                        labelAlign: 'right',
                        labelWidth: 150,
                        flex: 1
                    },
                    items: [
                        {
                            xtype: 'combobox', editable: false,
                            floating: false,
                            name: 'IsCourt',
                            fieldLabel: 'Отменено судом',
                            displayField: 'Display',
                            store: B4.enums.YesNoNotSet.getStore(),
                            valueField: 'Value'
                        },
                        {
                            xtype: 'datefield',
                            name: 'DateCancel',
                            fieldLabel: 'Дата отмены',
                            format: 'd.m.Y',
                            labelWidth: 130
                        }
                    ]
                },
                {
                    xtype: 'textarea',
                    name: 'Reason',
                    fieldLabel: 'Причина',
                    maxLength: 2000,
                    flex: 1
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
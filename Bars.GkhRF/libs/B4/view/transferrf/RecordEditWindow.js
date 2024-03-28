Ext.define('B4.view.transferrf.RecordEditWindow', {
    extend: 'B4.form.Window',
    requires: [
        'B4.form.FileField',
        'B4.ux.button.Close',
        'B4.ux.button.Save'
    ],

    mixins: [ 'B4.mixins.window.ModalMask' ],
    maximizable: true,
    maximized: true,
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    width: 800,
    minWidth: 600,
    height: 500,
    minHeight: 400,
    bodyPadding: 5,
    itemId: 'transferRfRecordEditWindow',
    title: 'Редактирование записи',
    closeAction: 'hide',
    trackResetOnLoad: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'fieldset',
                    title: 'Платежное поручение',
                    height: 140,
                    layout: 'anchor',
                    defaults: {
                        labelWidth: 130,
                        anchor: '100%',
                        labelAlign: 'right'

                    },
                    items: [
                        {
                            xtype: 'container',
                            layout: 'hbox',
                            items: [
                                {
                                    xtype: 'container',
                                    flex: 1,
                                    layout: 'anchor',
                                    defaults: {
                                        labelWidth: 130,
                                        anchor: '100%',
                                        labelAlign: 'right'

                                    },
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            name: 'DocumentNum',
                                            fieldLabel: 'Номер',
                                            maxLength: 50
                                        },
                                        {
                                            xtype: 'datefield',
                                            name: 'TransferDate',
                                            itemId: 'transferDate',
                                            format: 'd.m.Y',
                                            fieldLabel: 'Месяц перечисления',
                                            allowBlank: false
                                        }
                                    ]
                                },
                                {
                                    xtype: 'container',
                                    flex: 1,
                                    layout: 'anchor',
                                    defaults: {
                                        labelWidth: 100,
                                        anchor: '100%',
                                        labelAlign: 'right'

                                    },
                                    items: [
                                        {
                                            xtype: 'datefield',
                                            name: 'DateFrom',
                                            format: 'd.m.Y',
                                            fieldLabel: 'от',
                                            itemId: 'dfDateFrom'
                                        },
                                        {
                                            xtype: 'b4filefield',
                                            name: 'File',
                                            fieldLabel: 'Файл'
                                        }
                                    ]
                                }
                            ]
                        },
                        {
                            xtype: 'textarea',
                            name: 'Description',
                            fieldLabel: 'Описание',
                            height: 40,
                            maxLength: 500
                        }
                    ]
                },
                {
                    xtype: 'transferrfrecobjgrid',
                    margins: -1,
                    flex:1
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
                            itemId: 'transferRfRecSaveButton',
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
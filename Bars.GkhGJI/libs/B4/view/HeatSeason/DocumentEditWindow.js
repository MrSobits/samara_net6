Ext.define('B4.view.heatseason.DocumentEditWindow', {
    extend: 'B4.form.Window',

    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: 'anchor',
    width: 500,
    bodyPadding: 5,
    itemId: 'heatSeasonDocEditWindow',
    title: 'Документ',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.form.FileField',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.ComboBox',
        'B4.enums.HeatSeasonDocType'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 100,
                labelAlign: 'right',
                anchor: '100%'
            },
            items: [
                {
                    xtype: 'b4combobox',
                    operand: CondExpr.operands.eq,
                    storeAutoLoad: true,
                    editable: false,
                    valueField: 'Id',
                    displayField: 'Display',
                    emptyItem: { Name: '-' },
                    url: '/HeatSeasonDoc/ListDocumentTypes',
                    name: 'TypeDocument',
                    itemId: 'cbTypeDocument',
                    fieldLabel: 'Тип документа'
                },
                {
                    xtype: 'container',
                    padding: '0 0 5 0',
                    anchor: '100%',
                    defaults: {
                        labelAlign: 'right',
                        flex: 1
                    },
                    layout: 'hbox',
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'DocumentNumber',
                            fieldLabel: 'Номер',
                            labelWidth: 100,
                            maxLength: 50,
                            itemId: 'tfDocumentNumber'
                        },
                        {
                            xtype: 'datefield',
                            name: 'DocumentDate',
                            fieldLabel: 'От',
                            format: 'd.m.Y',
                            labelWidth: 50,
                            allowBlank: false,
                            itemId: 'dfDocumentDate'
                        }
                    ]
                },
                {
                    xtype: 'b4filefield',
                    name: 'File',
                    fieldLabel: 'Файл',
                    allowBlank: false,
                    itemId: 'ffFile',
                    editable: false
                },
                {
                    xtype: 'textarea',
                    name: 'Description',
                    fieldLabel: 'Описание',
                    anchor: '100%, -75',
                    maxLength: 500,
                    itemId: 'taDescription'
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
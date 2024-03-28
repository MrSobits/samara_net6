Ext.define('B4.view.workscr.DefectListEditWindow', {
    extend: 'B4.form.Window',

    mixins: ['B4.mixins.window.ModalMask'],
    width: 500,
    minWidth: 400,
    maxWidth: 600,
    maxHeight: 180,
    minHeight: 180,
    bodyPadding: 5,

    title: 'Дефектная ведомость',
    alias: 'widget.workscrdefectlistwin',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.form.SelectField',
        'B4.store.dict.Work',
        'B4.form.FileField',
        'B4.ux.button.Close',
        'B4.ux.button.Save'
    ],

    layout: {
        type: 'vbox',
        align: 'stretch'
    },

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 150,
                labelAlign: 'right',
                allowBlank: false
            },
            items: [
                {
                    xtype: 'textfield',
                    name: 'DocumentName',
                    fieldLabel: 'Документ',
                    itemId: 'tfDocumentName',
                    maxLength: 300
                },
                {
                    xtype: 'datefield',
                    name: 'DocumentDate',
                    fieldLabel: 'Дата документа',
                    format: 'd.m.Y',
                    width: 250,
                    anchor: null,
                    itemId: 'dfDocumentDate'
                },
                {
                    xtype: 'b4filefield',
                    editable: false,
                    name: 'File',
                    fieldLabel: 'Файл',
                    itemId: 'ffFile'
                },
                {
                    xtype: 'numberfield',
                    hideTrigger: true,
                    name: 'Sum',
                    fieldLabel: 'Сумма по ведомости, руб',
                    keyNavEnabled: false,
                    mouseWheelEnabled: false,
                    decimalSeparator: ',',
                    allowBlank: true
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            items: [{ xtype: 'b4savebutton' }]
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
                                    disabled: true,
                                    menu: []
                                }
                            ]
                        },
                        {
                            xtype: 'buttongroup',
                            items: [{ xtype: 'b4closebutton' }]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});
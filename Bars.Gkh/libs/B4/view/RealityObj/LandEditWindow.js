Ext.define('B4.view.realityobj.LandEditWindow', {
    extend: 'B4.form.Window',

    alias: 'widget.realityobjlandeditwindow',

    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: 'form',
    minWidth: 450,
    minHeight: 210,
    width: 500,
    bodyPadding: 5,
    title: 'Земельный участок',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.form.FileField',
        'B4.ux.button.Close',
        'B4.ux.button.Save'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 150,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'textfield',
                    name: 'CadastrNumber',
                    fieldLabel: 'Кадастровый номер*',
                    allowBlank: false,
                    maxLength: 50
                },
                {
                    xtype: 'datefield',
                    name: 'DateLastRegistration',
                    maxWidth: 250,
                    labelWidth: 150,
                    fieldLabel: 'Дата постановки на учёт',
                    format: 'd.m.Y'
                },
                {
                    xtype: 'textfield',
                    name: 'DocumentName',
                    fieldLabel: 'Документ',
                    maxLength: 100
                },
                {
                    xtype: 'container',
                    padding: '0 0 5 0',
                    layout: {
                        pack: 'end',
                        type: 'hbox'
                    },
                    defaults: {
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'DocumentNum',
                            fieldLabel: 'Номер документа',
                            labelWidth: 150,
                            maxLength: 50,
                            flex: 1
                        },
                        {
                            xtype: 'datefield',
                            name: 'DocumentDate',
                            fieldLabel: 'Дата документа',
                            format: 'd.m.Y',
                            labelWidth: 120,
                            width: 220
                        }
                    ]
                },
                {
                    xtype: 'b4filefield',
                    name: 'File',
                    fieldLabel: 'Файл'
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
                                { xtype: 'b4savebutton' }
                            ]
                        },
                        { xtype: 'tbfill' },
                        {
                            xtype: 'buttongroup',
                            columns: 2,
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
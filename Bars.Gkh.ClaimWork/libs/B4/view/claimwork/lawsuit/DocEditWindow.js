Ext.define('B4.view.claimwork.lawsuit.DocEditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.claimworklawsuitdoceditwindow',
    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: 'form',
    width: 700,
    minWidth: 700,
    minHeight: 250,
    maxHeight: 250,
    bodyPadding: 5,
    title: 'Документ',
    closeAction: 'destroy',
    trackResetOnLoad: true,

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
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
                    xtype: 'textfield',
                    name: 'DocName',
                    fieldLabel: 'Документ',
                    allowBlank: false,
                    maxLength: 100,
                    labelAlign: 'right'
                },
                {
                    xtype: 'container',
                    padding: '0 0 5 0',
                    layout: 'hbox',
                    defaults: {
                        labelWidth: 150,
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'DocNumber',
                            fieldLabel: 'Номер',
                            maxLength: 100,
                            labelAlign: 'right'
                        },
                        {
                            xtype: 'datefield',
                            name: 'DocDate',
                            fieldLabel: 'Дата',
                            allowBlank: false,
                            format: 'd.m.Y'
                        }
                    ]
                },
                {
                    xtype: 'b4filefield',
                    name: 'File',
                    allowBlank: true,
                    fieldLabel: 'Файл'
                },
                {
                    xtype: 'textarea',
                    name: 'Description',
                    fieldLabel: 'Примечание',
                    height:70,
                    maxLength: 500
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
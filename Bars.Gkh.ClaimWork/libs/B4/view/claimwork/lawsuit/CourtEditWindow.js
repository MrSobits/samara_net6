Ext.define('B4.view.claimwork.lawsuit.CourtEditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.claimworklawsuitcourteditwindow',
    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'form',
    width: 700,
    minWidth: 700,
    minHeight: 200,
    maxHeight: 200,
    bodyPadding: 5,
    title: 'Претензия в суд',
    closeAction: 'destroy',
    trackResetOnLoad: true,

    requires: [
        'B4.form.EnumCombo',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.enums.LawsuitCourtType',
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
                    fieldLabel: 'Суд',
                    enumName: 'B4.enums.LawsuitCourtType',
                    name: 'LawsuitCourtType'
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
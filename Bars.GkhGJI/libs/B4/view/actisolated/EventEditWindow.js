Ext.define('B4.view.actisolated.EventEditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.actisolatedeventeditwindow',

    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    width: 500,
    minHeight: 250,
    bodyPadding: 5,

    title: 'Форма мероприятия',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.view.Control.GkhIntField',
        'B4.form.FileField',
        'B4.ux.button.Close',
        'B4.ux.button.Save'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 100,
                labelAlign: 'right',
                allowBlank: false
            },
            items: [
                {
                    xtype: 'textfield',
                    name: 'Name',
                    fieldLabel: 'Наименование мероприятия',
                    maxLength: 300
                },
                {
                    xtype: 'gkhintfield',
                    name: 'Term',
                    fieldLabel: 'Срок проведения мероприятия',
                    hideTrigger: true,
                    minValue: 0
                },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    defaults: {
                        labelWidth: 100,
                        labelAlign: 'right',
                        flex: 1,
                        allowBlank: false
                    },
                    items: [
                        {
                            xtype: 'datefield',
                            name: 'DateStart',
                            fieldLabel: 'Дата начала',
                            format: 'd.m.Y'
                        },
                        {
                            xtype: 'datefield',
                            name: 'DateEnd',
                            fieldLabel: 'Дата окончания',
                            format: 'd.m.Y'
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
                            columns: 1,
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
                            columns: 1,
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
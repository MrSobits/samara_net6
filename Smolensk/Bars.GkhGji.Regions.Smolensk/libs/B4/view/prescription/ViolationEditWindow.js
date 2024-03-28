Ext.define('B4.view.prescription.ViolationEditWindow', {
    extend: 'B4.form.Window',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    width: 600,
    minHeight: 300,
    bodyPadding: 5,
    alias: 'widget.prescrvioleditwindow',
    title: 'Нарушение',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.ux.form.field.TabularTextArea'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 120,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'textfield',
                    fieldLabel: 'Код ПиН',
                    name: 'ViolationGjiPin',
                    readOnly: true,
                    maxWidth: 250
                },
                {
                    xtype: 'textfield',
                    fieldLabel: 'Текст нарушения',
                    name: 'ViolationGji',
                    readOnly: true
                },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    padding: '0 0 5 0',
                    defaults: {
                        xtype: 'datefield',
                        labelAlign: 'right',
                        flex: 1,
                        format: 'd.m.Y'
                    },
                    items: [
                        {
                            name: 'DatePlanRemoval',
                            fieldLabel: 'Срок устранения',
                            allowBlank: false,
                            labelWidth: 120
                        },
                        {
                            name: 'DateFactRemoval',
                            fieldLabel: 'Дата факт. исполнения',
                            readOnly: true,
                            labelWidth: 150
                        }
                    ]
                },
                {
                    xtype: 'tabtextarea',
                    name: 'Description',
                    fieldLabel: 'Описание',
                    maxLength: 2000,
                    flex: 1
                },
                {
                    xtype: 'tabtextarea',
                    name: 'Action',
                    fieldLabel: 'Мероприятия для устранения',
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
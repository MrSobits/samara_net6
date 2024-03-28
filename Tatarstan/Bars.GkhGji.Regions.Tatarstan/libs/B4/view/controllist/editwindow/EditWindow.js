Ext.define('B4.view.controllist.editwindow.EditWindow', {
    extend: 'B4.form.Window',

    mixins: ['B4.mixins.window.ModalMask'],
    width: 1200,
    height: 750,
    bodyPadding: 5,
    title: 'Редактирование проверочного листа',
    alias: 'widget.controllisteditwindow',
    layout: 'border',

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.view.controllist.editwindow.ControlListQuestionGrid',
        'B4.form.FileField'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'container',
                    layout: 'anchor',
                    region: 'north',
                    defaults: {
                        anchor: '100%',
                        labelAlign: 'right'
                    },
                    items:
                        [
                            {
                                xtype: 'fieldset',
                                defaults: {
                                    anchor: '100%',
                                    labelAlign: 'right',
                                    labelWidth: 170
                                },
                                title: 'Общие сведения',
                                items: [
                                    {
                                        xtype: 'container',
                                        layout: 'hbox',
                                        defaults: {
                                            margin: '0 5 5 0'
                                        },
                                        items: [
                                            {
                                                xtype: 'textfield',
                                                name: 'Name',
                                                flex: 1,
                                                fieldLabel: 'Наименование',
                                                maxLength: 512
                                            },
                                            {
                                                xtype: 'textfield',
                                                name: 'ApprovalDetails',
                                                flex: 1,
                                                labelWidth: 170,
                                                fieldLabel: 'Сведения об утверждении',
                                                maxLength: 2048
                                            }
                                        ]
                                    },
                                    {
                                        xtype: 'container',
                                        layout: 'hbox',
                                        defaults: {
                                            margin: '0 5 5 0'
                                        },
                                        items: [
                                            {
                                                xtype: 'textfield',
                                                name: 'TorId',
                                                flex: 1,
                                                fieldLabel: 'Идентификатор',
                                                readOnly: true
                                            },
                                            {
                                                xtype: 'b4filefield',
                                                name: 'File',
                                                flex: 1,
                                                labelWidth: 170,
                                                fieldLabel: 'Скан-копия',
                                                editable: false
                                            }
                                        ]
                                    },
                                    {
                                        xtype: 'container',
                                        layout: 'hbox',
                                        defaults: {
                                            margin: '0 5 5 0'
                                        },
                                        items: [
                                            {
                                                xtype: 'datefield',
                                                name: 'StartDate',
                                                flex: 1,
                                                allowBlank: false,
                                                fieldLabel: 'Дата создания'
                                            },
                                            {
                                                xtype: 'datefield',
                                                name: 'EndDate',
                                                flex: 1,
                                                labelWidth: 170,
                                                fieldLabel: 'Дата окончания действия'
                                            }
                                        ]
                                    }
                                ]
                            }
                        ]
                },
                {
                    xtype: 'tabpanel',
                    region: 'center',
                    border: false,
                    items: [
                        {
                            xtype: 'controllistquestiongrid'
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
                            columns: 2,
                            items: [
                                {
                                    xtype: 'b4savebutton'
                                },
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

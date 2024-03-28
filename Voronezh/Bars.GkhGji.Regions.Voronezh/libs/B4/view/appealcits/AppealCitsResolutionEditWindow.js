Ext.define('B4.view.appealcits.AppealCitsResolutionEditWindow', {
    extend: 'B4.form.Window',

    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: { type: 'vbox', align: 'stretch' },
    width: 1000,
    minWidth: 520,
    minHeight: 310,
    height: 510,
    bodyPadding: 5,
    itemId: 'appealCitsResolutionEditWindow',
    title: 'Форма редактирования резолюции',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.form.FileField',
        'B4.form.SelectField',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.view.appealcits.AppealCitsResolutionExecutorGrid',
        'B4.store.appealcits.AppealCitsResolutionExecutor',
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 170,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'textfield',
                    name: 'ResolutionText',
                    fieldLabel: 'Текст резолюции',
                    maxLength: 300
                },
                {
                    xtype: 'textfield',
                    name: 'ResolutionAuthor',
                    fieldLabel: 'Автор резолюции',
                    maxLength: 150
                },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    defaults: {
                        xtype: 'datefield',
                        labelWidth: 170,
                        labelAlign: 'right',
                        flex: 1
                    },
                    items: [
                        {
                            xtype: 'datefield',
                            name: 'ResolutionDate',
                            fieldLabel: 'Дата резолюции',
                            format: 'd.m.Y',
                            allowBlank: false
                        },
                        {
                            xtype: 'datefield',
                            name: 'ResolutionTerm',
                            fieldLabel: 'Срок резолюции',
                            format: 'd.m.Y',
                            labelWidth: 150,
                            allowBlank: false
                        }
                    ]
                },
                {
                    xtype: 'textarea',
                    height: 50,
                    anchor: '100%',
                    name: 'ResolutionContent',
                    fieldLabel: 'Содержание',
                    labelAlign: 'right',
                    maxLength: 500,
                    margin: '5 0 0 0'
                },
                {
                    xtype: 'textfield',
                    name: 'ParentResolutionData',
                    fieldLabel: 'Информация по родительской резолюции',
                    maxLength: 150,
                    readOnly: true
                },
                {
                    xtype: 'tabpanel',
                    border: false,
                    flex: 1,
                    defaults: {
                        border: false
                    },
                    items: [
                    {
                        xtype: 'appealCitsResolutionExecutorGrid',
                        flex: 1
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
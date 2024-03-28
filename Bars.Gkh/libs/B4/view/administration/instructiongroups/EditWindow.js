Ext.define('B4.view.administration.instructiongroups.EditWindow', {
    extend: 'B4.form.Window',

    mixins: [ 'B4.mixins.window.ModalMask' ],
    width: 615,
    height: 600,
    bodyPadding: 5,
    alias: 'widget.instructiongroupeditwindow',
    title: 'Инструкция',
    closeAction: 'hide',
    trackResetOnLoad: true,
    layout: {
        type: 'vbox',
        align: 'stretch',
        pack: 'start'
    },

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.FileField',
        'B4.view.administration.instructiongroups.RoleGrid',
        'B4.view.administration.instructions.Grid',
        'B4.view.administration.instructions.EditWindow'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'textfield',
                    name: 'DisplayName',
                    fieldLabel: 'Категория',
                    allowBlank: false,
                    maxLength: 255,
                    anchor: '100%'
                },
                {
                    xtype: 'tabpanel',
                    flex: 1,
                    items: [
                        {
                            title: 'Документы',
                            layout: {
                                type: 'vbox',
                                align: 'stretch',
                                pack: 'start'
                            },
                            items: [
                                {
                                    xtype: 'instructionsGrid',
                                    flex: 1,
                                    bodyStyle: 'backrgound-color:transparent;',
                                    region: 'center'
                                }
                            ]
                        },
                        {
                            title: 'Доступ',
                            layout: {
                                type: 'vbox',
                                align: 'stretch',
                                pack: 'start'
                            },
                            items: [
                                {
                                    xtype: 'instructiongroupsrolegrid',
                                    flex: 1,
                                    bodyStyle: 'backrgound-color:transparent;',
                                    region: 'center'
                                }
                            ]
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
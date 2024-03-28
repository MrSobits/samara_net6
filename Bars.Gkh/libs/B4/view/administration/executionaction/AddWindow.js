Ext.define('B4.view.administration.executionaction.AddWindow', {
    extend: 'B4.form.Window',

    mixins: ['B4.mixins.window.ModalMask'],

    width: 720,
    bodyPadding: 10,
    closeAction: 'destroy',
    overflowY: 'auto',

    title: 'Создание задачи',

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.SelectField',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.form.SchedulerPanel',
        'B4.store.administration.executionaction.ExecutionAction'
    ],

    alias: 'widget.executionactionaddwindow',

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'container',
                    layout: {
                        type: 'hbox',
                        align: 'stretch'
                    },
                    padding: '10 5',
                    items: [
                        {
                            xtype: 'form',
                            border: false,
                            bodyStyle: Gkh.bodyStyle,
                            name: 'ExectionActionForm',
                            flex: 1,
                            layout: {
                                type: 'vbox',
                                align: 'stretch',
                                padding: 5
                            },
                            defaults: {
                                labelAlign: 'right',
                                labelWidth: 100
                            },
                            items: [
                                {
                                    xtype: 'b4selectfield',
                                    editable: false,
                                    name: 'ExecutionAction',
                                    fieldLabel: 'Действие',
                                    labelWidth: 120,
                                    store: 'B4.store.administration.executionaction.ExecutionAction',
                                    selectionMode: 'SINGLE',
                                    flex: 1,
                                    idProperty: 'Code',
                                    textProperty: 'Name',
                                    width: 500,
                                    allowBlank: false,
                                    columns: [
                                        {
                                            dataIndex: 'Name',
                                            header: 'Наименование',
                                            flex: 1.5,
                                            filter: { xtype: 'textfield' }
                                        },
                                        {
                                            dataIndex: 'Description',
                                            header: 'Описание',
                                            flex: 2,
                                            filter: { xtype: 'textfield' }
                                        }
                                    ],
                                    windowCfg: {
                                        width: 800,
                                        title: 'Выбор действия'
                                    }

                                },
                                {
                                    xtype: 'textarea',
                                    height: 75,
                                    readOnly: true
                                },
                                {
                                    xtype: 'fieldset',
                                    name: 'ExecutionActionParams',
                                    title: 'Параметры действия',
                                    hidden: true,
                                    items: [
                                        {
                                            xtype: 'form',
                                            bodyStyle: Gkh.bodyStyle,
                                            border: false,
                                            defaults: {
                                                bodyStyle: Gkh.bodyStyle
                                            },
                                            layout: {
                                                type: 'vbox',
                                                align: 'stretch',
                                                padding: 5,
                                                border: false
                                            }
                                        }
                                    ],
                                },
                            ]
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    title: 'Параметры запуска',
                    collapsible: true,
                    collapsed: true,
                    items: [
                        {
                            xtype: 'schedulerpanel'
                        }
                    ],
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
                            items: [
                                {
                                    xtype: 'b4closebutton',
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
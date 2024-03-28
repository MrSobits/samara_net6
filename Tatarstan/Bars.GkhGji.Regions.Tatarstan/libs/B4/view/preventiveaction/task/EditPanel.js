Ext.define('B4.view.preventiveaction.task.EditPanel', {
    extend: 'Ext.form.Panel',

    requires: [
        'B4.view.GjiDocumentCreateButton',
        'B4.view.Control.GkhButtonPrint',
        'B4.view.preventiveaction.task.MainInfoTabPanel',
        'B4.view.preventiveaction.task.PlannedActionGrid',
        'B4.view.preventiveaction.task.TaskOfPreventiveActionTaskGrid',
        'B4.view.preventiveaction.task.RegulationsTabPanel',
        'B4.view.preventiveaction.task.ConsultingQuestingGrid',
        'B4.view.preventiveaction.task.ObjectiveTabPanel',
        'B4.view.preventiveaction.task.ItemGrid',
        'B4.ux.button.Save',
        'B4.form.EnumCombo',

        'B4.enums.YesNo',
        'B4.enums.NotificationType'
    ],

    title: 'Задание',
    itemId: 'preventiveActionTaskEditPanel',
    autoscroll: true,
    trackResetOnLoad: true,
    layout: { type: 'vbox', align: 'stretch' },

    initComponent: function(){
        var me = this;

        Ext.applyIf(me, {
            bodyStyle: Gkh.bodyStyle,
            layout: {
                type: 'vbox',
                align: 'middle'
            },
            defaults: {
                xtype: 'container',
                layout: {
                    type: 'hbox',
                    align: 'stretch',
                }
            },
            items: [
                {
                    defaults: {
                        labelAlign: 'right',
                        labelWidth: 50,
                        flex: 1,
                        margin: '10 50 10 0'
                    },
                    items: [
                        {
                            xtype: 'datefield',
                            name: 'DocumentDate',
                            fieldLabel: 'Дата',
                            format: 'd.m.Y',
                            margin: '10 50 10 -10',
                            allowBlank: false
                        },
                        {
                            xtype: 'textfield',
                            name: 'DocumentYear',
                            fieldLabel: 'Год'
                        },
                        {
                            xtype: 'textfield',
                            name: 'DocumentNumber',
                            fieldLabel: 'Номер документа',
                            labelWidth: 120,
                            width: 190,
                            readOnly: true
                        },
                        {
                            xtype: 'textfield',
                            name: 'DocumentNum',
                            fieldLabel: 'Номер',
                            margin: '10 5 10 0',
                            readOnly: true
                        }
                    ]
                },
                {
                    xtype: 'tabpanel',
                    itemId: 'taskTabPanel',
                    flex: 1,
                    bodyStyle: Gkh.bodyStyle,
                    items: [
                        {
                            xtype: 'preventiveactiontaskmaininfotabpanel'
                        },
                        {
                            xtype: 'preventiveactiontaskitemgrid',
                            height: 300
                        },
                        {
                            xtype: 'taskofpreventiveactiontaskgrid',
                            height: 300
                        },
                        {
                            xtype: 'preventiveactiontaskregulationstabpanel',
                            height: 300
                        },
                        {
                            xtype: 'consultingquestiongrid',
                            hidden: true,
                            height: 300,
                            padding: 5
                        },
                        {
                            xtype: 'objectivetabpanel'
                        }
                    ]
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    itemId: 'documentGJIToolBar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 5,
                            items: [
                                {
                                    xtype: 'b4savebutton'
                                },
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-decline',
                                    text: 'Отмена',
                                    textAlign: 'left',
                                    itemId: 'btnCancel'
                                },
                                {
                                    xtype: 'gjidocumentcreatebutton'
                                },
                                {
                                    xtype: 'gkhbuttonprint',
                                    itemId: 'btnPrint'
                                },
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-delete',
                                    text: 'Удалить',
                                    textAlign: 'left',
                                    itemId: 'btnDelete'
                                }
                            ]
                        },
                        {
                            xtype: 'tbfill'
                        },
                        {
                            xtype: 'buttongroup',
                            columns: 1,
                            itemId: 'statusButtonGroup',
                            items: [
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-accept',
                                    itemId: 'btnState',
                                    text: 'Статус',
                                    menu: []
                                }
                            ]
                        }
                    ]
                }]
        });

        me.callParent(arguments);
    }
});
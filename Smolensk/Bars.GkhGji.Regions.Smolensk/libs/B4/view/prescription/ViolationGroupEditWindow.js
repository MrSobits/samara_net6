﻿Ext.define('B4.view.prescription.ViolationGroupEditWindow', {
    extend: 'B4.form.Window',

    alias: 'widget.prescriptionViolationGroupEditWindow',
    
    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    width: 700,
    minHeight: 200,
    bodyPadding: 5,
    modal: true,
    itemId: 'prescriptionViolationGroupEditWindow',
    title: 'Редактирвоание описания нарушений',
    closeAction: 'destroy',
    trackResetOnLoad: true,

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 200,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'textfield',
                    name: 'PointCodes',
                    readOnly: true,
                    fieldLabel: 'Пункты нормативных документов'
                },
                {
                    xtype: 'tabtextarea',
                    name: 'Description',
                    fieldLabel: 'Описание нарушения',
                    itemId: 'taDescription',
                    flex:1,
                    maxLength: 500
                },
                {
                    xtype: 'tabtextarea',
                    name: 'Action',
                    fieldLabel: 'Мероприятия по устранению',
                    itemId: 'taAction',
                    flex: 1,
                    maxLength: 500
                },
                {
                    xtype: 'container',
                    layout: {
                        type: 'hbox',
                        align: 'stretch'
                    },
                    defaults: {
                        labelWidth: 200,
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            xtype: 'datefield',
                            name: 'DatePlanRemoval',
                            fieldLabel: 'Срок устранения',
                            format: 'd.m.Y',
                            flex:1
                        },
                        {
                            xtype: 'component',
                            flex: 1
                        }
                    ]
                },
                {
                    xtype: 'container',
                    style: 'border: 1px solid #a6c7f1 !important; font: 12px tahoma,arial,helvetica,sans-serif; background: transparent; margin: 10px 30px 0 5px; padding: 5px 10px; line-height: 16px;',
                    html: '<span style="display: table-cell"><span class="im-info" style="vertical-align: top;"></span></span><span style="display: table-cell; padding-left: 5px;">Если в полях "Описание нарушения" и "Мероприятия по устранению" необходимо внести текст размером более трех строк, то воспользуйтесь кнопкой "Редактор"</span>'
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
                                    xtype: 'b4savebutton',
                                    itemId: 'prescriptionViolGroupEditWindowSaveButton'
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
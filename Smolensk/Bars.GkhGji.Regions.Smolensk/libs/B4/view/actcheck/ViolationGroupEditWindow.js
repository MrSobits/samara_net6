﻿Ext.define('B4.view.actcheck.ViolationGroupEditWindow', {
    extend: 'B4.form.Window',

    alias: 'widget.actCheckViolationGroupEditWindow',
    
    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    width: 700,
    minHeight: 200,
    bodyPadding: 5,
    modal: true,
    itemId: 'actCheckViolationGroupEditWindow',
    title: 'Форма описания нарушений',
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
                    xtype: 'gkhtriggerfield',
                    name: 'Points',
                    itemId: 'tfPoints',
                    allowBlank: false,
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
                    xtype: 'container',
                    style: 'border: 1px solid #a6c7f1 !important; font: 12px tahoma,arial,helvetica,sans-serif; background: transparent; margin: 10px 30px 0 5px; padding: 5px 10px; line-height: 16px;',
                    html: '<span style="display: table-cell"><span class="im-info" style="vertical-align: top;"></span></span><span style="display: table-cell; padding-left: 5px;">Если в поле "Описание нарушения" необходимо внести текст размером более трех строк, то воспользуйтесь кнопкой "Редактор".<br/>Нажмите кнопку "Сохранить", чтобы кнопка "Редактор" стала активной</span>'
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
                                    itemId: 'actCheckViolGroupEditWindowSaveButton'
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
Ext.define('B4.view.housemanaging.ViewPanel', {
    extend: 'Ext.form.Panel',

    alias: 'widget.housemanagingviewpanel',

    closable: true,
    layout: 'border',
    bodyPadding: 5,
    itemId: 'housemanagingViewPanel',
    title: 'Управление домом',
    trackResetOnLoad: true,
    autoScroll: true,
    requires: [
        'B4.ux.grid.Panel',
        'B4.ux.button.Update'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            items: [
                                {
                                    xtype: 'b4updatebutton'
                                }
                            ]
                        }
                    ]
                }
            ],

            items: [
                {
                    xtype: 'container',
                    region: 'north',
                    padding: 2,
                    style: 'font: 12px tahoma,arial,helvetica,sans-serif; background: transparent; margin: 3px; line-height: 16px;',
                    html: '<span style="display: table-cell"><span class="im-info" style="vertical-align: top;"></span></span><span style="display: table-cell; padding-left: 5px;">На данной странице представлены данные только для чтения. Для редактирования данных необходимо перейти в раздел Участники процесса/Роли контрагента/Управляющие организации/Управление домами</span>'
                },
                {
                    xtype: 'container',
                    region: 'north',
                    items: [
                        {
                            xtype: 'fieldset',
                            itemId: 'fsActState',
                            defaults: {
                                anchor: '100%',
                                labelAlign: 'right',
                                labelWidth: 320
                            },
                            title: 'Общие сведения',
                            items: [
                                 {
                                     xtype: 'datefield',
                                     name: 'DateStart',
                                     fieldLabel: 'Дата начала управления',
                                     flex: 0.8,
                                     maxWidth: 850,
                                     format: 'd.m.Y',
                                     hideTrigger: true,
                                     readOnly: true,
                                     qtipText: 'Данные из [Участники процесса] - [УО] - [Управление домами] - [Дата начала управления]'
                                 },
                                {
                                    xtype: 'textfield',
                                    name: 'ContractFoundation',
                                    fieldLabel: 'Основание управления',
                                    hideTrigger: true,
                                    flex: 0.8,
                                    maxWidth: 850,
                                    readOnly: true,
                                    qtipText: 'Данные из [Участники процесса] - [УО] - [Управление домами] - [Основание]'
                                },
                                 {
                                     xtype: 'textfield',
                                     name: 'DocumentName',
                                     fieldLabel: 'Документ, подтверждающий выбранный способ управления',
                                     hideTrigger: true,
                                     flex: 0.8,
                                     maxWidth: 850,
                                     readOnly: true,
                                     qtipText: 'Данные из [Участники процесса] - [УО] - [Управление домами] - [Документ]'
                                 },
                                {
                                    xtype: 'datefield',
                                    name: 'DocumentDate',
                                    fieldLabel: 'Дата документа',
                                    flex: 0.8,
                                    maxWidth: 850,
                                    format: 'd.m.Y',
                                    hideTrigger: true,
                                    readOnly: true,
                                    qtipText: 'Данные из [Участники процесса] - [УО] - [Управление домами] - [от]'
                                },
                                 {
                                     xtype: 'textfield',
                                     name: 'DocumentNumber',
                                     fieldLabel: 'Номер документа',
                                     hideTrigger: true,
                                     flex: 0.8,
                                     maxWidth: 850,
                                     readOnly: true,
                                     qtipText: 'Данные из [Участники процесса] - [УО] - [Управление домами] - [Номер]'
                                 },
                                 {
                                     xtype: 'textfield',
                                     name: 'ContractStopReason',
                                     fieldLabel: 'Основание окончания управления',
                                     hideTrigger: true,
                                     flex: 0.8,
                                     maxWidth: 850,
                                     readOnly: true,
                                     qtipText: 'Данные из [Участники процесса] - [УО] - [Управление домами] - [Основание окончания управления]'
                                 },
                                  {
                                      xtype: 'datefield',
                                      name: 'DateEnd',
                                      fieldLabel: 'Дата окончания управления',
                                      format: 'd.m.Y',
                                      hideTrigger: true,
                                      flex: 0.8,
                                      maxWidth: 850,
                                      readOnly: true,
                                      qtipText: 'Данные из [Участники процесса] - [УО] - [Управление домами] - [Дата окончания управления]'
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
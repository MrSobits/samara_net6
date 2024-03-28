Ext.define('B4.view.reminder.FilterPanel', {
    extend: 'Ext.form.Panel',
    requires: [ 'B4.DisposalTextValues' ],
    alias: 'widget.reminderfilterpnl',
    layout:  'anchor',
    bodyPadding: 5,
    width: 250,
    itemId: 'reminderFilterPanel',
    trackResetOnLoad: true,
    autoScroll: true,
    frame: true,
    border: false,
    title: 'Фильтрация',

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 140,
                labelAlign: 'right'
            },
            items: [
                 {
                     xtype: 'button',
                     text: 'Обновить',
                     tooltip: 'Обновить',
                     iconCls: 'icon-arrow-refresh',
                     itemId: 'btnReminderRefresh',
                     margin: '0 0 0 65'
                 },
                  {
                      xtype: 'datefield',
                      labelWidth: 60,
                      fieldLabel: 'Период с',
                      width: 160,
                      itemId: 'dfDateStart',
                      padding: '20 0 0 0'
                  },
                  {
                      xtype: 'datefield',
                      labelWidth: 60,
                      fieldLabel: 'по',
                      width: 160,
                      itemId: 'dfDateEnd'
                  },
                     {
                            xtype: 'fieldset',
                            title: 'Типы задач',
                            anchor: '100%',
                            layout: 'form',
                            margin: '30 0 0 0',
                            defaults: {
                                labelWidth: 130,
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'checkboxfield',
                                    fieldLabel: 'Обращения',
                                    itemId: 'cbStatement',
                                    boxLabelAlign: 'before',
                                    checked: true
                                },
                                {
                                    xtype: 'checkboxfield',
                                    fieldLabel: B4.DisposalTextValues.getSubjectiveManyCase(),
                                    itemId: 'cbDisposal',
                                    boxLabelAlign: 'before',
                                    checked: true
                                },
                                {
                                    xtype: 'checkboxfield',
                                    fieldLabel: 'Предписания',
                                    itemId: 'cbPrescription',
                                    boxLabelAlign: 'before',
                                    checked: true
                                },
                                 {
                                     xtype: 'checkboxfield',
                                     fieldLabel: 'Основание проверки',
                                     itemId: 'cbBaseInspection',
                                     boxLabelAlign: 'before',
                                     checked: true
                                 },
                                 {
                                     xtype: 'checkboxfield',
                                     fieldLabel: 'Протокол',
                                     itemId: 'cbProtocol',
                                     boxLabelAlign: 'before',
                                     checked: true
                                 }
                            ]
                     },
               {
                   xtype: 'buttongroup',
                   columns: 1,
                   margin: '30 0 0 0',
                   items: [
                       {
                           xtype: 'button',
                           actionName: 'AllTask',
                           enableToggle: true,
                           toggleGroup: 'clientsToggleGroup',
                           pressed: false,
                           filterValue: 10,
                           textAlign: 'left',
                           iconCls: 'icon-page-white-magnify'
                       },
                       {
                           xtype: 'button',
                           actionName: 'ComeUpToTerm',
                           enableToggle: true,
                           toggleGroup: 'clientsToggleGroup',
                           pressed: false,
                           filterValue: 20,
                           textAlign: 'left',
                           iconCls: 'icon-page-white-magnify'
                       },
                       {
                           xtype: 'button',
                           actionName: 'Expired',
                           enableToggle: true,
                           toggleGroup: 'clientsToggleGroup',
                           pressed: false,
                           filterValue: 30,
                           textAlign: 'left',
                           iconCls: 'icon-page-white-magnify'
                       },
                       {
                           xtype: 'button',
                           actionName: 'Unformed',
                           enableToggle: true,
                           toggleGroup: 'clientsToggleGroup',
                           pressed: false,
                           filterValue: 40,
                           textAlign: 'left',
                           iconCls: 'icon-page-white-magnify'
                       }
                   ]
               },
               {
                   xtype: 'fieldset',
                   title: 'Контрольный срок равен',
                   anchor: '100%',
                   layout: 'form',
                   margin: '30 0 0 0',
                   defaults: {
                       labelWidth: 130,
                       labelAlign: 'right'
                   },
                   items: [
                        {
                            xtype: 'label',
                            html: '<div style="margin-bottom: 15px; font-size: 11.5px;">По ' + B4.DisposalTextValues.getDativeCase().toLowerCase() + ' - <div style="margin-left: 5px; margin-top: 4px; line-height: 17px;">дате окончания периода обследования</div></div>'
                        },
                        {
                            xtype: 'label',
                            html: '<div style="margin-bottom: 15px; font-size: 11.5px;">По предписанию - <div style="margin-left: 5px; margin-top: 4px; line-height: 17px;">сроку устранения нарушений</div></div>'
                        },
                        {
                            xtype: 'label',
                            html: '<div style="font-size: 11.5px;">Обращение граждан - <div style="margin-left: 5px; margin-top: 4px; line-height: 17px;">контрольному сроку обращения</div></div>'
                        }

                   ]
               }
            

            ]
        });

        me.callParent(arguments);
    }
});
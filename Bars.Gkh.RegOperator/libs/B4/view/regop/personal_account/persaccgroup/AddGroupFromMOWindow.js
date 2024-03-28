/**
 * Форма добавления группы ЛС.
 * Вызывается из "Региональный фонд / Реестр лицевых счетов / Другие операции - Группы лицевых счетов".
 * Подключается в B4.controller.regop.personal_account.BasePersonalAccount для аспекта PersonalAccountMassGroupOperationsWindowAspect.
 *
 * По сути не отличается от AddGroupGrid. Добавлена ради того, чтобы был другой алиас.
 * Иначе, если была открыта карточка ЛС, то при сохранении на сервер уходит 2 дублирующихся запроса.
 * Т.к. и "Реестр ЛС" и "Карточка ЛС" подключают форму по одному и тому-же алиасу.
 */
Ext.define('B4.view.regop.personal_account.persaccgroup.AddGroupFromMOWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.persaccgroupfrommoaddwindow',
        
    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'fit',
    width: 480,
    bodyPadding: 5,
    title: 'Новая группа лицевых счетов',

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.store.dict.PersAccGroup'
    ],

    store: 'dict.PersAccGroup',

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 200,
                labelAlign: 'right'
            },
            layout: {
                type: 'vbox',
                align: 'stretch'
            },
            items: [
                {
                    xtype: 'textfield',
                    name: 'Name',
                    fieldLabel: 'Наименование',
                    labelAlign: 'right',
                    labelWidth: 110,
                    maxLength: 100,
                    allowBlank: false
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            items: [{ xtype: 'b4savebutton' }]
                        },
                        { xtype: 'tbfill' },
                        {
                            xtype: 'buttongroup',
                            items: [{ xtype: 'b4closebutton' }]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});
Ext.define('B4.view.dict.qualificationmember.EditWindow', {
    extend: 'B4.form.Window',

    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: 'form',
    width: 500,
    minWidth: 300,
    maxWidth: 600,
    maxHeight: 175,
    bodyPadding: 5,
    itemId: 'qualificationMemberEditWindow',
    title: 'Участник квалификационного отбора',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires:
    [
        'B4.form.SelectField',
        'B4.view.Control.GkhTriggerField',
        'B4.store.dict.Period',
        'B4.store.Role',

        'B4.ux.button.Close',
        'B4.ux.button.Save'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 100,
                labelAlign: 'right',
                anchor: '100%'
            },
            items: [
                {
                    xtype: 'textfield',
                    name: 'Name',
                    fieldLabel: 'Наименование',
                    allowBlank: false,
                    maxLength: 300
                },
                {
                    xtype: 'checkboxfield',
                    name: 'IsPrimary',
                    fieldLabel: 'Основной'
                },
                {
                    xtype: 'b4selectfield',
                    name: 'Period',
                    fieldLabel: 'Период',
                   

                    store: 'B4.store.dict.Period',
                    editable: false,
                    allowBlank: false
                },
                {
                    xtype: 'gkhtriggerfield',
                    name: 'roleQualmember',
                    itemId: 'roleQualmemberTrigerField',
                    fieldLabel: 'Роли',
                    editable: false,
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
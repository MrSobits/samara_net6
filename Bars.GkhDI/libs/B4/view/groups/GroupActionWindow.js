Ext.define('B4.view.groups.GroupActionWindow', {
    extend: 'B4.form.Window',

    mixins: [ 'B4.mixins.window.ModalMask' ],
    width: 500,
    bodyPadding: 5,
    closeAction: 'hide',
    trackResetOnLoad: true,
    title: 'Групповая операция',
    itemId: 'groupActionWindow',
    layout: 'anchor',

    requires: [
        'B4.ux.button.Save',
        'B4.ux.button.Close',
        'B4.ux.grid.toolbar.Paging',
        'B4.form.SelectField',
        'B4.form.ComboBox',
        'B4.view.Control.GkhTriggerField'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'b4combobox',
                    name: 'Action',
                    fieldLabel: 'Тип операции',
                    valueField: 'Key',
                    fields: ['Key', 'Name'],
                    editable: false,
                    url: '/GroupDi/GetGroupActions',
                    anchor: '100%'
                },
                {
                    xtype: 'gkhtriggerfield',
                    name: 'groupsDi',
                    itemId: 'groupsTriggerField',
                    fieldLabel: 'Группы',
                    anchor: '100%'
                }
            ],

            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 3,
                            items: [
                                {
                                    xtype: 'button',
                                    itemId: 'nextGroupActionButton',
                                    text: 'Продолжить',
                                    tooltip: 'Продолжить',
                                    iconCls: 'icon-add'
                                },
                                {
                                    xtype: 'tbfill'
                                },
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

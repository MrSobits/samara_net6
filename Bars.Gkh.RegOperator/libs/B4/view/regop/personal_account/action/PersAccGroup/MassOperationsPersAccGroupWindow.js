Ext.define('B4.view.regop.personal_account.action.PersAccGroup.MassOperationsPersAccGroupWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.massoperationspersaccgroupwindow',
    mixins: ['B4.mixins.window.ModalMask'],
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    width: 960,
    height: 480,
    bodyPadding: 5,
    maximizable: true,
    title: 'Группы лицевых счетов',

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.store.dict.PersAccGroup',
        'B4.view.regop.personal_account.action.PersAccGroup.AddGroupGrid'
    ],

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
                    xtype: 'masspaccountaddgroupgrid',
                    flex: 1
                }
            ],
            dockedItems: []
        });

        me.callParent(arguments);
    }
    
});
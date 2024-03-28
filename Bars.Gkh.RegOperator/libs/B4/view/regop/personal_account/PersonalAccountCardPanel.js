Ext.define('B4.view.regop.personal_account.PersonalAccountCardPanel', {
    extend: 'Ext.form.Panel',
    requires: [
        'B4.view.regop.personal_account.PersonalAccountEditPanel',
        'B4.view.regop.personal_account.PersonalAccountOperationGrid',
        'B4.view.regop.personal_account.PersonalAccountHistoryGrid',
        'B4.view.regop.personal_account.privilegedcategory.Grid',
        'B4.view.regop.personal_account.persaccgroup.Grid',
        'B4.view.regop.personal_account.ownerinformation.Grid',
        'B4.view.regop.personal_account.banrecalc.Grid'
    ],
    alias: 'widget.personalaccountcardpanel',
    closable: true,
    title: 'Лицевой счет',
    bodyStyle: Gkh.bodyStyle,
    layout: 'fit',
    
    initComponent: function () {
        var me = this,
            panels;

        panels = [
            {
                xtype: 'personalaccounteditpanel',
                closable: false
            },
            {
                xtype: 'paccounthistorygrid'
            },
            {
                xtype: 'paccountprivilegedcategorygrid'
            },
            {
                xtype: 'paccountgroupgrid'
            },
            {
                xtype: 'paccountownerinformationgrid'
            },
            {
                xtype: 'paccountbanrecalcgrid'
            }
        ];
        
        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'tabpanel',
                    border: false,
                    activeTab: 0,
                    items: panels
                }
            ]
        });

        me.callParent(arguments);
    }
});
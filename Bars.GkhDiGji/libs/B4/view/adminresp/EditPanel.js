Ext.define('B4.view.adminresp.EditPanel', {
    extend: 'Ext.form.Panel',
    closable: true,
    bodyPadding: 2,
    trackResetOnLoad: true,
    autoScroll: true,
    title: 'Административная ответственность',
    itemId: 'adminRespEditPanel',
    layout: 'fit',

    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Save',
        'B4.ux.button.Update',
        'B4.view.adminresp.GridPanel'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'adminrespgridpanel'
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 4,
                            items: [
                                {
                                    xtype: 'b4addbutton',
                                    itemId: 'addAdminRespButton'
                                },
                                {
                                    xtype: 'b4updatebutton'
                                },
                                {
                                    xtype: 'button',
                                    itemId: 'loadGjiResolutionButton',
                                    text: 'Загрузить данные из блока ГЖИ',
                                    tooltip: 'Загрузить данные из блока ГЖИ',
                                    iconCls: 'icon-add'
                                },
                                {
                                    xtype: 'button',
                                    itemId: 'helpAdminRespButton',
                                    text: 'Справка',
                                    tooltip: 'Справка',
                                    iconCls: 'icon-help'
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

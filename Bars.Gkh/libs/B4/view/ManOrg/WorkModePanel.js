Ext.define('B4.view.manorg.WorkModePanel', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.manorgworkmodepanel',
    requires: [
        'B4.ux.button.Update',
        'B4.ux.button.Save',
        'B4.view.Control.GkhWorkModeGrid',
        'B4.view.manorg.DispatchPanel'
    ],
    closable: true,
    autoScroll: true,
    title: 'Режимы работы',
    layout: 'anchor',

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'gkhworkmodegrid',
                    title: 'Режим работы',
                    store: 'manorg.WorkMode',
                    itemId: 'workModeGrid'
                },
                {
                    xtype: 'gkhworkmodegrid',
                    title: 'Прием граждан',
                    store: 'manorg.ReceptionCitizens',
                    itemId: 'receptionCitizensGrid'
                },
                {
                    xtype: 'gkhworkmodegrid',
                    title: 'Работа диспетчерских служб',
                    store: 'manorg.DispatcherWork',
                    itemId: 'dispatcherWorkGrid',
                    minHeight: 350,
                    flex:1,
                    dockedItems: [
                        
                        {
                            xtype: 'toolbar',
                            dock: 'top',
                            items: [
                                {
                                    xtype: 'container',
                                    layout: {
                                        type: 'vbox',
                                        align: 'stretch'
                                    },
                                    items: [

                                        {
                                            xtype: 'buttongroup',
                                            items: [
                                                {
                                                    xtype: 'b4updatebutton'
                                                },
                                                {
                                                    xtype: 'b4savebutton'
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'manorgDispatchPanel',
                                            name : 'edit',
                                            flex: 1
                                        }
                                    ]
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

Ext.define('B4.view.Portal', {
    extend: 'Ext.container.Viewport',
    requires: [
        'Ext.tab.Panel',
        'B4.view.desktop.TabPanel',
        'B4.view.desktop.Desktop',
        'B4.view.desktop.Workplace',
        'B4.view.desktop.Widget',
        'B4.view.desktop.menu.Main',
        'B4.view.desktop.portal.PortalPanel',
        'B4.view.desktop.History',
        'B4.view.desktop.FavoritesBar',
        'B4.view.desktop.FavoriteSlot',
        'B4.view.instructions.Window'
    ],
    alias: 'widget.b4portal',
    layout: 'fit',

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'b4desktop',
                    ctxKey: 'root',
                    ui: 'b4desktop',
                    tabPanel: {
                        xtype: 'b4tabpanel',
                        id: 'contentPanel',
                        activeItem: 1,
                        layout: {
                            type: 'fit',
                            align: 'stretch'
                        }
                    },
                    mainMenu:
                    {
                        xtype: 'panel',
                        ui: 'menupanel',
                        layout: {
                            type: 'fit',
                            align: 'stretch'
                        },
                        tabConfig: {
                            hidden: true
                        },
                        items: [{
                            xtype: 'mainmenu',
                            store: 'MenuItemStore'
                        }]
                    },

                    quickMenuButton: {
                        ui: 'quickMenu',
                        disabled: true,
                        xtype: 'button',
                        arrowCls: 'b4arrow',
                        text: '<span class="icn-middle-history"><span>',
                        tooltip: 'Быстрая навигация',
                        styleHtmlContent: true,
                        styleHtmlCls: 'btn-b4',
                        padding: '6 0',
                        width: 50,
                        menu: {
                            xtype: 'menu',
                            ui: 'Qmenu',
                            overflowY: 'auto',
                            items: [{
                                xtype: 'panel',
                                ui: 'QmenuItem',
                                items: [{
                                    xtype: 'history'
                                }]
                            }],
                            plain: true,
                            maxHeight: 700,
                            width: 550
                        }
                    },
                    helpButton: {
                        tooltip: 'Помощь',
                        ui: 'help',
                        xtype: 'button',
                        text: '<span class="icn-middle-info"><span>',
                        styleHtmlContent: true,
                        styleHtmlCls: 'btn-b4',
                        width: 50,
                        padding: '6 0',
                        itemId: 'helpBtn'
                    },
                    workplace: {
                        xtype: 'workplace',
                        layout: 'fit',
                        portalPanel: {
                            xtype: 'portalpanel',
                            ui: 'portalpanel',
                            items: [
                                {
                                    id: 'col-1'
                                },
                                {
                                    id: 'col-2',
                                    items: [
                                        {
                                            xtype: 'portlet',
                                            ui: 'b4portlet',
                                            cls: 'x-portlet orange',
                                            itemId: 'faq',
                                            wtype:'faq',
                                            hidden: true,
                                            closable: false,
                                            collapsible: false,
                                            title: 'База знаний',
                                            iconCls: 'wic-docs',
                                            draggable: false,
                                            items: [{
                                                xtype: 'component',
                                                itemId : 'faqBtn',
                                                renderTpl: '<div class="widget-item" style="border-top: none !important;">' +
                                                    '<ul class="widget-item-list">' +
                                                    '<li>' +
                                                    '<div class="pull-left text">Скачайте документацию по работе в системе.</div>' +
                                                    '<div class="pull-right btn-wrap"><a href="' + B4.Url.action('/Instruction/GetMainInstruction') + '" class="download-btn"></a></div>' +
                                                    '<div class="clearfix"></div>' +
                                                    '</li>' +
                                                    '</ul>' +
                                                    '</div>'
                                            }],
                                            dockedItems: [{
                                                xtype: 'toolbar',
                                                dock: 'bottom',
                                                ui: 'b4portlet-footer',
                                                items: [
                                                    '->',
                                                    {
                                                        xtype: 'button',
                                                        id: 'goToInstructionsBtn',
                                                        ui: 'searchportlet-footer-btn-rt',
                                                        height: 30,
                                                        text: 'Все документы'
                                                    }
                                                ]
                                            }]
                                        }
                                    ]
                                },
                                {
                                    id: 'col-3'
                                }
                            ]
                        }
                    }
                }
            ]
        });

        me.callParent(arguments);
    }
});
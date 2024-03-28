Ext.define('B4.controller.PortalController', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.permission.GkhFormPermissionAspect',
        'B4.aspects.permission.GkhPermissionAspect',
        'B4.mixins.LayoutControllerLoader',
        'B4.mixins.MaskBody',
        'B4.mixins.Context'
    ],

    refs: [
        {
            ref: 'mainMenu',
            selector: '#mainMenu'
        },
        {
            ref: 'b4TabPanel',
            selector: '#contentPanel'
        }
    ],
    views: ['Portal', 'instructions.Window'],
    stores: [
        'MenuItemStore',
        'SearchIndex',
        'News'
    ],
    mixins: {
        controllerLoader: 'B4.mixins.LayoutControllerLoader',
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    containerSelector: '#contentPanel',

    allowLoadMask: false,
    deployViewKeys: {
        'default': 'defaultDeploy'
    },
    widgets: null,
    
    permissions: [
        {
            name: 'Widget.Faq',
            applyTo: '[wtype=faq]',
            selector: 'portalpanel',
            applyBy: function (component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        },
        {
            name: 'B4.Security.AccessRights',
            applyTo: '#permissionButton',
            selector: 'desktoptopbar',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) {
                        component.show();
                    } else {
                        component.hide();
                    }
                }
            }
        },
        {
            name: 'Widget.Faq',
            applyTo: '[wtype=faq]',
            selector: 'portalpanel',
            applyBy: function(component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        }
    ],

    aspects: [
        {
            xtype: 'formpermissionaspect',
            buttonSelector: '#permissionButton'
        }
    ],

    /**
     * @private
     * Метод по умолчанию для добавления компонент на рабочий стол.
     * Добавляет компоненту в виде новой вкладки.
     */
    defaultDeploy: function (controller, view) {
        var container = this.getB4TabPanel(),
            viewSelector = '#' + view.getId();

        if (!container.down(viewSelector)) {
            container.add(view);
        }
        container.setActiveTab(view);
    },
    
    constructor: function(){
        var me = this;
        
        me.widgets = new Map();
        me.widgets.set(1, []);
        me.widgets.set(2, []);
        me.widgets.set(3, []);

        Ext.require(['B4.view.desktop.portlet.*'], function (_) {
            var classes = arguments;

            Ext.iterate(classes, function (elDefinition) {
                var el = elDefinition.create();

                if(el.permissions){
                    Array.prototype.push.apply(me.permissions, el.permissions);
                }

                el.hide();
                me.widgets.get(el.column || 1).push(el);
            });
        }, me, ['B4.view.desktop.portlet.LoginForm', 'B4.view.desktop.portlet.Map']);
        
        
        me.aspects.push({
            xtype: 'gkhpermissionaspect',
            permissions: me.permissions
        });
        
        me.callParent(arguments);
    },

    init: function () {
        var me = this,
            permissions = [],
            actions = {
                // Временное решение для работы старой версии routing
                'b4portal': {
                    afterrender: function () {
                        var token = Ext.History.getToken();
                        if (token && token.indexOf("B4.controller") == 0) {
                            Ext.History.add('');
                        }
                    }
                },
                '#contentPanel': {
                    tabchange: function (tabPanel, newPanel, oldPanel, eOpts) {
                        if (!newPanel.ctxKey) {
                            Ext.History.add(newPanel.token || '');
                        }
                    }
                },
                // Конец - Временное решение для работы старой версии routing

                'menufirstlevelitem': {
                    tap: me.onMenuItemClick
                },

                'iconablemenu': {
                    itemtap: me.onMenuItemClick
                },

                '#searchList': {
                    itemtap: me.onMenuItemClick
                },

                '#helpBtn': {
                    click: me.onHelpBtnClick
                },

                '#goToInstructionsBtn': {
                    click: me.goToInstructionsBtnClick
                },

                '#logoutBt': {
                    click: this.onLogout
                }
            };
        
        me.widgets.forEach(function(widgets, col){
            Ext.iterate(widgets, function(widget){
                if(widget.actions){
                    Ext.iterate(widget.actions, function(action){
                        action.scope = action.scope || me;
                    });
                    
                    Ext.apply(actions, widget.actions);
                }
            });
        });

        me.control(actions);
        me.callParent(arguments);
    },

    onLaunch: function () {
        // jQuery
        Ext.Loader.loadScript(rootUrl + 'libs/jQuery/jquery-1.9.1.min.js');
      //  Gkh.signalR.start();
        var me = this;

        Gkh.signalR.start();
        var viewPortal = this.getView('Portal');
        if (viewPortal) {
            var portal = viewPortal.create(),
                panel = portal.down('portalpanel');

            me.widgets.forEach(function (elements, col) {
                var columnContainer = panel.getComponent('col-' + col);

                Ext.iterate(elements.sort(function(a, b){
                    return (a.position || Number.MAX_VALUE) - (b.position || Number.MAX_VALUE);
                }), function (el) {
                    columnContainer.add(el);
                });
            });
        }
    },

    onMenuItemClick: function(record, icmenu) {
        var moduleScript = record.get('moduleScript');

        if (moduleScript.indexOf('B4.controller') > -1) {
            if (Ext.History.getToken() == moduleScript) {
                return;
            }
            this.loadController(record.get('moduleScript'), null, null, function() { icmenu.enableItem(record.id); });
        } else {
            Ext.History.add(moduleScript);
        }
    },
    
    selectHomeView: function () {
        var container = Ext.ComponentQuery.query(this.containerSelector)[0];
        container.setActiveTab(0);
    },

    onLogout: function () {
      //  Gkh.signalR.stop();
        window.location = B4.Url.action('LogOut', 'Login');
    },

    onProfile: function () {
        Ext.History.add('profilesettingadministration');
        //this.loadController('B4.controller.administration.ProfileSetting');
    },

    onAllNews: function () {
        this.loadController('B4.controller.Public.News');
    },
    
    onHelpBtnClick: function() {
        var url = B4.Url.action('/Instruction/GetMainInstruction');
        window.open(url, '_blank');
    },

    goToInstructionsBtnClick: function() {
        var me = this;
        me.getB4TabPanel();
        if (!me.helpWindow) {
            me.helpWindow = Ext.create('B4.view.instructions.Window', {
                renderTo: B4.getBody().getEl()
            });
        }
        this.helpWindow.show();
    },

    onLogout: function() {
        Gkh.signalR.stop();
        window.location = B4.Url.action('LogOut', 'Login');
    }
});
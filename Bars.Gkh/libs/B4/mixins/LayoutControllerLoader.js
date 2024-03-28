/**
* @mixin
*
*/
Ext.define('B4.mixins.LayoutControllerLoader', {
    containerSelector: '#contentPanel',
    controllers: null,
    allowUrlHistory: true,

    loadController: function (controllerName, params, containerSelector, callback, callbackUnMask) {
        var me = this,
            portal,
            controllerIds,
            controller;

        if (!me.controllers) {
            me.controllers = new Ext.util.MixedCollection();
        }

        if (me.application && me.application.controllers) {
            controllerIds = Ext.Array.pluck(me.application.controllers.items, "id");
            if ($.inArray('PortalController', controllerIds) !== -1) {
                portal = me.getController('PortalController');
            }
        }
        
        if (portal && portal.controllers) {
            portal.controllers.each(function (item) {
                if (!this.controllers.containsKey(item.id)) {
                    this.controllers.add(item.id, item);
                }
            }, me);
        }

        Ext.History.add(controllerName, true);
        if (me.controllers.containsKey(controllerName)) {
            controller = me.controllers.getByKey(controllerName);

            if (params) {
                params.callbackUnMask = callbackUnMask;
                controller.params = params;
            }

            if (!controller.mainViewIsClosed) {

                var container;
                if (containerSelector)
                    container = Ext.ComponentQuery.query(containerSelector)[0];
                else
                    container = Ext.ComponentQuery.query(me.containerSelector)[0];

                var component = controller.getMainComponent({ token: controllerName });

                if (container.items.containsKey(component.itemId)) {
                    if (container.xtype == 'b4tabpanel' || container.xtype == 'tabpanel') {
                        container.setActiveTab(component);
                    } else if (container.xtype == 'formwindow' || container.xtype == 'window') {
                        container.show();
                    }
                } else {
                    me.addComponent(controller, controllerName, containerSelector);
                }

            } else {
                me.addComponent(controller, controllerName, containerSelector);
            }

            controller.onLaunch(me.application);
            
            if (callback && Ext.isFunction(callback)) {
                callback.call();
            }
                    }
        else {
            Ext.require(controllerName, function () {
                controller = this.application.getController(controllerName);
                this.controllers.add(controllerName, controller);

                //Если в родительской коллекции нет такого контроллера то добавляем его и в родителя
                if (portal && portal.controllers && !portal.controllers.containsKey(controllerName)) {
                    portal.controllers.add(controllerName, controller);
                }

                controller.init(this.application);

                if (params) {
                    params.callbackUnMask = callbackUnMask;
                    controller.params = params;
                }

                this.addComponent(controller, controllerName, containerSelector);

                controller.onLaunch(this.application);

                if (callback && Ext.isFunction(callback)) {
                    callback.call();
                }
                
            }, me);
        }
    },

    addComponent: function (controller, controllerName, containerSelector) {
        var me = this;
        var container;
        if (containerSelector)
            container = Ext.ComponentQuery.query(containerSelector)[0];
        else
            container = Ext.ComponentQuery.query(me.containerSelector)[0];

        var component = controller.getMainComponent({token: controllerName});

        if (!Ext.isFunction(controller.onBeforeAddComponent) || controller.onBeforeAddComponent(container, component) !== false) {
            component.moduleName = controllerName;
            container.add(component);
            if (container.xtype == 'b4tabpanel' || container.xtype == 'tabpanel') {
                container.setActiveTab(component);
            } else if (container.xtype == 'formwindow' || container.xtype == 'window') {
                container.show();
            }
            container.doLayout();
        }
    }
});
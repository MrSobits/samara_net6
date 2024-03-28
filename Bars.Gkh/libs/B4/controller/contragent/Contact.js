Ext.define('B4.controller.contragent.Contact', {
    extend: 'B4.controller.MenuItemController',

    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.permission.GkhPermissionAspect',
        'B4.aspects.EntityChangeLog'
    ],

    models: [
        'contragent.Contact'
    ],

    stores: [
        'contragent.Contact'
    ],

    views: [
        'contragent.ContactPanel',
        'contragent.ContactEditWindow',
        'contragent.ContactCasesPanel'
    ],

    mainView: 'contragent.ContactPanel',
    mainViewSelector: 'contragentcontactpanel',

    mixins: {
        context: 'B4.mixins.Context'
    },

    refs: [
        {
            ref: 'mainView',
            selector: 'contragentcontactpanel'
        }
    ],

    parentCtrlCls: 'B4.controller.contragent.Navi',

    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                { name: 'Gkh.Orgs.Contragent.Register.Contact.Create', applyTo: 'b4addbutton', selector: 'contragentContactGrid' },
                { name: 'Gkh.Orgs.Contragent.Register.Contact.Edit', applyTo: 'b4savebutton', selector: '#contragentContactEditWindow' },
                { name: 'Gkh.Orgs.Contragent.Register.Contact.Delete', applyTo: 'b4deletecolumn', selector: 'contragentContactGrid',
                    applyBy: function(component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                },
                {
                    name: 'Gkh.Orgs.Contragent.Register.Contact.ChangeLog_View',
                    applyTo: 'entitychangeloggrid',
                    selector: 'contragentcontactpanel',
                    applyBy: function (component, allowed) {
                        var tabPanel = component.ownerCt;
                        if (allowed) {
                            tabPanel.showTab(component);
                        } else {
                            tabPanel.hideTab(component);
                        }
                    }
                }
            ]
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'contragentContactGridWindowAspect',
            gridSelector: 'contragentContactGrid',
            editFormSelector: '#contragentContactEditWindow',
            storeName: 'contragent.Contact',
            modelName: 'contragent.Contact',
            editWindowView: 'contragent.ContactEditWindow',
            listeners: {
                getdata: function (asp, record) {
                    if (!record.data.Id) {
                        record.data.Contragent = asp.controller.getContextValue(asp.controller.getMainComponent(), 'contragentId');
                    }
                }
            },
            otherActions: function (actions) {
                actions['contragentContactGrid #btnFillDL'] = { 'click': { fn: this.fillFromDL, scope: this } };              
            },
            fillFromDL: function (btn) {
                var me = this;
                debugger;
                me.mask('Получение данных из реестра должностных лиц', me.controller.getMainComponent());

                B4.Ajax.request({
                    url: B4.Url.action('UpdateContactsFromDL', 'Contragent'),
                    params: {
                        contragentId: me.controller.getContextValue(me.controller.getMainComponent(), 'contragentId')
                    },
                    timeout: 9999999
                }).next(function (response) {                  

                    me.unmask();
                    var grid = btn.up('contragentContactGrid'),
                        store = grid.getStore();
                    store.load();
                    return true;
                }).error(function (response) {
                    Ext.Msg.alert('Ошибка', response.message);
                    me.unmask();

                    me.getStore('contragent.Contact').load();
                    return false;
                });
            },
        },
        {
            xtype: 'entitychangelogaspect',
            gridSelector: 'contragentcontactpanel entitychangeloggrid',
            entityType: 'Bars.Gkh.Entities.ContragentContact',
            inheritEntityChangeLogCode: 'ContragentContact',
            getEntityId: function() {
                var asp = this,
                    me = asp.controller;
                return me.getContextValue(me.getMainView(), 'contragentId');
            }
        }
    ],

    init: function () {
        var me = this;

        me.getStore('contragent.Contact').on('beforeload', me.onBeforeLoad, me);
        me.callParent(arguments);
    },

    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('contragentcontactpanel');

        me.bindContext(view);
        me.setContextValue(view, 'contragentId', id);
        me.application.deployView(view, 'contragent_info');

        this.getStore('contragent.Contact').load();
    },

    onBeforeLoad: function (store, operation) {
        var me = this;
        operation.params.contragentId = me.getContextValue(me.getMainComponent(), 'contragentId');
    }
});
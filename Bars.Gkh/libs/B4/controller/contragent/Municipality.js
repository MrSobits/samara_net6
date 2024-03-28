Ext.define('B4.controller.contragent.Municipality', {
    extend: 'B4.controller.MenuItemController',
    
    requires: [
        'B4.form.SelectWindow',
        'B4.aspects.permission.GkhGridPermissionAspect'
    ],

    stores: [
        'contragent.MunicipalityStore',
        'dict.Municipality'
    ],

    views: [
        'contragent.MunicipalityGrid'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    refs: [
        {
            ref: 'mainView',
            selector: 'contragentmunicipalitygrid'
        }
    ],

    parentCtrlCls: 'B4.controller.contragent.Navi',

    aspects:[
        {
            xtype: 'gkhgridpermissionaspect',
            gridSelector: 'contragentmunicipalitygrid',
            permissionPrefix: 'Gkh.Orgs.Contragent.Register.Municipality'
        }
    ],

    init: function () {
        var me = this;
        this.control({
            'contragentmunicipalitygrid': {            
                render: function(g) {
                    g.getStore().on('beforeload', me.onBeforeLoad, me);
                },
                rowaction: { fn: me.onRowAction, scope: me }
            },
            
            'contragentmunicipalitygrid [actionName="add"]': {
                click: { fn: me.onAddClick, scope: me }
            },
            
            'contragentmunicipalitygrid [actionName="update"]': {
                click: { fn: me.onUpdateClick, scope: me }
            }
        });

        me.callParent(arguments);
    },
    
    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('contragentmunicipalitygrid');

        me.bindContext(view);
        me.setContextValue(view, 'contragentId', id);
        me.application.deployView(view, 'contragent_info');

        this.getMainComponent().getStore().load();
        me.callParent(arguments);
    },
    
    onBeforeLoad: function (store) {
        var me = this;
        Ext.apply(store.getProxy().extraParams, {            
            contragentId: me.getContextValue(me.getMainComponent(), 'contragentId')
        });
    },
    
    onUpdateClick: function () {
        this.getMainComponent().getStore().load();
    },
    
    onAddClick: function() {
        var me = this;

        me.addView = Ext.create('B4.form.SelectWindow', {
            modal: true,
            selectionMode: 'MULTI',
            store: 'B4.store.contragent.MunicipalitySelectStore',
            renderTo: B4.getBody().getActiveTab().getEl(),
            loadDataOnShow: true,
            columns: [
                { header: 'Наименование', dataIndex: 'Name', filter: { xtype: 'textfield' }, flex: 1 }
            ],
            selectWindowCallback: function(data) {
                me.onMunicipalitySelect(data);
            }
        });

        me.addView.on('beforeload', me.onMunicipalityBeforeLoad, me);
        me.addView.show();
    },
    
    onMunicipalityBeforeLoad: function (w, opts, store) {
        var me = this;
        Ext.apply(store.getProxy().extraParams, {
            contragentId: me.getContextValue(me.getMainComponent(), 'contragentId')
        });
    },
    
    onMunicipalitySelect: function (data) {
        var me = this;
        var result = [],
            contragent = { Id: me.getContextValue(me.getMainComponent(), 'contragentId') };
        Ext.each(data, function(el) {
            result.push({
                Contragent: contragent,
                Municipality: { Id: el.Id }
            });
        });

        me.getMainComponent().mask();
        B4.Ajax.request({
            url: B4.Url.action('Create', 'ContragentMunicipality'),
            params: {
                records: Ext.JSON.encode(result)
            }
        }).next(function() {
            me.getMainComponent().unmask();
            me.getMainComponent().getStore().load();
        }).error(function (e) {
            me.getMainComponent().unmask();
            throw e;
        });
    },
    
    onRowAction: function (grid, action, rec) {
        var me = this;
        if (action === 'delete') {
            me.getMainComponent().mask();
            B4.Ajax.request({
                url: B4.Url.action('Delete', 'ContragentMunicipality'),
                params: {
                    records: Ext.JSON.encode([rec.get('Id')])
                }
            }).next(function() {
                me.getMainComponent().unmask();
                me.getMainComponent().getStore().load();
            }).error(function(e) {
                me.getMainComponent().unmask();
                throw e;
            });
        }
    }
});
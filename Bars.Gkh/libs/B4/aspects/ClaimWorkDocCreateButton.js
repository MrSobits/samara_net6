/*
Данный аспект предназначен для кнопки в которой будут подтягиватся возможные пункты для формирваония документов
*/

Ext.define('B4.aspects.ClaimWorkDocCreateButton', {
    extend: 'B4.base.Aspect',

    alias: 'widget.claimworkdoccreatebuttonaspect',

    buttonSelector: null,
    typeDocument: null,
    rulesStore: null,
    claimWorkId: 0,
    claimWorkType: null,
    params: {},

    constructor: function (config) {
        var me = this;

        Ext.apply(me, config);
        me.callParent(arguments);

        me.addEvents(
            'createsuccess',
            'beforecreate',
            'validateparams'
        );

        me.on('createsuccess', me.onCreateSuccess, me);
        me.on('validateparams', me.onValidateParams, me);
    },
    
    init: function (controller) {
        var me = this,
            actions = {};

        me.callParent(arguments);

        actions[me.buttonSelector + ' menuitem'] = { 'click': { fn: me.onMenuItemClick, scope: me } };

        me.rulesStore = Ext.create('Ext.data.Store', {
            autoLoad: false,
            fields: ['Id', 'Name', 'ActionUrl'],
            proxy: {
                autoLoad: false,
                type: 'ajax',
                url: B4.Url.action('GetListRules', 'BaseClaimWork'),
                reader: {
                    type: 'json',
                    root: 'data'
                }
            }
        });

        me.rulesStore.on('beforeload', me.onBeforeLoadStore, me);
        me.rulesStore.on('load', me.onLoadStore, me);

        controller.control(actions);
    },

    setData: function (id, type) {
        var me = this,
            proxy,
            controllerName = me.getControllerByType(type);

        me.claimWorkId = id;
        me.claimWorkType = type;

        if (controllerName !== null) {
            proxy = {
                autoLoad: false,
                type: 'ajax',
                url: B4.Url.action('GetListRules', controllerName, {
                    claimWorkId: id
                }),
                reader: {
                    type: 'json',
                    root: 'data'
                }
            };

            me.rulesStore.setProxy(proxy);
        }

        me.rulesStore.load();
    },

    onCreateSuccess:function() {
        var me = this;
        me.rulesStore.load();
    },

    reloadMenu: function() {
        this.rulesStore.load();
    },

    onBeforeLoadStore: function () {
    },

    onLoadStore: function (store) {
        var btn = this.componentQuery(this.buttonSelector);

        if (btn) {
            btn.setDisabled(true);
            
            btn.menu.removeAll();

            store.each(function (rec) {
                
                btn.setDisabled(false);
                
                btn.menu.add({
                    xtype: 'menuitem',
                    text: rec.get('Name'),
                    textAlign: 'left',
                    ruleId: rec.get('Id'),
                    actionUrl: rec.get('ActionUrl'),
                    iconCls: 'icon-report'
                });
            });
        }
    },

    getParams: function (ruleBtn) {
       var me = this;

        return {
            ruleId: ruleBtn.ruleId,
            claimWorkId: me.claimWorkId,
            claimWorkType: me.claimWorkType,
            actionUrl: ruleBtn.actionUrl
        };
    },

    onMenuItemClick: function (item) {
        var me = this,
            params = me.getParams(item);
        
        if (me.fireEvent('validateparams', me, params) !== false) {
            me.createDocument(params);
        }
    },

    onValidateParams:function(asp, params) {
        if (!params) {
            Ext.Msg.alert('Ошибка формирования документа', 'Не заполнены параметры, необходимые для формирования документа.');
            
            return false;
        }
        
        if (!params.ruleId) {
            Ext.Msg.alert('Ошибка формирования документа', 'Не заполнен параметр ruleId');

            return false;
        }
        
        if (!params.claimWorkId) {
            Ext.Msg.alert('Ошибка формирования документа', 'Не заполнен параметр claimWorkId');
            return false;
        }

        if (!params.claimWorkType) {
            Ext.Msg.alert('Ошибка формирования документа', 'Не заполнен параметр claimWorkType');
            return false;
        }

        return this.onValidateUserParams(params);
    },
    
    onValidateUserParams: function(params)
    {
        return true;    
    },

    createDocument: function (params) {
        var me = this,
            data,
            container = me.componentQuery(me.containerSelector);
        
        me.controller.mask('Формирование документа', container);
        
        B4.Ajax.request({
            url: B4.Url.action('CreateDocument', 'ClaimWorkDocument'),
            method: 'POST',
            timeout: 9999999,
            params: params
        }).next(function (res) {
            data = Ext.decode(res.responseText);    

            me.fireEvent('createsuccess', me);

            Ext.History.add(Ext.String.format("claimwork/{0}/{1}/{2}/{3}/aftercreatedoc", params.claimWorkType, params.claimWorkId, data.Id, params.actionUrl));

            me.controller.unmask();
        }).error(function (e) {
            me.controller.unmask();
            Ext.Msg.alert('Ошибка формирования документа!', e.message||e);
        });
    },

    getControllerByType: function (type) {
        
        return type;
    }
});
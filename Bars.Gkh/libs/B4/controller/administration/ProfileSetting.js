Ext.define('B4.controller.administration.ProfileSetting', {
    extend: 'B4.base.Controller',
    params: null,
    requires: [
        'B4.Url',
        'B4.Ajax',
        'B4.aspects.GkhEditPanel',
        'B4.aspects.permission.administration.Profile'
    ],
    models: ['administration.Operator'],
    views: ['administration.profilesetting.EditPanel'],
    
    aspects: [
        {
            xtype: 'profileadminperm'
        },
        {
            xtype: 'gkheditpanel',
            name: 'profileSettingEditPanelAspect',
            editPanelSelector: 'profileSettingEditPanel',
            modelName: 'administration.Operator',
            saveRecord: function (rec) {
                var me = this;
                me.controller.mask('Сохранение', me.controller.getMainComponent());
                B4.Ajax.request({
                    method: 'POST',
                    url: B4.Url.action('ChangeProfile', 'Operator'),
                    params: {
                        record: Ext.JSON.encode(rec.data)
                    }
                }).next(function (response) { 
                    var resp = Ext.JSON.decode(response.responseText);
                    me.controller.unmask();
                    Ext.Msg.alert('Сохранение!', resp.message);
                    return true;
                }).error(function (response) {
                    var resp = Ext.decode(response.responseText);
                    me.controller.unmask();
                    Ext.Msg.alert('Ошибка!', resp.message);
                });
            }
        }
    ],
    
    mainView: 'administration.profilesetting.EditPanel',
    mainViewSelector: 'profileSettingEditPanel',
    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },
    
    refs: [
        {
            ref: 'mainView',
            selector: 'profileSettingEditPanel'
        }
    ],

    index: function () {
        var view = this.getMainView() || Ext.widget('profileSettingEditPanel');
        this.bindContext(view);
        this.application.deployView(view);
        var asp = this.getAspect('profileSettingEditPanelAspect');
                
        B4.Ajax.request(B4.Url.action('GetActiveOperatorId', 'Operator'))
            .next(function (response) {
                var obj = Ext.JSON.decode(response.responseText);
                asp.setData(obj.data.Id);
            }, this)
            .error(function () {

            }, this);
    }
});



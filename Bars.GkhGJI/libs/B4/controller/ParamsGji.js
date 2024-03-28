Ext.define('B4.controller.ParamsGji', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.view.paramsgji.Panel',
        'B4.aspects.permission.GkhPermissionAspect'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    refs: [
        {
            ref: 'mainPanel',
            selector: 'paramsgjipanel'
        }
    ],

    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            name: 'permissionaspect',
            permissions: [

            ]
        }
    ],

    init: function () {
        var me = this;

        me.control({
            'paramsgjipanel b4savebutton': { 'click': { fn: me.saveParams, scope: me } }
        });

        me.callParent(arguments);
    },

    index: function() {
        var me = this,
            view = me.getMainPanel() || Ext.widget('paramsgjipanel');

        me.setParams(view);
        me.bindContext(view);
        me.application.deployView(view);
    },

    setParams: function(panel) {
        var me = this,
            form = panel.getForm();

        B4.Ajax.request({
            url: B4.Url.action('GetParams', 'GjiParams')
        }).next(function(resp) {
            var response = Ext.decode(resp.responseText);
            form.setValues(response.data);
        }).error(function() {
            me.unmask();
            Ext.Msg.alert('Ошибка!', 'При получении параметров произошла ошибка!');
        });
    },

    saveParams: function (btn) {
        var me = this,
            panel = btn.up('paramsgjipanel'),
            form = panel.getForm(),
            valid = form.isValid(),
            values = form.getValues(false, false, false, true),
            gjiparams = Ext.encode(values);
        
        if (valid) {
            me.mask('Сохранение', B4.getBody());
            B4.Ajax.request({
                url: B4.Url.action('SaveParams', 'GjiParams'),
                params: { gjiparams: gjiparams }
            }).next(function () {
                me.unmask();
            }).error(function () {
                me.unmask();
                Ext.Msg.alert('Ошибка!', 'При сохранении параметров произошла ошибка!');
            });
        } else {
            Ext.Msg.alert('Ошибка!', 'Проверьте правильность заполнения формы!');
        }
    }
});
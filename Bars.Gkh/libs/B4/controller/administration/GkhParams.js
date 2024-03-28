Ext.define('B4.controller.administration.GkhParams', {
    extend: 'B4.base.Controller',
    requires: ['B4.aspects.permission.GkhPermissionAspect'],

    views: [
        'administration.GkhParamsPanel'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    mainView: 'administration.GkhParamsPanel',
    mainViewSelector: 'gkhparamspanel',

    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                {
                    name: 'Administration.GkhParams.Edit',
                    applyTo: 'b4savebutton',
                    selector: 'gkhparamspanel'
                }
            ]
        }
    ],

    init: function () {
        var me = this,
            actions = {
                'gkhparamspanel b4savebutton': {
                    click: {
                        fn: me.saveParams,
                        scope: me
                    }
                },
                'gkhparamspanel #MoLevel': {
                    change: {
                        fn: me.showDkrMessage,
                        scope: me
                    }
                }
            };

        me.control(actions);
        me.callParent(arguments);
    },

    index: function () {
        var view = this.getMainView() || Ext.widget('gkhparamspanel');
        
        this.bindContext(view);
        this.application.deployView(view);
        
        this.setParams(view);
    },
    
    showDkrMessage: function (me, newValue, oldValue, eOpts) {
        
        if (newValue!='' && newValue !== oldValue) {
            Ext.Msg.alert('Внимание!', 'Параметры приложения будут изменены. После сохранения параметров программу ДПКР необходимо рассчитать заново');
        }

    },

    setParams: function (panel) {
        var me = this,
            form = panel.getForm();

        B4.Ajax.request({
            url: B4.Url.action('GetParams', 'GkhParams')
        }).next(function (resp) {
            var response = Ext.decode(resp.responseText);
            form.setValues(response);
        }).error(function () {
            me.unmask();
            Ext.Msg.alert('Ошибка!', 'При получении параметров произошла ошибка!');
        });
    },

    saveParams: function (btn) {
        var me = this,
            panel = btn.up('gkhparamspanel'),
            form = panel.getForm();

        if (form.isValid()) {
            me.mask('Сохранение', B4.getBody());
            me.sendSaveParams(form.getValues(false, false, true, true), me);
        } else {
            Ext.Msg.alert('Ошибка!', 'Проверьте правильность заполнения формы!');
        }
    },

    sendSaveParams: function (values, scope) {
        B4.Ajax.request({
            url: B4.Url.action('SaveParams', 'GkhParams'),
            params: values
        }).next(function (resp) {
            scope.unmask();
        }).error(function () {
            scope.unmask();
            Ext.Msg.alert('Ошибка!', 'При сохранении параметров произошла ошибка!');
        });
    }
});
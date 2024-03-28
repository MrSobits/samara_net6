Ext.define('B4.controller.export.OverhaulToGasu', {
    extend: 'B4.base.Controller',

    requires: [
       'B4.aspects.ButtonDataExport',
       'B4.aspects.permission.GkhPermissionAspect'
    ],

    views: ['export.OverhaulToGasu'],

    refs: [{
        ref: 'mainView',
        selector: 'exportoverhaultogasu'
    }],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            name: 'exportOverhaulToGasu',
            permissions: [
                {
                    name: 'Export.GasuService_Edit',
                    applyTo: '[name=ServiceUrl]',
                    selector: 'exportoverhaultogasu',
                    applyBy: function(component, allowed)
                    {
                        component.setDisabled(!allowed);
                    }
                },
                {
                    name: 'Export.GasuService_Edit',
                    applyTo: '[name=UserName]',
                    selector: 'exportoverhaultogasu',
                    applyBy: function(component, allowed)
                    {
                        component.setDisabled(!allowed);
                    }
                },
                {
                    name: 'Export.GasuService_Edit',
                    applyTo: '[name=UserPassword]',
                    selector: 'exportoverhaultogasu',
                    applyBy: function(component, allowed)
                    {
                        component.setDisabled(!allowed);
                    }
                }
            ]
        }
    ],

    init: function () {
        var me = this;

        me.control({
            'exportoverhaultogasu [action=Export]': {
                click: { fn: me.onLoadData, scope: me }
            },
            'exportoverhaultogasu [name=ServiceUrl]': { change: { fn: me.changeFormValue, scope: me } },
            'exportoverhaultogasu [name=UserName]': { change: { fn: me.changeFormValue, scope: me } },
            'exportoverhaultogasu [name=UserPassword]': { change: { fn: me.changeFormValue, scope: me } },
            'exportoverhaultogasu [name=PeriodStart]': { change: { fn: me.changeFormValue, scope: me } },
            'exportoverhaultogasu [name=ProgramCr]': {
                change: { fn: me.changeFormValue, scope: me },
                trigger2Click: { fn: me.changeFormValue, scope: me }
            }
        });

        me.callParent(arguments);
    },

    changeFormValue: function()
    {
        var me = this,
            panel = me.getMainView();
        
        var serviceUrl = panel.down('[name=ServiceUrl]').getValue();
        var userName = panel.down('[name=UserName]').getValue();
        var userPassword = panel.down('[name=UserPassword]').getValue();
        var dateStart = panel.down('[name=PeriodStart]').getValue();
        var programCr = panel.down('[name=ProgramCr]').getValue();
        
        var areFieldsFilled = serviceUrl && userName && userPassword && dateStart && programCr;
        panel.down('[action=Export]').setDisabled(!areFieldsFilled);
    },

    onLoadData: function () {
        var me = this,
            panel = me.getMainView();

        me.mask('Передача данных...', me.getMainComponent());

        var serviceUrl = panel.down('[name=ServiceUrl]').getValue();
        var userName = panel.down('[name=UserName]').getValue();
        var userPassword = panel.down('[name=UserPassword]').getValue();
        var dateStart = panel.down('[name=PeriodStart]').getValue();
        var programCrId = panel.down('[name=ProgramCr]').getValue();
        
        var params = {
            serviceUrl: serviceUrl,
            userName: userName,
            userPassword: userPassword,
            dateStart: dateStart,
            programCrId: programCrId,
            exportName: 'OverhaulToGasuExport'
        };

        B4.Ajax.request({
            url: B4.Url.action('SendGasu', 'GasuImportExport'),
            method: 'POST',
            timeout: 9999999,
            params: params
        }).next(function (res) {
            me.unmask();
            Ext.Msg.show({
                title: 'Передача данных',
                msg: 'Передача данных прошла успешно',
                width: 300,
                buttons: Ext.Msg.OK
            });
        }).error(function (e) {
            me.unmask();
            Ext.Msg.alert('Ошибка', 'Не удалось передать данные');
        });
    },

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget('exportoverhaultogasu');
        me.bindContext(view);
        me.application.deployView(view);
        
        B4.Ajax.request({
            url: B4.Url.action('GetServiceData', 'GasuImportExport'),
            method: 'GET',
            timeout: 9999999
        }).next(function (result) {
            var data = JSON.parse(result.responseText);
            view.down('[name=ServiceUrl]').setValue(data.ServiceUrl);
            view.down('[name=UserName]').setValue(data.UserName);
            view.down('[name=UserPassword]').setValue(data.UserPassword);
        });
    }
});
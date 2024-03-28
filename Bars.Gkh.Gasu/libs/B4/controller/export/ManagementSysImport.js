Ext.define('B4.controller.export.ManagementSysImport', {
    extend: 'B4.base.Controller',

    requires: [
       'B4.aspects.ButtonDataExport',
       'B4.aspects.permission.GkhPermissionAspect'
    ],

    views: ['export.ManagementSysImport'],

    mainView: 'export.ManagementSysImport',
    mainViewSelector: 'managementsysimport',

    refs: [{
        ref: 'mainView',
        selector: 'managementsysimport'
    }],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            name: 'managementSysImport',
            permissions: [
                {
                    name: 'Export.GasuService_Edit',
                    applyTo: '[name=ServiceUrl]',
                    selector: 'managementsysimport',
                    applyBy: function(component, allowed)
                    {
                        component.setDisabled(!allowed);
                    }
                },
                {
                    name: 'Export.GasuService_Edit',
                    applyTo: '[name=UserName]',
                    selector: 'managementsysimport',
                    applyBy: function(component, allowed)
                    {
                        component.setDisabled(!allowed);
                    }
                },
                {
                    name: 'Export.GasuService_Edit',
                    applyTo: '[name=UserPassword]',
                    selector: 'managementsysimport',
                    applyBy: function(component, allowed)
                    {
                        component.setDisabled(!allowed);
                    }
                }
            ]
        },
        {
            xtype: 'b4buttondataexportaspect',
            name: 'objectCrButtonExportAspect',
            gridSelector: '#objectCrGrid',
            buttonSelector: 'managementsysimport #btnExport',
            controllerName: 'ObjectCr',
            actionName: 'Export'
        }
    ],

    init: function () {
        var me = this, panel;

        me.control({
            'managementsysimport [action=Export]': {
                click: { fn: me.onLoadData, scope: me }
            },
            'managementsysimport [action=Save]': {
                click: { fn: me.onSaveData, scope: me }
            },

            'managementsysimport [name=ServiceUrl]': { change: { fn: me.changeFormValue, scope: me } },
            'managementsysimport [name=UserName]': { change: { fn: me.changeFormValue, scope: me } },
            'managementsysimport [name=UserPassword]': { change: { fn: me.changeFormValue, scope: me } },
            'managementsysimport [name=PeriodStart]': { change: { fn: me.changeFormValue, scope: me } }
        });

        me.callParent(arguments);
    },

    changeFormValue: function()
    {
        var me = this,
            panel = me.getMainView(),
            serviceUrl = panel.down('[name=ServiceUrl]').getValue(),
           userName = panel.down('[name=UserName]').getValue(),
           userPassword = panel.down('[name=UserPassword]').getValue(),
           dateStart = panel.down('[name=PeriodStart]').getValue();

        var areFieldsFilled = serviceUrl && userName && userPassword && dateStart;
        panel.down('[action=Export]').setDisabled(!areFieldsFilled);
        panel.down('[action=Save]').setDisabled(!(serviceUrl && userName && userPassword));
    },

    onLoadData: function () {
        var asp = this,
            panel = asp.getMainView();

        asp.mask('Передача данных...', asp.getMainComponent());

        var serviceUrl = panel.down('[name=ServiceUrl]').getValue();
        var userName = panel.down('[name=UserName]').getValue();
        var userPassword = panel.down('[name=UserPassword]').getValue();
        var dateStart = panel.down('[name=PeriodStart]').getValue();

        var params = {
            serviceUrl: serviceUrl,
            userName: userName,
            userPassword: userPassword,
            dateStart: dateStart,
            exportName: 'ManagementSysExport'
        };

        B4.Ajax.request({
            url: B4.Url.action('SendGasu', 'GasuImportExport'),
            method: 'POST',
            timeout: 9999999,
            params: params
        }).next(function (res) {
            asp.unmask();
            Ext.Msg.show({
                title: 'Передача данных',
                msg: 'Передача данных прошла успешно',
                width: 300,
                buttons: Ext.Msg.OK
            });
        }).error(function (e) {
            asp.unmask();
            Ext.Msg.alert('Ошибка', 'Не удалось передать данные');
        });
    },

    onSaveData: function () {
        var asp = this,
            panel = asp.getMainView();

        asp.mask('Сохранение данных...', asp.getMainView());

        var serviceUrl = panel.down('[name=ServiceUrl]').getValue();
        var userName = panel.down('[name=UserName]').getValue();
        var userPassword = panel.down('[name=UserPassword]').getValue();

        var params = {
            serviceUrl: serviceUrl,
            userName: userName,
            userPassword: userPassword
        };

        B4.Ajax.request({
            url: B4.Url.action('SetServiceData', 'GasuImportExport'),
            method: 'POST',
            timeout: 9999999,
            params: params
        }).next(function (res) {
            asp.unmask();
            Ext.Msg.show({
                title: 'Сохранение данных',
                msg: 'Сохранение данных прошло успешно',
                width: 300,
                buttons: Ext.Msg.OK
            });
        }).error(function (e) {
            asp.unmask();
            Ext.Msg.alert('Ошибка', 'Не удалось сохранить данные');
        });
    },

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget('managementsysimport');
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
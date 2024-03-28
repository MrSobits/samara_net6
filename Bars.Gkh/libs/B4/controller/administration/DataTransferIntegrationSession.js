Ext.define('B4.controller.administration.DataTransferIntegrationSession', {
    extend: 'B4.base.Controller',
    params: {},
    requires: [
        'B4.aspects.permission.GkhPermissionAspect',
        'B4.enums.DataTransferOperationType',
        'B4.enums.TransferingState'
    ],
    models: [
        'administration.DataTransferIntegrationSession'
    ],
    stores: [
        'administration.DataTransferIntegrationSession'
    ],

    views: [
        'administration.TransferIntegrationSessionGrid',
        'B4.view.administration.ExportableTypesGrid'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    mainView: 'administration.TransferIntegrationSessionGrid',
    mainViewSelector: 'transferintegrationsessiongrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'transferintegrationsessiongrid'
        },
        {
            ref: 'exportView',
            selector: 'exportabletypeswindow'
        }
    ],

    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                { name: 'Administration.DataTransferIntegration.Import', applyTo: 'button[action=RunIntegration]', selector: 'transferintegrationsessiongrid' }
            ]
        }
    ],

    init: function () {
        var me = this;

        me.control({
            'exportabletypesgrid button[action=RunIntegration]': {
                'click': {
                    fn: me.runIntegration,
                    scope: me
                }
            },
            'transferintegrationsessiongrid button[action=RunIntegration]': {
                'click': {
                    fn: me.getExportableTypes,
                    scope: me
                }
            },
            'transferintegrationsessiongrid b4updatebutton': {
                'click': {
                    fn: function() {
                        me.getMainView().getStore().load();
                    },
                    scope: me
                }
            }
        });

        me.callParent(arguments);
    },

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget('transferintegrationsessiongrid');;
        
        me.bindContext(view);
        me.application.deployView(view);

        view.getStore().load();
    },

    runIntegration: function(button) {
        var me = this,
            grid = button.up('exportabletypesgrid'),
            exportDependencies = grid.down('checkbox[name=exportDependencies]').getValue(),
            records = button.typeIntegration == 'manual'
                ? Ext.Array.map(grid.getSelectionModel().getSelection(),
                    function(el) {
                        return el.get('Id');
                    })
                : null,
            params = { exportDependencies: exportDependencies };

        if (records) {
            params.typeNames = Ext.encode(records);
        }

        Ext.Msg.confirm('Подтвердите действия',
            'Будет запущена интеграция: импорт данных из внешней системы. Продолжить?',
            function(result) {
                if (result === 'yes') {
                    me.mask('Запуск интеграции...', me.getExportView());
                    B4.Ajax.request({
                        url: 'DataTransferIntegration/RunIntegration',
                        params: params
                        })
                    .next(function() {
                            me.unmask();
                            me.getMainView().getStore().load();
                        })
                    .error(function(response) {
                            var resp = response && response.responseText && Ext.decode(response.responseText),
                                respMessage = response.message || (resp && resp.message);

                            me.unmask();
                            Ext.Msg.alert('Ошибка!', respMessage || 'Ошибка при выполнении запуска интеграции');
                        });
                }
            });
    },

    getExportableTypes: function () {
        var window = Ext.create('B4.view.administration.ExportableTypesWindow', {
            renderTo: B4.getBody().getActiveTab().getEl()
        });

        window.down('exportabletypesgrid').getStore().load();

        window.show();
    }
});
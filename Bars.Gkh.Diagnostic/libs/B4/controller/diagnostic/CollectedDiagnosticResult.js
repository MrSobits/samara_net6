Ext.define('B4.controller.diagnostic.CollectedDiagnosticResult', {

    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GridEditWindow'
    ],
    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },
    models: [
        'diagnostic.CollectedDiagnosticResult',
        'diagnostic.DiagnosticResult'
    ],
    stores: [
        'diagnostic.CollectedDiagnosticResult',
        'diagnostic.DiagnosticResult'
    ],
    views: [
        'diagnostic.Grid',
        'diagnostic.EditWindow',
        'diagnostic.DiagnosticResultGrid'
    ],

    mainView: 'diagnostic.Grid',
    mainViewSelector: 'diagnosticgrid',

    refs: [{
        ref: 'mainView',
        selector: 'diagnosticgrid'
    }],

    aspects: [
        {
            xtype: 'grideditwindowaspect',
            name: 'diagnosticGridWindow',
            gridSelector: 'diagnosticgrid',
            editFormSelector: 'diagnosticeditwindow',
            storeName: 'diagnostic.CollectedDiagnosticResult',
            modelName: 'diagnostic.CollectedDiagnosticResult',
            editWindowView: 'diagnostic.EditWindow',
            listeners: {
                aftersetformdata: function (asp, record) {
                    var me = this,
                        grid = me.getForm().down('diagnosticresultgrid');

                    grid.getStore().filter('collectedDiagnosticResultId', record.getId());
                }
            }
        }
    ],

    init: function () {
        var me = this;

        me.control({

            'diagnosticgrid button[action=runDiagnostic]': { 'click': { fn: me.runDiagnostic, scope: me } }
        });

        me.callParent(arguments);
    },

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget('diagnosticgrid');
        me.bindContext(view);
        me.application.deployView(view);
        view.getStore().load();
    },

    runDiagnostic: function(button) {

        var me = this,
            view = me.getMainView();

        me.mask('Диагностика', view);

        B4.Ajax.request({
            url: B4.Url.action('RunDiagnostic', 'CollectedDiagnosticResult'),
            method: 'GET',
            params:{},
            timeout: 60*60*1000 // 1 час
        }).next(function (response) {
            var obj;
            me.unmask();

            me.getMainView().getStore().load();

            try {
                obj = Ext.decode(response.responseText);
            } catch (e) {
                obj = {};
            }

            B4.QuickMsg.msg('Успешно', obj.message || 'Успешно', 'success');
        }).error(function (response) {
            me.unmask();
            var message = 'При диагностике произошла ошибка!';
            if (response && response.message) {
                message = response.message;
            }

            Ext.Msg.alert('Ошибка!', message);
        });

    }



});
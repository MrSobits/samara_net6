Ext.define('B4.controller.integrationtor.IntegrationTor', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.Ajax',
        'B4.Url',
        'B4.model.integrationtor.TorTask',
        'B4.store.integrationtor.TorTask',
        'B4.view.integrationtor.Grid'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    models: ['integrationtor.TorTask'],
    stores: ['integrationtor.TorTask'],
    views: ['integrationtor.Grid'],

    mainView: 'integrationtor.Grid',
    mainViewSelector: 'integrationtorgrid',

    init: function() {
        var me = this;
        me.control({
            'integrationtorgrid actioncolumn[name=getRequest]': {
                'click': function(gridView, rowIndex, colIndex, el, e, rec) {
                    var fileId = rec.get('RequestFileId');
                    me.loadFile(fileId);
                }
            },
            'integrationtorgrid actioncolumn[name=getResponse]': {
                'click': function(gridView, rowIndex, colIndex, el, e, rec) {
                    var fileId = rec.get('ResponseFileId');
                    me.loadFile(fileId);
                }
            },
            'integrationtorgrid actioncolumn[name=getLog]': {
                'click': function(gridView, rowIndex, colIndex, el, e, rec) {
                    var fileId = rec.get('LogFileId');
                    me.loadFile(fileId);
                }
            },
            'integrationtorgrid button[name=getSubjectsButton]': {
                'click': function(btn) {
                    me.mask('Получение субъектов', me.getMainComponent());
                    B4.Ajax.request({
                            url: B4.Url.action('GetAllSubjectsFromTor', 'TorIntegration'),
                            timeout: 9999999
                        })
                        .next(function (response) {
                            btn.up('integrationtorgrid').getStore().load();
                            me.unmask();
                            Ext.Msg.alert('Получение субъектов', response.message || 'Выполнено успешно');
                            return true;
                        }).error(function(e) {
                            me.unmask();
                            Ext.Msg.alert('Ошибка', e.message || 'Произошла ошибка');
                        });
                }
            }
        });

        me.callParent(arguments);
    },

    index: function() {
        var me = this,
            view = me.getMainView() || Ext.widget(me.mainViewSelector);
        me.bindContext(view);
        me.application.deployView(view);

        view.getStore().load();
    },

    loadFile: function (fileId) {
        B4.Ajax.request({
            url: B4.Url.action('CheckFile', 'FileUpload'),
            params: {
                id: fileId
            }
        }).next(function (response) {
            var data = Ext.decode(response.responseText);
            if (!(data && data.success)) {
                Ext.Msg.alert('Ошибка', 'Не удалось загрузить файл');
                return;
            }
            window.open(B4.Url.action('/FileUpload/Download?id=' + fileId));
        });
    }
});
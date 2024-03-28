Ext.define('B4.controller.SendEmsed', {
    extend: 'B4.base.Controller',
    views: ['EmsedListPanel'],

    params: null,
    title: null,
    requires:
    [
        'B4.aspects.DocumentListForEmsed',
        'B4.Ajax',
        'B4.Url'
    ],

    mainView: 'EmsedListPanel',
    mainViewSelector: 'emsedListPanel',

    refs: [
        {
            ref: 'mainView',
            selector: 'emsedListPanel'
        }
    ],

    stores: ['AppealCitsEdoInteg', 'DocsForSendInEmsed'],

    mixins: {
       controllerLoader: 'B4.mixins.LayoutControllerLoader',
       mask: 'B4.mixins.MaskBody',
       context: 'B4.mixins.Context'
    },

    aspects: [
        {
            xtype: 'documentlistforemsedaspect',
            name: 'emsedListPanelAspect',
            panelSelector: '#emsedListPanel',
            gridAppealCitsSelector: '#appealCitsSelectGrid',
            gridDocumentsSelector: '#emsedGrid',
            storeAppealCitsName: 'AppealCitsEdoInteg',
            storeDocuments: 'DocsForSendInEmsed',
            listeners: {
                getdata: function (asp, records) {
                    if (records.length > 0) {
                        asp.controller.mask('Сохранение', asp.controller.getMainComponent());

                        var docs = [];
                        Ext.each(records, function (rec) {
                            var r = {};
                            r.Id = rec.get('Id');
                            r.Type = rec.data.Type;
                            docs.push(r);
                        });

                        B4.Ajax.request(B4.Url.action('Send', 'EdoIntegration', {
                            docs: Ext.JSON.encode(docs),
                            appealCitsId: this.controller.params.appealCitsId
                        })).next(function () {
                            asp.controller.unmask();
                            Ext.Msg.alert('Сообщение', 'Отправка данных завершена успешно');
                            return true;
                        }).error(function (form) {
                            asp.controller.unmask();
                            Ext.Msg.alert('Ошибка при отправки данных', form.message);
                        });
                    }
                    else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать документы');
                        return false;
                    }
                    return true;
                }
            }
        }
    ],

    index: function () {
        var view = this.getMainView() || Ext.widget('emsedListPanel');
        this.bindContext(view);
        this.application.deployView(view);
        this.getAspect('emsedListPanelAspect').loadAppealCitsStore();
    }
});
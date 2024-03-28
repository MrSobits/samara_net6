Ext.define('B4.controller.BaseIntegration', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.Ajax',
        'B4.Url',
        'B4.aspects.GridEditWindow',
        'B4.enums.TypeBase',
        'B4.enums.TypeDocumentGji'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody',
        controllerLoader: 'B4.mixins.LayoutControllerLoader'
    },

    models: [
        'baseintegration.RisTask',
        'baseintegration.TriggerProtocolDataRecord',
        'InspectionGji'
    ],

    stores: [
        'baseintegration.RisTask',
        'baseintegration.TriggerProtocolDataRecord'
    ],

    views: [
        'baseintegration.ProtocolWindow',
        'baseintegration.XmlDataWindow'
    ],

    editFormSelector: null,
    editWindowView: null,
    modelAndStoreName: null,

    aspects: [
        {
            xtype: 'grideditwindowaspect',
            name: 'documentGridWindowAspect',
            listeners: {
                aftersetformdata: function(asp, record) {
                    var view = asp.getForm(),
                        risTaskGrid = view.down('integrationristaskgrid'),
                        risTaskStore = risTaskGrid.getStore();

                    asp.controller.setCurrentParams(record);

                    view.down('actioncolumn[name=showRequestXml]').on('click', asp.controller.showRequestXml, asp.controller);
                    view.down('actioncolumn[name=showResponseXml]').on('click', asp.controller.showResponseXml, asp.controller);
                    view.down('actioncolumn[name=showProtocol]').on('click', asp.controller.showProtocol, asp.controller);
                    view.down('actioncolumn[name=showLog]').on('click', asp.controller.showLog, asp.controller);
                    view.down('button[name=goToDocumentButton]').on('click', asp.controller.goToDocument, asp.controller);
                    
                    risTaskStore.on('beforeload', asp.controller.onBeforeLoad, asp.controller);
                    risTaskStore.load();
                }
            },
            editRecord: function (rec) {
                // заполнение формы без вызова get-метода
                // новый объект, чтобы не происходило изменение записи грида
                this.setFormData(Object.create(rec));
            },
            updateGrid: function () {
                // обновление стора через грид напрямую
                this.getGrid().getStore().load();
            }
        }
    ],

    init: function () {
        var me = this,
            documentGridWindowAspect = me.getAspect('documentGridWindowAspect');

        // Инициализация под конкретный реестр
        documentGridWindowAspect.gridSelector = me.mainViewSelector;
        documentGridWindowAspect.modelName = me.modelAndStoreName;
        documentGridWindowAspect.storeName = me.modelAndStoreName;
        documentGridWindowAspect.editFormSelector = me.editFormSelector;
        documentGridWindowAspect.editWindowView = me.editWindowView;

        me.callParent(arguments);
    },

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget(me.mainViewSelector);
        me.bindContext(view);
        me.application.deployView(view);

        view.getStore().load();
    },

    onBeforeLoad: function (store, operation) {
        var me = this;

        operation.params = operation.params || {};
        operation.params.documentId = me.params.documentId;
    },

    setCurrentParams: function (record) {
        var me = this;

        me.params = me.params || {};
        me.params.documentId = record.getId();
        me.params.inspectionId = record.get('InspectionId');
        me.params.documentType = record.get('DocumentType');
        me.params.documentTypeBase = record.get('DocumentTypeBase');
    },
    
    goToDocument: function () {
        var me = this,
            controllerEditName = me.getEditControllerName(),
            defaultParams = me.getDefaultParams(),
            model = me.getModel('InspectionGji'),
            inspect = new model({ Id: me.params.inspectionId });

        inspect.defaultController = defaultParams.controllerName;
        inspect.defaultParams = {
            inspectionId: inspect.Id,
            documentId: me.params.documentId,
            typeDocument: me.params.documentType,
            title: defaultParams.title
        };

        if (controllerEditName) {
            var portal = me.getController('PortalController');
            portal.loadController(controllerEditName, inspect, portal.containerSelector);
        }
    },
    
    showLog: function(gridView, rowIndex, colIndex, el, e, rec) {
        var triggerId = rec.get('TriggerId');

        if (triggerId > 0) {
            window.open(B4.Url.action('/BaseIntegration/GetLogFile?triggerId=' + triggerId));
        }
    },
    
    showProtocol: function(gridView, rowIndex, colIndex, el, e, rec) {
        var win = Ext.widget('integrationprotocolwindow'),
            grid = win.down('gridpanel');
        grid.store.on({
            beforeload: function(store, operation) {
                operation.params = operation.params || {};
                operation.params.triggerId = rec.get('TriggerId');
            }
        });

        grid.store.load();
        win.show();
    },

    showRequestXml: function (gridView, rowIndex, colIndex, el, e, rec) {
        var me = this;
        if (rec.get('RequestXmlFileId') === 0) {
            return;
        }
        me.showXmlData(rec, true);
    },

    showResponseXml: function (gridView, rowIndex, colIndex, el, e, rec) {
        var me = this;
        if (rec.get('ResponseXmlFileId') === 0) {
            return;
        }
        me.showXmlData(rec, false);
    },

    showXmlData: function (rec, isRequest) {
        var me = this;
        B4.Ajax.request({
            url: B4.Url.action('GetXmlData', 'BaseIntegration'),
            params: {
                taskId: rec.get('Id'),
                isRequest: isRequest
            },
            timeout: 9999999
        }).next(function(response) {
            var xmlWin = Ext.create('B4.view.baseintegration.XmlDataWindow',
                {
                    xmlData: Ext.decode(response.responseText),
                    title: 'XML- файл ' + (isRequest ? 'запроса' : 'ответа')
                });

            xmlWin.show();
        }, me).error(function (e) {
            Ext.Msg.alert('Ошибка!', 'Не получены xml данные' + '<br>' + (e.message || e));
        }, me);
    },

    getDefaultParams: function () {
        var me = this,
            documentType = me.params.documentType,
            title = '', controllerName = '';

        switch (documentType) {
            //Распоряжение
            case B4.enums.TypeDocumentGji.Disposal:
                title = B4.DisposalTextValues.getSubjectiveCase(); 
                controllerName = 'B4.controller.Disposal';
                break;

            //Предостережение
            case B4.enums.TypeDocumentGji.WarningDoc:
                title = 'Предостережение';
                controllerName = 'B4.controller.WarningDoc';
                break;

            //Профилактическое мероприятие
            case B4.enums.TypeDocumentGji.PreventiveAction:
                title = 'Профилактическое мероприятие';
                controllerName = 'B4.controller.preventiveaction.Edit';
                break;

            //Решение
            case B4.enums.TypeDocumentGji.Decision:
                title = 'Решение';
                controllerName = 'B4.controller.Disposal';
                break;
        }

        return { title: title, controllerName: controllerName };
    },

    getEditControllerName: function () {
        var me = this,
            typeBase = me.params.documentTypeBase,
            controllerName;

        switch (typeBase) {
            //Инспекционная проверка
            case B4.enums.TypeBase.Inspection: controllerName = 'B4.controller.baseinscheck.Navigation'; break;

            //Обращение граждан
            case B4.enums.TypeBase.CitizenStatement: controllerName = 'B4.controller.basestatement.Navigation'; break;

            //Плановая проверка юр лиц
            case B4.enums.TypeBase.PlanJuridicalPerson: controllerName = 'B4.controller.basejurperson.Navigation'; break;

            //Распоряжение руководства
            case B4.enums.TypeBase.DisposalHead: controllerName = 'B4.controller.basedisphead.Navigation'; break;

            //Основание предостережения ГЖИ(РТ)
            case B4.enums.TypeBase.GjiWarning: controllerName = 'B4.controller.warninginspection.Navigation'; break;

            //Требование прокуратуры
            case B4.enums.TypeBase.ProsecutorsClaim: controllerName = 'B4.controller.baseprosclaim.Navigation'; break;

            //Постановление прокуратуры
            case B4.enums.TypeBase.ProsecutorsResolution: controllerName = 'B4.controller.resolpros.Navigation'; break;

            //Проверка деятельности ТСЖ
            case B4.enums.TypeBase.ActivityTsj: controllerName = 'B4.controller.baseactivitytsj.Navigation'; break;

            //Отопительный сезон
            case B4.enums.TypeBase.HeatingSeason: controllerName = 'B4.controller.baseheatseason.Navigation'; break;

            //Протокол МВД
            case B4.enums.TypeBase.ProtocolMvd: controllerName = 'B4.controller.protocolmvd.Navigation'; break;

            //План мероприятий
            case B4.enums.TypeBase.PlanAction: controllerName = 'B4.controller.baseplanaction.Navigation'; break;

            //Протокол МЖК
            case B4.enums.TypeBase.ProtocolMhc: controllerName = 'B4.controller.protocolmhc.Navigation'; break;

            //Проверка соискателей лицензии
            case B4.enums.TypeBase.LicenseApplicants: controllerName = 'B4.controller.baselicenseapplicants.Navigation'; break;

            //Без основания
            case B4.enums.TypeBase.Default: controllerName = 'B4.controller.basedefault.Navigation'; break;

            //КНМ без взаимодействия с контролируемыми лицами
            case B4.enums.TypeBase.ActionIsolated: controllerName = 'B4.controller.actionisolated.Navigation'; break;

            //Профилактическое мероприятие
            case B4.enums.TypeBase.PreventiveAction: controllerName = 'B4.controller.preventiveaction.Navigation'; break;

            //Проверки по КНМ без взаимодействия
            case B4.enums.TypeBase.InspectionActionIsolated: controllerName = 'B4.controller.basedefault.Navigation'; break;
        }

        return controllerName;
    }
});
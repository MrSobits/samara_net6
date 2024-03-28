Ext.define('B4.controller.ActSurvey', {
    extend: 'B4.base.Controller',
    params: null,
    objectId: 0,
    requires: [
        'B4.aspects.GjiDocument',
        'B4.aspects.GridEditWindow',
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.aspects.StateButton',
        'B4.aspects.permission.ActSurvey',
        'B4.aspects.GkhButtonPrintAspect',
        'B4.Ajax',
        'B4.Url',
        'B4.aspects.GkhBlobText'
    ],

    models: [
        'ActSurvey',
        'actsurvey.Annex',
        'actsurvey.Photo',
        'actsurvey.Owner',
        'actsurvey.InspectedPart'
    ],

    stores: [
        'actsurvey.Annex',
        'actsurvey.InspectedPart',
        'actsurvey.Owner',
        'actsurvey.Photo',
        'dict.InspectedPartGji',
        'dict.InspectedPartGjiForSelect',
        'dict.InspectedPartGjiForSelected',
        'dict.Inspector',
        'dict.InspectorForSelect',
        'dict.InspectorForSelected'
    ],

    views: ['actsurvey.EditPanel', 

        'actsurvey.AnnexGrid',
        'actsurvey.AnnexEditWindow',
        'actsurvey.OwnerGrid',
        'actsurvey.PhotoGrid',
        'actsurvey.PhotoEditWindow',
        'actsurvey.InspectedPartGrid',
        'actsurvey.InspectedPartEditWindow',
        'SelectWindow.MultiSelectWindow'
    ],

    mainView: 'actsurvey.EditPanel',
    mainViewSelector: '#actSurveyEditPanel',

    mixins: {
        controllerLoader: 'B4.mixins.LayoutControllerLoader',
        mask: 'B4.mixins.MaskBody'
    },

    aspects: [
        {
            xtype: 'gkhblobtextaspect',
            name: 'actSurveyBlobDescriptionAspect',
            fieldSelector: '[name=Description]',
            editPanelAspectName: 'actSurveyEditPanelAspect',
            editPanelSelector: '#actSurveyEditPanel',
            controllerName: 'SmolenskActSurvey',
            valueFieldName: 'Description',
            previewLength: 500,
            autoSavePreview: true,
            previewField: 'Description',

            getParentRecordId: function () {
                return this.editPanelAspect.getRecord().getId();
            }
        },
        {
            /*
            Вешаем аспект смены статуса в карточке редактирования
            */
            xtype: 'statebuttonaspect',
            name: 'actSurveyStateButtonAspect',
            stateButtonSelector: '#actSurveyEditPanel #btnState',
            listeners: {
                transfersuccess: function (asp, entityId) {
                    //После успешной смены статуса запрашиваем по Id актуальные данные записи
                    //и обновляем панель
                    var editPanelAspect = asp.controller.getAspect('actSurveyEditPanelAspect');

                    editPanelAspect.setData(entityId);
                    editPanelAspect.reloadTreePanel();
                }
            }
        },
        {
           /*
            * permissions
            * B4/libs/aspects/permission
            */
            xtype: 'actsurveyperm',
            editFormAspectName: 'actSurveyEditPanelAspect'
        },
        {
            xtype: 'gkhbuttonprintaspect',
            name: 'actSurveyPrintAspect',
            buttonSelector: '#actSurveyEditPanel #btnPrint',
            codeForm: 'ActSurvey',
            getUserParams: function (reportId) {
                var param = { DocumentId: this.controller.params.documentId };
                this.params.userParams = Ext.JSON.encode(param);
            }
        },
        {
            /*
            Аспект основной панели Акта обследования Вообщем этот документ используется в регионах во всех кроме РТ так что осторожнее с этим
            */
            xtype: 'gjidocumentaspect',
            name: 'actSurveyEditPanelAspect',
            editPanelSelector: '#actSurveyEditPanel',
            modelName: 'ActSurvey',

            otherActions: function (actions) {
                actions[this.editPanelSelector + ' #cbFactSurveyed'] = { 'change': { fn: this.onFactSurveyedChange, scope: this } };
            },

            //перекрываем метод После загрузки данных на панель
            onAfterSetPanelData: function (asp, rec, panel) {
                var me = this;
                
                asp.controller.params = asp.controller.params || {};

                // Поскольку в параметрах могли передать callback который срабатывает после открытия карточки
                // Будем считать что данный метод и есть тот самый метод котоырй будет вызывать callback который ему передали
                var callbackUnMask = asp.controller.params.callbackUnMask;
                if (callbackUnMask && Ext.isFunction(callbackUnMask)) {
                    callbackUnMask.call();
                }
                
                //После проставления данных обновляем title вкладки

                if (rec.get('DocumentNumber'))
                    panel.setTitle('Акт обследования ' + rec.get('DocumentNumber'));
                else
                    panel.setTitle('Акт обследования');

                panel.down('#actSurveyTabPanel').setActiveTab(0);
                
                //Делаем запросы на получение инспекторов
                //и обновляем соответсвующие Тригер филды
                
                asp.controller.mask('Загрузка', me.controller.getMainComponent());
                B4.Ajax.request(B4.Url.action('GetInfo', 'ActSurvey', {
                    documentId: asp.controller.params.documentId
                })).next(function (response) {
                    asp.controller.unmask();
                    //десериализуем полученную строку
                    var obj = Ext.JSON.decode(response.responseText);

                    var fieldInspectors = panel.down('#trigfInspectors');
                    fieldInspectors.updateDisplayedText(obj.inspectorNames);
                    fieldInspectors.setValue(obj.inspectorIds);

                    var fieldAddress = panel.down('#actSurveyAddressTextField');
                    fieldAddress.setValue(obj.objectAddress);

                    me.disableButtons(false);
                }).error(function () {
                    asp.controller.unmask();
                });

                me.controller.getAspect('actSurveyBlobDescriptionAspect').doInjection();
                
                //Передаем аспекту смены статуса необходимые параметры
                this.controller.getAspect('actSurveyStateButtonAspect').setStateData(rec.get('Id'), rec.get('State'));
                //обновляем стор отчетов
                this.controller.getAspect('actSurveyPrintAspect').loadReportStore();

            },
            onFactSurveyedChange: function (field, newValue, oldValue) {
                var panel = this.getPanel();

                if (newValue == 10) {
                    panel.down('#tfReason').setDisabled(false);
                } else {
                    panel.down('#tfReason').setDisabled(true);
                }
            },

            disableButtons: function (value) {
                //получаем все батон-группы
                var groups = Ext.ComponentQuery.query(this.editPanelSelector + ' buttongroup');
                var idx = 0;
                //теперь пробегаем по массиву groups и активируем их
                while (true) {

                    if (!groups[idx])
                        break;

                    groups[idx].setDisabled(value);
                    idx++;
                }
            }
        },
        {
            /*
            аспект взаимодействия триггер-поля инспекторы с массовой формой выбора инспекторов
            по нажатию на кнопку отбора показывается форма массового выбора после чего идет отбор
            По нажатию на кнопку Применить в методе getdata мы обрабатываем полученные значения
            и сохраняем инспекторов через серверный метод /ActSurveyGJI/AddInspectors
            */
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'actSurveyInspectorMultiSelectWindowAspect',
            fieldSelector: '#actSurveyEditPanel #trigfInspectors',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#actSurveyInspectorSelectWindow',
            storeSelect: 'dict.InspectorForSelect',
            storeSelected: 'dict.InspectorForSelected',
            textProperty: 'Fio',
            columnsGridSelect: [
                { header: 'ФИО', xtype: 'gridcolumn', dataIndex: 'Fio', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Код', xtype: 'gridcolumn', dataIndex: 'Code', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'ФИО', xtype: 'gridcolumn', dataIndex: 'Fio', flex: 1, sortable: false }
            ],
            titleSelectWindow: 'Выбор инспекторов',
            titleGridSelect: 'Инспекторы для отбора',
            titleGridSelected: 'Выбранные инспекторы',
            listeners: {
                getdata: function (asp, records) {
                    var recordIds = [];

                    records.each(function (rec, index) { recordIds.push(rec.get('Id')); });

                    asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                    B4.Ajax.request(B4.Url.action('AddInspectors', 'DocumentGjiInspector', {
                        inspectorIds: recordIds,
                        documentId: asp.controller.params.documentId
                    })).next(function (response) {
                        asp.controller.unmask();
                        Ext.Msg.alert('Сохранение!', 'Инспекторы сохранены успешно');
                        return true;
                    }).error(function () {
                        asp.controller.unmask();
                    });

                    return true;
                }
            }
        },
        {
            /*
            Аспект взаимодействия таблицы Собственников, как инлайн грид
            */
            xtype: 'gkhinlinegridaspect',
            name: 'actSurveyOwnerAspect',
            storeName: 'actsurvey.Owner',
            modelName: 'actsurvey.Owner',
            gridSelector: '#actSurveyOwnerGrid',
            saveButtonSelector: '#actSurveyEditPanel #actSurveyOwnerSaveButton',
            listeners: {
                beforesave: function (asp, store) {
                    store.each(function (record, index) {
                        if (!record.get('Id')) {
                            record.set('ActSurvey', asp.controller.params.documentId);
                        }
                    });

                    return true;
                }
            }
        },
        {
            /*
            Аспект взаимодействия Таблицы приложений с формой редактирования
            */
            xtype: 'grideditwindowaspect',
            name: 'actSurveyAnnexAspect',
            gridSelector: '#actSurveyAnnexGrid',
            editFormSelector: '#actSurveyAnnexEditWindow',
            storeName: 'actsurvey.Annex',
            modelName: 'actsurvey.Annex',
            editWindowView: 'actsurvey.AnnexEditWindow',
            listeners: {
                getdata: function (asp, record) {
                    if (!record.get('Id')) {
                        record.set('ActSurvey', this.controller.params.documentId);
                    }
                }
            }
        },
        {
            /*
            Аспект взаимодействия Таблицы изображений с формой редактирования
            */
            xtype: 'grideditwindowaspect',
            name: 'actSurveyPhotoAspect',
            gridSelector: '#actSurveyPhotoGrid',
            editFormSelector: '#actSurveyPhotoEditWindow',
            storeName: 'actsurvey.Photo',
            modelName: 'actsurvey.Photo',
            editWindowView: 'actsurvey.PhotoEditWindow',
            listeners: {
                getdata: function (asp, record) {
                    if (!record.get('Id')) {
                        record.set('ActSurvey', this.controller.params.documentId);
                    }
                }
            }
        },
        {
            /* 
            Аспект взаимодействия таблицы инспектируемых частей с массовой формой выбора инсп. частей
            По нажатию на Добавить открывается форма выбора инсп. частей.
            По нажатию Применить на форме массовго выбора идет обработка выбранных строк в getdata
            И сохранение инсп. частей
            */
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'actSurveyInspectedPartAspect',
            gridSelector: '#actSurveyInspectedPartGrid',
            storeName: 'actsurvey.InspectedPart',
            modelName: 'actsurvey.InspectedPart',
            editFormSelector: '#actSurveyInspectedPartEditWindow',
            editWindowView: 'actsurvey.InspectedPartEditWindow',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#actSurveyInspectedPartMultiSelectWindow',
            storeSelect: 'dict.InspectedPartGjiForSelect',
            storeSelected: 'dict.InspectedPartGjiForSelected',
            titleSelectWindow: 'Выбор инспектируемых частей',
            titleGridSelect: 'Элементы для отбора',
            titleGridSelected: 'Выбранные элементы',
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }
            ],

            listeners: {
                getdata: function (asp, records) {

                    var recordIds = [];

                    records.each(function (rec, index) {
                        recordIds.push(rec.get('Id'));
                    });

                    if (recordIds[0] > 0) {
                        asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                        B4.Ajax.request(B4.Url.action('AddInspectedParts', 'ActSurveyInspectedPart', {
                            partIds: recordIds,
                            documentId: asp.controller.params.documentId
                        })).next(function (response) {
                            asp.controller.unmask();
                            asp.controller.getStore(asp.storeName).load();
                            return true;
                        }).error(function () {
                            asp.controller.unmask();
                        });
                    }
                    else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать инспектируемые части');
                        return false;
                    }
                    return true;
                }
            }
        }
    ],

    init: function () {
        this.callParent(arguments);

        this.getStore('actsurvey.Annex').on('beforeload', this.onObjectBeforeLoad, this);
        this.getStore('actsurvey.Owner').on('beforeload', this.onObjectBeforeLoad, this);
        this.getStore('actsurvey.InspectedPart').on('beforeload', this.onObjectBeforeLoad, this);
        this.getStore('actsurvey.Photo').on('beforeload', this.onObjectBeforeLoad, this);

    },

    onLaunch: function () {
        if (this.params) {
            this.getAspect('actSurveyEditPanelAspect').setData(this.params.documentId);

            //Обновляем стор собственников
            this.getStore('actsurvey.Owner').load();

            //Обновляем стор инспектируемых частей
            this.getStore('actsurvey.InspectedPart').load();

            //Обновляем стор приложений
            this.getStore('actsurvey.Annex').load();

            //Обновляем стор фотографий
            this.getStore('actsurvey.Photo').load();
        }
    },

    onObjectBeforeLoad: function (store, operation) {
        if (this.params && this.params.documentId > 0)
            operation.params.documentId = this.params.documentId;
    }

});
Ext.define('B4.controller.protocol197.EditForm', {
    extend: 'B4.base.Controller',
    params: null,
    objectId: 0,
    requires: [
        'B4.aspects.StateButton',
        'B4.aspects.GjiDocument',
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.GridEditWindow',
        'B4.aspects.GkhButtonPrintAspect',
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.aspects.GkhInlineGridMultiSelectWindow',
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.aspects.GkhBlobText',
        'B4.aspects.GkhGridMultiSelectWindowTree',
        'B4.Ajax',
        'B4.Url'
    ],

    models: [
        'protocol197.Protocol197',
        'protocol197.Violation',
        'protocol197.ArticleLaw',
        'protocol197.Annex'
    ],

    stores: [
        'protocol197.Protocol197',
        'protocol197.Violation',
        'protocol197.ArticleLaw',
        'protocol197.Annex',
        'dict.InspectorForSelect',
        'dict.InspectorForSelected',
        'dict.ArticleLawGjiForSelect',
        'dict.ArticleLawGjiForSelected',
        'dict.ExecutantDocGji',
        'Contragent',
        'dict.SurveySubjectRequirementSelect',
        'dict.SurveySubjectRequirementSelected',
        'dict.NormativeDoc'
    ],

    views: [
        'protocol197.EditPanel',
        'protocol197.ArticleLawGrid',
        'protocol197.AnnexEditWindow',
        'protocol197.AnnexGrid',
        'protocol197.ViolationEditWindow',
        'SelectWindow.MultiSelectWindow',
        'SelectWindow.MultiSelectWindowTree'
    ],

    mainView: 'protocol197.EditPanel',
    mainViewSelector: '#protocol197EditPanel',

    mixins: {
        controllerLoader: 'B4.mixins.LayoutControllerLoader',
        mask: 'B4.mixins.MaskBody'
    },

    aspects: [
        {
            /*
            Вешаем аспект смены статуса в карточке редактирования
            */
            xtype: 'statebuttonaspect',
            name: 'protocol197StateButtonAspect',
            stateButtonSelector: '#protocol197EditPanel #btnState',
            listeners: {
                transfersuccess: function (asp, entityId) {
                    var editPanelAspect = asp.controller.getAspect('protocol197EditPanelAspect');
                    editPanelAspect.setData(entityId);
                }
            }
        },
        {
            xtype: 'gkhbuttonprintaspect',
            name: 'protocol197PrintAspect',
            buttonSelector: '#protocol197EditPanel #btnPrint',
            codeForm: 'Protocol197',
            getUserParams: function (reportId) {

                var param = { DocumentId: this.controller.params.documentId };

                this.params.userParams = Ext.JSON.encode(param);
            }
        },
        {   /*
            Апект для основной панели Протокола
            */
            xtype: 'gjidocumentaspect',
            name: 'protocol197EditPanelAspect',
            editPanelSelector: '#protocol197EditPanel',
            modelName: 'protocol197.Protocol197',

            otherActions: function (actions) {
                actions[this.editPanelSelector + ' #cbExecutant'] = { 'change': { fn: this.onChangeTypeExecutant, scope: this } };
                actions[this.editPanelSelector + ' #ecTypePresence'] = { 'change': { fn: this.onChangeTypePresence, scope: this } };
                actions[this.editPanelSelector + ' #sfContragent'] = { 'beforeload': { fn: this.onBeforeLoadContragent, scope: this } };
                actions[this.editPanelSelector + ' #cbToCourt'] = { 'change': { fn: this.onChangeToCourt, scope: this } };
                actions[this.editPanelSelector + ' #cbNotifDeliveredThroughOffice'] = { 'change': { fn: this.onChangeNotifDeliveredThroughOffice, scope: this } };
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
                
                panel.down('#protocol197TabPanel').setActiveTab(0);
                
                //включаем/выключаем поле "Дата передачи документов"
                var dfToCourt = panel.down('#dfDateToCourt');

                dfToCourt.setDisabled(true);
                if (rec.get('ToCourt')) {
                    dfToCourt.setDisabled(false);
                }
                
                //После проставления данных обновляем title вкладки
                if (rec.get('DocumentNumber'))
                    panel.setTitle('Протокол по ст.19.7 КоАП РФ ' + rec.get('DocumentNumber'));
                else
                    panel.setTitle('Протокол по ст.19.7 КоАП РФ');
                
                //Делаем запросы на получение Инспекторов и документа основания
                //и обновляем соответсвующие Тригер филды
                
                me.controller.mask('Загрузка', me.controller.getMainComponent());
                B4.Ajax.request(B4.Url.action('GetInfo', 'Protocol197', {
                    documentId: asp.controller.params.documentId
                })).next(function (response) {
                    //десериализуем полученную строку
                    var obj = Ext.JSON.decode(response.responseText),
                        fieldInspectors = panel.down('#trigfInspector'),
                        fieldReqs = panel.down('#trigfSurveySubjectRequirements'),
                        sfResolveViolationClaim = panel.down('#sfResolveViolationClaim'),
                        fieldDirections = panel.down('#protocol197DirectionsTrigerField'),
                        sfNormativeDoc = panel.down('#sfNormativeDoc');

                    fieldInspectors.setValue(obj.inspectorIds);
                    fieldInspectors.updateDisplayedText(obj.inspectorNames);

                    if (obj.inspectorNames) {
                        fieldInspectors.clearInvalid();
                    } else {
                        fieldInspectors.markInvalid();
                    }

                    fieldReqs.setValue(obj.requirementIds);
                    fieldReqs.updateDisplayedText(obj.requirementNames);

                    fieldDirections.setValue(obj.directionIds);
                    fieldDirections.updateDisplayedText(obj.directionNames);

                    sfResolveViolationClaim.setVisible(!!obj.documentGjiCheck);

                    sfNormativeDoc.setVisible(!obj.hasRealityObjects);

                    me.disableButtons(false);
                    me.controller.unmask();
                }).error(function () {
                    me.controller.unmask();
                });

                //Передаем аспекту смены статуса необходимые параметры
                this.controller.getAspect('protocol197StateButtonAspect').setStateData(rec.get('Id'), rec.get('State'));
                //обновляем отчеты
                this.controller.getAspect('protocol197PrintAspect').loadReportStore();
                
                this.controller.getAspect('protocol197ViolationWitnessesAspect').doInjection();
                this.controller.getAspect('protocol197ViolationVictimsAspect').doInjection();

                this.setTypeExecutantPermission(rec.get('Executant'));
                this.onChangeTypePresence(null, rec.get('TypePresence'));
            },
            onChangeToCourt: function (field, data) {
                if (data == true) {
                    this.getPanel().down('#dfDateToCourt').setDisabled(false);
                } else {
                    this.getPanel().down('#dfDateToCourt').setDisabled(true);
                }
            },

            setTypeExecutantPermission: function (typeExec) {
                var me = this,
                    panel = this.getPanel(),
                    sfContragent = panel.down('#sfContragent'),
                    tfPhysPerson = panel.down('#tfPhysPerson'),
                    tfPosition = panel.down('#tfPosition'),
                    tfDatePlaceOfBirth = panel.down('#tfDatePlaceOfBirth'),
                    tfRegistrationAddress = panel.down('#tfRegistrationAddress'),
                    tfFactAddress = panel.down('#tfFactAddress');

                sfContragent.setDisabled(true);
                sfContragent.allowBlank = true;
                tfPhysPerson.setDisabled(true);
                tfPosition.setDisabled(true);
                tfDatePlaceOfBirth.setDisabled(true);
                tfRegistrationAddress.setDisabled(true);
                tfFactAddress.setDisabled(true);

                if (typeExec) {
                    var type = me.controller.getExecutantTypeByCode(typeExec.Code);

                    // включено только для юр лиц и ип
                    sfContragent.setDisabled(!(type === 2 || type === 4));
                    sfContragent.allowBlank = sfContragent.disabled;

                    // только для должностных лиц, граждан и ип
                    tfPhysPerson.setDisabled(!(type === 1 || type === 3 || type === 4));

                    // только для должностных лиц
                    tfPosition.setDisabled(type !== 1);

                    // только для должностных лиц, граждан и ип
                    tfDatePlaceOfBirth.setDisabled(!(type === 1 || type === 3 || type === 4));

                    // только для должностных лиц, граждан и ип
                    tfRegistrationAddress.setDisabled(!(type === 1 || type === 3 || type === 4));

                    // только для должностных лиц, граждан и ип
                    tfFactAddress.setDisabled(!(type === 1 || type === 3 || type === 4));
                }
            },

            onChangeNotifDeliveredThroughOffice: function (_, value) {
                var panel = this.getPanel(),
                    dfNotifDeliveryDate = panel.down('#dfNotifDeliveryDate'),
                    nfNotifNum = panel.down('#nfNotifNum');

                dfNotifDeliveryDate.setDisabled(!value);
                nfNotifNum.setDisabled(!value);
            },

            onChangeTypePresence: function(_, value) {
                var panel = this.getPanel(),
                    tfRepresentative = panel.down('#tfRepresentative'),
                    taReasonTypeRequisites = panel.down('#taReasonTypeRequisites');

                tfRepresentative.setDisabled(!value);
                taReasonTypeRequisites.setDisabled(value !== 20);
            },
            onChangeTypeExecutant: function (field, value) {

                var data = field.getRecord(value);

                if (data) {
                    if (this.controller.params) {
                        this.controller.params.typeExecutant = data.Code;
                    }
                    this.setTypeExecutantPermission(data);
                }
            },
            onBeforeLoadContragent: function (field, options, store) {
                var executantField = this.controller.getMainView().down('#cbExecutant');

                var typeExecutant = executantField.getRecord(executantField.getValue());
                if (!typeExecutant)
                    return true;

                options = options || {};
                options.params = options.params || {};

                options.params.typeExecutant = typeExecutant.Code;

                return true;
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
            },
            //после нажатия на Удалить идет удаление документа
            btnDeleteClick: function () {
                var panel = this.getPanel();
                var record = panel.getForm().getRecord();

                Ext.Msg.confirm('Удаление записи!', 'Вы действительно хотите удалить документ?', function (result) {
                    if (result == 'yes') {
                        this.mask('Удаление', B4.getBody());
                        record.destroy()
                            .next(function () {
                                Ext.Msg.alert('Удаление!', 'Документ успешно удален');

                                panel.close();
                                this.unmask();
                            }, this)
                            .error(function (result) {
                                Ext.Msg.alert('Ошибка удаления!', Ext.isString(result.responseData) ? result.responseData : result.responseData.message);
                                this.unmask();
                            }, this);

                    }
                }, this);
            }
        },
        {
            /*
            аспект взаимодействия триггер-поля инспекторы с массовой формой выбора инспекторов
            по нажатию на кнопку отбора показывается форма массового выбора после чего идет отбор
            По нажатию на кнопку Применить в методе getdata мы обрабатываем полученные значения
            и сохраняем инспекторов через серверный метод /DocumentGjiInspector/AddInspectors
            */
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'protocol197InspectorMultiSelectWindowAspect',
            fieldSelector: '#protocol197EditPanel #trigfInspector',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#protocol197InspectorSelectWindow',
            storeSelect: 'dict.InspectorForSelect',
            storeSelected: 'dict.InspectorForSelected',
            textProperty: 'Fio',
            columnsGridSelect: [
                { header: 'ФИО', xtype: 'gridcolumn', dataIndex: 'Fio', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Код', xtype: 'gridcolumn', dataIndex: 'Code', flex: 1, filter: { xtype: 'textfield'} }
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

                    records.each(function (rec) { recordIds.push(rec.get('Id')); });

                    asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                    B4.Ajax.request(B4.Url.action('AddInspectors', 'DocumentGjiInspector', {
                        inspectorIds: recordIds,
                        documentId: asp.controller.params.documentId
                    })).next(function () {
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
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'protocol197SurSubjReqMultiSelectWindowAspect',
            fieldSelector: '#protocol197EditPanel #trigfSurveySubjectRequirements',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#surveySubjectRequirementsSelectWindow',
            storeSelect: 'dict.SurveySubjectRequirementSelect',
            storeSelected: 'dict.SurveySubjectRequirementSelected',
            textProperty: 'Name',
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Код', xtype: 'gridcolumn', dataIndex: 'Code', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }
            ],
            titleSelectWindow: 'Выбор требований',
            titleGridSelect: 'Требования для отбора',
            titleGridSelected: 'Выбранные требования',
            listeners: {
                getdata: function (asp, records) {
                    var recordIds = [];

                    records.each(function (rec) { recordIds.push(rec.get('Id')); });

                    asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                    B4.Ajax.request(B4.Url.action('AddRequirements', 'Protocol197', {
                        requirementIds: recordIds,
                        documentId: asp.controller.params.documentId
                    })).next(function () {
                        asp.controller.unmask();
                        Ext.Msg.alert('Сохранение!', 'Требования сохранены успешно');
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
            Аспект взаимодействия Таблицы приложений с формой редактирования
            */
            xtype: 'grideditwindowaspect',
            name: 'protocol197AnnexAspect',
            gridSelector: '#protocol197AnnexGrid',
            editFormSelector: '#protocol197AnnexEditWindow',
            storeName: 'protocol197.Annex',
            modelName: 'protocol197.Annex',
            editWindowView: 'protocol197.AnnexEditWindow',
            listeners: {
                getdata: function (asp, record) {
                    if (!record.get('Id')) {
                        record.set('Protocol197', this.controller.params.documentId);
                    }
                }
            }
        },
        {
            /* 
            * Аспект взаимодействия таблицы статьи закона с массовой формой выбора статей
            * По нажатию на Добавить открывается форма выбора статей.
            * По нажатию Применить на форме массовго выбора идет обработка выбранных строк в getdata
            * И сохранение статей
            */
            xtype: 'gkhinlinegridmultiselectwindowaspect',
            name: 'protocol197ArticleLawAspect',
            gridSelector: '#protocol197ArticleLawGrid',
            saveButtonSelector: '#protocol197ArticleLawGrid #protocolSaveButton',
            storeName: 'protocol197.ArticleLaw',
            modelName: 'protocol197.ArticleLaw',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#protocol197ArticleLawMultiSelectWindow',
            storeSelect: 'dict.ArticleLawGjiForSelect',
            storeSelected: 'dict.ArticleLawGjiForSelected',
            titleSelectWindow: 'Выбор статей закона',
            titleGridSelect: 'Статьи для отбора',
            titleGridSelected: 'Выбранные статьи',
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield'} },
                { header: 'Код', xtype: 'gridcolumn', dataIndex: 'Code', flex: 1, filter: { xtype: 'textfield'} }
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
                        B4.Ajax.request({
                            url: B4.Url.action('AddArticles', 'Protocol197ArticleLaw'),
                            method: 'POST',
                            params: {
                                articleIds: Ext.encode(recordIds),
                                documentId: asp.controller.params.documentId
                            }
                        }).next(function (response) {
                            asp.controller.unmask();
                            asp.controller.getStore(asp.storeName).load();
                            return true;
                        }).error(function () {
                            asp.controller.unmask();
                        });
                    }
                    else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать статьи закона');
                        return false;
                    }
                    return true;
                }
            }
        },
        {
            xtype: 'gkhblobtextaspect',
            name: 'protocol197ViolationWitnessesAspect',
            fieldSelector: '[name=Witnesses]',
            editPanelAspectName: 'protocol197EditPanelAspect',
            controllerName: 'Protocol197',
            valueFieldName: 'Witnesses',
            previewLength: 2000,
            autoSavePreview: true,
            previewField: false
        },
        {
            xtype: 'gkhblobtextaspect',
            name: 'protocol197ViolationVictimsAspect',
            fieldSelector: '[name=Victims]',
            editPanelAspectName: 'protocol197EditPanelAspect',
            controllerName: 'Protocol197',
            valueFieldName: 'Victims',
            previewLength: 2000,
            autoSavePreview: true,
            previewField: false
        },
        {
            /*
            аспект для поля с массовым добавлением Направлений деятельности
            */
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'protocol197DirectionMultiSelectWindowAspect',
            fieldSelector: '#protocol197EditPanel #protocol197DirectionsTrigerField',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#protocol197DirectionSelectWindow',
            storeSelect: 'dict.ActivityDirection',
            storeSelected: 'dict.ActivityDirection',
            textProperty: 'Name',
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Код', xtype: 'gridcolumn', dataIndex: 'Code', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }
            ],
            titleSelectWindow: 'Выбор направлений деятельности',
            titleGridSelect: 'Направления для отбора',
            titleGridSelected: 'Выбранные направления',
            listeners: {
                getdata: function (asp, records) {
                    var recordIds = [];

                    records.each(function (rec) {
                        recordIds.push(rec.getId());
                    });

                    asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                    B4.Ajax.request(B4.Url.action('AddDirections', 'Protocol197', {
                        directionIds: recordIds,
                        documentId: asp.controller.params.documentId
                    })).next(function () {
                        asp.controller.unmask();
                        Ext.Msg.alert('Сохранение!', 'Направления деятельности сохранены успешно');
                        return true;
                    }).error(function () {
                        asp.controller.unmask();
                    });
                }
            }
        },
        {
            xtype: 'gkhmultiselectwindowtreeaspect',
            name: 'protocol197ViolationAspect',
            gridSelector: '#protocol197ViolationGrid',
            saveButtonSelector: '#protocol197ViolationGrid #protocol197ViolationSaveButton',
            storeName: 'protocol197.Violation',
            modelName: 'protocol197.Violation',
            multiSelectWindow: 'SelectWindow.MultiSelectWindowTree',
            multiSelectWindowSelector: '#multiSelectWindowTree',
            storeSelect: 'dict.ViolationGjiForTreeSelect',
            storeSelected: 'dict.ViolationGjiForSelected',
            titleSelectWindow: 'Выбор нарушений',
            titleGridSelect: 'Нарушения для отбора',
            titleGridSelected: 'Выбранные нарушения',
            isTbar: true,
            tbarCmp: [
                {
                    xtype: 'textfield',
                    ident: 'searchfield',
                    width: 350,
                    emptyText: 'Поиск',
                    enableKeyEvents: true
                },
                {
                    xtype: 'button',
                    text: 'Искать',
                    iconCls: 'icon-page-white-magnify',
                    ident: 'searchbtn'
                }
            ],
            otherActions: function (actions) {
                var me = this;

                actions[me.multiSelectWindowSelector + ' [ident=searchbtn]'] = { 'click': { fn: me.goFilter, scope: me } };
                actions[me.multiSelectWindowSelector + ' [ident=searchfield]'] = {
                    'keypress': {
                        fn: function (scope, e) {
                            if (e.getKey() == 13) {
                                me.goFilter(scope);
                            }
                        }, scope: me
                    }
                };
            },
            goFilter: function (btn) {
                var filterData = btn.up('#multiSelectWindowTree').down('[ident=searchfield]').getValue(),
                    treepanel = btn.up('#multiSelectWindowTree').down('treepanel');
                treepanel.getStore().reload({
                    params: { filter: filterData }
                });
            },
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'treecolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Пункты НПД', xtype: 'gridcolumn', dataIndex: 'Code', width: 140, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'ViolationGjiName', flex: 1, sortable: false },
                { header: 'Пункты НПД', xtype: 'gridcolumn', dataIndex: 'ViolationGjiPin', width: 80, sortable: false }
            ],
            listeners: {
                getdata: function (asp, records) {
                    var currentViolationStore = asp.controller.getStore(asp.storeName),
                        range = currentViolationStore.getRange(0, currentViolationStore.getCount());

                    asp.controller.mask('Выбор нарушений', asp.controller.getMainComponent());

                    currentViolationStore.removeAll();

                    Ext.Array.each(records.items, function (rec) {
                        currentViolationStore.add({
                            Id: 0,
                            Protocol197: asp.controller.params.documentId,
                            CodesPin: rec.get('ViolationGjiPin'),
                            ViolationGjiName: rec.get('ViolationGjiName'),
                            ViolationGjiId: rec.get('ViolationGjiId'),
                            Features: rec.get('FeatViol'),
                            DatePlanRemoval: null
                        });
                    });

                    Ext.Array.each(range, function (rec) {
                        currentViolationStore.add(rec);
                    });

                    asp.controller.unmask();

                    return true;
                }
            },
            onCheckRec: function (node, checked) {
                var me = this,
                    grid = me.getSelectedGrid(),
                    storeSelected = grid.getStore(),
                    model = me.controller.getModel(me.modelName);

                if (grid && node.get('leaf')) {
                    if (checked) {
                        if (storeSelected.find('Id', node.get('Id'), 0, false, false, true) == -1) {
                            storeSelected.add(new model({
                                Id: node.get('Id'),
                                ViolationGjiPin: node.get('Code'),
                                ViolationGjiName: node.get('Name'),
                                ViolationGjiId: node.get('ViolationGjiId')
                            }));
                        }
                    } else {
                        storeSelected.remove(storeSelected.getById(node.get('Id')));
                    }
                }
            },
            getSelectGrid: function () {
                var me = this,
                    win;
                if (me.componentQuery) {
                    win = me.componentQuery(me.multiSelectWindowSelector);
                }

                if (!win) {
                    win = Ext.ComponentQuery.query(me.multiSelectWindowSelector)[0];
                }

                if (win) {
                    return win.down('#tpSelect');
                }
            },
            selectedGridRowActionHandler: function (action, record) {
                var me = this,
                    gridSelect = me.getSelectGrid(),
                    gridSelected = me.getSelectedGrid();

                if (gridSelect && gridSelected) {
                    gridSelected.fireEvent('rowaction', gridSelected, action, record);

                    var node = gridSelect.getRootNode().findChild('Id', record.getId(), true);
                    if (node) {
                        node.set('checked', false);
                    }
                }
            },
            getForm: function () {
                var me = this,
                    win = Ext.ComponentQuery.query(me.multiSelectWindowSelector)[0],
                    stSelected,
                    stSelect;

                if (win && !win.getBox().width) {
                    win = win.destroy();
                }

                if (!win) {
                    stSelected = me.storeSelected instanceof Ext.data.AbstractStore ? me.storeSelected : Ext.create('B4.store.' + me.storeSelected);
                    stSelected.on('beforeload', me.onSelectedBeforeLoad, me);

                    stSelect = me.storeSelect instanceof Ext.data.AbstractStore ? me.storeSelect : Ext.create('B4.store.' + me.storeSelect);
                    stSelect.on('beforeload', me.onBeforeLoad, me);
                    stSelect.on('load', me.onLoad, me);

                    win = me.controller.getView(me.multiSelectWindow).create({
                        itemId: me.multiSelectWindowSelector.replace('#', ''),
                        storeSelect: stSelect,
                        storeSelected: stSelected,
                        columnsGridSelect: me.columnsGridSelect,
                        columnsGridSelected: me.columnsGridSelected,
                        title: me.titleSelectWindow,
                        titleGridSelect: me.titleGridSelect,
                        titleGridSelected: me.titleGridSelected,
                        selModelMode: me.selModelMode,
                        isTbar: me.isTbar,
                        tbarCmp: me.tbarCmp,
                        constrain: true,
                        modal: false,
                        closeAction: 'destroy',
                        renderTo: B4.getBody().getActiveTab().getEl()
                    });

                    win.on('afterrender', me.formAfterrender, me);

                    if (Ext.isNumber(me.multiSelectWindowWidth) && win.setWidth) {
                        win.setWidth(me.multiSelectWindowWidth);
                    }

                    stSelected.sorters.clear();
                    stSelect.sorters.clear();
                }

                return win;
            }
        },
        {
            /*
            * Аспект взаимодействия Таблицы 'Дата и время проведения проверки' с формой редактирования
            */
            xtype: 'grideditwindowaspect',
            name: 'protocol197EditViolationAspect',
            gridSelector: 'protocol197ViolationGrid',
            editFormSelector: 'protocol197violationwin',
            storeName: 'protocol197.Violation',
            modelName: 'protocol197.Violation',
            editWindowView: 'protocol197.ViolationEditWindow',
            otherActions: function (actions) {
                actions['#protocol197ViolationGrid #updateButton'] = { 'click': { fn: this.onUpdateGrid, scope: this } };
                actions['#protocol197ViolationGrid #protocol197ViolationSaveButton'] = { 'click': { fn: this.onSaveViolations, scope: this } };
            },
            onUpdateGrid: function () {
                this.controller.getStore('protocol197.Violation').load();
            },
            onSaveViolations: function () {
                var me = this,
                    grid = me.getGrid(),
                    storeViolation = grid.getStore(),
                    deferred = new Deferred();

                deferred.next(function () {
                    storeViolation.load();
                        Ext.Msg.alert('Сохранение!', 'Результаты проверки сохранены успешно');
                    })
                    .error(function (e) {
                        if (e.message) {
                            Ext.Msg.alert('Ошибка сохранения!', e.message);
                        } else {
                            throw e;
                        }
                    });

                var panel = me.controller.getMainComponent(),
                    docDate = panel.down('datefield[name=DocumentDate]').value,
                    docDatePlus6 = new Date(new Date(docDate).setMonth(docDate.getMonth() + 6));

                var violations = [];
                var isCorrectDate = true;
                Ext.Array.each(storeViolation.getRange(0, storeViolation.getCount()),
                    function(item) {
                        var data = item.getData();

                        if (data.DatePlanRemoval) {
                            var datePlanRemoval;
                            if (data.DatePlanRemoval instanceof Date) {
                                datePlanRemoval = data.DatePlanRemoval;
                            } else {
                                var dateParts = data.DatePlanRemoval.split("-");
                                datePlanRemoval = new Date(dateParts[0], dateParts[1] - 1, dateParts[2].substring(0, dateParts[2].lenght - 9));
                            }

                            if (datePlanRemoval.getTime() > docDatePlus6.getTime()) {
                                isCorrectDate = false;
                            }
                        }

                        violations.push(
                            {
                                Id: data.Id || 0,
                                ViolationGjiId: data.ViolationGjiId,
                                DatePlanRemoval: data.DatePlanRemoval
                            });
                    });
                
                if (!isCorrectDate) {
                    deferred.fail({ message: 'Срок устранения нарушения не должен превышать 6 месяцев.' });
                    return false;
                }

                me.controller.mask('Сохранение', panel);
                B4.Ajax.request({
                    method: 'POST',
                    url: B4.Url.action('Save', 'Protocol197Violation'),
                    params: {
                        protocol197Id: me.controller.params.documentId,
                        violations: Ext.encode(violations)
                    } 
                }).next(function () {
                    me.controller.unmask();
                    deferred.call({ message: 'Сохранение нарушений прошло успешно' });
                }).error(function (e) {
                    me.controller.unmask();
                    deferred.fail(e);
                });               
                return true;

            },
            editRecord: function (record) {
                var me = this,
                    id = record ? record.getId() : null,
                    model;

                if (!id) return;

                model = me.getModel(record);
                model.load(id, {
                    success: function (rec) {
                        me.setFormData(rec);
                    },
                    scope: me
                });

                me.getForm().getForm().isValid();

                me.controller.currentViolId = id;
            }
        }
    ],

    init: function () {
        var me = this;

        me.getStore('protocol197.Violation').on('beforeload', me.onViolationBeforeLoad, me);
        me.getStore('protocol197.ArticleLaw').on('beforeload', me.onBeforeLoad, me);
        me.getStore('protocol197.Annex').on('beforeload', me.onBeforeLoad, me);

        me.callParent(arguments);
    },

    onLaunch: function () {
        var me = this;
        if (me.params) {

            me.params.documentId = me.params.get('Id');
            me.getAspect('protocol197EditPanelAspect').setData(me.params.documentId);

            me.getStore('protocol197.Violation').load();

            //Обновляем стор приложений
            me.getStore('protocol197.Annex').load();

            //Обновляем стор статьей закона
            me.getStore('protocol197.ArticleLaw').load();
        }
    },

    onBeforeLoad: function (store, operation) {
        if (this.params && this.params.documentId > 0)
            operation.params.documentId = this.params.documentId;
    },

    getExecutantTypeByCode: function (code) {
        switch (code) {
            // Должностное лицо
            case "1":
            case "3":
            case "5":
            case "10":
            case "12":
            case "13":
            case "16":
            case "19":
                return 1;
            // Юр.лицо
            case "0":
            case "4":
            case "8":
            case "9":
            case "11":
            case "15":
            case "18":
                return 2;
            // Физ.лицо
            case "6":
            case "7":
            case "14":
                return 3;
            // Индивидуальный предприниматель
            case "21":
                return 4;
        }
    },

    onViolationBeforeLoad: function (store, operation) {
        var me = this;

        if (me.params && me.params.documentId > 0) {
            operation.params.documentId = me.params.documentId;
        }
    },
});
Ext.define('B4.controller.actionisolated.actaction.ActActionIsolated', {
    extend: 'B4.base.Controller',
    params: null,
    requires: [
        'B4.aspects.GjiDocument',
        'B4.aspects.GjiDocumentCreateButton',
        'B4.aspects.GkhButtonMultiSelectWindow',
        'B4.aspects.GridEditWindow',
        'B4.aspects.GridDictEditWindow',
        'B4.aspects.GkhButtonPrintAspect',
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.GkhInlineGridMultiSelectWindow',
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.aspects.permission.actionisolated.ActActionIsolated',
        'B4.aspects.ActCheckActionViolation',
        'B4.aspects.StateButton',

        'B4.Ajax',
        'B4.Url',

        'B4.enums.ActActionType',
        'B4.enums.ActCheckActionCarriedOutEventType',
        'B4.enums.KindAction',
        'B4.enums.YesNo',
        'B4.enums.YesNoNotSet',
        'B4.enums.HasValuesNotSet',

        'B4.store.actcheck.Action',
        'B4.store.actcheck.ActionCarriedOutEvent',
        'B4.store.actcheck.ActionFile',
        'B4.store.actcheck.ActionInspector',
        'B4.store.actcheck.ActionRemark',
        'B4.store.actcheck.ActionViolation',
        'B4.store.actcheck.InstrExamActionNormativeDoc',
    ],

    models: [
        'ActActionIsolated',
        'actcheck.Action',
        'actcheck.ActionCarriedOutEvent',
        'actcheck.ActionFile',
        'actcheck.ActionInspector',
        'actcheck.ActionRemark',
        'actcheck.ActionViolation',
        'actcheck.InspectionAction',
        'actcheck.InstrExamAction',
        'actcheck.InstrExamActionNormativeDoc',
        'actcheck.RealityObject',
        'actcheck.Violation',
        'actactionisolated.RealityObject',
        'actcheck.Annex',
        'actactionisolated.Definition'
    ],

    stores: [
        'ActActionIsolated',
        'actcheck.RealityObject',
        'actcheck.Violation',
        'actactionisolated.RealityObjectForSelect',
        'dict.InspectorForSelect',
        'dict.InspectorForSelected',
        'dict.ViolationGjiForSelect',
        'dict.ViolationGjiForSelected',
        'realityobj.RealityObjectForSelected',
        'actactionisolated.Annex',
        'actactionisolated.Definition',
        'actactionisolated.DefenitionRealityObjectForSelect'
    ],

    views: [
        'actionisolated.actactionisolated.RealityObjectEditWindow',
        'actionisolated.actactionisolated.EditPanel',
        'actcheck.ActionFileEditWindow',
        'actionisolated.actactionisolated.ActionGrid',
        'actionisolated.actactionisolated.ActionAddWindow',
        'actionisolated.actactionisolated.ActionFileEditWindow',
        'actionisolated.actactionisolated.inspectionaction.ActionEditWindow',
        'actionisolated.actactionisolated.instrexamaction.ActionEditWindow',
        'actionisolated.actactionisolated.AnnexGrid',
        'actionisolated.actactionisolated.AnnexEditWindow',
        'actionisolated.actactionisolated.DefinitionPanel',
        'actionisolated.actactionisolated.DefinitionEditWindow'
    ],

    mainView: 'actionisolated.actactionisolated.EditPanel',
    mainViewSelector: '#actActionIsolatedEditPanel',
    
    mixins: {
        controllerLoader: 'B4.mixins.LayoutControllerLoader',
        mask: 'B4.mixins.MaskBody',
    },
    
    aspects: [
        {
            xtype: 'actactionisolatedperm',
            editFormAspectName: 'actActionIsolatedEditPanelAspect'
        },
        {
            /*
            Аспект формирвоания документов для Акта проверки
            */
            xtype: 'gjidocumentcreatebuttonaspect',
            name: 'actCreateButtonAspect',
            buttonSelector: '#actActionIsolatedEditPanel gjidocumentcreatebutton',
            containerSelector: '#actActionIsolatedEditPanel',
            typeDocument: 200, // ActActionIsolated
            onValidateUserParams: function (params) {
                // если правила ожидают пользовательские параметры
                if (params.ruleId === 'ActActionToProtocolRule' ||
                    params.ruleId === 'ActActionToMotivatedPresentationRule') {
                    return false;
                }
            }
        },
        {   /* 
               Аспект взаимодействия для кнопки Протокол с массовой формой выбора Нарушений
               По нажатию на кнопку Протокол будет открыта форма массовго выбора
               а после выбранные Id будут отправлены на формирование докумета Протокола
             */
            xtype: 'gkhbuttonmultiselectwindowaspect',
            name: 'actToProtocolAspect',
            buttonSelector: '#actActionIsolatedEditPanel [ruleId=ActActionToProtocolRule]',
            multiSelectWindowSelector: '#actToProtocolMultiSelectWindow',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            storeSelect: 'actcheck.ViolationForSelect',
            storeSelected: 'actcheck.ViolationForSelected',
            columnsGridSelect: [
                { header: 'Код ПиН', xtype: 'gridcolumn', dataIndex: 'ViolationGjiPin', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Текст нарушения', xtype: 'gridcolumn', dataIndex: 'ViolationGjiName', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Срок устранения', xtype: 'datecolumn', dataIndex: 'DatePlanRemoval', format: 'd.m.Y', width: 150, filter: { xtype: 'datefield', operand: CondExpr.operands.eq } },
                { header: 'Муниципальное образование', xtype: 'gridcolumn', dataIndex: 'Municipality', flex: 1 },
                { header: 'Адрес', xtype: 'gridcolumn', dataIndex: 'RealityObject', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Код ПиН', xtype: 'gridcolumn', dataIndex: 'ViolationGjiPin', flex: 1, sortable: false },
                { header: 'Текст нарушения', xtype: 'gridcolumn', dataIndex: 'ViolationGjiName', flex: 1, sortable: false },
                { header: 'Адрес', xtype: 'gridcolumn', dataIndex: 'RealityObject', flex: 1, sortable: false }
            ],
            titleSelectWindow: 'Выбор нарушений',
            titleGridSelect: 'Нарушения для отбора',
            titleGridSelected: 'Выбранные нарушения',
            onBeforeLoad: function (store, operation) {
                if (this.controller.params && this.controller.params.documentId > 0) {
                    operation.params.documentId = this.controller.params.documentId;
                    operation.params.forSelect = true;
                }
            },
            listeners: {
                getdata: function (asp, records) {
                    var me = this,
                        listIds = [],
                        btn = Ext.ComponentQuery.query(me.buttonSelector)[0],
                        creationAspect,
                        params;

                    Ext.Array.each(records.items,
                        function (item) {
                            listIds.push(item.get('InspectionViolationId'));
                        }, this);

                    if (listIds[0] > 0) {
                        creationAspect = asp.controller.getAspect('actCreateButtonAspect');
                        // еще раз получаем параметры и добавляем к уже созданным еще один (Выбранные пользователем нарушения)
                        params = creationAspect.getParams(btn);
                        params.violationIds = listIds;

                        creationAspect.createDocument(params);
                    } else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать нарушения');
                        return false;
                    }
                    return true;
                }
            }
        },
        {
            xtype: 'gkhbuttonmultiselectwindowaspect',
            name: 'actToMotivatedPresentationHouseAspect',
            buttonSelector: '#actActionIsolatedEditPanel [ruleId=ActActionToMotivatedPresentationRule]',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#actToMotivatedPresentationByRoRuleSelectWindow',
            storeSelect: 'actcheck.RealityObject',
            storeSelected: 'actcheck.RealityObject',
            columnsGridSelect: [
                { header: 'Муниципальное образование', xtype: 'gridcolumn', dataIndex: 'Municipality', flex: 1 },
                { header: 'Адрес', xtype: 'gridcolumn', dataIndex: 'RealityObject', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Адрес', xtype: 'gridcolumn', dataIndex: 'RealityObject', flex: 1, sortable: false }
            ],
            titleSelectWindow: 'Выбор дома',
            titleGridSelect: 'Дома для отбора',
            titleGridSelected: 'Выбранный дом',
            onBeforeLoad: function (store, operation) {
                if (this.controller.params && this.controller.params.documentId > 0) {
                    operation.params.documentId = this.controller.params.documentId;
                    operation.params.onlyHasViolations = true;
                }
            },
            listeners: {
                getdata: function (asp, records) {
                    var me = this,
                        listIds = [],
                        btn = Ext.ComponentQuery.query(me.buttonSelector)[0],
                        creationAspect,
                        params;

                    Ext.Array.each(records.items,
                        function (item) {
                            listIds.push(item.get('RealityObjectId'));
                        }, this);

                    if (listIds[0] > 0) {
                        creationAspect = asp.controller.getAspect('actCreateButtonAspect');
                        // еще раз получаем параметры и добавляем к уже созданным еще один (Выбранные пользователем дома)
                        params = creationAspect.getParams(btn);
                        params.realityIds = listIds;

                        creationAspect.createDocument(params);
                    } else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать дома');
                        return false;
                    }
                    return true;
                }
            }
        },
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'actActionIsolatedInspectorMultiSelectWindowAspect',
            fieldSelector: '#actActionIsolatedEditPanel #trigfInspectors',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#actActionIsolatedInspectorSelectWindow',
            storeSelect: 'dict.InspectorForSelect',
            storeSelected: 'dict.InspectorForSelected',
            textProperty: 'Fio',
            columnsGridSelect: [
                { header: 'ФИО', xtype: 'gridcolumn', dataIndex: 'Fio', flex: 1, filter: { xtype: 'textfield'} },
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

                    asp.controller.mask('Сохранение', asp.controller.getMainComponent());

                    Ext.Array.each(records.items,
                        function (item) {
                            recordIds.push(item.get('Id'));
                        });

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
            xtype: 'gjidocumentaspect',
            name: 'actActionIsolatedEditPanelAspect',
            editPanelSelector: '#actActionIsolatedEditPanel',
            modelName: 'ActActionIsolated',
            otherActions: function(actions) {
            },
            onSaveSuccess: function (asp, rec) {
                
            },
            onAfterSetPanelData: function(asp, rec, panel) {
                this.disableButtons(false);
                this.updateFieldValue(panel, rec);

                var actionPanel = panel.down('actactionisolatedactiongrid');

                // Указываем отображение от вида мероприятия (используем в правах)
                actionPanel.recordAllowed = rec.get('KindAction') === B4.enums.KindAction.Survey;

                if (actionPanel.recordAllowed) {
                    actionPanel.getStore().load();
                }

                this.controller.getAspect('actActionIsolatedStateButtonAspect').setStateData(rec.get('Id'), rec.get('State'));
                this.controller.getAspect('actCreateButtonAspect').setData(rec.get('Id'));

                var callbackUnMask = asp.controller.params?.callbackUnMask;
                if (callbackUnMask && Ext.isFunction(callbackUnMask)) {
                    callbackUnMask.call();
                }
            },
            disableButtons: function(value) {
                //получаем все батон-группы
                var groups = Ext.ComponentQuery.query(this.editPanelSelector + ' buttongroup'),
                    idx = 0;
                //теперь пробегаем по массиву groups и активируем их
                while (true) {

                    if (!groups[idx])
                        break;

                    groups[idx].setDisabled(value);
                    idx++;
                }
            },
            updateFieldValue: function (panel, rec) {
                var fieldInspectors = panel.down('#trigfInspectors');

                fieldInspectors.updateDisplayedText(rec.data.Inspectors);
                fieldInspectors.setValue(rec.data.InspectorIds);
            },
            saveRealityObjectViolation: function (actObjectId, haveViolation, description, violationStore, deferred, notRevViol) {
                var me = this;

                //Блокируем сохранение если не выполняется ряд условий
                if (violationStore.getCount() == 0 && haveViolation == B4.enums.YesNo.Yes) {
                    deferred.fail({ message: 'Если нарушения выявлены, то необходимо в таблице нарушений добавить записи нарушений' });
                    return false;
                }

                if (violationStore.getCount() != 0 && haveViolation != B4.enums.YesNo.Yes) {
                    deferred.fail({ message: 'Записи в таблице нарушений должны быть только если нарушения выявлены' });
                    return false;
                }

                //формируем записи нарушений из стора для последующей обработки на сервере
                var actCheckViolationRecords = [];
                Ext.Array.each(violationStore.getRange(0, violationStore.getCount()),
                    function(item) {
                        var data = item.getData();
                        actCheckViolationRecords.push(
                            {
                                Id: data.Id || 0,
                                ViolationGjiId: data.ViolationGjiId,
                                DatePlanRemoval: data.DatePlanRemoval,
                                ViolationWording: data.ViolationWording,
                                DateFactRemoval: data.DateFactRemoval,
                                DateCancel: data.DateCancel
                            });
                    });

                me.controller.mask('Сохранение', me.controller.getMainComponent());
                B4.Ajax.request({
                    method: 'POST',
                    url: B4.Url.action('SaveParams', 'ActCheckRealityObject'),
                    params: {
                        actObjectId: actObjectId,
                        haveViolation: haveViolation,
                        actViolationJson: Ext.encode(actCheckViolationRecords),
                        changeCancelAndFactDates: true,
                        description: description,
                        notRevViol: notRevViol
                    }
                }).next(function () {
                    me.controller.unmask();
                    deferred.call({ message: 'Сохранение результатов проверки прошло успешно' });
                }).error(function (e) {
                    me.controller.unmask();
                    deferred.fail(e);
                });
                return true;
            },
        },
        {
            /*
            * Аспект взаимодействия кнопки таблицы проверяемых домов с массовой формой выбора домов
            */
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'actActionIsolatedRealityObjectGjiAspect',
            gridSelector: '#actActionIsolatedResultPanel #actActionIsolatedRealityObjectGrid',
            storeName: 'actcheck.RealityObject',
            modelName: 'actcheck.RealityObject',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#actActionIsolatedRealityObjectMultiSelectWindow',
            storeSelect: 'actactionisolated.RealityObjectForSelect',
            storeSelected: 'realityobj.RealityObjectForSelected',
            titleSelectWindow: 'Выбор дома',
            titleGridSelect: 'Дома для отбора',
            titleGridSelected: 'Выбранные дома',
            columnsGridSelect: [
                {
                    header: 'Муниципальный район', xtype: 'gridcolumn', dataIndex: 'Municipality', flex: 1,
                    filter: {
                        xtype: 'b4combobox',
                        operand: CondExpr.operands.eq,
                        storeAutoLoad: false,
                        hideLabel: true,
                        editable: false,
                        valueField: 'Name',
                        emptyItem: { Name: '-' },
                        url: '/Municipality/ListMoAreaWithoutPaging'
                    }
                },
                { header: 'Адрес', xtype: 'gridcolumn', dataIndex: 'Address', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Адрес', xtype: 'gridcolumn', dataIndex: 'Address', flex: 1, sortable: false }
            ],
            listeners: {
                getdata: function (asp, records) {

                    var recordIds = [];

                    records.each(function (rec, index) {
                        recordIds.push(rec.get('Id'));
                    });

                    if (recordIds.length === 0) {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать дома');
                        return false;
                    }

                    asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                    B4.Ajax.request({
                        url: B4.Url.action('SaveRealityObjects', 'ActActionIsolated'),
                        method: 'POST',
                        params: {
                            roIds: Ext.encode(recordIds),
                            actId: asp.controller.params.documentId
                        }
                    }).next(function (response) {
                        asp.controller.unmask();
                        asp.controller.getMainView().down('#actActionIsolatedResultPanel #actActionIsolatedRealityObjectGrid').getStore().load();
                        return true;
                    }).error(function () {
                        asp.controller.unmask();
                    });

                    return true;
                }
            },
            onBeforeLoad: function(store, operation){
                operation.params.documentId = this.controller.params.documentId;
            },
            deleteRecord: function (record) {
                var me = this;

                Ext.Msg.confirm('Удаление записи!', 'Вы действительно хотите удалить результаты мероприятия?', function(res) {
                    if (res == 'yes') {
                        record.destroy()
                            .next(function() {
                                me.updateGrid();
                            }, me)
                            .error(function(result) {
                                Ext.Msg.alert('Ошибка удаления!', Ext.isString(result.responseData) ? result.responseData : result.responseData.message);
                            }, me);
                    }
                }, me);
            },
        },
        {
            /*
            Аспект взаимодействия Таблицы Проверяемых домов акта и формы редактирования
            */
            xtype: 'grideditwindowaspect',
            name: 'actActionIsolatedRealityObjectAspect',
            gridSelector: '#actActionIsolatedRealityObjectGrid',
            editFormSelector: '#actActionIsolatedRealityObjectEditWindow',
            storeName: 'actcheck.RealityObject',
            modelName: 'actcheck.RealityObject',
            editWindowView: 'actionisolated.actactionisolated.RealityObjectEditWindow',
            gridAction: function (grid, action) {},
            listeners: {
                aftersetformdata: function (asp, record, form) {
                    asp.controller.setCurrentRoId(record.getId());
                }
            },
            otherActions: function (actions) {
                actions[this.editFormSelector + ' #cbHaveViolation'] = { 'change': { fn: this.changeHaveViolation, scope: this} };
            },
            changeHaveViolation: function (combobox, newValue) {
                var me = this,
                    actViolationGridAddButton = me.getForm().down('#actViolationGridAddButton');
                
                if(newValue === 0)
                {
                    combobox.setValue(B4.enums.YesNoNotSet.No);
                }

                actViolationGridAddButton.setDisabled(false);

                // Нельзя добавлять, если не 'Да' блокируем кнопку добавления
                if (newValue !== B4.enums.YesNoNotSet.Yes) {
                    actViolationGridAddButton.setDisabled(true);
                }
            },
            //переопределен метод сохранения. Сохраняется форма редактирования и таблица дочерних нарушений
            saveRecord: function (rec) {
                var me = this,
                    editWindow = me.getForm(),
                    storeViolation = editWindow.down('#actCheckViolationGrid').getStore(),
                    cbHaveViolation = editWindow.down('#cbHaveViolation').getValue(),
                    description = editWindow.down('#taDescription').getValue(),
                    deferred = new Deferred();

                editWindow.setDisabled(true);

                deferred.next(function(res) {
                    editWindow.setDisabled(false);
                    editWindow.close();
                    me.getGrid().getStore().load();
                    Ext.Msg.alert('Сохранение!', 'Результаты проверки сохранены успешно');
                }, me)
                    .error(function(e) {
                        editWindow.setDisabled(false);
                        if (e.message) {
                            Ext.Msg.alert('Ошибка сохранения!', e.message);
                        } else {
                            throw e;
                        }
                    }, me);

                me.controller.getAspect('actActionIsolatedEditPanelAspect')
                    .saveRealityObjectViolation(this.controller.currentRoId, cbHaveViolation, description, storeViolation, deferred);
            }
        },
        {
            // Аспект взаимодействия таблицы нарушений по дому с массовой формой выбора нарушений
            // При добавлении открывается форма массового выбора нарушений. После выбора список получается через подписку 
            // на событие getdata идет добавление записей в сторе
            xtype: 'gkhinlinegridmultiselectwindowaspect',
            name: 'actActionIsolatedViolationAspect',
            gridSelector: '#actCheckViolationGrid',
            saveButtonSelector: '#actCheckViolationGrid #actCheckViolationSaveButton',
            storeName: 'actcheck.Violation',
            modelName: 'actcheck.Violation',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#actCheckViolationMultiSelectWindow',
            multiSelectWindowWidth: 1024,
            storeSelect: 'dict.ViolationGjiForSelect',
            storeSelected: 'dict.ViolationGjiForSelected',
            titleSelectWindow: 'Выбор нарушений',
            titleGridSelect: 'Нарушения для отбора',
            titleGridSelected: 'Выбранные нарушения',
            columnsGridSelect: [
                { header: 'Код ПиН', flex: 1, xtype: 'gridcolumn', dataIndex: 'CodePin', filter: { xtype: 'textfield' }, sortable: false },
                {
                    header: 'Наименование',
                    xtype: 'gridcolumn',
                    dataIndex: 'Name',
                    flex: 3,
                    filter: {
                        xtype: 'textfield'
                    },
                    sortable: false,
                    renderer: function(v, m) {
                        m.tdAttr = 'data-qtip="' + v + '"';
                        return v;
                    }
                }
            ],
            columnsGridSelected: [
                { header: 'Код ПиН', xtype: 'gridcolumn', flex: 1, dataIndex: 'CodePin', sortable: false },
                {
                    header: 'Наименование',
                    xtype: 'gridcolumn',
                    dataIndex: 'Name',
                    flex: 2,
                    filter: {
                        xtype: 'textfield'
                    },
                    sortable: false,
                    renderer: function(v, m) {
                        m.tdAttr = 'data-qtip="' + v + '"';
                        return v;
                    }
                }
            ],

            listeners: {
                //В данном методе принимаем массив записей из формы выбора и вставляем их в сторе грида нарушений без сохранения
                getdata: function (asp, records) {
                    var currentViolationStore = asp.controller.getStore(asp.storeName),
                        range = currentViolationStore.getRange(0, currentViolationStore.getCount());

                    asp.controller.mask('Выбор нарушений', asp.controller.getMainComponent());
                    
                    currentViolationStore.removeAll();

                    //сначала добавлем вверх новые нарушения
                    Ext.Array.each(records.items,
                        function (rec) {
                            currentViolationStore.add({
                                Id: 0,
                                ActObject: asp.controller.objectId,
                                ViolationGjiPin: rec.get('CodePin'),
                                ViolationGjiName: rec.get('Name'),
                                ViolationGjiId: rec.get('Id'),
                                ActionsRemovViolName: rec.get('ActRemViol'),
                                DatePlanRemoval: null,
                                NormDocNum: rec.get('NormDocNum')
                            });
                        }, this);

                    //теперь добавляем старые вконец
                    Ext.Array.each(range,
                        function (rec) {
                            currentViolationStore.add(rec);
                        }, this);

                    asp.controller.unmask();

                    return true;
                }
            }
        },
        {
            xtype: 'gkhbuttonprintaspect',
            name: 'actPrintAspect',
            buttonSelector: '#actActionIsolatedEditPanel #btnPrint',
            codeForm: 'ActActionIsolated',
            getUserParams: function () {
                var param = { DocumentId: this.controller.params.documentId };

                this.params.userParams = Ext.JSON.encode(param);
            }
        },
        {
            xtype: 'gkhbuttonprintaspect',
            name: 'actionPrintAspect',
            buttonSelector: '#actActionIsolatedActionAddWindow #btnPrint',
            codeForm: 'ProtocolActAction',
            getUserParams: function () {
                var param = { DocumentId: this.controller.getCurrentActCheckAction() };

                this.params.userParams = Ext.JSON.encode(param);
            }
        },
        {
            xtype: 'griddicteditwindowaspect',
            name: 'actActionIsolatedAspect',
            gridSelector: '#actActionIsolatedActionGrid',
            entityPropertyName: 'ActionType',
            listeners: {
                beforesetformdata: function (asp, record, form) {
                    var actionCarriedOutEvents = form.down('#actCheckActionCarriedOutEventSelectField'),
                        actcheckActionFileGrid = form.down('#actCheckActionFileGridActActionIsolated'),
                        trigfActionInspectors = form.down('#trigfActionInspectors'),
                        actCheckActionRemarkGrid = form.down('#actCheckActionRemarkGridActActionIsolated'),
                        actCheckActionViolationGrid = form.down('#actCheckActionViolationGridActActionIsolated'),
                        instrExamActionNormativeDocGrid = form.down('#instrExamActionNormativeDocGridActActionIsolated'),
                        stores = [];

                    asp.currentActCheckActionId = record.getId();
                    asp.actCheckActionInspectorIds = null;

                    if (actionCarriedOutEvents) {
                        stores.push(asp.controller.getActionObjectStore('actcheck.ActionCarriedOutEvent'));
                    }

                    if (actcheckActionFileGrid) {
                        stores.push(asp.controller.getActionObjectStore('actcheck.ActionFile'));
                    }

                    if (trigfActionInspectors) {
                        stores.push(asp.controller.getActionObjectStore('actcheck.ActionInspector'));
                    }

                    if (actCheckActionRemarkGrid) {
                        stores.push(asp.controller.getActionObjectStore('actcheck.ActionRemark'));
                    }

                    if (actCheckActionViolationGrid) {
                        stores.push(asp.controller.getActionObjectStore('actcheck.ActionViolation'));
                    }

                    if (instrExamActionNormativeDocGrid) {
                        stores.push(asp.controller.getActionObjectStore('actcheck.InstrExamActionNormativeDoc'));
                    }

                    stores.forEach(function (store) {
                        store.removeAll();
                        store.load();
                    });
                },
                aftersetformdata: function (asp, record, form) {
                    asp.controller.getAspect('actionPrintAspect').loadReportStore();
                    form.center();
                },
                beforesave: function (asp, record) {
                    var form = asp.getForm(),
                        actionCarriedOutEvent = form.down('#actCheckActionCarriedOutEventSelectField');

                    if (actionCarriedOutEvent){
                        var enumValues = [], fieldValue = actionCarriedOutEvent.value;

                        if (fieldValue) {
                            var enumItemsMeta = fieldValue;

                            if (fieldValue === 'All'){
                                enumItemsMeta = B4.enums.ActCheckActionCarriedOutEventType.getItemsMeta();
                            }

                            enumItemsMeta.forEach(function (enumItemMeta) {
                                enumValues.push(enumItemMeta.Value);
                            })
                        }

                        B4.Ajax.request(B4.Url.action('AddCarriedOutEvents', 'ActCheckAction', {
                            enumValues: enumValues.length === 0 ? '' : enumValues,
                            actCheckActionId: record.getId()
                        })).error(function (err) {
                            Ext.Msg.alert('Ошибка!', err.message);
                            return false;
                        });
                    }

                    if (asp.actCheckActionInspectorIds) {
                        B4.Ajax.request(B4.Url.action('AddInspectors', 'ActCheckAction', {
                            inspectorIds: asp.actCheckActionInspectorIds,
                            actCheckActionId: record.getId()
                        })).error(function (err) {
                            Ext.Msg.alert('Ошибка!', err.message);
                            return false;
                        });
                    }
                }
            },
            setModelAndEditWindowDict: function () {
                var me = this;

                // Для добавления новой записи
                me.modelAndEditWindowMap.set(0, ['actcheck.Action', '#actActionIsolatedActionAddWindow', 'actionisolated.actactionisolated.ActionAddWindow']);

                B4.enums.ActActionType.getItems().forEach(function (item) {
                    var key, properties;
                    switch (item[0]) {
                        case B4.enums.ActActionType.Inspection:
                            key = B4.enums.ActActionType.Inspection;
                            properties = ['actcheck.InspectionAction', 'inspectionactactionisolatededitwindow', 'actionisolated.actactionisolated.inspectionaction.ActionEditWindow'];
                            break;
                        case B4.enums.ActActionType.InstrumentalExamination:
                            key = B4.enums.ActActionType.InstrumentalExamination;
                            properties = ['actcheck.InstrExamAction', 'instrexamactactionisolatededitwindow', 'actionisolated.actactionisolated.instrexamaction.ActionEditWindow'];
                            break;
                        default:
                            return;
                    }
                    me.modelAndEditWindowMap.set(key, properties);
                });
            },
            getRecordBeforeSave: function (record) {
                var asp = this;

                if (!record.get('ActCheck')) {
                    record.set('ActCheck', asp.ActCheckId);
                }

                return record;
            },
            // Получить окно редактирования действия (без создания, в отличие от .getForm())
            getActionEditForm: function () {
                var me = this;

                if (me.editFormSelector) {
                    return me.componentQuery(me.editFormSelector);
                }

                return null;
            }
        },
        {
            /*
            Вешаем аспект смены статуса в карточке редактирования
            */
            xtype: 'statebuttonaspect',
            name: 'actActionIsolatedStateButtonAspect',
            stateButtonSelector: '#actActionIsolatedEditPanel #btnState',
            listeners: {
                transfersuccess: function (asp, entityId) {
                    //После успешной смены статуса запрашиваем по Id актуальные данные записи
                    //и обновляем панель
                    asp.controller.getAspect('actActionIsolatedEditPanelAspect').setData(entityId);
                    asp.controller.getMainView().up('#actionIsolatedNavigationPanel').getComponent('actionisolatedMenuTree').getStore().load();
                }
            }
        },
        {
            // Аспект, который обеспечивает работу грида "Нормативно-правовые акты" 
            // в окне редактирования действия акта проверки с типом "Инструментальное обследование"
            xtype: 'gkhinlinegridaspect',
            name: 'actActionIsolatedInstrExamActionNormativeDocAspect',
            storeName: 'actcheck.InstrExamActionNormativeDocActActionIsolated',
            modelName: 'actcheck.InstrExamActionNormativeDoc',
            gridSelector: '#instrExamActionNormativeDocGridActActionIsolated',
            saveButtonSelector: '#instrExamActionNormativeDocGridActActionIsolated #saveButton',
            listeners: {
                beforesave: function (asp, store) {
                    asp.controller.setActCheckActionForStoreRecords(store, 'InstrExamAction');
                    return true;
                }
            }
        },
        {
            // Аспект, который обеспечивает работу грида "Нарушения" 
            // в окне редактирования действия акта проверки
            xtype: 'gkhinlinegridaspect',
            name: 'actActionIsolatedActionViolationAspect',
            storeName: 'actcheck.ActionViolationActActionIsolated',
            modelName: 'actcheck.ActionViolation',
            gridSelector: '#actCheckActionViolationGridActActionIsolated',
            saveButtonSelector: '#actCheckActionViolationGridActActionIsolated #saveButton',
            listeners: {
                beforesave: function (asp, store) {
                    asp.controller.setActCheckActionForStoreRecords(store);
                    return true;
                }
            }
        },
        {
            xtype: 'actcheckactionviolationaspect',
            buttonSelector: '#actCheckActionViolationGridActActionIsolated #addButton',
            multiSelectWindowSelector: '#actCheckActionIsolatedViolationGridSelector',
            inlineGridAspectName: 'actActionIsolatedActionViolationAspect'
        },
        {
            // Аспект, который обеспечивает работу грида "Замечания" 
            // в окне редактирования действия акта проверки
            xtype: 'gkhinlinegridaspect',
            name: 'actActionIsolatedActionRemarkAspect',
            storeName: 'actcheck.ActionRemarkActActionIsolated',
            modelName: 'actcheck.ActionRemark',
            gridSelector: '#actCheckActionRemarkGridActActionIsolated',
            saveButtonSelector: '#actCheckActionRemarkGridActActionIsolated #saveButton',
            listeners: {
                beforesave: function (asp, store) {
                    asp.controller.setActCheckActionForStoreRecords(store);
                    return true;
                }
            }
        },
        {
            // Аспект, который обеспечивает работу грида "Файлы" 
            // в окне редактирования действия акта проверки
            xtype: 'grideditwindowaspect',
            name: 'actActionIsolatedActionFileAspect',
            gridSelector: '#actCheckActionFileGridActActionIsolated',
            editFormSelector: '#actActionIsolatedFileEditWindow',
            storeName: 'actcheck.ActionFileActActionIsolated',
            modelName: 'actcheck.ActionFile',
            editWindowView: 'actionisolated.actactionisolated.ActionFileEditWindow',
            listeners: {
                beforerowaction: function (asp, grid, action, rec) {
                    if (action.toLowerCase() === 'doubleclick') {
                        return false;
                    }
                },
                getdata: function (asp, record) {
                    asp.controller.setActCheckActionForRecord(record);
                    return true;
                }
            }
        },
        {
            // Аспект, который обеспечивает добавление инспекторов 
            // в окне редактирования действия акта проверки
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'actActionIsolatedActionInspectorMultiSelectWindowAspect',
            fieldSelector: '#actActionIsolatedActionAddWindow #trigfActionInspectors',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#actActionIsolatedActionInspectorSelectWindow',
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
                    var actionEditAspect = asp.controller.getAspect('actActionIsolatedAspect');

                    actionEditAspect.actCheckActionInspectorIds = [];

                    records.each(function (rec) {
                        actionEditAspect.actCheckActionInspectorIds.push(rec.get('Id'));
                    });

                    return true;
                }
            }
        },
        {
            /*
            * Аспект взаимодействия Таблицы приложений с формой редактирования
            */
            xtype: 'grideditwindowaspect',
            name: 'actActionIsolatedAnnexAspect',
            gridSelector: '#actActionIsolatedAnnexGrid',
            editFormSelector: '#actActionIsolatedAnnexEditWindow',
            storeName: 'actactionisolated.Annex',
            modelName: 'actcheck.Annex',
            editWindowView: 'actionisolated.actactionisolated.AnnexEditWindow',
            listeners: {
                getdata: function (asp, record) {
                    //Для новой записи присваиваем родительский документ
                    if (!record.get('Id')) {
                        record.set('ActCheck', this.controller.params.documentId);
                    }
                }
            }
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'actActionIsolatedDefinitionAspect',
            gridSelector: '#actActionIsolatedDefinitionPanel',
            editFormSelector: '#actActionIsolatedDefinitionEditWindow',
            storeName: 'actactionisolated.Definition',
            modelName: 'actactionisolated.Definition',
            editWindowView: 'actionisolated.actactionisolated.DefinitionEditWindow',
            listeners: {
                getdata: function (asp, record) {
                    record.data.Act = {Id: asp.controller.params.documentId};
                    return true;
                }
            }
        }
    ],

    init: function () {
        var me = this;

        me.getActionObjectStore('actcheck.Action').on('beforeload', me.onActionBeforeLoad, me);
        me.getActionObjectStore('actcheck.ActionCarriedOutEvent').on('beforeload', me.onActionObjectBeforeLoad, me);
        me.getActionObjectStore('actcheck.ActionCarriedOutEvent').on('load', me.onActionCarriedOutEventLoad, me);
        me.getActionObjectStore('actcheck.ActionFile').on('beforeload', me.onActionObjectBeforeLoad, me);
        me.getActionObjectStore('actcheck.ActionInspector').on('beforeload', me.onActionObjectBeforeLoad, me);
        me.getActionObjectStore('actcheck.ActionInspector').on('load', me.onActionInspectorStoreLoad, me);
        me.getActionObjectStore('actcheck.ActionRemark').on('beforeload', me.onActionObjectBeforeLoad, me);
        me.getActionObjectStore('actcheck.ActionViolation').on('beforeload', me.onActionObjectBeforeLoad, me);
        me.getActionObjectStore('actcheck.InstrExamActionNormativeDoc').on('beforeload', me.onActionObjectBeforeLoad, me);
        me.getStore('actactionisolated.Annex').on('beforeload', me.onObjectBeforeLoad, me);

        me.getStore('actcheck.Violation').on('beforeload', me.onViolationBeforeLoad, me);
        me.getStore('actcheck.RealityObject').on('beforeload', me.onBeforeLoad, me);
        me.getStore('actactionisolated.Definition').on('beforeload', me.onBeforeLoad, me);
        
        me.control({
            '#actActionIsolatedDefinitionEditWindow [name=RealityObject]': {
                'beforeload': me.onBeforeLoad
            },
            '#actActionIsolatedActionAddWindow b4combobox[name=ActionType]': {
                storebeforeload: function (field, store, options) {
                    me.onBeforeLoad(store, options);
                }
            }
        });

        me.callParent(arguments);
    },

    onLaunch: function () {
        var me = this;

        if (me.params) {
            me.getAspect('actActionIsolatedEditPanelAspect').setData(me.params.documentId);
            me.getAspect('actPrintAspect').loadReportStore();
        }
        
        me.getMainView().down('b4enumcombo[name=AcquaintState]')
            .on('change', me.acquaintStateChanged, me);
        me.getStore('actcheck.RealityObject').load();
        me.getStore('actactionisolated.Annex').load();
        me.getStore('actactionisolated.Definition').load();
    },

    onBeforeLoad: function(store, operation)
    {
        operation.params.documentId = this.params.documentId;
    },

    onViolationBeforeLoad: function (store, operation) {
        if (this.currentRoId > 0)
            operation.params.objectId = this.currentRoId;
    },

    setCurrentRoId: function (id) {
        var me = this;

        me.currentRoId = id;
        me.getStore('actcheck.Violation').load();
    },

    acquaintStateChanged: function(component, newValue) {
        var fieldset = component.up('fieldset[name=AcquaintInfo]'),
            refusedToAcquaintPersonField = fieldset.down('textfield[name=RefusedToAcquaintPerson]'),
            acquaintedPersonField = fieldset.down('textfield[name=AcquaintedPerson]');

        refusedToAcquaintPersonField.hide();
        acquaintedPersonField.hide();

        switch (newValue) {
            case B4.enums.AcquaintState.NotAcquainted:
                break;

            case B4.enums.AcquaintState.RefuseToAcquaint:
                refusedToAcquaintPersonField.setVisible(true);
                break;

            case B4.enums.AcquaintState.Acquainted:
                acquaintedPersonField.setVisible(true);
                break;
        }
    },

    onActionBeforeLoad: function (store, operation) {
        var me = this,
            actionEditAspect = me.getAspect('actActionIsolatedAspect');

        if (me.params && me.params.documentId > 0) {
            operation.params.documentId = me.params.documentId;
            actionEditAspect.ActCheckId = me.params.documentId;
        }
    },

    onActionObjectBeforeLoad: function (store, operation) {
        var me = this,
            actionEditAspect = me.getAspect('actActionIsolatedAspect');

        if (actionEditAspect.currentActCheckActionId &&
            actionEditAspect.currentActCheckActionId > 0) {
            operation.params.actCheckActionId = actionEditAspect.currentActCheckActionId;
        }
    },

    onActionCarriedOutEventLoad: function (store, records) {
        var me = this,
            actionEditAspect = me.getAspect('actActionIsolatedAspect'),
            form = actionEditAspect.getActionEditForm();

        if (form) {
            var carriedOutEventSelectField = form.down('#actCheckActionCarriedOutEventSelectField'),
                eventType, value = [];

            if (records.length === B4.enums.ActCheckActionCarriedOutEventType.getItems().length) {
                carriedOutEventSelectField.updateDisplayedText('Выбраны все');
                carriedOutEventSelectField.value = 'All';
            }
            else {
                records.forEach(function (rec) {
                    eventType = rec.get('EventType');

                    if (eventType) {
                        value.push(B4.enums.ActCheckActionCarriedOutEventType.getMeta(eventType));
                    }
                });

                if (value.length > 0) {
                    carriedOutEventSelectField.setValue(value);
                }
            }
        }
    },

    onActionInspectorStoreLoad: function (store, records) {
        var me = this,
            actionEditAspect = me.getAspect('actActionIsolatedAspect'),
            form = actionEditAspect.getActionEditForm();

        if (form) {
            var trigfActionInspectors = form.down('#trigfActionInspectors'),
                inspector, inspectorIds = [], inspectorNames = [];

            records.forEach(function (rec) {
                inspector = rec.get('Inspector');

                if (inspector) {
                    inspectorIds.push(inspector.Id);
                    inspectorNames.push(inspector.Fio);
                }
            });

            if (inspectorIds.length > 0) {
                trigfActionInspectors.updateDisplayedText(inspectorNames.join(', '));
                trigfActionInspectors.setValue(inspectorIds.join(', '));
            }
        }
    },

    setActCheckActionForRecord: function (record, propertyName) {
        var me = this;

        if (!propertyName){
            propertyName = 'ActCheckAction';
        }

        if (!record.get('Id')) {
            record.set(propertyName, me.getCurrentActCheckAction());
        }
    },

    setActCheckActionForStoreRecords: function (store, propertyName) {
        var me = this;

        store.each(function (record) {
            me.setActCheckActionForRecord(record, propertyName);
        });
    },

    getCurrentActCheckAction: function () {
        var me = this,
            asp = me.getAspect('actActionIsolatedAspect');

        return asp.currentActCheckActionId;
    },

    // Получить дублирующий стор по storeId (создать его)
    getActionObjectStore: function(name) {
        var storeId = name + 'ActActionIsolated',
            store = Ext.StoreManager.get(storeId);

        if (!store) {
            store = Ext.create(this.application.getModuleClassName(name, 'store'), {
                storeId: storeId
            });
        }

        return store;
    },

    onObjectBeforeLoad: function (store, operation) {
        var me = this;

        if (me.params && me.params.documentId > 0)
            operation.params.documentId = me.params.documentId;
    }
});
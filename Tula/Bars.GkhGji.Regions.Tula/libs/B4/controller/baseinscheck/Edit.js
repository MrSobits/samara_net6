//ToDo данный js перекрыт в связи с тем что понадобилось в ННовгород добавить для всех сонований поле Ликвидацию ЮЛ в котором
//ToDo при change поля Контрагент срабатывает получение ликвидации и вывода информации 
Ext.define('B4.controller.baseinscheck.Edit', {
    extend: 'B4.base.Controller',
    params: null,
    requires: [
        'B4.aspects.GjiInspection',
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.aspects.permission.BaseInsCheck',
        'B4.aspects.StateButton',
        'B4.aspects.GjiDocumentCreateButton',
        'B4.Ajax',
        'B4.Url'
    ],

    models: [
        'BaseInsCheck',
        'Disposal'
    ],

    stores: [
        'dict.InspectorForSelect',
        'dict.InspectorForSelected',
        'realityobj.ByTypeOrg',
        'realityobj.RealityObjectForSelect',
        'realityobj.RealityObjectForSelected'
    ],

    views: [
        'baseinscheck.EditPanel',
        'SelectWindow.MultiSelectWindow'
    ],

    mainView: 'baseinscheck.EditPanel',
    mainViewSelector: '#baseInsCheckEditPanel',

    mixins: {
        controllerLoader: 'B4.mixins.LayoutControllerLoader',
        mask: 'B4.mixins.MaskBody'
    },

    baseInsCheckEditPanelSelector : '#baseInsCheckEditPanel',

    staticText: {
        ContragentLiquidatedYes: 'Да',
        ContragentLiquidatedNo: 'Нет'
    },
    
    aspects: [
        {
            /*
            Аспект формирвоания документов для данного основания проверки
            */
            xtype: 'gjidocumentcreatebuttonaspect',
            name: 'baseInsCheckCreateButtonAspect',
            buttonSelector: '#baseInsCheckEditPanel gjidocumentcreatebutton',
            containerSelector: '#baseInsCheckEditPanel',
            typeBase: 10 // Тип инспекционная проверка
        },
        {
            xtype: 'baseinscheckperm',
            editFormAspectName: 'baseInsCheckEditPanelAspect'
        },
        {
            /*
            Вешаем аспект смены статуса в карточке редактирования
            */
            xtype: 'statebuttonaspect',
            name: 'baseInsCheckStateButtonAspect',
            stateButtonSelector: '#baseInsCheckEditPanel #btnState',
            listeners: {
                transfersuccess: function (asp, entityId) {
                    //После успешной смены статуса запрашиваем по Id актуальные данные записи
                    //и обновляем панель
                    asp.controller.getAspect('baseInsCheckEditPanelAspect').setData(entityId);
                }
            }
        },
        {
            /*
             *Аспект основной панели инспекционных проверок
             */
            xtype: 'gjiinspectionaspect',
            name: 'baseInsCheckEditPanelAspect',
            editPanelSelector: '#baseInsCheckEditPanel',
            modelName: 'BaseInsCheck',
            otherActions: function (actions) {
                actions[this.editPanelSelector + ' #cbTypeCheck'] = { 'change': { fn: this.onChangeTypeFact, scope: this } };
                actions[this.editPanelSelector + ' #sfContragent'] = {
                    'change': { fn: this.onChangeContragent, scope: this }
                };
            },
            onChangeTypeFact: function (field, newValue) {
                this.getPanel().down('#tfReason').setDisabled(newValue != 30);
            },
            onChangeContragent: function (field, newValue) {
                var me = this,
                    panel = field.up(me.editPanelSelector),
                    tfActiviryInfo = panel.down('textfield[name=ActivityInfo]');

                // поумолчанию ставим нет
                tfActiviryInfo.setValue(me.controller.staticText.ContragentLiquidatedNo);

                if (newValue) {
                    //& newValue.ContragentState == 40
                    if (newValue > 0) {
                        B4.Ajax.request(B4.Url.action('GetActivityInfo', 'Contragent', {
                            contragentId: newValue
                        })).next(function (response) {
                            //десериализуем полученную строку
                            var obj = Ext.JSON.decode(response.responseText);

                            tfActiviryInfo.setValue(obj.info);
                        }).error(function (e) {
                            Ext.Msg.alert('Ошибка получения ликвидации!', (e.message || e));
                        });
                    }
                    else if (newValue.ContragentState == 40) {
                        tfActiviryInfo.setValue(me.controller.staticText.ContragentLiquidatedYes);
                    }
                }
            },
            listeners: {
                aftersetpaneldata: function (asp, rec, panel) {

                    asp.controller.params = asp.controller.params || {};
                    
                    // Поскольку в параметрах могли передать callback который срабатывает после открытия карточки
                    // Будем считать что данный метод и есть тот самый метод котоырй будет вызывать callback который ему передали
                    var callbackUnMask = asp.controller.params.callbackUnMask;
                    if (callbackUnMask && Ext.isFunction(callbackUnMask)) {
                        callbackUnMask.call();
                    }
                    
                    //Делаем запрос на получение дополнительной информации после загрузки объекта
                    asp.controller.mask('Загрузка', asp.controller.getMainComponent());
                    B4.Ajax.request(B4.Url.action('GetInfo', 'BaseInsCheck', {
                        inspectionId: asp.controller.params.inspectionId
                    })).next(function(response) {
                        asp.controller.unmask();
                        //необходим для выбора домов чтобы можно было выбирать из тех домов которые находятся у этой организации
                        //robjectIds - строка идентификаторов домов Проверки для Тригер филда Домов
                        //robjectNames - строка адресов домов проверки для Тригер филда Домов
                        //inspectorIds - строка идентификаторов инспекторов проверки для Тригер филда Инспекторов
                        //inspectorNames - строка ФИО инспекторов проверки для тригер филда Инспекторов
                        //robjectArea - общая площадь домов

                        //десериализуем полученную строку
                        var obj = Ext.JSON.decode(response.responseText);

                        var fieldRealityObject = panel.down('#insCheckRealityObjectsTrigerField');
                        fieldRealityObject.updateDisplayedText(obj.robjectNames);
                        fieldRealityObject.setValue(obj.robjectIds);

                        var fieldInspectors = panel.down('#insCheckInspectorsTrigerField');
                        fieldInspectors.updateDisplayedText(obj.inspectorNames);
                        fieldInspectors.setValue(obj.inspectorIds);

                        var fieldArea = panel.down('#insCheckEditPanelAreaNumberField');
                        fieldArea.setValue(obj.robjectArea);
                    }).error(function () {
                        asp.controller.unmask();
                    });

                    //Обновляем статусы
                    this.controller.getAspect('baseInsCheckStateButtonAspect').setStateData(rec.get('Id'), rec.get('State'));
                    //Обновляем кнопку Сформировать
                    this.controller.getAspect('baseInsCheckCreateButtonAspect').setData(rec.get('Id'));
                }
            }
        },
        {
            /*
            аспект взаимодействия триггер-поля Дом с массовой формой выбора домов
            по нажатию на кнопку отбора показывается форма массового выбора после чего идет отбор
            По нажатию на кнопку Применить в методе getdata мы обрабатываем полученные значения
            и сохраняем инспекторов через серверный метод /InspectionGJI/AddInspectors
            
            тут есть специфический момент: в инспекционной проверке должен быть только один дом
            поэтому мы просто перекрываем метод onRowSelect и там контролируем чтобы был выбран только один дом
            */
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'baseInsCheckRealityObjectMultiSelectWindowAspect',
            fieldSelector: '#baseInsCheckEditPanel #insCheckRealityObjectsTrigerField',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#insCheckRealityObjectSelectWindow',
            storeSelect: 'realityobj.ByTypeOrg',
            storeSelected: 'realityobj.RealityObjectForSelected',
            textProperty: 'Address',
            selModelMode: 'SINGLE',
            columnsGridSelect: [
                {
                    header: 'Муниципальное образование', xtype: 'gridcolumn', dataIndex: 'Municipality', flex: 1,
                    filter: {
                        xtype: 'b4combobox',
                        operand: CondExpr.operands.eq,
                        storeAutoLoad: false,
                        hideLabel: true,
                        editable: false,
                        valueField: 'Name',
                        emptyItem: { Name: '-' },
                        url: '/Municipality/ListWithoutPaging'
                    }
                },
                { header: 'Адрес', xtype: 'gridcolumn', dataIndex: 'Address', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Адрес', xtype: 'gridcolumn', dataIndex: 'Address', flex: 1, sortable: false }
            ],
            titleSelectWindow: 'Выбор дома',
            titleGridSelect: 'Дома для отбора',
            titleGridSelected: 'Выбранные дома',

            // Закоментировал в связи с событием #22314 п.3
            //updateSelectedGrid: function () {
            //    //чтобы грид выбранных не перезагружался
            //},

            onRowSelect: function (rowModel, record) {
                //Поскольку наша форма множественного выборка должна возвращать только 1 значение
                //То Перекрываем метод select и перед добавлением выделенной записи сначала очищаем стор
                //куда хотим добавить запись
                var grid = this.getSelectedGrid();
                if (grid) {
                    var storeSelected = grid.getStore();
                    storeSelected.removeAll();
                    storeSelected.add(record);
                }
            },

            onBeforeLoad: function (store, operation) {
                var panel = Ext.ComponentQuery.query(this.controller.baseInsCheckEditPanelSelector)[0];
                operation.params.contragentId = panel.down('#sfContragent').getValue();
                operation.params.date = panel.down('#dfInsCheckDate').getValue();
                operation.params.typeJurPerson = 10;
            },

            onSelectedBeforeLoad: function (store, operation) {
                //поскольку при открытии формы выбора вверхнем гриде должны быть уже проставлены выбранные дома
                //то необходимо в контроллер в метод лист передать параметр с идентификаторами домов которые уже есть в этой проверке
                var field = this.getSelectField();
                if (field) {
                    operation.params['realityObjIds'] = field.getValue();
                }
            },
            listeners: {
                getdata: function (asp, records) {
                    var record = null;

                    records.each(function (rec) {
                        record = rec;
                    });

                    var mainPanel = asp.controller.getMainView();

                    if (record && record.get('AreaMkd') > 0)
                        mainPanel.down('#insCheckEditPanelAreaNumberField').setValue(record.get('AreaMkd'));
                    else
                        mainPanel.down('#insCheckEditPanelAreaNumberField').setValue(0);
                }
            }
        },
        {
            /*
            аспект взаимодействия триггер-поля инспекторы с массовой формой выбора инспекторов
            по нажатию на кнопку отбора показывается форма массового выбора после чего идет отбор
            По нажатию на кнопку Применить в методе getdata мы обрабатываем полученные значения
            и сохраняем инспекторов через серверный метод /InspectionGJI/AddInspectors
            
            тут есть специфический момент: в инспекционной проверке должен быть тольк оодин инспектор
            поэтому мы просто перекрываем метод onRowSelect и там контролируем чтобы был выбран тольк оодин инспектор
            */
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'baseInsCheckInspectorMultiSelectWindowAspect',
            fieldSelector: '#baseInsCheckEditPanel #insCheckInspectorsTrigerField',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#baseInsCheckInspectorSelectWindow',
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

            onRowSelect: function (rowModel, record) {
                //Поскольку наша форма множественного выборка должна возвращать только 1 значение
                //То Перекрываем метод select и перед добавлением выделенной записи сначала очищаем стор
                //куда хотим добавить запись
                var grid = this.getSelectedGrid();
                if (grid) {
                    var storeSelected = grid.getStore();
                    storeSelected.removeAll();
                    storeSelected.add(record);
                }
            }
        }
    ],

    onLaunch: function () {
        if (this.params) {
            this.getAspect('baseInsCheckEditPanelAspect').setData(this.params.inspectionId);
            var mainView = this.getMainComponent();
            if(mainView)
                mainView.setTitle(this.params.title);
        }
    }
});
Ext.define('B4.controller.admincase.Edit', {
    extend: 'B4.base.Controller',
    params: null,
    objectId: 0,

    requires: [
        'B4.aspects.GjiDocument',
        'B4.aspects.GridEditWindow',
        'B4.aspects.GkhButtonMultiSelectWindow',
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.aspects.GkhInlineGridMultiSelectWindow',
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.aspects.StateButton', 'B4.Ajax', 'B4.Url',
        'B4.aspects.GjiDocumentCreateButton',
        'B4.aspects.GkhButtonPrintAspect',
        'B4.aspects.permission.AdminCase',
        'B4.aspects.GkhBlobText'
    ],

    models: [
        'AdminCase',
        'admincase.Doc',
        'admincase.Annex',
        'admincase.ArticleLaw',
        'admincase.ProvidedDoc',
        'requirement.Requirement',
        'admincase.Violation'
    ],

    stores: [
        'AdminCase',
        'admincase.Doc',
        'admincase.Annex',
        'admincase.ArticleLaw',
        'admincase.ProvidedDoc',
        'dict.ArticleLawGjiForSelect',
        'dict.ArticleLawGjiForSelected',
        'dict.ProvidedDocGjiForSelect',
        'dict.ProvidedDocGjiForSelected',
        'RealityObject',
        'dict.Inspector',
        'Contragent',
        'admincase.Requirement',
        'requirement.Type',
        'admincase.Violation',
        'admincase.ViolationForSelect',
        'admincase.ViolationForSelected'
    ],

    views: [
        'admincase.EditPanel',
        'admincase.AnnexEditWindow',
        'admincase.AnnexGrid',
        'admincase.DocGrid',
        'admincase.DocEditWindow',
        'admincase.ArticleLawGrid',
        'admincase.ProvidedDocGrid',
        'SelectWindow.MultiSelectWindow',
        'admincase.RequirementGrid',
        'admincase.RequirementEditWindow',
        'admincase.ViolationGrid'
    ],

    mainView: 'admincase.EditPanel',
    mainViewSelector: '#adminCaseEditPanel',

    mixins: {
        controllerLoader: 'B4.mixins.LayoutControllerLoader',
        mask: 'B4.mixins.MaskBody'
    },

    aspects: [
        {
            xtype: 'admincaseperm',
            editFormAspectName: 'adminCaseEditPanelAspect'
        },
        {
            /*
            Аспект формирвоания документов для Постановление прокуратуры
            */
            xtype: 'gjidocumentcreatebuttonaspect',
            name: 'adminCaseCreateButtonAspect',
            buttonSelector: '#adminCaseEditPanel gjidocumentcreatebutton',
            containerSelector: '#adminCaseEditPanel',
            typeDocument: 110,// Тип документа Административное дело
            onValidateUserParams: function (params) {
                // ставим возврат false, для того чтобы оборвать выполнение операции
                // для следующих парвил необходимы пользовательские параметры
                if (params.ruleId == 'AdminCaseToProtocolRule' || params.ruleId == 'AdminCaseToPrescriptionlRule') {
                    return false;
                }
            }
        },
        {
            xtype: 'b4_state_contextmenu',
            name: 'admincaseRequirementStateTransferAspect',
            gridSelector: 'admincaserequirementgrid',
            menuSelector: 'admincaserequirementgridmenu',
            stateType: 'gji_requirement'
        },
        {
            /**
            * Вешаем аспект смены статуса на форме Требования
            */
            xtype: 'statebuttonaspect',
            name: 'admincaseRequirementStateButtonAspect',
            stateButtonSelector: 'admincaserequirementeditwin #btnState',
            listeners: {
                transfersuccess: function (asp, entityId) {

                    //После перевода статуса необходимо обновить форму
                    //чтобы права вступили в силу
                    var me = this,
                        model = this.controller.getModel('requirement.Requirement');

                    model.load(entityId, {
                        success: function (rec) {
                            me.controller.getAspect('admincaseRequirementAspect').setFormData(rec);
                        },
                        scope: this
                    });
                }
            }
        },
        {
            xtype: 'gkhbuttonprintaspect',
            name: 'adminCasePrintAspect',
            buttonSelector: 'adminCaseEditPanel #btnPrint',
            codeForm: 'AdministrativeCase',
            displayField: 'Description',
            getUserParams: function () {

                var param = { DocumentId: this.controller.params.documentId };

                this.params.userParams = Ext.JSON.encode(param);
            }
        },
        {
            xtype: 'gkhbuttonprintaspect',
            name: 'adminCaseDocPrintAspect',
            buttonSelector: 'admincasedoceditwindow #btnPrint',
            codeForm: 'AdminCaseDoc',
            displayField: 'Description',
            getUserParams: function () {

                var param = { DocumentId: this.controller.params.adminDocId };

                this.params.userParams = Ext.JSON.encode(param);
            }
        },
        {
            /*
            Вешаем аспект смены статуса в карточке редактирования
            */
            xtype: 'statebuttonaspect',
            name: 'adminCaseStateButtonAspect',
            stateButtonSelector: '#adminCaseEditPanel #btnState',
            listeners: {
                transfersuccess: function (asp, entityId) {
                    //После успешной смены статуса запрашиваем по Id актуальные данные записи
                    //и обновляем панель
                    var editPanelAspect = asp.controller.getAspect('adminCaseEditPanelAspect');
                    
                    editPanelAspect.setData(entityId);
                    editPanelAspect.reloadTreePanel();
                }
            }
        },
        {   /*
            Апект для основной панели Постановления прокуратуры
            */
            xtype: 'gjidocumentaspect',
            name: 'adminCaseEditPanelAspect',
            editPanelSelector: '#adminCaseEditPanel',
            modelName: 'AdminCase',
            listeners: {
                savesuccess: function (asp) {
                    asp.reloadTreePanel();
                }
            },
            //перекрываем метод После загрузки данных на панель
            onAfterSetPanelData: function (asp, rec, panel) {
                asp.controller.params = asp.controller.params || {};

                // Поскольку в параметрах могли передать callback который срабатывает после открытия карточки
                // Будем считать что данный метод и есть тот самый метод котоырй будет вызывать callback который ему передали
                var callbackUnMask = asp.controller.params.callbackUnMask;
                if (callbackUnMask && Ext.isFunction(callbackUnMask)) {
                    callbackUnMask.call();
                }
                
                //После проставления данных обновляем title вкладки
                if (rec.get('DocumentNumber')) 
                    panel.setTitle('Административное дело ' + rec.get('DocumentNumber'));
                else
                    panel.setTitle('Административное дело ');

                this.disableButtons(false);

                B4.Ajax.request(B4.Url.action('GetInfo', 'AdministrativeCase', {
                    documentId: asp.controller.params.documentId
                })).next(function(response) {
                    var obj = Ext.decode(response.responseText);
                    asp.controller.unmask();
                    panel.down('[name=ParentDocument]').setValue(obj.data.parentDocument);
                }).error(function() {
                    panel.down('[name=ParentDocument]').setValue(null);
                    asp.controller.unmask();
                });

                //Обновляем сторы
                this.controller.getStore('admincase.Annex').load();
                this.controller.getStore('admincase.ArticleLaw').load();
                this.controller.getStore('admincase.ProvidedDoc').load();
                this.controller.getStore('admincase.Doc').load();
                panel.down('admincaserequirementgrid').getStore().load();
                
                //загружаем стор печаток
                this.controller.getAspect('adminCasePrintAspect').loadReportStore();

                //Обновляем статусы
                this.controller.getAspect('adminCaseStateButtonAspect').setStateData(rec.get('Id'), rec.get('State'));
                
                // обновляем кнопку Сформирвоать
                this.controller.getAspect('adminCaseCreateButtonAspect').setData(rec.get('Id'));

                this.controller.getAspect('adminCaseBlobDescriptionAspect').doInjection();
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
            xtype: 'gkhblobtextaspect',
            name: 'adminCaseBlobDescriptionAspect',
            fieldSelector: '[name="DescriptionSet"]',
            editPanelAspectName: 'adminCaseEditPanelAspect',
            controllerName: 'AdministrativeCase',
            valueFieldName: 'DescriptionSet',
            previewLength: 2000,
            autoSavePreview: true,
            previewField: 'DescriptionSet',
            getParentRecordId: function () {
                return this.editPanelAspect.getRecord().getId();
            }
        },
        {
            /*
            Аспект взаимодействия Таблицы приложений с формой редактирования
            */
            xtype: 'grideditwindowaspect',
            name: 'adminCaseAnnexAspect',
            gridSelector: '#adminCaseAnnexGrid',
            editFormSelector: '#adminCaseAnnexEditWindow',
            storeName: 'admincase.Annex',
            modelName: 'admincase.Annex',
            editWindowView: 'admincase.AnnexEditWindow',
            listeners: {
                getdata: function (asp, record) {
                    if (!record.get('Id')) {
                        record.set('AdministrativeCase', this.controller.params.documentId);
                    }
                }
            }
        },
        {
            /*
            * Аспект взаимодействия Таблицы приложений с формой редактирования
            */
            xtype: 'grideditwindowaspect',
            name: 'adminCaseDocAspect',
            gridSelector: 'admincasedocgrid',
            editFormSelector: '#admincasedoceditwindow',
            storeName: 'admincase.Doc',
            modelName: 'admincase.Doc',
            editWindowView: 'admincase.DocEditWindow',
            otherActions: function(actions) {
                actions['#admincasedoceditwindow [name=TypeAdminCaseDoc]'] = {
                    change: {
                        fn: function (cmp, newVal) {
                            this.hideControls(cmp.up('#admincasedoceditwindow'), newVal);
                        },
                        scope: this
                    }
                };
            },
            hideControls: function(panel, typeDoc) {
                var sflInspector = panel.down('[name=EntitiedInspector]'),
                    dfNeedTerm = panel.down('[name=NeedTerm]'),
                    dfRenewalTerm = panel.down('[name=RenewalTerm]'),
                    tfDocumentNumber = panel.down('[name=DocumentNumber]'),
                    tfDocumentDate = panel.down('[name=DocumentDate]'),
                    taDescriptionSet = panel.down('[name=DescriptionSet]');

                switch (typeDoc) {
                    case 10:
                        this.setHidden(sflInspector, true);
                        this.setHidden(dfNeedTerm, true);
                        this.setHidden(dfRenewalTerm, true);
                        this.setHidden(tfDocumentNumber, false);
                        this.setHidden(tfDocumentDate, false);
                        this.setHidden(taDescriptionSet, false);
                        break;
                    case 20:
                        this.setHidden(sflInspector, false);
                        this.setHidden(dfNeedTerm, true);
                        this.setHidden(dfRenewalTerm, false);
                        this.setHidden(tfDocumentNumber, true);
                        this.setHidden(tfDocumentDate, false);
                        this.setHidden(taDescriptionSet, true);
                        break;
                    case 30:
                        this.setHidden(sflInspector, true);
                        this.setHidden(dfNeedTerm, true);
                        this.setHidden(dfRenewalTerm, false);
                        this.setHidden(tfDocumentNumber, true);
                        this.setHidden(tfDocumentDate, true);
                        this.setHidden(taDescriptionSet, false);
                        break;
                }
            },
            setHidden: function(control, hide) {
                control.setDisabled(hide);
                control.setVisible(!hide);
                if (hide) {
                    control.setValue(null);
                }
            },

            listeners: {
                getdata: function (asp, record) {
                    if (!record.get('Id')) {
                        record.set('AdministrativeCase', this.controller.params.documentId);
                    }
                },
                aftersetformdata :  function(asp, record) {
                    asp.controller.params.adminDocId = record.getId();
                    asp.controller.getAspect('adminCaseDocPrintAspect').loadReportStore();
                },
                beforesetformdata: function(asp, record, panel) {
                    asp.hideControls(panel, record.get('TypeAdminCaseDoc'));
                }
            }
        },
        {
            /* 
            Аспект взаимодействия таблицы статьи закона с массовой формой выбора статей
            По нажатию на Добавить открывается форма выбора статей.
            По нажатию Применить на форме массовго выбора идет обработка выбранных строк в getdata
            И сохранение статей
            */
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'adminCaseArticleLawAspect',
            gridSelector: 'admincasearticlelawgrid',
            storeName: 'admincase.ArticleLaw',
            modelName: 'admincase.ArticleLaw',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#adminCaseArticleLawMultiSelectWindow',
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
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable:false }
            ],

            listeners: {
                getdata: function (asp, records) {

                    var recordIds = [];

                    records.each(function (rec) {
                        recordIds.push(rec.get('Id'));
                    });

                    if (recordIds[0] > 0) {
                        
                        asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                        B4.Ajax.request({
                            url: B4.Url.action('AddArticles', 'AdministrativeCaseArticleLaw'),
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
                        
                    } else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать статьи закона');
                        return false;
                    }
                    return true;
                }
            }
        },
        {
            /* 
            Аспект взаимодействия таблицы статьи закона с массовой формой выбора статей
            По нажатию на Добавить открывается форма выбора статей.
            По нажатию Применить на форме массовго выбора идет обработка выбранных строк в getdata
            И сохранение статей
            */
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'admincaseprovdocAspect',
            gridSelector: 'admincaseprovdocgrid',
            storeName: 'admincase.ProvidedDoc',
            modelName: 'admincase.ProvidedDoc',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#admincaseprovdocmultiselectwindow',
            storeSelect: 'dict.ProvidedDocGjiForSelect',
            storeSelected: 'dict.ProvidedDocGjiForSelected',
            titleSelectWindow: 'Выбор предоставляемых документов',
            titleGridSelect: 'Предоставляемые документы для отбора',
            titleGridSelected: 'Выбранные предоставляемые документы',
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Код', xtype: 'gridcolumn', dataIndex: 'Code', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }
            ],

            listeners: {
                getdata: function (asp, records) {

                    var recordIds = [];

                    records.each(function (rec) {
                        recordIds.push(rec.get('Id'));
                    });

                    if (recordIds[0] > 0) {

                        asp.controller.mask('Сохранение', B4.getBody().getActiveTab());
                        B4.Ajax.request({
                            url: B4.Url.action('AddProvidedDocs', 'AdministrativeCaseProvidedDoc'),
                            method: 'POST',
                            params: {
                                objectIds: Ext.encode(recordIds),
                                documentId: asp.controller.params.documentId
                            }
                        }).next(function (response) {
                            asp.controller.unmask();
                            asp.controller.getStore(asp.storeName).load();
                            return true;
                        }).error(function () {
                            asp.controller.unmask();
                        });

                    } else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать предоставляемые документы');
                        return false;
                    }
                    return true;
                }
            }
        },
        {
            /*
            Аспект взаимодействия Таблицы требований с формой редактирования
            */
            xtype: 'grideditwindowaspect',
            name: 'admincaseRequirementAspect',
            gridSelector: 'admincaserequirementgrid',
            editFormSelector: 'admincaserequirementeditwin',
            modelName: 'requirement.Requirement',
            editWindowView: 'admincase.RequirementEditWindow',
            otherActions: function (actions) {
                var me = this;
                actions[me.editFormSelector + ' #btnCreateProtocol'] = {
                    click: {
                        fn: function () {
                            me.createProtocol();
                        }
                    }
                };
            },
            onSaveSuccess: function (asp, record) {
                if (record && record.getId()) {
                    var model = this.getModel(record);

                    model.load(record.getId(), {
                        success: function (rec) {
                            asp.setFormData(rec);
                        },
                        scope: this
                    });
                }
            },
            createProtocol: function () {
                var me = this,
                    form = me.getForm(),
                    rec;

                form.getForm().updateRecord();
                rec = form.getRecord();

                me.controller.mask('Формирование протокола', me.controller.getMainComponent());
                B4.Ajax.request({
                    url: B4.Url.action('CreateProtocol', 'RequirementDocument'),
                    timeout: 9999999,
                    method: 'POST',
                    params: {
                        requirementId: rec.getId()
                    }
                }).next(function (res) {
                    form.close();

                    var data = Ext.decode(res.responseText);

                    // Обновляем дерево меню
                    var tree = Ext.ComponentQuery.query(me.controller.params.treeMenuSelector)[0];
                    if (tree) {
                        tree.getStore().load();
                    }

                    var docParams = {};
                    docParams.inspectionId = data.inspectionId;
                    docParams.documentId = data.documentId;
                    docParams.containerSelector = me.controller.params.containerSelector;
                    docParams.treeMenuSelector = me.controller.params.treeMenuSelector;

                    // Для того чтобы маска снялась только после показа новой карточки, формирую функцию обратного вызова
                    if (!me.controller.hideMask) {
                        me.controller.hideMask = function () { me.controller.unmask(); };
                    }

                    me.controller.loadController('B4.controller.ProtocolGji', docParams, me.controller.params.containerSelector, null, me.controller.hideMask);

                    me.controller.unmask();

                    return true;
                }).error(function (e) {
                    me.controller.unmask();
                    Ext.Msg.alert('Ошибка формирования документа!', e.message || e);
                });
            },
            listeners: {
                getdata: function (asp, record) {
                    if (!record.get('Id')) {
                        record.set('Document', this.controller.params.documentId);
                    }
                },
                aftersetformdata: function (asp, record) {
                    var form = asp.getForm(),
                        requirementStore = form.down('[name=TypeRequirement]').getStore(),
                        trfArticles = form.down('gkhtriggerfield[name=ArticleLaw]');

                    requirementStore.clearFilter(true);
                    requirementStore.filter('docId', this.controller.params.documentId);

                    //Передаем аспекту смены статуса необходимые параметры
                    this.controller.getAspect('admincaseRequirementStateButtonAspect').setStateData(record.get('Id'), record.get('State'));
                    this.controller.getAspect('admincaseRequirementPrintAspect').loadReportStore();
                    
                    asp.controller.params.reqId = record.getId();

                    trfArticles.setValue('');
                    trfArticles.setDisabled(true);
                    if (record.getId()) {
                        asp.controller.mask('Загрузка', B4.getBody().getActiveTab());
                        trfArticles.setDisabled(false);
                        
                        B4.Ajax.request(B4.Url.action('GetInfo', 'Requirement', {
                            reqId: record.getId()
                        })).next(function (response) {
                            
                            //десериализуем полученную строку
                            var obj = Ext.JSON.decode(response.responseText);

                            trfArticles.setValue(obj.data.artIds);
                            trfArticles.updateDisplayedText(obj.data.artNames);
                            asp.controller.unmask();
                        }).error(function () {
                            asp.controller.unmask();
                        });
                    }
                }
            }
        },
        {
            xtype: 'gkhbuttonprintaspect',
            name: 'admincaseRequirementPrintAspect',
            buttonSelector: 'admincaserequirementeditwin #btnPrint',
            codeForm: 'RequirementGji',
            getUserParams: function () {
                var param = { documentId: this.controller.params.reqId };

                this.params.userParams = Ext.JSON.encode(param);
            }
        },
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'admincaserequirementarticlelawMultiSelectWindowAspect',
            fieldSelector: 'admincaserequirementeditwin gkhtriggerfield[name=ArticleLaw]',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#admincaserequirementarticlelawSelectWindow',
            storeSelect: 'dict.ArticleLawGjiForSelect',
            storeSelected: 'dict.ArticleLawGjiForSelected',
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Код', xtype: 'gridcolumn', dataIndex: 'Code', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }
            ],
            titleSelectWindow: 'Выбор статей закона',
            titleGridSelect: 'Статьи закона для отбора',
            titleGridSelected: 'Выбранные статьи закона',
            listeners: {
                getdata: function (asp, records) {
                    var objectIds = [];

                    Ext.Array.each(records.items, function (item) {
                        objectIds.push(item.get('Id'));
                    });

                    asp.controller.mask('Сохранение', B4.getBody().getActiveTab());

                    B4.Ajax.request(B4.Url.action('AddArticles', 'RequirementArticleLaw', {
                        objectIds: Ext.encode(objectIds),
                        reqId: asp.controller.params.reqId
                    })).next(function () {
                        asp.controller.unmask();
                        Ext.Msg.alert('Сохранение!', 'Статьи закона сохранены успешно');
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
            Аспект взаимодействия таблицы нарушения с массовой формой выбора нарушений
            Тут есть 2 варианта
            */
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'admincaseViolationAspect',
            gridSelector: '#admincaseViolationGrid',
            storeName: 'admincase.Violation',
            modelName: 'admincase.Violation',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#dadmincaseViolationMultiSelectWindow',
            titleSelectWindow: 'Выбор нарушений',
            titleGridSelect: 'Нарушения для отбора',
            titleGridSelected: 'Выбранные нарушения',
            storeSelect: 'dict.ViolationGjiForSelect',
            storeSelected: 'dict.ViolationGjiForSelected',
            columnsGridSelect: [
                { header: 'Нарушение', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Нарушение', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
            ],
            otherActions: function (actions) {
                var me = this;
                actions['#admincaseViolationGrid #updateButton'] = {
                    click: {
                        fn: function () {
                            me.controller.getStore(me.storeName).load();
                        }
                    }
                };
            },
            listeners: {
                getdata: function (asp, records) {
                    var recordIds = [];

                    records.each(function (rec) {
                        recordIds.push(rec.getId());
                    });

                    if (recordIds[0] > 0) {
                        asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                        B4.Ajax.request({
                            url: B4.Url.action('AddViolations', 'AdministrativeCaseViol'),
                            method: 'POST',
                            params: {
                                violIds: Ext.encode(recordIds),
                                documentId: asp.controller.params.documentId
                            }
                        }).next(function () {
                            asp.controller.unmask();
                            asp.controller.getStore(asp.storeName).load();
                            return true;
                        }).error(function (e) {
                            asp.controller.unmask();
                            Ext.Msg.alert('Ошибка!', e.message || e);
                        });
                    } else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать нарушения');
                        return false;
                    }
                    return true;
                }
            }
        },
        {
            /* 
            Аспект взаимодействия кнопки создания Предписания и массовой формы выбора Нарушений (для распоряжения на проверку предписания)
            */
            xtype: 'gkhbuttonmultiselectwindowaspect',
            name: 'adminCaseToPrescriptionByViolationsAspect',
            buttonSelector: '#adminCaseEditPanel [ruleId=AdminCaseToPrescriptionlRule]',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#adminCaseToPrescriptionByViolationsSelectWindow',
            storeSelectSelector: '#admincaselViolationsForSelect',
            storeSelect: 'admincase.ViolationForSelect',
            storeSelected: 'admincase.ViolationForSelected',
            columnsGridSelect: [
                {
                    header: 'Муниципальное образование',
                    xtype: 'gridcolumn',
                    dataIndex: 'Municipality',
                    flex: 1,
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
                { header: 'Адрес', xtype: 'gridcolumn', dataIndex: 'RealityObject', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Нарушение', xtype: 'gridcolumn', dataIndex: 'ViolationGji', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Нарушение', xtype: 'gridcolumn', dataIndex: 'ViolationGji', flex: 1, filter: { xtype: 'textfield' } }
            ],
            titleSelectWindow: 'Выбор нарушения',
            titleGridSelect: 'Нарушения для отбора',
            titleGridSelected: 'Выбранные нарушения',

            onBeforeLoad: function (store, operation) {
                if (this.controller.params && this.controller.params.documentId > 0)
                    operation.params.documentId = this.controller.params.documentId;
            },

            listeners: {
                getdata: function (asp, records) {
                    var me = this,
                        recordIds = [],
                        btn = Ext.ComponentQuery.query(me.buttonSelector)[0],
                        creationAspect,
                        params;

                    records.each(function (rec, index) { recordIds.push(rec.get('InspectionViolationId')); });

                    if (recordIds[0] > 0) {
                        creationAspect = asp.controller.getAspect('adminCaseCreateButtonAspect');

                        params = creationAspect.getParams(btn);
                        params.violationIds = recordIds;

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
            /* 
            Аспект взаимодействия кнопки создания Предписания и массовой формы выбора Нарушений (для распоряжения на проверку предписания)
            */
            xtype: 'gkhbuttonmultiselectwindowaspect',
            name: 'adminCaseToProtocolByViolationsAspect',
            buttonSelector: '#adminCaseEditPanel [ruleId=AdminCaseToProtocolRule]',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#adminCaseToProtocolByViolationsSelectWindow',
            storeSelectSelector: '#admincaselViolationsForSelect',
            storeSelect: 'admincase.ViolationForSelect',
            storeSelected: 'admincase.ViolationForSelected',
            columnsGridSelect: [
                {
                    header: 'Муниципальное образование',
                    xtype: 'gridcolumn',
                    dataIndex: 'Municipality',
                    flex: 1,
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
                { header: 'Адрес', xtype: 'gridcolumn', dataIndex: 'RealityObject', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Нарушение', xtype: 'gridcolumn', dataIndex: 'ViolationGji', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Нарушение', xtype: 'gridcolumn', dataIndex: 'ViolationGji', flex: 1, filter: { xtype: 'textfield' } }
            ],
            titleSelectWindow: 'Выбор нарушения',
            titleGridSelect: 'Нарушения для отбора',
            titleGridSelected: 'Выбранные нарушения',

            onBeforeLoad: function (store, operation) {
                if (this.controller.params && this.controller.params.documentId > 0)
                    operation.params.documentId = this.controller.params.documentId;
            },

            listeners: {
                getdata: function (asp, records) {
                    var me = this,
                        recordIds = [],
                        btn = Ext.ComponentQuery.query(me.buttonSelector)[0],
                        creationAspect,
                        params;

                    records.each(function (rec, index) { recordIds.push(rec.get('InspectionViolationId')); });

                    if (recordIds[0] > 0) {
                        creationAspect = asp.controller.getAspect('adminCaseCreateButtonAspect');

                        params = creationAspect.getParams(btn);
                        params.violationIds = recordIds;

                        creationAspect.createDocument(params);
                    } else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать нарушения');
                        return false;
                    }
                    return true;
                }
            }
        }
    ],

    init: function () {
        var me = this;
        
        me.getStore('admincase.Violation').on('beforeload', me.onBeforeLoad, me);
        me.getStore('admincase.Annex').on('beforeload', me.onBeforeLoad, me);
        me.getStore('admincase.ArticleLaw').on('beforeload', me.onBeforeLoad, me);
        me.getStore('admincase.ProvidedDoc').on('beforeload', me.onBeforeLoad, me);
        me.getStore('admincase.Doc').on('beforeload', me.onBeforeLoad, me);

        me.callParent(arguments);
    },

    onLaunch: function () {
        var me = this,
            requirementgrid;
        
        if (me.params) {
            me.getAspect('adminCaseEditPanelAspect').setData(me.params.documentId);
            
            requirementgrid = me.getMainComponent().down('admincaserequirementgrid');
            requirementgrid.getStore().on('beforeload', me.onBeforeLoad, me);
        }
    },

    onBeforeLoad: function (store, operation) {
        var me = this;
        
        if (me.params && me.params.documentId > 0)
            operation.params.documentId = me.params.documentId;
    }
});
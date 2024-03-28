Ext.define('B4.controller.Presentation', {
    extend: 'B4.base.Controller',
    params: null,
    objectId: 0,
    requires: [
        'B4.aspects.StateButton',
        'B4.aspects.GridEditWindow',
        'B4.aspects.GjiDocument',
        'B4.aspects.GkhButtonPrintAspect',
        'B4.aspects.permission.Presentation',
        'B4.aspects.GkhBlobText',
        'B4.Ajax',
        'B4.Url'
    ],

    models: [
        'Presentation',
        'presentation.Annex'
    ],

    stores: [
        'Presentation',
        'presentation.Annex',
        'dict.ExecutantDocGji',
        'dict.Inspector'
    ],

    views: [
        'presentation.AnnexEditWindow',
        'presentation.AnnexGrid',
        'presentation.EditPanel'
    ],
    
    mixins: {
        mask: 'B4.mixins.MaskBody'
    },
    
    mainView: 'presentation.EditPanel',
    mainViewSelector: '#presentationEditPanel',

    aspects: [
        {
            /*
            * Вешаем аспект смены статуса в карточке редактирования
            */
            xtype: 'statebuttonaspect',
            name: 'presentationStateButtonAspect',
            stateButtonSelector: '#presentationEditPanel #btnState',
            listeners: {
                transfersuccess: function (asp, entityId) {
                    //После успешной смены статуса запрашиваем по Id актуальные данные записи
                    //и обновляем панель
                    var editPanelAspect = asp.controller.getAspect('presentationEditPanelAspect');

                    editPanelAspect.setData(entityId);
                    editPanelAspect.reloadTreePanel();
                }
            }
        },
        {
            xtype: 'presentationperm',
            editFormAspectName: 'presentationEditPanelAspect'
        },
        {
            xtype: 'gkhbuttonprintaspect',
            name: 'presentationPrintAspect',
            buttonSelector: '#presentationEditPanel #btnPrint',
            codeForm: 'Presentation',
            getUserParams: function (reportId) {
                var param = { DocumentId: this.controller.params.documentId };

                this.params.userParams = Ext.JSON.encode(param);
            }
        },
        {   /*
            Апект для основной панели постановления
            */
            xtype: 'gjidocumentaspect',
            name: 'presentationEditPanelAspect',
            editPanelSelector: '#presentationEditPanel',
            modelName: 'Presentation',

            otherActions: function (actions) {
                actions[this.editPanelSelector + ' #cbExecutant'] = { 'change': { fn: this.onChangeTypeExecutant, scope: this} };
                actions[this.editPanelSelector + ' #sfContragent'] = { 'beforeload': { fn: this.onBeforeLoadContragent, scope: this } };
                actions[this.editPanelSelector + ' #sfPresentationOfficial'] = { 'beforeload': { fn: this.onBeforeLoadOfficial, scope: this} };
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
                    panel.setTitle('Представление ' + rec.get('DocumentNumber'));
                else
                    panel.setTitle('Представление');

                panel.down('#presentationTabPanel').setActiveTab(0);

                //Делаем запросы на получение документа основания
                var me = this;
                me.controller.mask('Загрузка', me.controller.getMainComponent());
                B4.Ajax.request(B4.Url.action('GetInfo', 'Presentation', {
                    documentId: asp.controller.params.documentId
                })).next(function (response) {
                    me.controller.unmask();
                    //десериализуем полученную строку
                    var obj = Ext.JSON.decode(response.responseText);

                    var fieldBaseName = panel.down('#presentationBaseNameTextField');
                    fieldBaseName.setValue(obj.baseName);
                    me.disableButtons(false);
                }).error(function () {
                    me.controller.unmask();
                });

                //Обновляем отчеты
                this.controller.getAspect('presentationPrintAspect').loadReportStore();

                //Передаем аспекту смены статуса необходимые параметры
                this.controller.getAspect('presentationStateButtonAspect').setStateData(rec.get('Id'), rec.get('State'));
                
                this.setTypeExecutantPermission(rec.get('Executant'));

                this.fireEvent('aftersetformdata', asp, rec, panel.getForm());
            },
            setTypeExecutantPermission: function (typeExec) {
                var me = this;
                var panel = this.getPanel();
                var permissions = [
                    'GkhGji.DocumentsGji.Presentation.Field.Contragent_Edit',
                    'GkhGji.DocumentsGji.Presentation.Field.PhysicalPerson_Edit',
                    'GkhGji.DocumentsGji.Presentation.Field.PhysicalPersonInfo_Edit'
                ];

                if (typeExec) {
                    me.controller.mask('Загрузка', me.controller.getMainComponent());
                    B4.Ajax.request({
                        method: 'POST',
                        url: B4.Url.action('GetObjectSpecificPermissions', 'Permission', {
                            permissions: Ext.encode(permissions),
                            ids: Ext.encode([me.controller.params.documentId])
                        })
                    }).next(function (response) {
                        me.controller.unmask();
                        var perm = Ext.decode(response.responseText)[0];
                        switch (typeExec.Code) {
                            //Активны все поля                                                                         
                            case "1":
                            case "5":
                            case "10":
                            case "12":
                            case "13":
                            case "16":
                            case "19":
                                panel.down('#sfContragent').setDisabled(!perm[0]);

                                panel.down('#tfPhysPerson').setDisabled(!perm[1]);
                                panel.down('#taPhysPersonInfo').setDisabled(!perm[2]);

                                panel.down('#sfContragent').allowBlank = false;
                                break;

                                //Активно поле Юр.лицо                                                                         
                            case "0":
                            case "4":
                            case "8":
                            case "9":
                            case "11":
                            case "15":
                            case "18":
                                panel.down('#sfContragent').setDisabled(!perm[0]);

                                panel.down('#tfPhysPerson').setDisabled(true);
                                panel.down('#taPhysPersonInfo').setDisabled(true);

                                panel.down('#sfContragent').allowBlank = false;
                                break;

                                //Активны поля Физ.лица                                                                         
                            case "6":
                            case "7":
                            case "14":
                                panel.down('#sfContragent').setDisabled(true);

                                panel.down('#tfPhysPerson').setDisabled(!perm[1]);
                                panel.down('#taPhysPersonInfo').setDisabled(!perm[2]);

                                panel.down('#sfContragent').allowBlank = true;
                                break;
                        }
                    }).error(function () {
                        me.controller.unmask();
                    });
                }
            },
            onChangeTypeExecutant: function (field, value, oldValue) {
                var data = field.getRecord(value);

                var contragentField = field.up(this.editPanelSelector).down('#sfContragent');

                if (!Ext.isEmpty(contragentField) && !Ext.isEmpty(oldValue)) {
                    contragentField.setValue(null);
                }
                
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

            onBeforeLoadOfficial: function (field, options, store) {
                options = options || {};
                options.params = options.params || {};
                options.params.headOnly = true;

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
            }
        },
        {
            /*
            Аспект взаимодействия Таблицы приложений с формой редактирования
            */
            xtype: 'grideditwindowaspect',
            name: 'presentationAnnexAspect',
            gridSelector: '#presentationAnnexGrid',
            editFormSelector: '#presentationAnnexEditWindow',
            storeName: 'presentation.Annex',
            modelName: 'presentation.Annex',
            editWindowView: 'presentation.AnnexEditWindow',
            listeners: {
                getdata: function (asp, record) {
                    if (!record.get('Id')) {
                        record.set('Presentation', this.controller.params.documentId);
                    }
                }
            }
        },
        {
            xtype: 'gkhblobtextaspect',
            name: 'presentationBlobDescriptionAspect',
            fieldSelector: '[name="DescriptionSet"]',
            editPanelAspectName: 'presentationEditPanelAspect',
            controllerName: 'Presentation',
            valueFieldName: 'DescriptionSet',
            previewLength: 2000,
            autoSavePreview: true,
            previewField: 'DescriptionSet',
            getParentRecordId: function () {
                return this.editPanelAspect.getRecord().getId();
            }
        }
    ],

    init: function () {
        this.getStore('presentation.Annex').on('beforeload', this.onBeforeLoad, this);

        this.callParent(arguments);
    },

    onLaunch: function () {
        if (this.params) {
            this.getAspect('presentationEditPanelAspect').setData(this.params.documentId);

            //Обновляем стор приложений
            this.getStore('presentation.Annex').load();
        }
    },

    onBeforeLoad: function (store, operation) {
        if (this.params && this.params.documentId > 0)
            operation.params.documentId = this.params.documentId;
    }
});
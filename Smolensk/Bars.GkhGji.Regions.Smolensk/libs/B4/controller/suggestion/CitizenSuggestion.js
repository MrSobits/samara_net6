Ext.define('B4.controller.suggestion.CitizenSuggestion', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.GridEditCopyWindow',
        'B4.aspects.permission.GkhPermissionAspect',
        'B4.aspects.permission.GkhStatePermissionAspect',
        'B4.aspects.StateGridWindowColumn',
        'B4.store.suggestion.CitizenSuggestion',
        'B4.aspects.GkhButtonPrintAspect',
        'B4.aspects.StateContextButton',
        'B4.aspects.StateContextMenu'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    stores: [
        'suggestion.CitizenSuggestion',
        'suggestion.ProblemFiles',
        'suggestion.AnswerFiles',
        'suggestion.Comment',
        'suggestion.CommentQuestionFiles',
        'suggestion.CommentAnswerFiles',
        'suggestion.History'
    ],

    models: [
        'suggestion.CitizenSuggestion',
        'suggestion.File',
        'suggestion.CommentFile',
        'suggestion.History'
    ],

    views: [
        'suggestion.citizensuggestion.Grid',
        'suggestion.citizensuggestion.EditWindow',
        'suggestion.citizensuggestion.CommentGrid',
        'suggestion.citizensuggestion.FileGrid',
        'suggestion.citizensuggestion.CommentEditWindow'
    ],

    mainView: 'suggestion.citizensuggestion.Grid',
    mainViewSelector: 'citizensuggestionpanel',

    refs: [
        { ref: 'mainView', selector: 'citizensuggestionpanel' },
        { ref: 'editWindow', selector: 'citizensuggestionwindow' }
    ],

    aspects: [
        {
            /*
            Вешаем аспект смены статуса
            */
            xtype: 'b4_state_contextmenu',
            name: 'citizensuggestionStateTransferAspect',
            gridSelector: 'citizensuggestionpanel',
            menuSelector: 'citizensuggestionpaneltateMenu',
            stateType: 'gkh_emergency_object'
        },
        {
            /*
            Вешаем аспект смены статуса
            */
            xtype: 'statecontextbuttonaspect',
            name: 'citizensuggestionStateWindowAspect',
            stateButtonSelector: 'citizensuggestionwindow button[name=btnState]',
            listeners: {
                transfersuccess: function (me, entityId, newState) {
                    //Если статус изменен успешно, то проставляем новый статус
                    me.setStateData(entityId, newState);
                    var editWindowAspect = me.controller.getAspect('citizensuggestionGridWindowAspect');
                    //и перезагружаем грид, т.к. в гриде нужно обновить столбец Статус
                    editWindowAspect.updateGrid();
                    var model = me.controller.getModel(editWindowAspect.modelName);
                    model.load(entityId, {
                        success: function (rec) {
                            editWindowAspect.setFormData(rec);
                        }
                    });
                }
            }
        },
        {
            xtype: 'gkhbuttonprintaspect',
            name: 'citizenSuggestionPrintAspect',
            buttonSelector: 'citizensuggestionwindow #btnPrint',
            codeForm: 'CitizenSuggestion',
            getUserParams: function (reportId) {
                this.params.userParams = Ext.JSON.encode({ CitizenSuggestionId: this.controller.citSuggId });
            },
            onLoadReportStore: function (store) {
                var me = this,
                    btn = me.componentQuery(this.buttonSelector);

                if (btn) {
                    btn.menu.removeAll();

                    store.each(function (rec) {
                        btn.menu.add({
                            xtype: 'menuitem',
                            text: rec.get(me.displayField),
                            textAlign: 'left',
                            actionName: rec.data.Id,
                            iconCls: 'icon-report'
                        });
                    });

                    btn.menu.add({
                        xtype: 'menuitem',
                        text: 'Обращения граждан (с портала)',
                        textAlign: 'left',
                        actionName: 'CitizenSuggestionPortalReport',
                        iconCls: 'icon-report'
                    });
                }
            },
            printReport: function(itemMenu) {
                var me = this;
                if (itemMenu.actionName === 'CitizenSuggestionPortalReport') {
                    var citizenSuggestionId = me.controller.citSuggId;
                    if (citizenSuggestionId) {
                        var params = {
                            citizenSuggestionId: citizenSuggestionId
                        };

                        Ext.DomHelper.append(document.body, {
                            tag: 'iframe',
                            id: 'downloadIframe',
                            frameBorder: 0,
                            width: 0,
                            height: 0,
                            css: 'display:none;visibility:hidden;height:0px;',
                            src: B4.Url.action('PrintSuggestionPortalReport', 'CitizenSuggestion', params)
                        });
                    }
                } else {
                    var frame = Ext.get('downloadIframe');
                    if (frame != null) {
                        Ext.destroy(frame);
                    }

                    me.getUserParams(itemMenu.actionName);
                    Ext.apply(me.params, { reportId: itemMenu.actionName });

                    var urlParams = Ext.urlEncode(me.params);

                    var newUrl = Ext.urlAppend(Ext.String.format('/{0}/{1}/?{2}', me.printController, me.printAction, urlParams), '_dc=' + (new Date().getTime()));
                    newUrl = B4.Url.action(newUrl);
                    if (me.openInNewWindow()) {
                        window.open(newUrl);
                    } else {
                        Ext.DomHelper.append(document.body, {
                            tag: 'iframe',
                            id: 'downloadIframe',
                            frameBorder: 0,
                            width: 0,
                            height: 0,
                            css: 'display:none;visibility:hidden;height:0px;',
                            src: newUrl
                        });
                    }
                }
            }
        },
        {
            xtype: 'grideditcopywindowaspect',
            name: 'citizensuggestionGridWindowAspect',
            gridSelector: 'citizensuggestionpanel',
            editFormSelector: 'citizensuggestionwindow',
            modelName: 'suggestion.CitizenSuggestion',
            editWindowView: 'suggestion.citizensuggestion.EditWindow',
            copyFields: ['ApplicantFio', 'ApplicantAddress', 'ApplicantPhone', 'ApplicantEmail'],
            otherActions: function (actions) {
                actions['citizensuggestionwindow b4selectfield[name="Executor"]'] = { 'beforeload': { fn: this.onExecutorBeforeLoad, scope: this } };
                actions['citizensuggestionwindow b4selectfield[name="Rubric"]'] = { 'change': { fn: this.onRubricChange, scope: this } };
                actions['citizensuggestionwindow combobox[name="ExecutorType"]'] = { 'change': { fn: this.onExecutorTypeChange, scope: this } };
                actions['citizensuggestionwindow button[name=btnExportToGji]'] = { 'click': { fn: this.onExportToGjiClick, scope: this } };
                actions['citizensuggestionwindow button[name=btnGetArchive]'] = { 'click': { fn: this.onGetArchiveClick, scope: this } };
                actions['citizensuggestionwindow button[name=btnSendToEmail]'] = { 'click': { fn: this.onBtnSendToEmailClick, scope: this } };
            },
            onSaveSuccess: function (asp, record) {
                asp.controller.citSuggId = record.getId();
            },
            onExecutorTypeChange: function (cbx, newValue) {
                this.controller.executorType = newValue;
                //cbx.up().down('b4selectfield[name="Executor"]').setValue(null);
            },
            onRubricChange: function (f, newValue) {
                if (newValue) {
                    f.up().down('combobox[name="ExecutorType"]').setValue(newValue.FirstExecutorType);
                }
            },
            onExecutorBeforeLoad: function (f, opts, store) {
                Ext.apply(store.getProxy().extraParams, {
                    executorType: this.controller.executorType
                });
            },
            onExportToGjiClick: function(btn) {
                var asp = this,
                    rec = asp.getForm().getRecord();

                asp.controller.mask('Экспорт в реестр ГЖИ', asp.controller.getMainComponent());
                B4.Ajax.request({
                    method: 'POST',
                    url: B4.Url.action('AddCitizenSuggestion', 'AppealCits'),
                    params: {
                        Id: rec.getId(),
                        ApplicantFio: rec.get('ApplicantFio'),
                        ApplicantAddress: rec.get('ApplicantAddress'),
                        ApplicantPhone: rec.get('ApplicantPhone'),
                        ApplicantEmail: rec.get('ApplicantEmail'),
                        Description: rec.get('Description')
                    }
                }).next(function () {
                    B4.QuickMsg.msg('Экспорт в реестр ГЖИ', 'Данные успешно экспортированы', 'success');
                    btn.setDisabled(true);
                    asp.controller.unmask();
                }).error(function (resp) {
                    B4.QuickMsg.msg('Экспорт в реестр ГЖИ', resp.message || 'При экспорте данных возникла ошибка', 'error');
                    asp.controller.unmask();
                });

            },
            onGetArchiveClick: function(btn) {
                var asp = this,
                    rec = asp.getForm().getRecord();

                asp.controller.mask('Создание архива', asp.controller.getMainComponent());
                B4.Ajax.request({
                    //method: 'POST',
                    url: B4.Url.action('GetArchive', 'CitizenSuggestion'),
                    params: {
                        Id: rec.getId(),
                        citizenSuggestionId: this.controller.citSuggId
                    }
                }).next(function (resp) {
                    var tryDecoded;

                    asp.controller.unmask();
                    try {
                        tryDecoded = Ext.JSON.decode(resp.responseText);
                    } catch (e) {
                        tryDecoded = {};
                    }

                    var id = resp.data ? resp.data : tryDecoded.data;

                    if (id > 0) {
                        Ext.DomHelper.append(document.body, {
                            tag: 'iframe',
                            id: 'downloadIframe',
                            frameBorder: 0,
                            width: 0,
                            height: 0,
                            css: 'display:none;visibility:hidden;height:0px;',
                            src: B4.Url.action('Download', 'FileUpload', { Id: id })
                        });

                        //me.fireEvent('onprintsucess', me);
                    }
                }).error(function (err) {
                    asp.controller.unmask();
                    Ext.Msg.alert('Ошибка', err.message || err.message || err);
                });
            },
            onBtnSendToEmailClick: function (btn,asp,record) {

                var asp = this;
                asp.controller.mask('Отправка e-mail...', asp.controller.getMainComponent());
                var recId = btn.up('citizensuggestionwindow').getRecord().get('Id');

                B4.Ajax.request({
                    url: B4.Url.action('SendAnswerEmail', 'CitizenSuggestion'),
                    params: {
                        citsugId: recId
                    }
                }).next(function (resp) {
                    var tryDecoded;

                    asp.controller.unmask();
                    try {
                        tryDecoded = Ext.JSON.decode(resp.responseText);
                    } catch (e) {
                        tryDecoded = {};
                    }

                    Ext.Msg.alert('E-mail отправлено', resp.message ? resp.message : tryDecoded.message);
                    
                }).error(function (err) {
                    asp.controller.unmask();
                    Ext.Msg.alert('Ошибка', err.message || err.message || err);
                });
            },
            deleteRecord: function (record) {
                var me = this;

                if (record.getId()) {
                    me.controller.getAspect('deleteSuggestionStatePerm').loadPermissions(record)
                        .next(function (response) {
                            var grants = Ext.decode(response.responseText);

                            if (grants && grants[0]) {
                                grants = grants[0];
                            }

                            // проверяем пермишшен колонки удаления
                            if (grants[0] == 0) {
                                Ext.Msg.alert('Сообщение', 'Удаление на данном статусе запрещено');
                            } else {
                                Ext.Msg.confirm('Удаление записи!', 'Вы действительно хотите удалить запись?', function (result) {
                                    if (result == 'yes') {
                                        var model = me.getModel(record);

                                        var rec = new model({ Id: record.getId() });
                                        me.mask('Удаление', me.controller.getMainComponent());
                                        rec.destroy()
                                            .next(function () {
                                                me.fireEvent('deletesuccess', me);
                                                me.updateGrid();
                                                me.unmask();
                                            }, me)
                                            .error(function (result) {
                                                Ext.Msg.alert('Ошибка удаления!', Ext.isString(result.responseData) ? result.responseData : result.responseData.message);
                                                me.unmask();
                                            }, me);
                                    }
                                }, me);
                            }

                        }, this);
                }
            },
            listeners: {
                beforesave: function (asp, rec) {
                    var window = Ext.ComponentQuery.query('citizensuggestionwindow')[0],
                        executorType = window.down('combobox[name="ExecutorType"]').value,
                        executor = rec.get('Executor');

                    rec.set('ExecutorManagingOrganization', null);
                    rec.set('ExecutorMunicipality', null);
                    rec.set('ExecutorZonalInspection', null);
                    rec.set('ExecutorCrFund', null);

                    switch (executorType) {
                        case 10: rec.set('ExecutorManagingOrganization', executor); break;
                        case 20: rec.set('ExecutorMunicipality', executor); break;
                        case 30: rec.set('ExecutorZonalInspection', executor); break;
                        case 40: rec.set('ExecutorCrFund', executor); break;
                    }

                    return true;
                },
                aftersetformdata: function (asp, record) {
                    if (record.getId() === 0) {
                        asp.getForm().down('citsugcommentgrid b4addbutton').setDisabled(true);
                    } else {
                        asp.getForm().down('citsugcommentgrid b4addbutton').setDisabled(false);

                        B4.Ajax.request({
                            method: 'POST',
                            url: B4.Url.action('CheckSuggestionExists', 'AppealCits'),
                            params: {
                                Id: record.getId()
                            }
                        }).next(function(resp) {
                            var exists = Ext.JSON.decode(resp.responseText);

                            asp.getForm().down('button[name=btnExportToGji]').setDisabled(exists);
                        });
                    }

                    this.controller.getAspect('citizensuggestionStateWindowAspect').setStateData(record.get('Id'), record.get('State'));
                    var commentStore = asp.getForm().down('citsugcommentgrid').getStore(),
                        problemFilesStore = asp.getForm().down('citsugfilegrid[type="Problem"]').getStore(),
                        answerFilesStore = asp.getForm().down('citsugfilegrid[type="Answer"]').getStore();

                    commentStore.clearFilter(true);
                    commentStore.filter([
                        { property: "citSuggId", value: record.getId() }
                    ]);

                    problemFilesStore.clearFilter(true);
                    problemFilesStore.filter([
                        { property: "citSuggId", value: record.getId() },
                        { property: "isAnswer", value: false }
                    ]);

                    answerFilesStore.clearFilter(true);
                    answerFilesStore.filter([
                        { property: "citSuggId", value: record.getId() },
                        { property: "isAnswer", value: true }
                    ]);

                    //загружаем стор для кнопки печати
                    asp.controller.getAspect('citizenSuggestionPrintAspect').loadReportStore();
                    asp.controller.citSuggId = record.getId();
                    
                    var form = asp.getForm();

                    var email = form.down('#answerEmailAddress').value;
                    var checkEmail = Ext.data.validations.email(null, email);
                    var answerText = form.down('#answerText').value;                                                           
                    asp.getForm().down('button[name=btnSendToEmail]').setDisabled(!(checkEmail && answerText));
                   
                }
            }
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'citsugCommentGridWindowAspect',
            gridSelector: 'citsugcommentgrid',
            editFormSelector: 'citsugcommentwindow',
            modelName: 'suggestion.Comment',
            editWindowView: 'suggestion.citizensuggestion.CommentEditWindow',
            onSaveSuccess: function (asp, record) {
                asp.controller.citSuggCommentId = record.getId();
            },
            listeners: {
                beforesave: function (asp, rec) {
                    rec.set('CitizenSuggestion', asp.controller.citSuggId);
                    return true;
                },
                aftersetformdata: function (asp, record) {
                        var commentFilesStore = asp.getForm().down('citsugfilegrid[type="CommentQuestion"]').getStore(),
                            answerFilesStore = asp.getForm().down('citsugfilegrid[type="CommentAnswer"]').getStore();

                        commentFilesStore.clearFilter(true);
                        commentFilesStore.filter([
                            { property: "citSuggCommentId", value: record.getId() },
                            { property: "isAnswer", value: false }
                        ]);

                        answerFilesStore.clearFilter(true);
                        answerFilesStore.filter([
                            { property: "citSuggCommentId", value: record.getId() },
                            { property: "isAnswer", value: true }
                        ]);
                }
            }
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'citsugProblemFilesGridWindowAspect',
            gridSelector: 'citsugfilegrid[type="Problem"]',
            storeName: 'suggestion.ProblemFiles',
            modelName: 'suggestion.File'
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'citsugAnswerFilesGridWindowAspect',
            gridSelector: 'citsugfilegrid[type="Answer"]',
            storeName: 'suggestion.AnswerFiles',
            modelName: 'suggestion.File'
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'citsugCommentQuesFilesGridWindowAspect',
            gridSelector: 'citsugfilegrid[type="CommentQuestion"]',
            storeName: 'suggestion.CommentQuestionFiles',
            modelName: 'suggestion.CommentFile'
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'citsugCommentAnswerFilesGridWindowAspect',
            gridSelector: 'citsugfilegrid[type="CommentAnswer"]',
            storeName: 'suggestion.CommentAnswerFiles',
            modelName: 'suggestion.CommentFile'
        },
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                { name: 'Gkh.Dictionaries.Suggestion.CitizenSuggestion.Create', applyTo: 'b4addbutton', selector: 'citizensuggestionpanel' },
                {
                    name: 'Gkh.Dictionaries.Suggestion.CitizenSuggestion.Delete', applyTo: 'b4deletecolumn', selector: 'citizensuggestionpanel',
                    applyBy: function (component, allowed) {
                        if (allowed) {
                            component.show();
                        } else {
                            component.hide();
                        }
                    }
                },
                {
                    name: 'Gkh.Dictionaries.Suggestion.CitizenSuggestion.ExportToGji',
                    applyTo: 'button[name=btnExportToGji]',
                    selector: 'citizensuggestionwindow',
                    applyBy: function(component, allowed) {
                        component.setVisible(allowed);
                    }
                }
            ]
        },
        {
            xtype: 'gkhstatepermissionaspect',
            name: 'deleteSuggestionStatePerm',
            permissions: [
                { name: 'Gkh.Dictionaries.Suggestion.CitizenSuggestion.Delete' }
            ]
        },
        {
            xtype: 'gkhstatepermissionaspect',
            editFormAspectName: 'citizensuggestionGridWindowAspect',
            setPermissionEvent: 'aftersetformdata',
            permissions: [
                { name: 'Gkh.Dictionaries.Suggestion.CitizenSuggestion.Edit', applyTo: 'b4savebutton', selector: 'citizensuggestionwindow' },
                { name: 'Gkh.Dictionaries.Suggestion.CitizenSuggestion.Field.ProblemPlace', applyTo: '[name=ProblemPlace]', selector: 'citizensuggestionwindow',
                    applyBy: function (component, allowed) {
                        if (allowed) {
                            component.show();
                        } else {
                            component.hide();
                        }
                    }
                },
                {
                    name: 'Gkh.Dictionaries.Suggestion.CitizenSuggestion.Field.ExecutionDeadline.View', applyTo: '[name=Deadline]', selector: 'citizensuggestionwindow',
                    applyBy: function (component, allowed) {
                        if (allowed) {
                            component.show();
                        } else {
                            component.hide();
                        }
                    }
                },
                {
                    name: 'Gkh.Dictionaries.Suggestion.CitizenSuggestion.Field.ExecutionDeadline.Edit', applyTo: '[name=Deadline]', selector: 'citizensuggestionwindow',
                    applyBy: function (component, allowed) {
                        if (allowed) {
                            component.setDisabled(false);
                        } else {
                            component.setDisabled(true);
                        }
                    }
                },
                {
                    name: 'Gkh.Dictionaries.Suggestion.CitizenSuggestion.Field.Rubric', applyTo: 'b4selectfield[name=Rubric]', selector: 'citizensuggestionwindow',
                    applyBy: function (component, allowed) {
                        if (allowed) {
                            component.show();
                        } else {
                            component.hide();
                        }
                    }
                },
                {
                    name: 'Gkh.Dictionaries.Suggestion.CitizenSuggestion.CloseExpired', applyTo: 'button[name=CloseExpired]', selector: 'citizensuggestionpanel',
                    applyBy: function (component, allowed) {
                        if (allowed) {
                            component.show();
                        } else {
                            component.hide();
                        }
                    }
                }
            ]
        }
    ],
    
    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget('citizensuggestionpanel');

        me.bindContext(view);
        me.application.deployView(view);

        me.getStore('suggestion.CitizenSuggestion').load();

        me.getStore('suggestion.History').on('beforeload', function (store, operation) {
            operation.params = operation.params || {};
            operation.params.suggId = me.citSuggId;
        });

        B4.Ajax.request({
            url: B4.Url.action('GetParams', 'GkhParams')
        }).next(function (response) {

            var json = Ext.JSON.decode(response.responseText);

            if (json.ShowStlRealityGrid) {

            } else {

                var col = Ext.ComponentQuery.query('citizensuggestionpanel #SettlementColumn')[0];
                if (col) {
                    col.hide();
                }
            }

        }).error(function () {
            Log('Ошибка получения параметров приложения');
        });
    },

    init: function() {
        var me = this;
        me.control({
            'citizensuggestionwindow grid[name=history]': {
                render: function(grid) {
                    grid.getStore().load();
                }
            },
            'citizensuggestionpanel button[name=CloseExpired]': {
                click: function() {
                    B4.Ajax.request({
                        url: B4.Url.action('CloseExpired', 'ExpiredSuggetsionClosing')
                    }).next(function (response) {
                        var json = Ext.JSON.decode(response.responseText);
                        if (json.success) {
                            B4.QuickMsg.msg('Закрытие порсроченных обращений', 'Закрытие прошло успешно', 'success');
                        } else {
                            B4.QuickMsg.msg('Закрытие порсроченных обращений', 'Ошибка при закрытии: ' + json.message, 'warning');
                        }
                    }).error(function () {
                        B4.QuickMsg.msg('Закрытие порсроченных обращений', 'Возникла непредвиденная ошибка', 'error');
                    });
                }
            }
        });
        me.callParent(arguments);
    }
});
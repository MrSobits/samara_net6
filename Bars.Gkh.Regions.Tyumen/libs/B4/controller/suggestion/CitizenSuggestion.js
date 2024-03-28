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

    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            permissions: [{ name: 'Gkh.Dictionaries.CitizenSuggestion.Comment.Create', applyTo: 'b4addbutton', selector: 'citsugcommentgrid' }]
        },
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
            },
            onSaveSuccess: function (asp, record) {
                asp.controller.citSuggId = record.getId();
            },
            onExecutorBeforeLoad: function (f, opts, store) {
                Ext.apply(store.getProxy().extraParams, {
                    executorType: this.controller.executorType
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
                    this.controller.getAspect('citizensuggestionStateWindowAspect').setStateData(record.get('Id'), record.get('State'));
                    var commentStore = asp.getForm().down('citsugcommentgrid').getStore();

                    commentStore.clearFilter(true);
                    commentStore.filter([
                        { property: "citSuggId", value: record.getId() }
                    ]);

                    //загружаем стор для кнопки печати
                    asp.controller.getAspect('citizenSuggestionPrintAspect').loadReportStore();
                    asp.controller.citSuggId = record.getId();
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
                //asp.controller.citSuggId = record.getId();
                asp.controller.citSuggCommentId = record.getId();
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

                    var form = asp.getForm(),
                        executorType = rec.get('ExecutorType');
                    rec.set('CitizenSuggestion', asp.controller.citSuggId);
                    if (executorType != 50) {
                        return B4.Ajax.request({
                            url: B4.Url.action('ChangeExecutorType', 'SuggestionComment'),
                            params: {
                                commentId: rec.getId(),
                                executorType: executorType
                            }
                        }).next(function(response) {
                            var result = Ext.JSON.decode(response.responseText),
                                data = result.data,
                                field = form.down('textfield[name=Executor]');
                            field.setValue(data);
                            return true;
                        }).error(function() {
                            B4.QuickMsg.msg('Ошибка!', 'Данный дом не обслуживается управляющей компанией. Выберите другого исполнителя', 'error');
                            return false;
                        });
                    }
                    
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
                },
                beforesetformdata: function (asp, record) {
                    var isfirst = record.get('IsFirst'),
                        comment = asp.getForm().down('fieldset[name="comment"]'),
                        firstComment = asp.getForm().down('fieldset[name="firstcomment"]'),
                        answerDate = asp.getForm().down('datefield[name="AnswerDate"]'),
                        answer = asp.getForm().down('textarea[name="Answer"]'),
                        islast = record.get('IsLast');

                    if (!islast) {
                        answerDate.setReadOnly(true);
                        answer.setReadOnly(true);
                    }

                    if (isfirst) {
                        comment.disable();
                        comment.hide();
                    } else {
                        firstComment.disable();
                        firstComment.hide();
                    }
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
        },
        {
            xtype: 'gkhstatepermissionaspect',
            editFormAspectName: 'citizensuggestionGridWindowAspect',
            setPermissionEvent: 'aftersetformdata',
            permissions: [
                { name: 'Gkh.Dictionaries.Suggestion.CitizenSuggestion.SuggestionComment.Edit', applyTo: 'b4savebutton', selector: 'citsugcommentwindow' },
                {
                    name: 'Gkh.Dictionaries.Suggestion.CitizenSuggestion.SuggestionComment.Field.ExecutorType',
                    applyTo: '[name=ExecutorType]',
                    selector: 'citsugcommentwindow',
                    applyBy: function(component, allowed) {
                        if (allowed) {
                            component.setDisabled(false);
                        } else {
                            component.setDisabled(true);
                        }
                    }
                }
            ]
        }
    ],

    refs: [
        { ref: 'mainView', selector: 'citizensuggestionpanel' }
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
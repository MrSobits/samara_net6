Ext.define('B4.controller.suggestion.Rubric', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.permission.GkhPermissionAspect',
        'B4.Ajax',
        'B4.QuickMsg'
    ],

    parentId: null,

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    stores: [
        'suggestion.Rubric',
        'suggestion.Transition',
        'suggestion.SugTypeProblem', // Добавил для "Тип проблемы"

    ],

    models: [
        'suggestion.Rubric',
        'suggestion.Transition',
        'suggestion.SugTypeProblem'// Добавил для "Тип проблемы"
    ],

    views: [
        'suggestion.rubric.Grid',
        'suggestion.rubric.EditWindow',
        'suggestion.TransitionGrid',
        'suggestion.TransitionEditWindow',
        'suggestion.rubric.TypeProblemGrid',// Добавил для "Тип проблемы"
        'suggestion.rubric.TypeProblemEditWindow',// Добавил для "Тип проблемы"

    ],

    mainView: 'suggestion.rubric.Grid',
    mainViewSelector: 'rubricpanel',

    aspects: [
        {
            xtype: 'grideditwindowaspect',
            name: 'rubricGridWindowAspect',
            gridSelector: 'rubricpanel',
            editFormSelector: 'rubricwindow',
            storeName: 'suggestion.Rubric',
            modelName: 'suggestion.Rubric',
            editWindowView: 'suggestion.rubric.EditWindow',
            onSaveSuccess: function () {
                // перекрываем чтобы окно незакрывалось после сохранения
                B4.QuickMsg.msg('Сохранение', 'Данные успешно сохранены', 'success');
                //   typeKNDDictId = record.getId();
                //   asp.controller.setTypeKNDDictId(typeKNDDictId);
            },
            listeners: {
                aftersetformdata: function (asp, rec, win) {
                    debugger;
                    var rubricId = rec.get('Id');
                    if (rubricId > 0) {
                        asp.controller.parentId = rubricId;
                        var tpanel = win.down('#rubricTabPanel');
                        tpanel.setDisabled(false);
                        var transitionStore = win.down('transitiongrid').getStore();
                        transitionStore.on('beforeload', function (store, op) {
                            op.params = op.params || {};
                            op.params.rubricId = rubricId;
                        });
                        var typeProblemstore = win.down('rubrictypeproblemgrid').getStore();
                        typeProblemstore.on('beforeload', function (store, op) {
                            op.params = op.params || {};
                            op.params.rubricId = rubricId;
                        });
                        typeProblemstore.load();
                    }
                    else
                    {
                        var tpanel = win.down('#rubricTabPanel');
                        tpanel.setDisabled(true);
                    }
                }
            }
        },
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                { name: 'Gkh.Dictionaries.Suggestion.Rubric.Create', applyTo: 'b4addbutton', selector: 'rubricpanel' },
                { name: 'Gkh.Dictionaries.Suggestion.Rubric.Edit', applyTo: 'b4savebutton', selector: 'rubricwindow' },
                {
                    name: 'Gkh.Dictionaries.Suggestion.Rubric.Delete', applyTo: 'b4deletecolumn', selector: 'rubricpanel',
                    applyBy: function (component, allowed) {
                        if (allowed) {
                            component.show();
                        } else {
                            component.hide();
                        }
                    }
                },
                {
                    name: 'Gkh.Dictionaries.Suggestion.Rubric.Field.RunBP', applyTo: 'button[name=RunBP]', selector: 'transitiongrid',
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
            xtype: 'grideditwindowaspect',
            name: 'transitiongridWindowAspect',
            gridSelector: 'transitiongrid',
            editFormSelector: 'transitionwindow',
            modelName: 'suggestion.Transition',
            editWindowView: 'suggestion.TransitionEditWindow',
            listeners: {
                aftersetformdata: function (asp, rec, win) {
                    var rubricId = asp.controller.getRubricEditWin().down('hidden[name=Id]').getValue();
                    rec.set('Rubric', rubricId);
                }
            }
        },
        { // Добавил для "Тип проблемы"
            xtype: 'grideditwindowaspect',
            name: 'rubrictypeproblemgridWindowAspect',
            gridSelector: 'rubrictypeproblemgrid',
            editFormSelector: '#rubricTypeProblemEditWindow',
            modelName: 'suggestion.SugTypeProblem',
            editWindowView: 'suggestion.rubric.TypeProblemEditWindow',
            listeners: {
                aftersetformdata: function (asp, rec, win) {
                    debugger;
                    rec.set('Rubric', asp.controller.parentId);
                }
            }
        }
    ],

    refs: [
        { ref: 'mainView', selector: 'rubricpanel' },
        { ref: 'rubricEditWin', selector: 'rubricwindow' }
    ],

    index: function () {
        var view = this.getMainView() || Ext.widget('rubricpanel');

        this.bindContext(view);
        this.application.deployView(view);

        view.getStore().load();
    },
    
    init: function () {
        var me = this;
        me.control({
            'transitiongrid button[name=ValidateBP]': {
                'click': function (btn) {
                    me.mask('Проверка процесса', btn.up('grid'));

                    B4.Ajax.request({
                        url: B4.Url.action('Validate', 'TransitionBusinessProcess'),
                        params: {
                            rubricId: me.getRubricEditWin().down('hidden[name=Id]').getValue()
                        }
                    }).next(function (resp) {
                        var json = Ext.JSON.decode(resp.responseText);
                        me.unmask();
                        if (json.success) {
                            B4.QuickMsg.msg('Проверка процесса', 'Процесс успешно прошел валидацию', 'success');
                        } else {
                            B4.QuickMsg.msg('Проверка процесса', 'Ошибка при валидации: ' + json.message, 'warning');
                        }
                    }).error(function () {
                        me.unmask();
                        console.log(arguments);
                        B4.QuickMsg.msg('Проверка процесса', 'При проверке процесса произошла ошибка', 'error');
                    });
                }
            },
            'transitiongrid button[name=RunBP]': {
                'click': function (btn) {
                    me.mask('Проверка процесса', btn.up('grid'));

                    B4.Ajax.request({
                        url: B4.Url.action('Run', 'TransitionBusinessProcess'),
                        params: {
                            rubricId: me.getRubricEditWin().down('hidden[name=Id]').getValue()
                        }
                    }).next(function (resp) {
                        var json = Ext.JSON.decode(resp.responseText);
                        me.unmask();
                        if (json.success) {
                            B4.QuickMsg.msg('Запуск процесса', 'Процесс прошел успешно', 'success');
                        } else {
                            B4.QuickMsg.msg('Запуск процесса', 'Ошибка при выполнении: ' + json.message, 'warning');
                        }
                    }).error(function () {
                        me.unmask();
                        console.log(arguments);
                        B4.QuickMsg.msg('Проверка процесса', 'При проверке процесса произошла ошибка', 'error');
                    });
                }
            }
        });
        me.callParent(arguments);
    }
});
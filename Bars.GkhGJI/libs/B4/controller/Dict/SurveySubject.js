Ext.define('B4.controller.dict.SurveySubject', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    models: ['dict.SurveySubject'],
    stores: ['dict.SurveySubject'],

    views: ['dict.surveysubject.Grid'],

    mainView: 'dict.surveysubject.Grid',
    mainViewSelector: 'surveysubjectgrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'surveysubjectgrid'
        }
    ],

    aspects: [
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: 'surveysubjectgrid',
            permissionPrefix: 'GkhGji.Dict.SurveySubject',
            permissions: [
                {
                    name: 'Create',
                    applyTo: 'b4addbutton',
                    applyBy: function(component, allowed) {
                        component.setDisabled(!allowed);
                    }
                },
                {
                    name: 'Delete',
                    applyTo: 'b4deletecolumn',
                    applyBy: function(component, allowed) {
                        if (allowed) {
                            component.show();
                        } else {
                            component.hide();
                        }
                    }
                },
                {
                    name: 'Edit',
                    applyTo: 'b4savebutton',
                    applyBy: function(component, allowed) {
                        component.setDisabled(!allowed);
                    }
                },
                {
                    name: 'Columns.Formulation',
                    applyTo: 'gridcolumn[dataIndex=Formulation]',
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
            xtype: 'gkhinlinegridaspect',
            gridSelector: 'surveysubjectgrid',
            name: 'surveySubjectGridAspect',
            storeName: 'dict.SurveySubject',
            modelName: 'dict.SurveySubject',
            gridAction: function (grid, action) {
                if (this.fireEvent('beforegridaction', this, grid, action) !== false) {
                    switch (action.toLowerCase()) {
                        case 'add':
                            this.addRecord();
                            break;
                        case 'update':
                            this.updateGrid();
                            break;
                        case 'save':
                            if (this.beforeSave(grid) !== false) {
                                this.save();
                            }
                            break;
                    }
                }
            },
            beforeSave: function (grid) {
                var me = this,
                    store = grid.getStore(),
                    valid = true;

                store.each(function (rec) {
                    valid = me.validateSurveySubject(rec);
                    return valid;
                });

                if (!valid) {
                    B4.QuickMsg.msg("Ошибка", "Необходимо заполнить поле 'Актуальность формулировки'", "error");
                    return false;
                }

                return true;
            },
            validateSurveySubject: function (rec) {
                return !Ext.isEmpty(rec.get('Relevance'));
            }
        }
    ],

    index: function () {
        var view = this.getMainView() || Ext.widget('surveysubjectgrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.SurveySubject').load();
    }
});
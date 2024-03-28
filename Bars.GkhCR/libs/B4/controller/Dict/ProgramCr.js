Ext.define('B4.controller.dict.ProgramCr', {
    /*
    * Контроллер раздела программы КР
    */
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.aspects.permission.ProgramCr',
        'B4.Ajax',
        'B4.Url'
    ],

    models: [
        'dict.ProgramCr',
        'dict.Period',
        'dict.ProgramCrFinSource'
    ],
    stores:  [
        'dict.ProgramCr',
        'dict.ProgramCrFinSource',
        
        'dict.FinanceSourceSelect',
        'dict.FinanceSourceSelected'
    ],
    views: [
        'dict.programcr.EditWindow',
        'dict.programcr.CopyWindow',
        'dict.programcr.Grid',
        'SelectWindow.MultiSelectWindow'
    ],
    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    mainView: 'dict.programcr.Grid',
    mainViewSelector: 'programCrGrid',

    editWindowSelector: '#programCrEditWindow',

    refs: [
        {
            ref: 'mainView',
            selector: 'programCrGrid'
        }
    ],

    aspects: [
        {
            xtype: "programcrperm"
        },
        {
            /*
            * Аспект взаимодействия таблицы и формы редактирования раздела Программы КР
            */
            xtype: 'grideditwindowaspect',
            name: 'programCrGridWindowAspect',
            gridSelector: 'programCrGrid',
            editFormSelector: '#programCrEditWindow',
            copyFormSelector: '#programCrCopyWindow',
            storeName: 'dict.ProgramCr',
            modelName: 'dict.ProgramCr',
            editWindowView: 'dict.programcr.EditWindow',
            copyWindowView: 'dict.programcr.CopyWindow',
            otherActions: function (actions) {
                actions[this.copyFormSelector + ' #copyProgramButton'] = { 'click': { fn: this.onCopyBtnClick, scope: this } };
                actions[this.copyFormSelector + ' b4closebutton'] = { 'click': { fn: this.closeCopyWin, scope: this } };
            },
            onSaveSuccess: function (asp, record) {
                asp.controller.setCurrentId(record.getId());
            },
            listeners: {
                aftersetformdata: function (asp, record) {
                    asp.controller.setCurrentId(record.getId());
                }
            },
            rowAction: function (grid, action, record) {
                if (this.fireEvent('beforerowaction', this, grid, action, record) !== false) {
                    switch (action.toLowerCase()) {
                        case 'edit':
                            this.editRecord(record);
                            break;
                        case 'delete':
                            this.deleteRecord(record);
                            break;
                        case 'copy':
                            this.copyRecord(record);
                            break;
                    }
                }
            },
            copyRecord: function(record) {
                var me = this,
                    id = record ? record.getId() : null,
                    model;

                this.controller.programCrId = id;

                model = this.getModel(record);

                id ? model.load(id, {
                    success: function (rec) {
                        me.setCopyFormData(rec);
                    },
                    scope: this
                }) : this.setFormData(new model({ Id: 0 }));

                this.getCopyWindow().getForm().isValid();
            },
            setCopyFormData: function (rec) {
                var form = this.getCopyWindow();
                if (this.fireEvent('beforesetformdata', this, rec, form) !== false) {
                    form.loadRecord(rec);
                    form.getForm().updateRecord();
                    form.getForm().isValid();
                }

                var periodModel = this.controller.getModel('dict.Period');
                periodModel.load(rec.get('Period'), {
                    success: function (recPeriod) {
                        form.down('#tfName').setValue('Копия ' + rec.get('Name'));
                        form.down('#tfCode').setValue(rec.get('Code'));
                        form.down('#sflPeriod').setValue({ Id: recPeriod.get('Id'), Name: recPeriod.get('Name') });
                        form.down('#cbTypeProgramCr').setValue(rec.get('TypeProgramCr'));
                        form.down('#chbUsedInExport').setValue(rec.get('UsedInExport'));
                        form.down('#chbNotAddHome').setValue(rec.get('NotAddHome'));
                        form.down('#chbMatchFl').setValue(rec.get('MatchFl'));
                        form.down('#taDescription').setValue(rec.get('Description'));
                    },
                    scope: this
                });



                form.show();
            },
            getCopyWindow : function() {
                if (this.copyFormSelector) {
                    var copyWindow = Ext.ComponentQuery.query(this.copyFormSelector)[0];

                    if (copyWindow && copyWindow.isHidden() && copyWindow.rendered) {
                        copyWindow = copyWindow.destroy();
                    }

                    if (!copyWindow) {
                        copyWindow = this.controller.getView(this.copyWindowView).create({ constrain: true, autoDestroy: true });
                        if (B4.getBody().getActiveTab()) {
                            B4.getBody().getActiveTab().add(copyWindow);
                        } else {
                            B4.getBody().add(copyWindow);
                        }
                    }
                    return copyWindow;
                }
                
                return null;
            },
            closeCopyWin: function (self) {
                self.up(this.copyFormSelector).destroy();
            },
            onCopyBtnClick: function (self) {
                var asp = this;
                var copyWin = self.up(asp.copyFormSelector);
                
                asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                B4.Ajax.request(
                {
                    method: 'POST',
                    url: B4.Url.action('CopyProgram', 'ProgramCr'),
                    params: {
                        programCrId: asp.controller.programCrId,
                        Name: copyWin.down('#tfName').getValue(),
                        Code: copyWin.down('#tfCode').getValue(),
                        Period: copyWin.down('#sflPeriod').getValue(),
                        Visible: copyWin.down('#cbTypeVisibilityProgramCr').getValue(),
                        Type: copyWin.down('#cbTypeProgramCr').getValue(),
                        State: copyWin.down('#cbTypeProgramStateCr').getValue(),
                        UseInExport: copyWin.down('#chbUsedInExport').getValue(),
                        IsAddHouse: copyWin.down('#chbNotAddHome').getValue(),
                        NotValidLaw: copyWin.down('#chbMatchFl').getValue(),
                        Description: copyWin.down('#taDescription').getValue(),
                        CopyWithoutAttachments: copyWin.down('#chbCopyWithoutAttachments').getValue()
                    },
                    timeout: 5 * 60 * 60 * 1000
                }).next(function () {
                    asp.controller.unmask();
                    Ext.Msg.alert('Успех!', 'Копирование успешно завершено');
                    asp.controller.getStore(asp.storeName).load();
                }).error(function (e) {
                    asp.controller.unmask();
                    Ext.Msg.alert('Ошибка!', 'Не удалось скопировать программу: ' + e.message);
                });
            }
        },
        {
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'programCrFinSourceGridAspect',
            gridSelector: '#programCrFinSourceGrid',
            storeName: 'dict.ProgramCrFinSource',
            modelName: 'dict.ProgramCrFinSource',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#programCrFinSourceGridMultiSelectWindow',
            storeSelect: 'dict.FinanceSourceSelect',
            storeSelected: 'dict.FinanceSourceSelected',
            titleSelectWindow: 'Выбор источников финансирования',
            titleGridSelect: 'Разрезы финансирования',
            titleGridSelected: 'Выбранные разрезы финансирования',
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1 }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1 }
            ],

            listeners: {
                getdata: function (asp, records) {
                    var recordIds = [];
                    records.each(function (rec) {
                        recordIds.push(rec.get('Id'));
                    });

                    if (recordIds[0] > 0) {
                        asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                        B4.Ajax.request(B4.Url.action('AddWorks', 'ProgramCrFinSource', {
                            objectIds: recordIds,
                            programCrId: asp.controller.programCrId
                        })).next(function () {
                            asp.controller.getStore(asp.storeName).load();
                            asp.controller.unmask();
                            return true;
                        }).error(function () {
                            asp.controller.unmask();
                        });
                    }
                    else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать разрезы финансирования');
                        return false;
                    }
                    return true;
                }
            }
        }],

    init: function () {
        var me = this;
        me.getStore('dict.ProgramCrFinSource').on('beforeload', this.onBeforeLoad, this);
        me.control({
            'programcrchangejournalgrid b4updatebutton': {
                change: {
                    fn: me.updateJournal,
                    scope: me
                }
            }
        });
        me.callParent(arguments);
    },

    index: function () {
        var view = this.getMainView() || Ext.widget('programCrGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.ProgramCr').load();
    },

    updateJournal: function (btn) {
        btn.up('grid').getStore().load();
    },

    onBeforeLoad: function (store, operation) {
        operation.params.programCrId = this.programCrId;
    },

    setCurrentId: function (id) {
        var editWindow = Ext.ComponentQuery.query(this.editWindowSelector)[0],
            storeProgramCrFinSource = this.getStore('dict.ProgramCrFinSource'),
            changeJournalGrid = editWindow.down('programcrchangejournalgrid'),
            changeJournalPanel = editWindow.down('[panelName=ChangeJournal]');
        this.programCrId = id;

        storeProgramCrFinSource.removeAll();
        if (id > 0) {
            editWindow.down('#programCrFinSourceGrid').setDisabled(false);
            storeProgramCrFinSource.load();
            changeJournalPanel.setDisabled(false);
            changeJournalGrid.getStore().filter('programCrId', id);
        } else {
            editWindow.down('#programCrFinSourceGrid').setDisabled(true);
            changeJournalPanel.setDisabled(true);
        }
    }
});
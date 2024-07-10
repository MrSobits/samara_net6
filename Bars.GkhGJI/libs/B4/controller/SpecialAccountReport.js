Ext.define('B4.controller.SpecialAccountReport', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhGjiDigitalSignatureGridAspect',
        'B4.aspects.GkhGridEditForm'
    ],

    models: ['specialaccount.SpecialAccountReport'],
    stores: ['specialaccount.SpecialAccountReport'],
    views: [
        'specialaccount.AddWindow',
        'specialaccount.Grid',
        'specialaccount.EditPanel',
        'specialaccount.RowGrid'
    ],

        mixins: {
            mask: 'B4.mixins.MaskBody',
            context: 'B4.mixins.Context'
        },

    aspects: [
        //{
        //    xtype: 'grideditwindowaspect',
        //    name: 'specialaccountgridAspect',
        //    gridSelector: 'specialaccountgrid',
        //    editFormSelector: '#specialAccountAddWindow',
        //    storeName: 'specialaccount.SpecialAccountReport',
        //    modelName: 'specialaccount.SpecialAccountReport',
        //    editWindowView: 'specialaccount.AddWindow'
        //},
        {
            xtype: 'gkhgrideditformaspect',
            name: 'specialAccountGridWindowAspect',
            gridSelector: 'specialaccountreportgrid',
            editFormSelector: '#specialAccountAddWindow',
            storeName: 'specialaccount.SpecialAccountReport',
            modelName: 'specialaccount.SpecialAccountReport',
            editWindowView: 'specialaccount.AddWindow',
            controllerEditName: 'B4.controller.specialaccount.Edit',
            otherActions: function (actions) {               
                actions[this.editFormSelector + ' #sfContragent'] = { 'beforeload': { fn: this.onBeforeLoadContragent, scope: this } };            },
         //   controllerEditName: 'B4.controller.specialaccount.Navigation',
            onSaveSuccess: function (aspect, rec) {
                //Закрываем окно после добавления новой записи
                aspect.getForm().close();
                //загружаем добавленный объект
                var model = this.controller.getModel(this.modelName);
                model.load(rec.getId(), {
                    success: function (record) {
                        //После загрузки объекта подменяем параметр и открываем вкладку
                        this.editRecord(record);
                    },
                    scope: this
                });
            },
            onBeforeLoadContragent: function (store, operation) {
                
                operation = operation || {};
                operation.params = operation.params || {};
                operation.params.onlySpecAcc = true;
            },
            rowAction: function (grid, action, record) {
                if (this.fireEvent('beforerowaction', this, grid, action, record) !== false) {
                    switch (action.toLowerCase()) {
                        case 'edit':
                            this.editRecord(record);
                            break;
                        case 'delete':
                            this.deleteRecord(grid, action, record);
                            break;
                    }
                }
            },
            editRecord: function (record) {
                var me = this,
                    id = record ? record.get('Id') : null,
                    model;
                model = me.controller.getModel(me.modelName);
                var p = me.controller.getModel(me.modelName);
                var v = me.controllerEditName;
                if (id) {
                    if (me.controllerEditName) {
                        me.controller.application.redirectTo(Ext.String.format('reporteditor/{0}', id));
                    } else {
                        model.load(id, {
                            success: function (rec) {
                                me.setFormData(rec);
                            },
                            scope: me
                        });
                    }
                } else {
                    me.setFormData(new model({ Id: 0 }));
                }
            },
            deleteRecord: function (grid, action, rec) {
                Ext.Msg.confirm('Удаление записи!',
                    'При удалении отчета теряются ранее заполненные вами данные. Продолжить?',
                    function(result) {
                        if (result === 'yes') {
                            grid.mask();
                            rec.destroy().next(function(response) {
                                grid.unmask();
                                if (response.responseData.success) {
                                    B4.QuickMsg.msg('Сообщение', 'Удаление прошло успешно', 'info');
                                    grid.getStore().reload();
                                } else {
                                    B4.QuickMsg.msg('Ошибка', 'Во время удаления произошла ошибка', 'error');
                                }
                            }).error(function() {
                                grid.unmask();
                                B4.QuickMsg.msg('Ошибка', 'Во время удаления произошла ошибка', 'error');
                            });
                        }
                    })
            }
        },
        {
            xtype: 'gkhgjidigitalsignaturegridaspect',
            gridSelector: 'specialaccountreportgrid',
            controllerName: 'SpecialAccountReport',
            signedFileField: 'SignedXMLFile'
        }
    ],

    mainView: 'specialaccount.Grid',
    mainViewSelector: 'specialaccountreportgrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'specialaccountreportgrid'
        }
    ],

    init: function () {
        
        this.control({
            'specialaccountreportgrid b4updatebutton': { click: { fn: this.updateGrid, scope: this } },
        });

        this.callParent(arguments);
    },

    updateGrid: function (btn) {
        btn.up('specialaccountreportgrid').getStore().load();
    },

    getGrid: function () {
        return this.getMainView();
    },

    index: function () {
        
        var view = this.getMainView() || Ext.widget('specialaccountreportgrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('specialaccount.SpecialAccountReport').load();
    }
});

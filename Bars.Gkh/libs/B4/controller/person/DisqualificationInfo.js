Ext.define('B4.controller.person.DisqualificationInfo', {
    extend: 'B4.controller.MenuItemController',
    
    requires: [
        'B4.aspects.GridEditCtxWindow',
        'B4.aspects.permission.GkhStatePermissionAspect'
    ],
    
    mixins: {
        context: 'B4.mixins.Context'
    },

    models: [
        'Person',
        'person.DisqualificationInfo'
    ],
    
    stores: [
        'person.DisqualificationInfo'
    ],
    
    views: [
        'person.DisqualificationInfoGrid',
        'person.DisqualificationInfoEditWindow'
    ],

    refs: [
        {
            ref: 'mainView',
            selector: 'persondisqualinfogrid'
        }
    ],

    mainView: 'person.DisqualificationInfoGrid',
    mainViewSelector: 'persondisqualinfogrid',
    
    parentCtrlCls: 'B4.controller.person.Navi',

    aspects: [
        {
            xtype: 'gkhstatepermissionaspect',
            name: 'personDisqualificationInfoStatePermAspect',
            permissions: [
                { name: 'Gkh.Person.PersonDisqualificationInfo.Create', applyTo: 'b4addbutton', selector: 'persondisqualinfogrid' },
                { name: 'Gkh.Person.PersonDisqualificationInfo.Edit', applyTo: 'b4savebutton', selector: 'persondisqualinfoeditwindow' }

            ]
        },
        {
            xtype: 'gkhstatepermissionaspect',
            name: 'personDisqualificationInfoDeleteStatePermAspect',
            permissions: [
                { name: 'Gkh.Person.PersonDisqualificationInfo.Delete' }
            ]
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'personDisquGridWindowAspect',
            gridSelector: 'persondisqualinfogrid',
            editFormSelector: 'persondisqualinfoeditwindow',
            modelName: 'person.DisqualificationInfo',
            editWindowView: 'person.DisqualificationInfoEditWindow',
            listeners: {
                getdata: function (asp, record) {
                    var me = this;
                    if (!record.data.Id) {
                        record.data.Person = me.controller.getContextValue(me.controller.getMainComponent(), 'personId');
                    }
                },
                aftersetformdata: function (me, record) {
                    var personId = me.controller.getContextValue(me.controller.getMainView(), 'personId');
                    me.controller.getAspect('personDisqualificationInfoDeleteStatePermAspect').setPermissionsByRecord({ getId: function () { return personId; } });
                }
            },
            deleteRecord: function (rec) {
                // проверка удаления записи Квал Аттестата происходит по статусу Должностного лицв. Внимание
                var me = this,
                    personId = me.controller.getContextValue(me.controller.getMainView(), 'personId'),
                    modelPerson = me.controller.getModel('Person');

                modelPerson.load(personId, {
                    success: function (record) {

                        me.controller.getAspect('personDisqualificationInfoStatePermAspect').loadPermissions(record)
                        .next(function (response) {
                            var grants = Ext.decode(response.responseText);
                            if (grants && grants[0]) {
                                grants = grants[0];
                            }
                            // проверяем пермишшен колонки удаления
                            if (grants[0] == 0) {
                                Ext.Msg.alert('Сообщение', 'Удаление на данном статусе должностного лица запрещено');
                            } else {
                                Ext.Msg.confirm('Удаление записи!', 'Вы действительно хотите удалить запись?', function (result) {
                                    if (result == 'yes') {
                                        var model = me.getModel(rec);
                                        var newRec = new model({ Id: rec.getId() });
                                        me.mask('Удаление', me.controller.getMainComponent());
                                        newRec.destroy()
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
                        }, me);


                    },
                    scope: me
                });

            }
        }
    ],

    init: function () {
        var me = this,
            actions = {};

        actions['persondisqualinfoeditwindow combobox[name=TypeDisqualification]'] = { 'change': { fn: this.onChangeTypeDisq, scope: this } };
        
        this.control(actions);
        
        me.callParent(arguments);
    },

    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('persondisqualinfogrid');

        me.bindContext(view);
        me.setContextValue(view, 'personId', id);
        me.application.deployView(view, 'person_info');

        var store = view.getStore();
        store.clearFilter(true);
        store.filter('personId', id);
        
        me.getAspect('personDisqualificationInfoStatePermAspect').setPermissionsByRecord({ getId: function () { return id; } });
    },
    
    onChangeTypeDisq: function(cmb, newVal) {
        var wnd = cmb.up('persondisqualinfoeditwindow'),
            tfPetitionNumber = wnd.down('[name=PetitionNumber]'),
            tfPetitionDate = wnd.down('[name=PetitionDate]');
        
        //  Назначение наказания в виде дисквалификации - только при таком основании поля долюны быт ьобязательны для заполнения
        if (newVal == 10) {
            tfPetitionNumber.allowBlank = false;
            tfPetitionDate.allowBlank = false;
        } else {
            tfPetitionNumber.allowBlank = true;
            tfPetitionDate.allowBlank = true;
        }

        wnd.getForm().isValid();
    }
});
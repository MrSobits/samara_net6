Ext.define('B4.controller.dict.QualificationMember', {
    /*
    * Контроллер раздела участники квалиф. отбора
    */
    extend: 'B4.base.Controller',
    requires:
    [
        'B4.Ajax',
        'B4.Url',
        'B4.aspects.GridEditWindow',
        'B4.aspects.permission.dict.QualificationMember',
        'B4.aspects.GkhTriggerFieldMultiSelectWindow'
    ],

    models: ['dict.QualificationMember'],
    stores: [
        'dict.QualificationMember',
        'dict.RoleForSelect',
        'dict.qualificationmember.RoleSelected'
    ],
    views: [
        'dict.qualificationmember.Grid',
        'dict.qualificationmember.EditWindow',
        'SelectWindow.MultiSelectWindow'
    ],

    mainView: 'dict.qualificationmember.Grid',
    mainViewSelector: 'qualificationMemberGrid',

    refs: [ {
        ref: 'mainView',
        selector: 'qualificationMemberGrid'
    }],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    aspects: [
        {
            xtype: 'qualificationmemberperm'
        },
        {
            /*
            * Аспект взаимодействия таблицы и формы редактирования раздела участники квалиф. отбора
            */
            xtype: 'grideditwindowaspect',
            name: 'qualificationMemberGridWindowAspect',
            gridSelector: 'qualificationMemberGrid',
            editFormSelector: '#qualificationMemberEditWindow',
            storeName: 'dict.QualificationMember',
            modelName: 'dict.QualificationMember',
            editWindowView: 'dict.qualificationmember.EditWindow',
            onSaveSuccess: function(asp, record) {
                asp.controller.qualMemberId = record.getId();
                this.updateControls(asp.controller.qualMemberId);
            },
            updateControls: function(objectId) {
                this.getForm().down('#roleQualmemberTrigerField').setDisabled(!objectId);
            },
            listeners: {
                aftersetformdata: function(asp, record, form) {

                    asp.controller.qualMemberId = record.getId();

                    this.updateControls(asp.controller.qualMemberId);

                    var fieldRoles = form.down('#roleQualmemberTrigerField');

                    if (asp.controller.qualMemberId > 0) {
                        asp.controller.mask('Загрузка', asp.controller.getMainComponent());
                        B4.Ajax.request(
                            {
                                method: 'POST',
                                url: B4.Url.action('GetInfo', 'QualificationMember'),
                                params: {
                                    qualMemberId: asp.controller.qualMemberId
                                }
                            }).next(function(response) {
                                //десериализуем полученную строку
                                var obj = Ext.JSON.decode(response.responseText);

                                fieldRoles.updateDisplayedText(obj.roleNames);
                                fieldRoles.setValue(obj.roleIds);

                                asp.controller.unmask();
                            }).error(function() {
                                asp.controller.unmask();
                            });
                    } else {
                        fieldRoles.updateDisplayedText(null);
                        fieldRoles.setValue(null);
                    }
                }
            }
        },
        {
            /*
           аспект взаимодействия триггер-поля ролей с массовой формой выбора ролей
           */
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'qualMemberRoleMultiSelectWindowAspect',
            fieldSelector: '#qualificationMemberEditWindow #roleQualmemberTrigerField',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#roleQualmemberSelectWindow',
            storeSelect: 'dict.RoleForSelect',
            storeSelected: 'dict.qualificationmember.RoleSelected',
            textProperty: 'Name',
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
            ],
            titleSelectWindow: 'Выбор ролей',
            titleGridSelect: 'Роли для отбора',
            titleGridSelected: 'Выбранные роли',
            onSelectedBeforeLoad: function (store, operation) {
                operation.params = operation.params || {};
                operation.params.memberId = this.controller.qualMemberId;
            },
            listeners: {
                getdata: function(asp, records) {
                    var recordIds = [];
                    records.each(function(rec) { recordIds.push(rec.get('Id')); });
                    asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                    B4.Ajax.request({
                        method: 'POST',
                        url: B4.Url.action('AddRoles', 'QualificationMember'),
                        params: {
                            rolesId: Ext.encode(recordIds),
                            qualMemberId: asp.controller.qualMemberId
                        }
                    }).next(function() {
                        Ext.Msg.alert('Сохранение!', 'Роли сохранены успешно');
                        asp.controller.unmask();
                        return true;
                    }).error(function() {
                        asp.controller.unmask();
                    });

                    return true;
                }
            }
        }
    ],

    index: function () {
        var view = this.getMainView() || Ext.widget('qualificationMemberGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.QualificationMember').load();
    }
});
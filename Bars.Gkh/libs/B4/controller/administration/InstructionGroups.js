Ext.define('B4.controller.administration.InstructionGroups', {

    extend: 'B4.base.Controller',
   
    requires: [
        'B4.aspects.GridEditWindow',
        'B4.form.SelectWindow',
        'B4.aspects.permission.GkhPermissionAspect'
    ],

    models: [
        'administration.instruction.InstructionGroups',
        'administration.instruction.InstructionGroupRole',
        'Role'
    ],

    stores: [
        'Role'
    ],

    views: [
        'administration.instructiongroups.Grid',
        'administration.instructiongroups.EditWindow'
    ],

    refs: [
        {
            ref: 'mainView',
            selector: 'instructionGroupsGrid'
        }
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    mainView: 'administration.instructiongroups.Grid',
    mainViewSelector: 'instructionGroupsGrid',

    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            event: 'afterrender',
            applyBy: function (component, allowed) {
                if (component)
                    component.setDisabled(!allowed);
            },
            permissions: [
                { name: 'Administration.InstructionGroups.Create', applyTo: 'b4addbutton', selector: 'instructionGroupsGrid', applyOn: { event: this.event, selector: this.selector } },
                { name: 'Administration.InstructionGroups.Edit', applyTo: 'b4savebutton', selector: 'instructiongroupeditwindow', applyOn: { event: this.event, selector: this.selector } },
                {
                    name: 'Administration.InstructionGroups.Delete', applyTo: 'b4deletecolumn', selector: 'instructionGroupsGrid', applyOn: { event: this.event, selector: this.selector },
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                },
                {
                    name: 'Administration.InstructionGroups.Edit', applyTo: 'b4editcolumn', selector: 'instructionGroupsGrid', applyOn: { event: this.event, selector: this.selector },
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                },

                { name: 'Administration.InstructionGroups.Register.Instructions.Create', applyTo: 'b4addbutton', selector: 'instructionsGrid', applyOn: { event: this.event, selector: this.selector } },
                { name: 'Administration.InstructionGroups.Register.Instructions.Edit', applyTo: 'b4savebutton', selector: 'instructionseditwindow', applyOn: { event: this.event, selector: this.selector } },
                {
                    name: 'Administration.InstructionGroups.Register.Instructions.Delete', applyTo: 'b4deletecolumn', selector: 'instructionsGrid', applyOn: { event: this.event, selector: this.selector },
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                },
                {
                    name: 'Administration.InstructionGroups.Register.Instructions.Edit', applyTo: 'b4editcolumn', selector: 'instructionsGrid', applyOn: { event: this.event, selector: this.selector },
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                }
            ]
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'instructionGroupsAspect',
            gridSelector: 'instructionGroupsGrid',
            editFormSelector: 'instructiongroupeditwindow',
            modelName: 'administration.instruction.InstructionGroups',
            editWindowView: 'administration.instructiongroups.EditWindow',
            otherActions: function (actions) {
                actions['instructionGroupsGrid b4deletecolumn'] = {
                    click: { fn: this.onDeleteRecord, scope: this }
                };
            },
            onDeleteRecord: function (a, b, t, y, r, rec) {
                var me = this;
                Ext.Msg.confirm('Удаление записи!', 'При удалении категории будут удалены все привязанные к ней документы. Вы действительно хотите удалить запись?', function (result) {
                    if (result == 'yes') {
                        me.mask("Удаление");
                        rec.destroy().next(me.onSuccess, me).error(me.onError, me);
                    }
                });
            },
            onSuccess: function() {
                var me = this;
                me.unmask();
                me.updateGrid();
            },
            onError: function (result) {
                var me = this;
                Ext.Msg.alert('Ошибка удаления!', Ext.isString(result.responseData) ? result.responseData : result.responseData.message);
                me.unmask();
            },
            listeners: {
                'aftersetformdata': function(asp, rec) {
                    var me = this;
                    var view = asp.getForm(),
                        roleGrid = view.down('instructiongroupsrolegrid'),
                        instrGrid = view.down('instructionsGrid'),
                        roleStore = roleGrid.getStore(),
                        instrStore = instrGrid.getStore();

                    var groupId = rec.get('Id');
                    asp.controller.setContextValue(view, 'instGroupId', groupId);
                    view.down('instructionsGrid').down('b4addbutton').setDisabled(groupId == 0);
                    view.down('instructiongroupsrolegrid').down('b4addbutton').setDisabled(groupId == 0);

                    roleStore.clearFilter(true);
                    roleStore.filter('instGroupId', groupId);
                    if (!roleStore.hasListener('datachanged')) {
                        roleStore.on('datachanged', me.onRolesDataChanged, me);
                    }
                    
                    instrStore.clearFilter(true);
                    instrStore.filter('instGroupId', groupId);
                }
            },
            onSaveSuccess: function (aspect, rec) {
                var view = aspect.getForm(),
                    roleGrid = view.down('instructiongroupsrolegrid'),
                    roleStore = roleGrid.getStore();
                
                if (roleStore.getModifiedRecords().length > 0) {
                    roleStore.sync({
                        failure: function () {
                            Ext.Msg.alert('Ошибка', 'Ошибка при сохранении прав доступа.');
                        },
                        success: function() {
                            roleStore.load();
                        }
                    });
                }
            },
            onRolesDataChanged: function (store) {
                var form = this.getForm();
                var hint = form.down('instructiongroupsrolegrid').down('label');
                if (store.getCount() > 0)
                    hint.setText('Категория будет доступна только для указанных ролей:');
                else
                    hint.setText('Категория доступна всем. Выберите роли, для которых предназначена эта категория:');
            }
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'instructionGroupRoleAspect',
            gridSelector: 'instructiongroupsrolegrid',
            editFormSelector: '#instructionGroupRoleAddWindow',
            parentForm: 'instructiongroupeditwindow',
            modelName: 'administration.instruction.InstructionGroupRole',
            initComponent: function() {
                var me = this,
                    win = Ext.create('B4.form.SelectWindow', {
                        xtype: 'b4selectwindow',
                        itemId: 'instructionGroupRoleAddWindow',
                        store: 'B4.store.Role',
                        loadDataOnShow: true,
                        textProperty: 'Name',
                        title: 'Выбор роли',
                        selectionMode: 'MULTI'
                    });
                Ext.apply(me, {
                    editWindowView: win
                });

                me.callParent(arguments);
            },

            editRecord: function () {
                this.editWindowView.performSelection(this.onRoleSelected(this));
            },

            onRoleSelected: function (scope) {
                var me = scope;
                return function (records) {
                    if (records) {
                        var perm = me.getModel();
                        var permStore = me.getGrid().getStore();
                        var contextView = me.getGrid().up('instructiongroupeditwindow');
                        var groupId = me.controller.getContextValue(contextView, 'instGroupId');
                        
                        Ext.each(records, function (record) {
                            var flag = false;
                            permStore.each(function(item) {
                                var role = item.get('Role');
                                if (role && role.Id == record.Id) {
                                    flag = true;
                                    return false;
                                }
                            });
                            if (!flag) {
                                var rec = perm.create({ Role: record, InstructionGroup: groupId });
                                permStore.add(rec);
                            }
                        });
                    }
                };
            }
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'instructionsAspect',
            gridSelector: 'instructionsGrid',
            editFormSelector: 'instructionseditwindow',
            modelName: 'administration.instruction.Instructions',
            editWindowView: 'administration.instructions.EditWindow',
            listeners: {
                beforesave: function (asp, record) {
                    var contextView = asp.getGrid().up('instructiongroupeditwindow');
                    var groupId = asp.controller.getContextValue(contextView, 'instGroupId');
                    record.set('InstructionGroup', groupId);
                },
            }
        }
    ],
    
    init: function () {
        this.callParent(arguments);
    },

    index: function () {
        var view = this.getMainView() || Ext.widget('instructionGroupsGrid');
        this.bindContext(view);
        this.application.deployView(view);
        view.getStore().load();
    }
});
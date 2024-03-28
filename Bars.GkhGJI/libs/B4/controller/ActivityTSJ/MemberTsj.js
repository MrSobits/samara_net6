Ext.define('B4.controller.activitytsj.MemberTsj', {
    extend: 'B4.base.Controller',

    params: null,

    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.StateGridWindowColumn',
        'B4.aspects.StateButton',
        'B4.aspects.StateContextMenu',
        'B4.aspects.permission.ActivityTsjMember',
        'B4.aspects.FieldRequirementAspect'
    ],

    models: ['activitytsj.MemberTsj'],
    stores: ['activitytsj.MemberTsj'],

    mainView: 'activitytsj.MemberTsjGrid',
    mainViewSelector: '#memberTsjGrid',
    views: [
        'activitytsj.MemberTsjGrid',
        'activitytsj.MemberTsjEditWindow'
    ],
    mixins: {
        mask: 'B4.mixins.MaskBody'
    },

    aspects: [
        {
            xtype: 'activitytsjmemberstateperm',
            editFormAspectName: 'activityTsjMemberEditWindow',
            setPermissionEvent: 'aftersetformdata',
            name: 'activityTsjMemberStatePerm'
        },
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                {
                    name: 'GkhGji.ActivityTsj.Register.Member.Field.Delete',
                    applyTo: 'b4deletecolumn',
                    selector: 'membertsjgrid',
                    applyBy: function (component, allowed) {
                        if (component) {
                            if (allowed)
                                component.show();
                            else
                                component.hide();
                        }
                    }
                }
            ]
        },
        {
            xtype: 'requirementaspect',
            requirements: [
                { name: 'GkhGji.ActivityTsj.Members.Field.File', applyTo: '#ffStatuteFile', selector: '#activityTsjMemberEditWindow' }
            ]
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'activityTsjMemberEditWindow',
            storeName: 'activitytsj.MemberTsj',
            modelName: 'activitytsj.MemberTsj',
            gridSelector: '#memberTsjGrid',
            editFormSelector: '#activityTsjMemberEditWindow',
            editWindowView: 'activitytsj.MemberTsjEditWindow',
            listeners: {
                getdata: function(asp, record) {
                    if (this.controller.params && !record.get('Id')) {
                        record.set('ActivityTsj', this.controller.params.get('Id'));
                    }
                },
                aftersetformdata: function (asp, record, form) {
                    //проставляем статус
                    asp.controller.getAspect('activityTsjMemberStateButtonAspect').setStateData(record.get('Id'), record.get('State'));
                }
            }
        },
        {
            /*Вешаем аспект смены статуса в гриде*/
            xtype: 'b4_state_contextmenu',
            name: 'activityTsjMemberStateTransferAspect',
            gridSelector: '#memberTsjGrid',
            stateType: 'gji_activity_tsj_member',
            menuSelector: 'activityTsjMemberGridStateMenu'
        },
        {
            /* Вешаем аспект смены статуса в карточке редактирования */
            xtype: 'statebuttonaspect',
            name: 'activityTsjMemberStateButtonAspect',
            stateButtonSelector: '#activityTsjMemberEditWindow #btnState',
            listeners: {
                transfersuccess: function(asp, entityId, newState) {
                    //Если статус изменен успешно, то проставляем новый статус
                    asp.setStateData(entityId, newState);
                    //и перезагружаем грид, т.к. в гриде нужно обновить столбец Статус
                    var editWindowAspect = asp.controller.getAspect('activityTsjMemberGridEditWindow');
                    editWindowAspect.updateGrid();                    
                }
            }
        }
    ],

    init: function () {
        this.getStore('activitytsj.MemberTsj').on('beforeload', this.onBeforeLoad, this);
        
        this.callParent(arguments);
    },

    onBeforeLoad: function (store, operation) {
        if (this.params) {
            operation.params.activityTSJ = this.params.get('Id');
        }
    },

    onLaunch: function () {
        this.getStore('activitytsj.MemberTsj').load();
    }
});
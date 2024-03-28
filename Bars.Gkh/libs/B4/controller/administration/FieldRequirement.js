Ext.define('B4.controller.administration.FieldRequirement', {
    extend: 'B4.base.Controller',

    models: ['administration.FieldRequirement'],
    stores: ['administration.FieldRequirement'],
    views: ['administration.fieldrequirement.Grid'],

    mainView: 'administration.fieldrequirement.Grid',
    mainViewSelector: 'fieldrequirementgrid',

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    index: function () {
        var view = this.getMainView() || Ext.widget('fieldrequirementgrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('administration.FieldRequirement').load();
    },

    init: function() {
        var actions = {            
            'fieldrequirementgrid b4savebutton': {
                click: this.saveRequirements
            }  
        };
        
        this.control(actions);
        this.callParent(arguments);
    },
    
    saveRequirements: function(button) {
        var store = this.getStore('administration.FieldRequirement');
        var recs = store.getModifiedRecords();

        var modifiedRecs = Ext.Array.map(recs, function (rec) {
            return {
                Id: rec.get('RecId'),
                RequirementId: rec.get('RequirementId'),
                Required: rec.get('Required')
            };
        });
        
        var recordsCreate = Ext.Array.filter(modifiedRecs, function (item) {
            return item.Required;
        }).map(function (item) {
            return {RequirementId: item.RequirementId};
        });
        
        var idsDelete = Ext.Array.filter(modifiedRecs, function (item) {
            return !item.Required;
        }).map(function (item) {
            return item.Id;
        });

        var me = this;

        if (!Ext.isEmpty(recordsCreate)) {
            me.mask('Сохранение...', this.getMainView());
            B4.Ajax.request({
                url: B4.Url.action('Create', 'FieldRequirement'),
                method: 'POST',
                params: {
                    records: Ext.encode(recordsCreate)
                }
            }).next(function (response) {
                me.unmask();
                store.load();
                return true;
            }).error(function() {
                Ext.Msg.alert('Ошибка!', 'При сохранении произошла ошибка!');
                me.unmask();
            });
        }

        if (!Ext.isEmpty(idsDelete)) {
            me.mask('Сохранение...', this.getMainView());
            B4.Ajax.request({
                url: B4.Url.action('Delete', 'FieldRequirement'),
                method: 'POST',
                params: {
                    records: Ext.JSON.encode(idsDelete)
                }
            }).next(function(response) {
                me.unmask();
                store.load();
                return true;
            }).error(function() {
                Ext.Msg.alert('Ошибка!', 'При сохранении произошла ошибка!');
                me.unmask();
            });
            
        }
    }
});
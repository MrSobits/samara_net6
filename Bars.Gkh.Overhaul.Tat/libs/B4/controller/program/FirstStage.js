Ext.define('B4.controller.program.FirstStage', {
    extend: 'B4.base.Controller',

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },
   
    requires: [
        'B4.view.program.FirstStageGrid',
        'B4.aspects.permission.GkhPermissionAspect'
    ],
    
    refs: [
        { ref: 'mainPanel', selector: 'programfirststagegrid' }
    ],
    
    stores: [
        'dict.MunicipalityByOperator',
        'program.FirstStage'
    ],
    
    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                {
                    name: 'Ovrhl.Program1Stage.MakeStage1',
                    applyTo: 'b4addbutton[action=makefirststage]',
                    selector: 'programfirststagegrid'
                },
                {
                    name: 'Ovrhl.Program1Stage.MakeStage2',
                    applyTo: 'b4addbutton[action=makesecondstage]',
                    selector: 'programfirststagegrid'
                }
            ]
        }
    ],
    
    // Init
    init: function () {
        this.control({
            'programfirststagegrid b4addbutton[action=makefirststage]': { click: { fn: this.makeFirstStage, scope: this } },
            'programfirststagegrid b4addbutton[action=makesecondstage]': { click: { fn: this.makeSecondStage, scope: this } },
            'programfirststagegrid b4updatebutton': { click: { fn: this.update, scope: this } },
            'programfirststagegrid b4combobox[name="Municipality"]': {
                change: { fn: this.changeMunicipality, scope: this },
                render: { fn: this.renderMuField, scope: this }
            },
            'programfirststagegrid': { render: { fn: this.renderMainPanel, scope: this } }
        });
        
        this.callParent(arguments);
    },
    
    // Action Index - shows LongProgram grid
    index: function () {
        var view = this.getMainPanel() || Ext.widget('programfirststagegrid');

        this.bindContext(view);
        this.application.deployView(view);
    },
    
    onLoadMunicipality: function (store, records) {
        var me = this,
            panel = me.getMainPanel(),
            muId = panel.down('b4combobox[name="Municipality"]').getValue();
        
        var countRecords = store.getCount();
        if (countRecords > 0 && !muId) {
            panel.down('b4combobox[name="Municipality"]').setValue(records[0].data);
        }
    },
    
    renderMainPanel: function (panel) {
        var me = this;
        
        panel.getStore().on('beforeload', me.onBeforeLoadFirstStage, me);
    },
    
    renderMuField: function (field) {
        var me = this,
            store = field.getStore();
        
        store.on('load', me.onLoadMunicipality, me);
        store.load();
    },
    
    changeMunicipality: function () {
        var me = this,
            panel = me.getMainPanel();
        
        me.mask("Загрузка...", panel);
        panel.getStore('program.FirstStage').load({
            callback: function () {
                me.unmask();
            },
            failure: function () {
                me.unmask();
                B4.QuickMsg.msg('При получении списка элементов произошла ошибка', message, 'error');
            }
        });
    },
    
    onBeforeLoadFirstStage: function (store, operation) {
        if (!operation.params) {
            operation.params = {};
        }
        
        operation.params.municipalityId = this.getMainPanel().down('b4combobox[name="Municipality"]').getValue();
    },
    
    update: function(btn) {
        btn.up('programfirststagegrid').getStore().load();
    },
    
    makeFirstStage: function (btn) {
        var me = this,
            grid = btn.up('programfirststagegrid'),
            moId = me.getMainPanel().down('b4combobox[name="Municipality"]').getValue();
        
        if (!moId) {
            Ext.Msg.alert('Внимание!', 'Выберите "Муниципальное образование" перед формированием плана!');
            return;
        }
        
        me.mask('Формирование 1 этапа программы...', grid);
        
        B4.Ajax.request({
            url: B4.Url.action('MakeStage1', 'RealityObjectStructuralElementInProgramm'),
            params: { moId: Ext.encode(moId) },
            timeout: 9999999
        }).next(function (resp) {
            var response = Ext.decode(resp.responseText);
            me.unmask();
            if (response.success) {
                grid.getStore().load();
            } else {
                Ext.Msg.alert('Ошибка!', response.message);
            }
        }).error(function () {
            me.unmask();
            Ext.Msg.alert('Ошибка!', 'При формировании плана произошла ошибка!');
        });
    },
    
    makeSecondStage: function (btn) {
        var me = this,
            grid = btn.up('programfirststagegrid'),
            muId = me.getMainPanel().down('b4combobox[name="Municipality"]').getValue();

        me.mask('Формирование программы...', grid);

        B4.Ajax.request({
            url: B4.Url.action('MakeStage3', 'RealityObjectStructuralElementInProgramm'),
            timeout: 9999999,
            params: {
                muId: muId
            }
        }).next(function (resp) {
            var response = Ext.decode(resp.responseText);
            me.unmask();
            if (response.success) {
                Ext.History.add('dpkr/' + muId);
            } else {
                Ext.Msg.alert('Ошибка!', response.message);
            }
        }).error(function () {
            me.unmask();
            Ext.Msg.alert('Ошибка!', 'При формировании плана произошла ошибка!');
        });
    }
});
/*
Аспект предназначен для редактирования текст большого объема
Посмотреть реализацию можно в проектах Саха, Томск, Смоленск

Пример минимальной конфигурации:
{
    xtype: 'gkhblobtextaspect',
    fieldSelector: '[name=Description]', // селектор родительского поля, к которому привязывается аспект
    editPanelAspectName: 'actSurveyEditPanelAspect', // имя аспекта, контролирующего panel, обладающий родительским полем
    controllerName: 'ActSurvey', // имя контроллера, через который производятся манипуляции над длинными полями
    valueFieldName: 'Description' // имя длинного поля. совпадает с именем поля в сущности, хранящей длинные поля
}

Допустимо наличие некольких длинных полей, находящихся на одной форме и хранящихся в одной сущности.
Взаимодействие такого рода потребует поочередного подключения нескольких копий аспекта, сконфигурированных
под работу с каждым из полей.

Авто-сохранение обрезанной копии длинного текста:
{
    ...
    previewLength: 500, // длина обрезанной копии текста
    autoSavePreview: true, // авто-сохранение обрезанной копии в родительскую сущность. установка в true блокирует ввод текста в родительском поле
    previewField: 'Description' // поле родительской сущности, в которое сохраняется обрезанная копия
}

Указание одного лишь первого параметра сформирует и вернет обрезаную версию текста,
которая будет подставлена в качестве значения родительского поля, однако
автоматического сохранения не произойдет.

Еще нюанс: если краткое поле не является хранимым, то можно установить значение previewField в false и указать желаемую длину предпросмотра через previewLength.
В этом случае, значение краткого поля будет сформировано из длинного. Редактирование краткого текста при этом будет заблокировано.
*/

Ext.define('B4.aspects.GkhBlobText', {
    extend: 'B4.base.Aspect',

    alias: 'widget.gkhblobtextaspect',

    mixins: {
        mask: 'B4.mixins.MaskBody'
    },

    requires: ['B4.QuickMsg', 'B4.form.Window', 'B4.ux.form.field.TabularTextArea'],

    // имя контроллера, через который осуществляется получение и сохранение 
    // длинных полей
    controllerName: null,

    // метод получения поля
    getAction: 'GetDescription',

    // метод сохранения поля
    saveAction: 'SaveDescription',

    // аспект, контролирующий родительскую панель
    editPanelAspectName: null,

    // селектор поля, к которому привязывается длинное поле
    fieldSelector: null,

    // длина краткой версии поля
    previewLength: null,

    // авто-сохранение краткой версии
    autoSavePreview: false,

    // поле в родительской сущности, в которой происходит сохранение
    // краткой версии
    previewField: null,

    // имя длинного поля
    valueFieldName: null,

    controller: null,

    editPanelAspect: null,

    editPanelSelector: null,

    injectionEvent: 'aftersetformdata',

    constructor: function (config) {
        var me = this;
        Ext.apply(me, config);
        me.callParent(arguments);

        me.addEvents(
            'beforesetdata',
            'savesuccess',
            'savefailure'
        );
    },

    init: function(controller) {
        var me = this;
        me.callParent(arguments);

        me.controller = controller;

        me.editPanelAspect = me.controller.getAspect(me.editPanelAspectName);
        if (!me.editPanelSelector) {
            me.editPanelSelector = me.editPanelAspect.editFormSelector || me.editPanelAspect.editPanelSelector;
        }

        me.editPanelAspect.on(me.injectionEvent, me.doInjection, me);
    },

    closeWindow: function(btn) {
        var wnd = btn.up('window');
        wnd.close();
    },

    doInjection: function() {
        var me = this,
            cmp = me.getField(),
            button,
            parentId;

        if (!cmp) {
            return;
        }

        button = me.createButton(cmp);

        if (button) {
            parentId = parseInt(me.getParentRecordId());
            button.setDisabled(parentId == 0);

            if (/*me.autoSavePreview ||*/ me.previewField === false) {
                cmp.setReadOnly(true);
            }

            if (me.previewField === false && parentId > 0) {
                me.loadPreview(parentId, cmp);
            }
        }
    },

    createWindow: function() {
        var wnd = Ext.create('B4.form.Window', {
            title: 'Редактирование',
            layout: 'fit',
            ctxKey: this.controller.getCurrentContextKey ? this.controller.getCurrentContextKey() : '',
            maximized: false,
            maximizable: true,
            closeAction: 'destroy',
            width: 800,
            height: 600,
            modal: true,
            items: [
                {
                    xtype: 'tabtextarea',
                    name: this.valueFieldName
                },
                {
                    xtype: 'hidden',
                    name: 'Id'
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 1,
                            items: [
                                {
                                    xtype: 'b4savebutton'
                                }
                            ]
                        },
                        { xtype: 'tbfill' },
                        {
                            xtype: 'buttongroup',
                            columns: 1,
                            items: [
                                {
                                    xtype: 'b4closebutton'
                                }
                            ]
                        }
                    ]
                }
            ]
        });

        var itemId = '#' + wnd.getId();

        var actions = {};
        actions[itemId + ' b4savebutton'] = { 'click': { fn: this.saveRecord, scope: this } };
        actions[itemId + ' b4closebutton'] = { 'click': { fn: this.closeWindow, scope: this } };
        this.controller.control(actions);

        return wnd;
    },

    getField: function() {
        return this.getParentPanel().down(this.fieldSelector);
    },

    findButton: function(cmp) {
        var buttons = Ext.ComponentQuery.query('#' + cmp.getItemId() + '-blob-editor-button');

        return buttons && buttons.length == 1 ? buttons[0] : null;
    },

    createButton: function(cmp) {
        var me = this,
            button = me.findButton(cmp);

        if (!button) {
            button = Ext.create('Ext.Button', {
                text: 'Редактор',
                iconCls: 'icon-pencil',
                floating: true,
                shadow: false,
                handler: me.showWindow.bind(this),
                disabled: true,
                minHeight: 21, //Высота 15px + 6px margin
                itemId: cmp.getItemId() + '-blob-editor-button'
            });

            cmp.on('destroy', function() {
                button = button.destroy();
            });

            if (!cmp.rendered) {
                cmp.on('resize', me.doRenderButton, this);
            } else {
                me.doRenderButton(cmp, button);
            }
        }

        return button;
    },

    doRenderButton: function (cmp, button) {
        var me = this;

        if (arguments.length > 2) {
            cmp.un('resize', this.doRenderButton, this);
            button = me.findButton(cmp);
        }

        if (button) {
            button.render(cmp.getEl());
            button.getEl().setPositioning({ left: 'AUTO', bottom: 'AUTO', right: 0, top: 0 });
        }
    },

    getParentPanel: function() {
        return this.componentQuery(this.editPanelSelector);
    },

    getParentRecordId: function() {
        var panel = this.getParentPanel();
        return panel.getRecord().getId();
    },

    showWindow: function() {
        var me = this,
            cmp = me.getField();

        if (cmp.isDisabled() || cmp.isHidden()) {
            Ext.Msg.alert('Внимание!', 'Недостаточно прав для редактирования данного поля');
        } else {
            me.setData(this.getParentRecordId());
        }
    },

    loadData: function(objectId, params) {
        var me = this,
            id = objectId > 0 ? objectId : 0,
            deferred = new Deferred();

        B4.Ajax.request({
                url: B4.Url.action(Ext.String.format('/{0}/{1}', me.controllerName, me.getAction)),
                method: 'GET',
                params: Ext.apply({
                    field: me.valueFieldName,
                    id: id
                }, params || {})
            })
            .next(function(response) {
                deferred.call(response);
            })
            .error(function(response) {
                deferred.fail(response);
            });

        return deferred;
    },

    loadPreview: function(objectId, cmp) {
        var me = this;

        if (cmp.rendered) {
            cmp.mask();
        }

        me.loadData(objectId, { previewOnly: true, previewLength: me.previewLength })
            .next(function(response) {
                var data = Ext.decode(response.responseText).data;
                if (data && data['preview']) {
                    cmp.setValue(data.preview);
                } else {
                    cmp.setValue(null);
                }

                if (cmp.rendered) {
                    cmp.unmask();
                }
            })
            .error(function() {
                if (cmp.rendered) {
                    cmp.unmask();
                }
            });
    },

    setData: function(objectId) {
        var me = this,
            id = objectId > 0 ? objectId : 0;

        if (this.fireEvent('beforesetdata', this, objectId) !== false) {
            me.loadData(id)
                .next(function(response) {
                    var data = Ext.decode(response.responseText).data,
                        panel = me.createWindow(),
                        field = panel.down('[name="' + me.valueFieldName + '"]'),
                        idField = panel.down('[name="Id"]');

                    idField.setValue(id);

                    if (data && data[me.valueFieldName]) {
                        field.setValue(data[me.valueFieldName]);
                    } else {
                        field.setValue(me.getParentPanel().down(me.fieldSelector).getValue());
                    }

                    panel.show();
                });
        }
    },

    saveRecord: function(btn) {
        var me = this,
            wnd = btn.up('window'),
            field = wnd.down('[name="' + me.valueFieldName + '"]'),
            idField = wnd.down('[name="Id"]');

        me.mask('Сохранение', wnd);
        B4.Ajax.request({
                url: B4.Url.action(Ext.String.format('/{0}/{1}', me.controllerName, me.saveAction)),
                method: 'POST',
                params: {
                    field: me.valueFieldName,
                    id: idField.getValue(),
                    value: field.getValue(),
                    previewLength: me.previewLength,
                    // нет смысла пытаться сохранять не-хранимые поля
                    autoSavePreview: me.autoSavePreview && (me.previewField !== false),
                    previewField: me.previewField
                }
            })
            .next(function(response) {
                me.unmask();
                var data = Ext.decode(response.responseText).data;
                if (typeof data.preview === 'string') {
                    me.getParentPanel().down(me.fieldSelector).setValue(data.preview);
                }

                me.onPreSaveSuccess(me, data, response);
            })
            .error(function(response) {
                me.unmask();
                me.fireEvent('savefailure', response);
                Ext.Msg.alert('Ошибка сохранения', 'Не удалось сохранить данные');
            });
    },

    onPreSaveSuccess: function(asp, data, response) {
        asp.fireEvent('savesuccess', asp, data, response);
        B4.QuickMsg.msg('Сохранение', 'Данные успешно сохранены', 'success');
    }
});
Ext.define('B4.controller.PassportStruct', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.mixins.Context',
        'B4.view.passport.StructEditor',
        'B4.view.passport.StructGrid',
        'B4.view.passport.AttributeEditor',
        'B4.dynamic.Helper',
        'B4.aspects.permission.GkhPermissionAspect',
        'B4.view.passport.CopyPassportWindow',
        'B4.view.passport.StructImportWindow'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    views: [
        'passport.StructEditor',
        'passport.StructGrid'
    ],

    refs: [
        {
            ref: 'editorPanel',
            selector: 'structeditor'
        },
        {
            ref: 'structForm',
            selector: 'structeditor form[entity="PassportStruct"]'
        },
        {
            ref: 'partTree',
            selector: 'structeditor parttreegrid'
        },
        {
            ref: 'partForm',
            selector: 'structeditor form[entity="Part"]'
        },
        {
            ref: 'gridPanel',
            selector: 'structgrid'
        },
        {
            ref: 'attrTree',
            selector: 'attrtreegrid'
        },
        {
            ref: 'menuItemSimple',
            selector: 'attrtreegrid menuitem[attrtype="simple"]'
        },
        {
            ref: 'menuItemGrouped',
            selector: 'attrtreegrid menuitem[attrtype="grouped"]'
        },
        {
            ref: 'menuItemGroupedWithVal',
            selector: 'attrtreegrid menuitem[attrtype="groupedwithval"]'
        },
        {
            ref: 'menuItemGroupedComplex',
            selector: 'attrtreegrid menuitem[attrtype="groupedcomplex"]'
        },
        {
            ref: 'attrEditor',
            selector: 'structeditor attreditor'
        },
        {
            ref: 'dataFiller',
            selector: 'structeditor attreditor *[name="DataFillerCode"]'
        },
        {
            ref: 'valueType',
            selector: 'structeditor attreditor *[name="ValueType"]'
        },
        {
            ref: 'attrType',
            selector: 'structeditor attreditor *[name="Type"]'
        }
    ],

    defaultPartConfig: {
        Name: 'Новый'
    },

    defaultAttrCfg: {
        Name: 'Новый атрибут',
        Type: 10,
        'Id': null,
        'Code': 0,
        'Parent': '',
        'ParentPart': '',
        'ValueType': '',
        'ValidateChilds': '',
        'GroupText': '',
        'UnitMeasure': '',
        'IntegrationCode': '',
        'MaxLength': 10,
        'MinLength': '',
        'Pattern': null,
        'Exp': 0,
        'Required': false,
        'AllowNegative': false
    },

    index: function () {
        var view = this.getGridPanel() || Ext.widget('structgrid');

        this.bindContext(view);
        this.application.deployView(view);
    },

    structeditor: function (id) {
        var view = this.getEditorPanel() || Ext.widget('structeditor',
            {
                structId: id
            });
        
        if (id === undefined) {
            var today = new Date();
            var currentMonth = today.getMonth() + 1;
            var currentYear = today.getFullYear();
            
            var cbMonth = view.down('combobox[name="ValidFromMonth"]');
            if (cbMonth) {
                cbMonth.setValue(currentMonth);
            }
            
            var nfYear = view.down('numberfield[name="ValidFromYear"]');
            if (nfYear) {
                nfYear.setValue(currentYear);
            }
        }

        this.bindContext(view);
        this.application.deployView(view);
    },

    aspects: [
       {
           xtype: 'gkhpermissionaspect',
           permissions: [
               { name: 'Gkh1468.Dictionaries.PassportStruct.Create', applyTo: 'b4addbutton', selector: 'structgrid' },
               {
                   name: 'Gkh1468.Dictionaries.PassportStruct.Delete', applyTo: 'b4deletecolumn', selector: 'structgrid',
                   applyBy: function (component, allowed) {
                       if (allowed) component.show();
                       else component.hide();
                   }
               },
               { name: 'Gkh1468.Dictionaries.PassportStruct.Export', applyTo: 'button[action="export"]', selector: 'structgrid' },
               { name: 'Gkh1468.Dictionaries.PassportStruct.Import', applyTo: 'button[action="import"]', selector: 'structgrid' },
               { name: 'Gkh1468.Dictionaries.PassportStruct.Edit', applyTo: 'button[cmd="removeattr"]', selector: 'attrtreegrid' },
               { name: 'Gkh1468.Dictionaries.PassportStruct.Edit', applyTo: 'button[cmd="addattr"]', selector: 'attrtreegrid' },
               { name: 'Gkh1468.Dictionaries.PassportStruct.Edit', applyTo: 'button[cmd="save"]', selector: 'structeditor attreditor' },
               { name: 'Gkh1468.Dictionaries.PassportStruct.Edit', applyTo: 'button[cmd="add"]', selector: 'structeditor' },
               { name: 'Gkh1468.Dictionaries.PassportStruct.Edit', applyTo: 'button[cmd="removepart"]', selector: 'structeditor' },
               { name: 'Gkh1468.Dictionaries.PassportStruct.Edit', applyTo: 'button[cmd="save"]', selector: 'structeditor form[formname="partform"]' },
               { name: 'Gkh1468.Dictionaries.PassportStruct.Edit', applyTo: 'textfield[name="Name"]', selector: 'structeditor' },
               { name: 'Gkh1468.Dictionaries.PassportStruct.Edit', applyTo: 'combobox[name="PassportType"]', selector: 'structeditor' },
               { name: 'Gkh1468.Dictionaries.PassportStruct.Edit', applyTo: 'datefield[name="ValidStart"]', selector: 'structeditor' },
               { name: 'Gkh1468.Dictionaries.PassportStruct.Edit', applyTo: 'datefield[name="ValidEnd"]', selector: 'structeditor' }
           ]
       }],

    init: function () {
        var me = this,
            actions = {
                'structeditor form[entity="PassportStruct"]': {
                    render: me.onPassportStructFormRender
                },

                'structeditor parttreegrid': {
                    selectionchange: me.onStructPartTreeSelect,
                    afterrender: me.onStructPartTreeRender,
                    nodesreordered: me.onNodesReordered
                },
                'structeditor parttreegrid b4updatebutton': {
                    click: me.reloadPartTree
                },
                'structgrid': {
                    show: me.onStructGridShow,
                    rowaction: me.onStructGridRowAction,
                    itemdblclick: me.onStructGridItemDblClick,
                    selectionchange: me.onStructGridSelectionChange
                },
                'structgrid b4addbutton': {
                    click: function () {
                        Ext.History.add('structeditor/new/');
                    }
                },
                'structgrid b4updatebutton': {
                    click: function (btn) {
                        btn.up('grid').getStore().load();
                    }
                },
                'structgrid actioncolumn[name="CopyPassportBtn"]': {
                    click: me.onShowCopyPassportWin
                },
                'structeditor menuitem[cmd="addpart"]': {
                    click: me.onAddPartBtnClick
                },
                'structeditor menuitem[cmd="addsubpart"]': {
                    click: me.onAddSubPartBtnClick
                },
                'structeditor button[cmd="removepart"]': {
                    click: me.onRemovePartBtnClick
                },
                'structeditor form[entity="Part"]': {
                    render: me.onPartFormRender
                },
                'structeditor form[entity="Part"] field': {
                    blur: me.onPartFormValuesChange
                },
                'attrtreegrid menuitem[attrtype]': {
                    click: me.onAttrTreeMenuItemClick
                },
                'attrtreegrid button[cmd="removeattr"]': {
                    click: me.onRemoveAttrBtnClick
                },

                'attrtreegrid b4updatebutton': {
                    click: me.reloadAttrTree
                },

                'attrtreegrid': {
                    selectionchange: me.onAttributeSelect,
                    attrtreenodesreordered: me.attrTreeNodesReordered
                },

                'structeditor form[entity] b4savebutton': {
                    click: me.saveFormBtnClick
                },

                'structeditor attreditor': {
                    render: me.onAttrEditorRender
                },

                'structeditor attreditor *[name="ValueType"]': {
                    select: me.onValueTypeSelect
                },

                'structeditor attreditor *[name="DataFillerCode"]': {
                    beforeload: me.onFillerStoreBeforeload
                },

                'structgrid button[action=import]': {
                    click: me.importPassportStruct
                },

                'structgrid button[action=export]': {
                    click: me.exportPassportStruct

                },
                'copypassportwin b4savebutton': {
                    click: me.onCopyPassportStruct
                },
                'attreditor #attributeType': {
                    change: me.onChangeAttrType
                },
                'attreditor #attributeValueType': {
                    change: me.onChangeAttrValueType
                }
            };

        me.control(actions);
        me.callParent(arguments);
    },

    onPartFormRender: function (form) {
        form.down('[name="Parent"]').hide();
        form.down('[name="PassportStruct"]').hide();
    },

    onAttrEditorRender: function (editor) {
        editor.down('[name="Parent"]').hide();
        editor.down('[name="ParentPart"]').hide();
    },

    onPassportStructFormRender: function (form) {
        var me = this,
            editorPanel = me.getEditorPanel(),
            structId = editorPanel.structId;
        
        if (structId) {
            B4.Ajax.request({
                url: B4.Url.action('Get', 'PassportStruct'),
                method: 'GET',
                params: { id: structId }
            }).next(function (response) {
                var values = Ext.JSON.decode(response.responseText).data;
                
                if (values && values.hasOwnProperty && values.hasOwnProperty('hasPassports')) {
                    me.restrictEditor(values.hasPassports);

                    editorPanel.isRestricted = values.hasPassports;
                }
                
                form.getForm().setValues(values);
            }).error(function () {
                B4.QuickMsg.msg('Ошибка', 'Ошибка при получении данных', 'error');
            });
        }
    },
    
    restrictEditor: function (isRestricted) {
        if (isRestricted == null) {
            return;
        }

        function applyResriction(component) {
            if (component) {
                component.setDisabled(isRestricted);
            }
        }
        
        function applyReadOnlyResriction(component) {
            if (component) {
                if (component.setReadOnly) {
                    component.setReadOnly(isRestricted);
                } else {
                    component.setDisabled(isRestricted);
                }
            }
        }

        var me = this,
            form = me.getStructForm(),
            partTree = me.getPartTree(),
            attrGrid = me.getAttrTree(),
            attrEditor = me.getAttrEditor();
        
        if (form) {
            applyResriction(form.down('combobox[name="PassportType"]'));
            applyResriction(form.down('combobox[name="ValidFromMonth"]'));
            applyResriction(form.down('numberfield[name="ValidFromYear"]'));

            var label = form.down('#editorRestricted');
            if (label) {
                label.setVisible(isRestricted);
            }
        }
        
        if (partTree) {
            applyResriction(partTree.down('b4addbutton'));
            applyResriction(partTree.down('button[cmd="removepart"]'));
        }
        
        if (attrGrid) {
            applyResriction(attrGrid.down('button[cmd="addattr"]'));
            applyResriction(attrGrid.down('button[cmd="removeattr"]'));
        }
        
        if (attrEditor) {
            if (attrEditor.items) {
                attrEditor.items.each(applyReadOnlyResriction);
            }

            var makeWritable = function(selector) {
                var element = attrEditor.down(selector);
                if (element) {
                    element.setReadOnly(false);
                }
            };

            makeWritable('textfield[name=Name]');
            makeWritable('textfield[name=IntegrationCode]');
            makeWritable('textfield[name=Code]');
            makeWritable('checkbox[name=UseInPercentCalculation]');
        }
    },

    onStructPartTreeSelect: function (panel, selected) {
        var me = this,
            form = me.getPartForm(), 
            attrGrid = me.getAttrTree(),
            addStructBtn = me.getPartTree().down('b4addbutton');

        if (selected[0]) {
            form.loadRecord(selected[0]);

            addStructBtn.setDisabled(!selected[0].parentNode.isRoot());
            form.enable();
            B4.Ajax.request({
                url: B4.Url.action('GetAttributes', 'PassportStruct'),
                method: 'GET',
                params: { partId: selected[0].get('Id') }
            }).next(function (response) {
                var data = Ext.JSON.decode(response.responseText);
                data.Name = "Атрибуты";
                attrGrid.setRootNode(data);
                attrGrid.enable();
            }).error(function (response) {
                throw new Error('Request error');
            });
        } else {
            form.disable();
            attrGrid.disable();
        }
    },

    onAddPartBtnClick: function () {
        var me = this,
            root = me.getPartTree().getRootNode();
        if (root) {
            me.addPart(root, me.defaultPartConfig);
        }
    },

    addPart: function (root, cfg) {
        var me = this,
            values = Ext.apply({}, {
                Struct: me.getEditorPanel().structId,
                Parent: root.get('Id'),
                OrderNum: root.childNodes ? root.childNodes.length + 1 : 1
            }, me.defaultPartConfig);
        B4.Ajax.request({
            url: B4.Url.action('Create', 'Part'),
            method: 'POST',
            params: { records: Ext.encode([values]) }
        }).next(function (response) {
            me.reloadPartTree();
        }).error(function (response) {
            throw new Error('Request error');
        });
    },

    reloadPartTree: function () {
        var me = this,
            structId = me.getEditorPanel().structId,
            selected = me.getPartTree().getSelectionModel().getSelection()[0],
            selectPath;

        if (selected) {
            selectPath = selected.getPath();
        }

        B4.Ajax.request({
            url: B4.Url.action('GetParts', 'PassportStruct'),
            method: 'GET',
            params: { structId: structId }
        }).next(function (response) {
            var data = Ext.JSON.decode(response.responseText),
                partTree = me.getPartTree();
            partTree.setRootNode(data);
            partTree.expandAll();
            if (selectPath) {
                partTree.selectPath(selectPath);
            }
        }).error(function (response) {
            throw new Error('Request error');
        });
    },

    onAddSubPartBtnClick: function () {
        var me = this,
            sm = me.getPartTree().getSelectionModel(),
            node = sm.getSelection()[0];
        if (node) {
            me.addPart(node);
        } else {
            B4.QuickMsg.msg('Выберите раздел',
                'Выберите раздел, в который надо добавить подраздел', 'warning');
        }
    },

    onRemovePartBtnClick: function () {
        var me = this,
            grid = me.getPartTree(),
            sm = grid.getSelectionModel(),
            node = sm.getSelection()[0];
        
        if (!node) {
            return;
        }

        Ext.Msg.confirm('Удаление раздела!', 'Вы действительно хотите удалить раздел и связанные с ним подразделы и атрибуты?', function (result) {
            if (result == 'yes') {
                me.mask('Производится удаление раздела...', grid);
                me.beforeStructPartRemove(node);

                B4.Ajax.request({
                    url: B4.Url.action('Delete', 'Part'),
                    timeout: 999999,
                    method: 'POST',
                    params: { id: node.data.Id }
                }).next(function (response) {
                    var response = Ext.decode(response.responseText);
                    if (response.success) {
                        me.reloadPartTree();
                    } else {
                        Ext.Msg.alert('Ошибка удаления раздела!', response.message);
                    }
                    me.unmask();
                }).error(function (response) {
                    me.unmask();
                    Ext.Msg.alert('Ошибка удаления!', 'При удалении раздела произошла ошибка');
                });
            }
        }, me);
    },

    onRemoveAttrBtnClick: function () {
        var me = this,
            grid = me.getAttrTree(),
            sm = grid.getSelectionModel(),
            node = sm.getSelection()[0],
            expandToPath;

        if (node) {
            if (node.parentNode != null) {
                expandToPath = node.parentNode.getPath('Id');
            }
            else {
                expandToPath = node.getPath('Id');
            }
        }

        Ext.Msg.confirm('Удаление атрибута!', 'Вы действительно хотите удалить атрибут?', function (result) {
            if (result == 'yes') {
                me.mask('Производится удаление атрибута...', grid);
                B4.Ajax.request({
                    url: B4.Url.action('Delete', 'MetaAttribute'),
                    timeout: 999999,
                    method: 'POST',
                    params: { id: node.data.Id }
                }).next(function (response) {
                    me.reloadAttrTree(expandToPath);
                    me.unmask();
                }).error(function (response) {
                    Ext.Msg.alert('Ошибка удаления!', 'При удалении атрибута произошла ошибка');
                    me.unmask();
                });
            }
        }, me);
    },

    onPartFormValuesChange: function (field, newValue, oldValue) {
        var form = field.up('form'),
            record = form.getRecord(),
            values = form.getValues();
        if (record) {
            record.set(values);
        }
    },

    onAttrTreeMenuItemClick: function (item) {
        var me = this,
            attrgrid = me.getAttrTree(),
            sm = attrgrid.getSelectionModel(),
            node = sm.getSelection()[0] || attrgrid.getRootNode();
        me.addAttribute(node, me.getAttributeTypeCode(item.attrtype));
    },

    addAttribute: function (parentNode, typecode) {
        var me = this,
            activePart = me.getPartTree().getSelectionModel().getSelection()[0],
            attrCfg = Ext.apply({}, {
                Type: typecode,
                Parent: parentNode.get('Id'),
                ParentPart: activePart.get('Id'),
                OrderNum: parentNode.childNodes.length + 1
            }, me.defaultAttrCfg);

        parentNode.appendChild(attrCfg);
        parentNode.set('leaf', false);
    },

    reloadAttrTree: function (expandToPath) {
        var me = this,
            form = me.getPartForm(),
            selected = me.getPartTree().getSelectionModel().getSelection(),
            attrGrid = me.getAttrTree();

        if (selected[0]) {
            form.loadRecord(selected[0]);
            B4.Ajax.request({
                url: B4.Url.action('GetAttributes', 'PassportStruct'),
                method: 'GET',
                params: { partId: selected[0].get('Id') }
            }).next(function (response) {
                var data = Ext.JSON.decode(response.responseText);
                data.Name = "Атрибуты";
                attrGrid.setRootNode(data);
                if (expandToPath) {
                    attrGrid.expandPath(expandToPath, 'Id');
                }
            }).error(function (response) {
                throw new Error('Request error');
            });
        }
    },

    getAttributeTypeCode: function (attrtype) {
        switch (attrtype) {
            case 'simple':
                return 10;
            case 'grouped':
                return 20;
            case 'groupedwithval':
                return 30;
            case 'groupedcomplex':
                return 40;

        }
    },
    getAttributeType: function (code) {
        switch (code) {
            case 10:
                return 'simple';
            case 20:
                return 'grouped';
            case 30:
                return 'groupedwithval';
            case 40:
                return 'groupedcomplex';

        }
    },

    onAttributeSelect: function (tree, selected) {
        var me = this;
        me.getAttrEditor().getForm().reset();

        if (selected[0] && !selected[0].isRoot()) {
            me.getAttrEditor().enable();
            me.setAttrMenuItemsAvailability(selected[0].get('Type'));

            me.getAttrEditor().loadRecord(selected[0]);
        } else {
            me.getAttrEditor().disable();
            me.setAttrMenuItemsAvailability();
        }
    },

    onStructGridShow: function (grid) {
        grid.getStore().load();
    },

    onStructGridRowAction: function (grid, action, record) {
        var me = this,
            id = record.getId();
        switch (action) {
            case 'edit':
                me.editStruct(id);
                break;
            case 'delete':
                me.removeStruct(record, grid);
                break;
            default:
                throw new Error("Undefined grid action");
        }
    },

    onStructGridItemDblClick: function (view, record) {
        var id = record.getId();
        this.editStruct(id);
    },

    editStruct: function (structId) {
        Ext.History.add(Ext.String.format('structeditor/{0}/', structId));
    },

    removeStruct: function (record, grid) {
        var me = this;
        
        Ext.Msg.confirm('Удаление структуры паспорта!', 'Структура паспорта будет удалена без возможности восстановления. Продолжить?', function (result) {
            if (result == 'yes') {
                B4.Ajax.request({
                    url: B4.Url.action('Delete', 'PassportStruct'),
                    method: 'POST',
                    timeout: 999999,
                    params: { id: record.getId() }
                }).next(function (resp) {
                    var response = Ext.decode(resp.responseText);
                    if (response.success) {
                        grid.getStore().load();
                    } else {
                        Ext.Msg.alert('Ошибка удаления структуры паспорта!', response.message);
                    }
                }).error(function (resp) {
                    throw new Error("Error on request");
                });
            }
        }, me);
    },

    setAttrMenuItemsAvailability: function (typecode) {
        var me = this, type = me.getAttributeType(typecode);
        switch (type) {
            case 'simple':
                me.getMenuItemSimple().disable();
                me.getMenuItemGrouped().disable();
                me.getMenuItemGroupedWithVal().disable();
                me.getMenuItemGroupedComplex().disable();
                break;
            case 'grouped':
            case 'groupedcomplex':
                me.getMenuItemSimple().enable();
                me.getMenuItemGrouped().enable();
                me.getMenuItemGroupedWithVal().enable();
                me.getMenuItemGroupedComplex().enable();
                break;
            case 'groupedwithval':
                me.getMenuItemSimple().enable();
                me.getMenuItemGrouped().disable();
                me.getMenuItemGroupedWithVal().disable();
                me.getMenuItemGroupedComplex().disable();
                break;
            default:
                me.getMenuItemSimple().enable();
                me.getMenuItemGrouped().enable();
                me.getMenuItemGroupedWithVal().enable();
                me.getMenuItemGroupedComplex().enable();

        }
    },

    onStructPartTreeRender: function () {
        this.reloadPartTree();
    },

    onValueTypeSelect: function (combo, records, opts) {
        //var dataFiller = this.getDataFiller();
        //dataFiller.disable();
    },

    onFillerStoreBeforeload: function (field, opts, store) {
        var filler = this.getDataFiller();

        Ext.apply(filler.getStore().getProxy().extraParams, {
            Type: this.getAttrType().getValue(),
            ValueType: this.getValueType().getValue()
        });
    },

    beforeStructPartRemove: function (part) {
        var me = this,
           grid = me.getPartTree(),
           sm = grid.getSelectionModel(),
           attrTree = me.getAttrTree(),
           partForm = me.getPartForm(),
           attrEditor = me.getAttrEditor();

        partForm.disable();
        attrTree.disable();
        attrEditor.disable();
        sm.deselect(part);
    },

    onNodesReordered: function (treegrid, node) {
        var me = this,
            i = 1,
            orders = [];

        Ext.each(node.childNodes, function (item) {
            var id = item.get('Id');
            if (id) {
                orders.push({ Id: id, OrderNum: i });
                i++;
            }

        });
        me.saveNewOrder(orders, treegrid.controllerName);
    },

    attrTreeNodesReordered: function (treegrid, node) {
        var me = this,
            i = 1,
            orders = [];

        for (var j = 0; j < node.childNodes.length; j++) {
            var item = node.childNodes[j];

            var id = item.get('Id');
            var parentId = node.get('Id');

            if (id) {
                orders.push({ Id: id, ParentId: parentId, OrderNum: i });
                i++;
            }
        }

        me.saveNewAttributeOrder(orders, treegrid.controllerName);
    },

    saveNewOrder: function (orders, controller) {
        B4.Ajax.request({
            url: B4.Url.action('Update', controller),
            params: { records: Ext.encode(orders) }
        }).next(function (response) {
            me.notifySave('success');
        }).error(function (response) {
            me.notifySave('error');
        });
    },

    saveNewAttributeOrder: function (orders, controller) {
        var me = this;

        me.mask("Обновление структуры аттибутов");

        B4.Ajax.request({
            url: B4.Url.action('SaveNewAttributeOrder', controller),
            params: { records: Ext.encode(orders) }
        }).next(function (response) {
            me.unmask();
            me.notifySave('success');
        }).error(function (response) {
            me.unmask();
            me.notifySave('error');
        });
    },
    
    saveFormBtnClick: function (btn) {
        var me = this,
            form = btn.up('form'),
            values = form.getValues(false, false, false, true);

        if (form.getForm().isValid()) {
            form.disable();
            B4.Ajax.request({
                url: B4.Url.action(values.Id ? 'Update' : 'Create', form.entity),
                method: 'POST',
                params: { records: Ext.encode([values]) }
            }).next(function (response) {
                var savedData = Ext.JSON.decode(response.responseText).data[0];
                form.getForm().setValues(savedData);
                if (form.entity === "PassportStruct") {
                    me.getEditorPanel().structId = savedData.Id;
                }
                var record = form.getRecord();
                if (record) {
                    record.set(savedData);
                }
                form.enable();
                me.notifySave('success');
            }).error(function (response) {
                form.enable();
                me.notifySave('error', response && response.message);
            });
        } else {
            B4.QuickMsg.msg('Предупреждение', 'Неверно заполнены данные', 'warning');
        }
    },

    notifySave: function (type, message) {
        var msg;
        switch (type) {
            case 'success':
                msg = message ? message : 'Изменения успешно сохранены.';
                B4.QuickMsg.msg('Сохранение', msg);
                break;
            case 'error':
                msg = message ? message : 'Произошла ошибка при сохранении.';
                B4.QuickMsg.msg('Сохранение', msg, 'error');
        }
    },
    
    onShowCopyPassportWin: function (view, cell, row, col, e, rec) {
        var win = Ext.widget('copypassportwin');

        if (win) {
            win.getForm().loadRecord(rec);
            var cbMonth = win.down("combobox[name='ValidFromMonth']");
            var monthNum = cbMonth.getStore().getAt(cbMonth.getStore().find("name", rec.get("ValidFromMonth"))).get("num");
            cbMonth.setValue(monthNum);
            win.down("textfield[name='Name']").setValue("");
            
            win.show();
        }
    },
    
    onCopyPassportStruct: function (btn) {
        var me = this,
            win = btn.up('copypassportwin'),
            form = win.getForm(),
            valid = form.isValid(),
            grid = me.getGridPanel();

        if (!valid) {
            B4.QuickMsg.msg("Предупреждение", "Не заполнены обязательные поля", "warning");
            return;
        }

        var values = form.getValues();
        values.passportStructId = form.getRecord().getId();
        
        me.mask('Копирование структуры паспорта...', grid);
        B4.Ajax.request({
            url: B4.Url.action('CopyPassportStruct', 'PassportStruct'),
            params: values,
            timeout: 9999999
        }).next(function (resp) {
            var response = Ext.decode(resp.responseText);
            me.unmask();
            if (response.success) {
                B4.QuickMsg.msg("Сохранение", "Структура паспорта успешно скопирована", 'success');
                grid.getStore().load();
            }

            win.close();
        }).error(function (resp) {
            me.unmask();
            B4.QuickMsg.msg("Ошибка", resp.message ? resp.message : "Ошибка при копировании структуры паспорта", "error");
            win.close();
        });
    },

    importPassportStruct: function (btn) {
        var win = this.structImportWin;
        if (!win) {
            win = this.structImportWin = Ext.widget('structimportwin', {
                closeAction: 'hide'
            });
        }
        win.show();
    },

    exportPassportStruct: function (btn) {
        var grid = btn.up('grid'),
            record = grid.getSelectionModel().getSelection()[0];

        if (record) {
            window.open(B4.Url.action('Export', 'PassportStruct', {
                structId: record.get('Id')
            }), '_blank');
        }
    },

    onStructGridSelectionChange: function (selecModel, selected) {
        if (selected.length > 0) {
            this.getGridPanel().down('button[action=export]').enable();
        }
    },
    
    onChangeAttrType: function (combo, newValue, oldValue) {
        var me = this,
            panel = combo.up('panel'),
            valueTypeField = panel.down('[name=ValueType]'),
            allowNegativeField = panel.down('[name=AllowNegative]'),
            expTypeField = panel.down('[name=Exp]'),
            useInPercentCalculation = panel.down('[name=UseInPercentCalculation]');

        valueTypeField.enable();
        allowNegativeField.enable();
        expTypeField.enable();

        if (newValue == 20 || newValue == 40) {
            useInPercentCalculation.setValue(false);
            useInPercentCalculation.setVisible(false);
        } else {
            useInPercentCalculation.setVisible(true);
        }

        if (newValue == 20) {
            if (oldValue != null) {
                Ext.Msg.confirm('Внимание!', 'При смене типа атрибута будут очищены значения полей: «Тип значения», «Знаков после запятой», «Отрицательное»', function (result) {
                    if (result == 'yes') {
                        valueTypeField.clearValue();
                        expTypeField.setValue(0);
                        allowNegativeField.setValue(false);

                        valueTypeField.disable();
                        allowNegativeField.disable();
                        expTypeField.disable();
                    } else {
                        combo.setValue(oldValue);
                    }
                }, me);
            }
        }

    },
    
    onChangeAttrValueType: function (combo, newValue) {
        var panel = combo.up('panel');
        var allowNegativeField = panel.down('[name=AllowNegative]');
        var expTypeField = panel.down('[name=Exp]');
        var dataFillerCodeField = panel.down('[name=DataFillerCode]');
        var dictCodeField = panel.down('[name=DictCode]');
        
        expTypeField.enable();
        allowNegativeField.enable();
        dataFillerCodeField.enable();
        dictCodeField.reset();
        dictCodeField.disable();

        switch (newValue) {
            case 10:  // Строка
                expTypeField.setValue(0);
                allowNegativeField.setValue(false);
                allowNegativeField.disable();
                expTypeField.disable();
                break;
            case 20: // Целочисленный
                expTypeField.setValue(0);
                expTypeField.disable();
                break;
            case 40: // Справочник
                expTypeField.setValue(0);
                allowNegativeField.setValue(false);
                allowNegativeField.disable();
                expTypeField.disable();
                dataFillerCodeField.disable();
                dictCodeField.enable();
                break;
        }
    }
});
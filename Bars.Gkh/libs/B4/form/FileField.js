Ext.define('B4.form.FileField', {
    extend: 'Ext.form.field.Trigger',
    alias: ['widget.b4filefield'],

    childEls: ['fileInputEl', 'fileInputWrap', 'downloadFrame', 'downloadForm'],
    requires: [
        'B4.view.PreviewFileWindow'
    ],
    iconClsSelectFile: 'x-form-search-trigger',
    iconClsDownloadFile: 'x-form-download-trigger',
    iconClsClearFile: 'x-form-clear-trigger',
    iconClsPreviewFile: 'x-form-picture-trigger',

    triggerTooltips: ['Загрузить', 'Просмотр файла', 'Скачать', 'Очистить'],

    fileController: 'FileUpload',
    downloadAction: 'Download',
    fileExtension: '',

    multiple: false,
    selectFileLimit: -1,

    /*Скрыть кнопку 'Загрузить'*/
    hideTrigger1: false,
    /*Скрыть кнопку 'Скачать'*/
    hideTrigger2: false,
    /*Скрыть кнопку 'Очистить'*/
    hideTrigger3: false,
    
    // Список расширений имен файлов, которые пользователь может выбрать для загрузки.
    // Представлен в виде строки разделенной запятыми, например: 'zip,rar'
    possibleFileExtensions: null,

    constructor: function(config) {
        var me = this,
            globalMaxFileSize,
            fileSize;

        Ext.apply(this, config || {});

        if (Gkh.config.General.MaxUploadFileSize) {
            globalMaxFileSize = Gkh.config.General.MaxUploadFileSize * 1048576; // Размер в байтах
            fileSize = me.maxFileSize || globalMaxFileSize;
            if (fileSize > globalMaxFileSize)
                fileSize = globalMaxFileSize;
        } else {
            fileSize = -1; //infinity
        }

        Ext.apply(me, {
            maxFileSize: fileSize
        });
        
        if (me.multiple) {
            Ext.apply(me, {
                hideTrigger2: true
            });
        }

        me.callParent(arguments);

        me.addEvents('beforeSelectClick', 'fileselected', 'filechange', 'fileclear');
    },

    isFileUpload: function () {
        return true;
    },

    /**
     * Возвращает размер файла. 
     */
    getSize: function () {
        var me = this;

        return me.fileInputEl.dom.files[0] ? me.fileInputEl.dom.files[0].size : 0;
    },

    extractFileInput: function () {
        var me = this,
            fileInput = me.fileInputEl.dom;
        me.reset();
        return fileInput;
    },

    createFileInput: function () {
        var me = this;
        me.fileInputEl = me.fileInputWrap.createChild({
            id: me.id + '-fileInputEl',
            name: me.getName(),
            cls: Ext.baseCSSPrefix + 'form-file-input',
            tag: 'input',
            type: 'file',
            size: 1,
            multiple: me.multiple
        });
        me.fileInputEl.on({
            scope: me,
            change: me.onInputFileChange
        });
    },

    initTriggers: function () {
        var me = this;

        if (!me.trigger1Cls) {
            me.trigger1Cls = me.iconClsSelectFile;
        }
        if (!me.trigger2Cls) {
            me.trigger2Cls = me.iconClsPreviewFile;
            me.trigger2tooltip = 'Предварительный просмотр';
        }
        if (!me.trigger3Cls) {
            me.trigger3Cls = me.iconClsDownloadFile;
          
        }
        if (!me.trigger4Cls) {
            me.trigger4Cls = me.iconClsClearFile;
        }
    },

    onApplyBy: function (allowed, triggerCls) {
        var field = this,
            hideField = true;
        var el = this.triggerCell.elements.filter(function (e) {
            return e.dom.innerHTML.indexOf(triggerCls) >= 0;
        })[0];

        if (el) {
            el.setStyle('display', allowed ? 'table-cell' : 'none');
        }

        // если все элементы управления скрыты, то скроем всё поле
        Ext.Array.forEach(this.triggerCell.elements,
            function (e) {
                hideField = hideField && !e.isDisplayed();
            });

        if (hideField) {
            field.hide();
        }
    },

    getTriggerMarkup: function () {
        var me = this,
            i,
            hideTrigger = (me.readOnly || me.hideTrigger),
            triggerCls,
            triggerBaseCls = me.triggerBaseCls,
            triggerConfigs = [],
            inputElCfg = {
                id: me.id + '-fileInputEl',
                name: me.getName(),
                cls: Ext.baseCSSPrefix + 'form-file-input',
                tag: 'input',
                type: 'file',
                size: 1
            };

        me.initTriggers();

        for (i = 0; (triggerCls = me['trigger' + (i + 1) + 'Cls']) || i < 1; i++) {
            triggerConfigs.push({
                tag: 'td',
                valign: 'top',
                cls: Ext.baseCSSPrefix + 'trigger-cell',
                style: 'width:' + me.triggerWidth + (hideTrigger ? 'px;display:none' : 'px'),
                cn: {
                    cls: [Ext.baseCSSPrefix + 'trigger-index-' + i, triggerBaseCls, triggerCls].join(' '),
                    role: 'button'
                }
            });
        }
        triggerConfigs[i - 1].cn.cls += ' ' + triggerBaseCls + '-last';

        triggerConfigs.push({
            id: me.id + '-fileInputWrap',
            tag: 'td',
            valign: 'top',
            cls: me.trigger1Cls,
            style: 'width:0px; display:none;',
            cn: inputElCfg
        },
            {
            id: me.id + '-downloadForm',
            tag: 'form',
            style: 'display: none'
        });

        return Ext.DomHelper.markup(triggerConfigs);
    },
    
    reset: function () {
        var me = this;
        if (me.rendered) {
            me.fileInputEl.remove();
            me.createFileInput();
        }
        me.callParent();
    },

    setValue: function (file) {
        var me = this;
        me.fileIsDelete = false;
        me.fileIsLoad = false;
        
        if (file) {
            me.fileId = file.id || file.Id;
            me.callParent([file.name || file.Name]);
        }
        else {
            me.fileId = null;
            me.callParent([]);
        }
    },

    getValue: function () {
        var me = this;
        return Ext.isEmpty(me.fileId) ? null : { Id: me.fileId };
    },
    
    /* Warning
    * В связи с тем, что пользователь может нажать кнопку удаления файла, потом прикрепить файл, потом ещё раз удалить, и еще раз прикрепить
    * не всегда получиться получить текущее значение, по этому отбработка удаления файла перенесена на сервер
    */
    getModelData: function () {
        var me = this,
            data = null;
        if (!me.disabled) {
            data = {};
            if (me.fileIsDelete) {
                data[me.getName()] = null;
            }
            else {
                data[me.getName()] = me.getValue();
            }
        }
        return data;
    },

    onRender: function () {
        var me = this,
            inputEl;
        me.callParent(arguments);

        inputEl = me.inputEl;
        inputEl.dom.name = ''; //name goes on the fileInput, not the text input

        me.fileInputEl.dom.name = me.getName();
        me.fileInputEl.on({
            scope: me,
            change: me.onInputFileChange
        });

        if(me.multiple){
            me.fileInputEl.set({
                multiple: 'multiple'
            });
        }
    },

    onTrigger1Click: function () {
        var me = this;
        
        if (me.fireEvent('beforeSelectClick', me)) {
            me.onSelectFile();
        }
    },


    onTrigger2Click: function () {
        this.onPreviewFile();
    },

    onTrigger3Click: function () {
        this.onFileDownLoad();
    },

    onTrigger4Click: function () {
        this.onClearFile();
    },

    onSelectFile: function () {
        this.fileInputEl.dom.click();
    },
    
    onPreviewFile: function () {
        if (!this.fileId) {
            B4.QuickMsg.msg('Внимание', 'Файл еще не загружен', 'warning');
            return;
        }

        var me = this,
            win = Ext.widget('previewFileWindow', {
                renderTo: B4.getBody().getActiveTab().getEl(),
                fileId: me.fileId
            });

        win.down('button[name=Save]').on({
            click: function () {
                me.onFileDownLoad();
            }
        });

        win.show();
    },

    onFileDownLoad: function () {
        var me = this;
        
        if (me.multiple) {
            return;
        }

        if (me.fileId) {
            if (me.downloadFrame) {
                Ext.destroy(me.downloadFrame);
            }

            me.downloadFrame = me.downloadForm.createChild({
                id: me.id + '-downloadFrame',
                tag: 'iframe',
                src: B4.Url.content(Ext.String.format('{0}/{1}?id={2}', me.fileController, me.downloadAction, me.fileId)),
                style: 'display: none'
            });
        }
    },

    onClearFile: function () {
        var me = this,
            currentValue = me.getValue();
        
        me.setValue(null);

        me.fileIsDelete = true;
        me.fileIsLoad = false;

        me.fireEvent('fileclear', me, me.getName(), currentValue);
        me.reset();
    },
    
    onInputFileChange: function () {
        var me = this,
            files = me.fileInputEl.dom.files,
            fileName = me.getFileName(files),
            invalidFileName = me.isFileExtensionOK();
        
        if (invalidFileName) {
            Ext.Msg.show({
                title: 'Поле выбора файла',
                msg: me.getInvalidExtensionMessage(invalidFileName, me.fileExtension, me.possibleFileExtensions),
                buttons: Ext.Msg.OK,
                icon: Ext.MessageBox.WARNING
            });

            me.reset();

            return;
        }

        invalidFileName = me.isFileSizeOK();
        
        if (invalidFileName) {
            Ext.Msg.show({
                title: 'Поле выбора файла',
                msg: me.getInvalidFileSizeMessage(invalidFileName, me.maxFileSize),
                buttons: Ext.Msg.OK,
                icon: Ext.MessageBox.WARNING
            });

            me.reset();

            return;
        }
        
        if (me.multiple) {
            if (!me.isFileLimitOK()) {
                Ext.Msg.show({
                    title: 'Поле выбора файла',
                    msg: me.getInvalidFileCountMessage(),
                    buttons: Ext.Msg.OK,
                    icon: Ext.MessageBox.WARNING
                });

                me.reset();

                return;
            }
        }

        me.fireEvent('fileselected', me, fileName);
        
        if (fileName) {
            me.fileId = null;
            me.fileIsDelete = false;
            me.fileIsLoad = true;
        }
        
        if (me.lastValue !== fileName) {
            me.fireEvent('filechange', me);
        }

        me.lastValue = fileName;
        B4.form.FileField.superclass.setValue.call(me, fileName);
    },
    
    isFileLoad: function () {
        return this.fileIsLoad;
    },

    isFileDelete: function () {
        return this.fileIsDelete;
    },

    getFileUrl: function (id) {
        var me = this;
        return B4.Url.content(Ext.String.format('{0}/{1}?id={2}', me.fileController, me.downloadAction, id));
    },

    isFileExtensionOK: function () {
        var me = this;

        if (!me.possibleFileExtensions) {
            return null;
        }

        var files = Array.from(me.fileInputEl.dom.files),
            uploadedFileExtensions = [...new Set(files.map(file => file.name.match(/\.([^.]+)$/)[1]))],
            allowedExtensions = me.possibleFileExtensions.split(','),
            nonMatchingExtensions = uploadedFileExtensions.filter(extension => !allowedExtensions.includes(extension));
        
        var invalidFile = files.find(file => {
            var fileExtension = file.name.match(/\.([^.]+)$/)[1];
            return nonMatchingExtensions.includes(fileExtension);
        });
        
        return invalidFile ? invalidFile.name : null;
    },
    
    isFileSizeOK: function () {
        var me = this;
        if (me.maxFileSize === -1) {
            return null;
        } else {
            Ext.each(Array.from(me.fileInputEl.dom.files), function (file) {
                if (file.size > me.maxFileSize) {
                    return file.name;
                }
            });

            return null;
        }
    },

    isFileLimitOK: function () {
        var me = this;

        if (me.selectFileLimit === -1 || !me.selectFileLimit) {
            return true;
        }

        if (me.fileInputEl.dom.files.length > me.selectFileLimit) {
            return false;
        }

        return true;
    },

    onDestroy: function () {
        var me = this;

        Ext.destroyMembers(me, 'fileInputEl', 'fileInputWrap', 'downloadFrame', 'downloadForm');
        me.callParent();
    },

    /**
    * Включить/отключить множественный выбор для поля
    * @param {boolean} value Множественный/Единичный выбор
    * @param {number} fileLimit Ограничение выбора количества файлов
    */
    setMultiple: function (value, fileLimit) {
        var me = this,
            multipleAttr = '';
        
        me.reset();
        
        var cfg = {
            multiple: value
        };
        
        if (value) {
            multipleAttr = 'multiple';
            cfg.selectFileLimit = fileLimit || -1;
            me.setVisibleTrigger(!value, me.iconClsDownloadFile);
        }
        
        Ext.apply(me, cfg);

        me.createFileInput();
        
        me.fileInputEl.set({
            multiple: multipleAttr
        });
    },
    
    getInvalidExtensionMessage: function (fileName, errorExtension, needExtensions) {
        return Ext.String.format('Выбранный файл {0} имеет недопустимое расширение.<br/>Допустимы следующие расширения: {1}', fileName, needExtensions);;
    },

    getInvalidFileSizeMessage: function (fileName, fileSize) {
        return Ext.String.format('Выбранный файл {0} имеет недопустимый размер.<br/>Размер файла не должен превышать: {1}', fileName, this.formatSize(fileSize));
    },

    getInvalidFileCountMessage: function () {
        return Ext.String.format('Превышено допустимое количество загружаемых файлов.<br/>Максимальные количество: {0}', this.selectFileLimit);
    },
    
    formatSize: function (size) {
        var kbConst = 1024,
            mbConst = kbConst * 1024,
            gbConst = mbConst * 1024,
            gb, mb, kb, result = '';
        
        if (size / gbConst > 1) {
            gb = Math.round(size / gbConst);
            size %= gbConst;

            result += gb + 'Гб ';
        }
        
        if (size / mbConst > 1) {
            mb = Math.round(size / mbConst);
            size %= mbConst;
            result += mb + ' Мб ';
        }
        
        if (size / kbConst > 1) {
            kb = Math.round(size / kbConst);
            result += kb + ' Кб ';
        }

        if (size > 0) {
            result += size + ' б';
        }

        return result.trim();
    },

    /* Получить наименования файлов через запятую */
    getFileName: function (fileList) {
        if (!Ext.isArray(fileList)) {
            return Array.from(fileList).map(f => f.name).join(', ');
        }

        return fileList.map(f => f.name || f.Name).join(', ');
    },

    /**
     * Установить видимость кнопки триггера.
     * Также, если все триггер элементы скрыты, то скроется и само поле.
     * @param {boolean} isVisible Видимый/скрытый
     * @param {number|string} trigger Номер триггера|Класс триггера
     */
    setVisibleTrigger: function (isVisible, trigger) {
        var me = this,
            triggerCls;

        if (Ext.isNumeric(trigger)) {
            triggerCls = me[`trigger${trigger}Cls`];

        } else if (Ext.isString(trigger)) {
            triggerCls = trigger;
        }

        if (!triggerCls) {
            return me;
        }

        var triggerEl,
            hideField = true;

        Ext.each(me.triggerCell.elements, el => {
            if (!triggerEl && el.dom.innerHTML.indexOf(triggerCls) >= 0) {
                triggerEl = el;
            }

            // если все элементы управления скрыты, то скроем всё поле
            hideField = hideField && !el.isDisplayed();
        });

        if (triggerEl) {
            triggerEl.setStyle('display', isVisible ? 'table-cell' : 'none');
        }

        if (hideField) {
            me.hide();
        }

        return me;
    }
});
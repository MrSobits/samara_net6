<style>
   p {
    text-indent: 20px;
    text-align: justify;
   }

    h3 { text-align: center}
</style>

<h1 style="text-align: center">Инструкция по работе с утилитой &#171;paket&#187;</h1>
<hr>
<p>
    В МЖФ установкой сторонних библиотек и их зависимостей, а также добавлением ссылок на эти библиотеки в .csproj файлах проектов занимается утилита <a href="https://fsprojects.github.io/Paket/index.html">paket</a>.
</p>
<p>
    Путь до файлов утилиты в проекте: "кореневая папка проекта\.paket". Для получения исполняемого файла paket.exe необходимо перейти по указанному пути и запустить <a href="https://fsprojects.github.io/Paket/bootstrapper.html">paket.bootstrapper.exe</a>. Данный исполняемый файл скачает последнюю стабильную версию paket.
</p>

<p>
    Для внесения изменений в состав библиотек проекта или добавления ссылок в проекты на уже имеющиеся пакеты необходимо провести работу с 3-мя файлами:
    <u>
        <li><a href="https://fsprojects.github.io/Paket/dependencies-file.html">paket.dependencies</a></li>
        <li><a href="https://fsprojects.github.io/Paket/lock-file.html">paket.lock</a></li>
        <li><a href="https://fsprojects.github.io/Paket/references-files.html">paket.references</a></li>
    </u>
</p>

<hr>
<h3>paket.dependencies</h3>
<hr>

<p>
    Файл используется для определения библиотек, их версий и источника, 
    в котором будет производиться поиск и скачивание указанного пакета. 
    Файл расположен в корневой папке проекта.
</p>
<p>
    Источник описывается с помощью директивы source, пример: <code>source https://nexus.bars.group/repository/nuget.org-proxy/index.json</code>, 
    после чего идет описание необходимых пакетов <code>nuget &lt;Имя пакета&gt; [Знак сравнения] &lt;Версия пакета&gt;</code>, 
    которые будут взяты из указанного выше источника.
</p>

<hr>
<h3>paket.lock</h3>
<hr>

<p>
    В данный файл записываются конкретные версии пакетов и их транзитивные зависимости. 
    Для ручного добавления пакета необходимо описание не только самого пакета, но и всех необходимых для него зависимостей с указанием соответствующих версий. 
    Файл расположен в корневой папке проекта.
</p>

<hr>
<h3>paket.references</h3>
<hr>

<p>
    Файл расположен в папке конкретного проекта и необходим для описания пакетов, которые следует в него включить. 
    Для добавления ссылки на пакет достаточно с новой строки в файле указать наименование пакета.
</p>
<p>
    Примечание: на каждый новый пакет необходимо продублировать ссылку в <code>paket.references</code> проекта <code>Bars.Gkh.App</code> 
    с указанием настройки: <code>&lt;Имя пакета&gt; copy_local: true</code>. 
    Настройка необходима для копирования файлов библиотеки в папку <code>bin</code> проекта <code>Bars.Gkh.App</code> для динамической подгрузки данных библиотек ядром системы.
</p>

<hr>
<h3>Основные команды</h3>
<hr>

<p>
    <code><a href="https://fsprojects.github.io/Paket/paket-add.html">paket add</a> &lt;Имя пакета&gt; --version &lt;Версия пакета&gt; [--project &lt;Путь до .csproj файла проекта&gt;]</code> - 
    команда для добавления новой зависимости в <code>paket.dependencies</code> и <code>paket.references</code> (если указан параметр <code>--project</code>).
</p>
<p>
    <code><a href="https://fsprojects.github.io/Paket/paket-install.html">paket install</a></code> - команда для потсроения графа зависимостей из <code>paket.dependencies</code> 
    и его запись в <code>paket.lock</code>. Не рекомендуется использовать, так как с новых версий <code>paket</code> данная команда начала ломать зависимости в <code>paket.lock</code>.
</p>
<p>
    <code><a href="https://fsprojects.github.io/Paket/paket-restore.html">paket restore</a></code> - команда для восстановления файлов пакетов, описанных в <code>paket.lock</code>. 
    Примечание: приоритетно файлы пакета будут восстанавливаться из кэша по пути: "C:\Users\user\.nuget\packages", 
    если хотите этого избежать, то предварительно очистите кэш по интересующим вас пакетам.
    Если утилита не видит изменений в <code>paket.lock</code> или все библиотеки уже скачаны, то восстановления не произойдет. 
    Для принудительного восстановления всех библиотек необходимо после директивы команды указать параметр <code>--force</code> 
    или запустить файл <code>paket_restore.bat</code> в папке утилиты.
</p>



Загрузчик для видео из архивов мыло.сру и конвертации их в wav.

Сделано на коленке, работает с божьей помощью.

# Инструкция
  ## Строка кук для доступа к архиву
1. ![image](https://github.com/user-attachments/assets/0e4c13f9-b992-4fed-86f5-f757edecfe3e)
Заходим в нужный нам архив на мыле (скрин, метка 1).
2. Жмякаем F12.
3. В открывшемся окне ищем вкладку "Network" (скрин, метка 2).
4.  Нажимаем F5, страница перезагружается, во вкладке "Network" ищем название нашего альбома.
5.  В случае если ссылка имеет вид "https://my.mail.ru/mail/имя_пользователя/video/2", то название альбома будет "2" (скрин, метка 3).
6.  Жмякаем на "2".
7.  Ищем уже во вкладке "Headers" (скрин, метка 3, чуть правее) или "Заголовки" строку "Cookies" (скрин, метка 4). Копируем оттуда всю строку (скрин, метка 5).

  ## Строка кук для доступа к конкретным видео
1. Переходим по ссылке вида https://my.mail.ru/+/video/meta/mail/имя_пользователя/1, где 1 - это айди видео.
2. Открывается страничка, на ней находим " "videos": [ " и переходим по ссылке " "url": " (обычно она начинается с "//cdn.my.mail.ru", и после cdn номер, неважно какой). Берём ту, у которой "key": "1080p".
3. Там проделываем всё те же действия из пункта 7 про куки для доступа к архиву.

  ## Строка для поиска
  Предусмотрен поиск по ключевым словам. Вводите и система ищет их, затем конвертирует в wav.

> [!IMPORTANT]
> При первом использовании система загрузит ВСЕ ссылки на все видео из архива в файл. Последующие поиски возможны без запроса на сервер.

> [!IMPORTANT]
> У печенек (куков) есть период протухания, когда нужно сходить на сервер. Если вы пользуетесь ссылками из файла, а у них истёк срок годности, то система предложит скачать заново с сервера.
> !!!КРАЙНЕ ВАЖНО!!! Если система сказала, что у ссылок истёк срок годности, нужно также ОБНОВИТЬ КУКИ по инструкции выше.

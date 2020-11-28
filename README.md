# PastebinAPI

**Code Usage**
> Import PastebinAPI
```CSharp
using PastebinAPI.API;
using PastebinAPI.API.APIObject;
```

> Connect with pastebin
```CSharp
PasteKey key = await PasteKeyBuilder.GenerateKeyAsync("your_developer_key", "your_username", "your_password");
Pastebin pastebin = new Pastebin(key);
```

> Create new paste
```CSharp
PasteBuilder paste = new PasteBuilder();
paste.Title = "Your paste title.cs";
paste.Text = "Your paste content";
paste.Visibility = Visibility.Public;
paste.ExpireTime = ExpireTimer.Never;
paste.Format = "csharp";

string pasteUrl = await pastebin.CreateNewPasteAsync(paste);
```

> Read paste
```CSharp
string content = await pastebin.ReadOwnPasteAsync(pasteUrl);

//public pastes
string content = await pastebin.ReadPasteAsync(pasteUrl);
```

> Delete paste
```CSharp
await pastebin.DeletePasteAsync(pasteUrl);
```

> Get list of your pastes
```CSharp
List<Paste> pastes = await pastebin.GetPastesAsync(limit: 100);
```

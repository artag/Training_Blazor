# Blazor + WebView = Desktop App?

*(Answer: No.)*

Attempt to run blazor app in desktop mode.

Using:
https://github.com/webview-cs/webview-cs (C# bindings to https://github.com/zserge/webview)
with my modifications for .Net Core 3.

## Result

Blazor app runs as desktop application, but:
* There is console window showing when running my application.
* Counter button doesn't work.
* Closed application still running in the background process.

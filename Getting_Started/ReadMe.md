# Blazor: Getting Started

## Exploring New Blazor Project

*Рассматривается Blazor on server*.

Рассматриваются классы `Program` и `Startup`.

В `Startup`:

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddRazorPages();           // Включение поддержки razor pages
    services.AddServerSideBlazor();     // Включение поддержки blazor на сервере
    // ...
}

public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    // ...

    app.UseEndpoints(endpoints =>
    {
        endpoints.MapBlazorHub();               // Включение SignalR соединения
        endpoints.MapFallbackToPage("/_Host");  // Перенаправление реквестов на страницу _Host.cshtml
    });
}
```

В `Pages\_Host.cshtml` - простой Razor page:

```html
...
<body>
    <!-- App - это App.razor -->
    <app>
        <component type="typeof(App)" render-mode="ServerPrerendered" />
    </app>
    ...
    <!-- Все взаимодействия с браузером производятся через этот файл -->
    <script src="_framework/blazor.server.js"></script>
</body>
...
```

Файл `App.razor` действует как роутер. В примере он переправляет на layout `Shared\MainLayout.razor`,
который используется по умолчанию:
```html
...
<Found Context="routeData">
    <RouteView RouteData="@routeData" DefaultLayout="@typeof(MainLayout)" />
</Found>
...
```

Файл `Shared\MainLayout.razor`:
```html
...
<!-- Боковое меню -->
<div class="sidebar">
    <NavMenu />
</div>
...
<div class="main">
    <!-- Верхняя шапка -->
    <div class="top-row px-4">
        <a href="https://docs.microsoft.com/aspnet/" target="_blank">About</a>
    </div>

    <!-- В @Body отрисовывается содержимое контейнера -->
    <div class="content px-4">
        @Body
    </div>
</div>
```

По умолчанию, входной точной в приложении является `Pages\Index.razor`.

Рассматриваются файлы в Pages: `Index.razor` и `Counter.razor`.

Рассматривается файл: `_Imports.razor`

Упоминается директория `wwwroot`.


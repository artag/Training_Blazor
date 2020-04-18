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

## Использование кода в Blazor

Два способа:
* Как в примере (используется `@code` вместе с html) - не рекомендуется так делать в реальных проектах.
```html
@page "/counter"

<h1>Counter</h1>
<p>Current count: @currentCount</p>
<button class="btn btn-primary" @onclick="IncrementCount">Click me</button>

@code {
    int currentCount = 0;
    void IncrementCount()
    {
        currentCount++;
    }
}
```

* Code-behind approach. Выделение исполняемого кода в отдельный файл. Рекомендуется так делать.
В данном tutorial будет использован именно этот подход.
```csharp
public class EmployeeOverviewBase : ComponentBase
{
    ...
}
```

И referencing:
```html
@page "/employeeoverview"
@inherits EmployeeOverviewBase
...
```

### Инициализация классов-наследников от `ComponentBase`

Для инициаизации `EmployeeOverviewBase` используется не конструктор, а перегруженный метод
`OnInitializedAsync()` (рекомендуется так делать):
```csharp
protected override Task OnInitializedAsync()
{
    InitializeCountries();
    InitializeJobCategories();
    InitializeEmployees();

    return base.OnInitializedAsync();
}
```

* *Роутинг*

  Примеры:
  ```html
  @page "/employeeoverview"
  @page "/employeedetail/{EmployeeId}"
  ```

* *Передача параметра в класс, производный от `ComponentBase` из razor*

  Используется свойство с атрибутом `Parameter`:
  ```csharp
  [Parameter]
  public string EmployeeId { get; set; }
  ```

### Добавление параметра для включения показа отладочной инфомации в браузере.
  
В `Startup в ConfigureServices добавить`:
```csharp 
public void ConfigureServices(IServiceCollection services)
{
    ...
    services.AddServerSideBlazor().AddCircuitOptions(options => options.DetailedErrors = true);
}
```

Теперь в консоли браузера можно увидеть более подробную информацию об ошибках.


## Working with data

3 способа доступа к данным:

* *Entity Framework Core*. Доступен только для серверного Blazor.

* *REST API*. Доступен для серверного и клиентского Blazor. Наиболее универсален. 
Используется для демонстрации в данном курсе.

* *Local Storage*. Доступен для клиентского Blazor.


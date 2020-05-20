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

### 3 способа доступа к данным

* *Entity Framework Core*. Доступен только для серверного Blazor.

* *REST API*. Доступен для серверного и клиентского Blazor. Наиболее универсален. 
Используется для демонстрации в данном курсе.

* *Local Storage*. Доступен для клиентского Blazor.

### Разновидности HttpClient

*Используется для взаимодействия с REST API.*

* `HttpClient (из System.Net.Http)` - может использоваться на серверном и клиентском Blazor.

* `IHttpClientFactory` (введен в Net Core 2.1) - используется только на серверном Blazor.
Рекомендуется использовать by MS. Демонстрируется в данном курсе.

* `HttpClient` - используется только на клиентском Blazor. В данном курсе не демонстрируется.

### Using the HttpClientFactory

#### Конфигурирование (регистрация) IHttpClientFactory

*Для использования в компоненте*

```csharp
public void ConfigureServices(IServiceCollection services)
{
    ...
    services.AddHttpClient();
    ...
}
```

#### Using the HttpClient in a Component

Используется в ComponentBase классах:
```csharp
[Inject]
public IHttpClientFactory _clientFactory { get; set; }
```

#### Использование HttpClient в качестве сервиса (рекомендуемый путь)

Лучше для взаимодействия с REST API использовать отдельный класс:

```csharp
public class EmployeeDataService : IEmployeeDataService
{
    private readonly HttpClient _httpClient;

    public EmployeeDataService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
}
```

Регистрация IEmployeeDataService через AddHttpClient в DI:

```csharp
public void ConfigureServices(IServiceCollection services)
{
    ...
    services.AddHttpClient<IEmployeeDataService, EmployeeDataService>(client =>
    {
        client.BaseAddress = new Uri("url_of_server_with_REST_API");
    });
    ...
}
```

Использование в ComponentBase классах:
```csharp
[Inject]
public IEmployeeDataService EmployeeDataService { get; set; }
```

### Запуск проекта

Надо запускать два проекта:
* Server с REST API.
* Server с Blazor.


### Data binding support in Blazor

* One-way
* Two-way
* Component parameter

#### One-way Binding

*Изменения в коде автоматически отображаются на UI.*

Значения свойств `@Employee.FirstName` и `@Employee.LastName` будут отображаться на UI.
Ввода значения из UI нет.
```csharp
<h1 class="page-title">
    Details for @Employee.FirstName @Employee.LastName
</h1>

public Employee Employee { get; set; }
```

Binding внутри элемента.

Значение свойства `@Employee.FirstName` будет отображено на UI. Ввода значения из UI нет.
```csharp
<label type="text" readonly class="form-control-plaintext">
    @Employee.FirstName
</label>

public Employee Employee { get; set; }
```

#### Two-way Binding

Пример, ввод текста в UI. Значение свойства `Employee.LastName` в компоненте изменится после
смены фокуса из поля ввода на UI:
```csharp
<input id="lastName" @bind="@Employee.LastName"
    placeholder="Enter last name" />
```

#### Two-way binding on a Different Event

Значение свойства `Employee.LastName` в компонента будет меняться сразу, при изменении значения в
поле ввода на UI.
```csharp
<input id="lastName" @bind-value="@Employee.LastName"
    @bind-value:event="oninput"
    placeholder="Enter last name" />
```

#### Пример

Пример трех видов binding'ов можно увидеть в примере `DataBindingSample`.

### Forms in Blazor

Функциональность форм:

* Input component (ввод данных)
* Data binding
* Validation (валидация вводимых данных)

#### Input Component

*Используемые компоненты для ввода в Blazor это обертка вокруг стандартных html элементов.*

* *InputText* - ввод одной строки.
* *InputTextArea* - ввод нескольких текстовых строк.
* *InputNumber* - ввод чисел.
* *InputSelect* - выпадающий список.
* *InputDate* - выбор даты.
* *InputCheckbox* - checkbox.

#### Пример создания формы

```html
<EditForm Model="@Employee"
    OnValidSubmit="@HandleValidSubmit"
    OnInvalidSubmit="@HandleInvalidSubmit">

    <InputText id="lastName"
        @bind-Value="@Employee.LastName"
        placeholder="Enter last name">
    </InputText>
</EditForm>
```

#### Adding Validation

*Blazor Forms поддерживают валидацию вводимых данных "из коробки"*.

* Similar to ASP.NET Core validations

* Data annotations (добавление аннотаций в модель данных (в свойства) для валидации вводимых данных в соответствующие поля)

* DataAnnotationsValidator

* ValidationSummary (вывод итоговой информации по всем ошибкам в веденных данных)

Для использования аннотаций:

1. Необходимо поставить nuget пакет `System.ComponentModel.Annotations` (в проект с классами-моделями).
2. Пометить атрибутами проверяемые свойства в модели (в примере - класс `Employee`).
3. На `EditForm` добавить следующие компоненты:
  3.1. `DataAnnotationsValidator` - для включения валидации на форме
  3.2. `ValidationSummary` - для показа суммарной информации об ошибках на форме
  3.3. `ValidationMessage` - для показа только одной ошибки

3.2. и 3.3. можно использовать как вместе, так и отдельно.
  
Пример `EditForm` с валидацией:
```html
<EditForm Model="@Employee" OnValidSubmit="@HandleValidSubmit">

    <DataAnnotationsValidator />
    <ValidationSummary />

    <div class="form-group row">
        <label for="lastName" class="col-sm-3">Last name: </label>
        <InputText id="lastName" class="form-control col-sm-8"
                   placeholder="Enter last name"
                   @bind-Value="@Employee.LastName">
        </InputText>
        <ValidationMessage class="offset-sm-3 col-sm-8" For="@(() => Employee.FirstName)"/>
    </div>
</EditForm>
```

### Добавление навигации

*Переход на другую страницу.*

Пример добалвения кнопки перехода на другую страницу.

EmployeeEditBase.cs:
```csharp
public class EmployeeEditBase : ComponentBase
{
    [Inject]
    public NavigationManager NavigationManager { get; set; }

    // ...

    protected void NavigateToOverview()
    {
        NavigationManager.NavigateTo("/employeeoverview");
    }
}
```
`NavigationManager` уже зарегистрирован к контейнере.

EmployeeEdit.razor:
```html
<a class="btn btn-outline-primary"
   @onclick="NavigateToOverview">Back to overview</a>
```

## Adding Features to the App

### Adding a Dialog Component

*Component are building block
  * Page
  * Dialog
* Reuse of functionality
* Make large pages smaller
* Can be nested (могут быть вложенными)
* In-project or separate library (могут располагаться внутри проекта или в отдельной библиотеке)
* Can receive parameters (может получать параметры из верхнего компонента)
* Event binding for component communication (может коммуницировать с другими компонентами, используя события)

#### Директории в проекте, содержащие компоненты

* Pages (компоненты генерируют страницы и их части)
* Shared
* Components

#### Using `_Imports.razor`

*Содержит using импортируемых namespace'ов.*

Пример:
```
@using BethanysPieShopHRM.Server.Components
```

#### Component Lifecycle Methods

1. `OnInitialized` / `OnInitializedAsync` (используется для инициализации, код в конструкторе выбросит исключение)
2. `OnParametersSet` / `OnParametersSetAsync` (устанавливаются параметры из верхнего/родительского компонента)
3. `OnAfterRender` / `OnAfterRenderAsync` (после отрисовки)

#### Ручное обновление Razor страницы из кода компонента

*Вызов метода `StateHasChanged()`*.

Из `AddEmployeeDialogBase.cs`:
```csharp
public void Show()
{
    ResetDialog();
    ShowDialog = true;
    StateHasChanged();
}
```

#### Demo. Добавление диалогового окна. Взаимодействие с родительским компонентом

*Добавление компонента - диалоговое окно. Для быстрого добавления нового сотрудника*.

Особенности:
* Имя добавляемого диалоговое окно AddEmployeeDialog.razor и AddEmployeeDialogBase.cs.

* Директория Components (туда помещается диалоговое окно).

* Диалоговое окно реализовывается средствами bootstrap (modal).

* Компонент EmployeeOverview родительский по отношению к AddEmployeeDialog.

* Вызов StateHasChanged() вручную обновляет DOM.

* В _Imports.razor добавляется namespace Components.

* В EmployeeOverview.razor добавляется компонент AddEmployeeDialog со ссылкой `@ref=AddEmployeeDialog`.
  ```html
  <AddEmployeeDialog @ref="AddEmployeeDialog"></AddEmployeeDialog>
  ```

* В EmployeeOverview.cs вставляется свойство для управления дочерним компонентом:
  ```csharp
  protected AddEmployeeDialog AddEmployeeDialog { get; set; }

  protected void QuickAddEmployee()
  {
      AddEmployeeDialog.Show();
  }
  ```

* Для включения перерисовки родительского компонента при сохранении и закрытии дочернего компонента (диалогового окна):
  * В `EmployeeOverview.razor`, в компонент AddEmployeeDialog добавляется CloseEventCallback:
  ```html
  <AddEmployeeDialog @ref="AddEmployeeDialog" CloseEventCallback="@AddEmployeeDialog_OnDialogClose">
  </AddEmployeeDialog>
  ```
  * В `AddEmployeeDialogBase.cs` добавляется свойство и его вызов при сохранении и закрытии
  дочернего компонента (диалогового окна):
  ```csharp
  [Parameter]
  public EventCallback<bool> CloseEventCallback { get; set; }

  protected async Task HandleValidSubmit()
  {
      ...
      await CloseEventCallback.InvokeAsync(true);
      StateHasChanged();
  }
  ```
  В `EmployeeOverview.cs` добавляется 
  ```csharp
  public async void AddEmployeeDialog_OnDialogClose()
  {
      ...
      StateHasChanged();
  }
  ```

### Integrating a JavaScript Component

* Not everything is possible via just .NET
* JavaScript interop
  * Call into JavaScript from Blazor code
* Runs on the client

#### JavaScript Interop

* .NET code calls into JavaScript
* JavaScript calls into .NET code
* Can be wrapped in a library

#### Injecting IJSRuntime

Доступ через Runtime Injection:
```csharp
[Inject]
public IJSRuntime JsRuntime { get; set; }
```

#### Adding a Script

Пример:
```javascript
<script>
    window.DoSomething = () => {
        //do some interesting task here
    }
</script>
```

#### Calling into JavaScript

Пример:
```csharp
var result = await
JsRuntime.InvokeAsync<object>("DoSomething", "");
```

#### Demo. Взаимодействие с JavaScript

*Добавление карты (на JavaScript) на страницу с деталями сотрудника.*

Особенности:
* Добавляемый проект `BethanysPieShopHRM.ComponentsLibrary`, который содержит компонент с картой -
это библиотека Razor Class Library (.NET Standart 2.0 + Razor).

* В `Map\Map.razor` взаимодействует с JavaScript.

* В `wwwroot` находятся файлы JavaScript и css.

* В `Map.razor` передается elementId и Markers (как параметры).

В проекте `BethanysPieShopHRM.Server`

* В `Pages\_Host.cshtml` добавляются css и js ссылки на новые файлы.

* В `_Imports.razor` добавляются using для новых namespace.

Не получилось:
* В `Pages\_Host.cshtml` странно добавились ссылки на css и js файлы из другого проекта 
(упорно указывается другой путь).

* Оформление на странице EmployeeDetail.razor.
Наблюдался сдвиг карты вниз, пришлось поколдовать самому над bootstrap.

## Preparing for Client-side Blazor

* *Кратко рассказывается про перенос кодовой базы в билиотеку для общего использования как на сервере, так и на клиенте Blazor.*

* *Быстро показывается процесс создания проекта Blazor client с нуля.*

### Start preparing

*Примерная последовательность действий по выделению общих библиотек для создания общеиспользуемого кода.*

* Ensure your app can be easily switched.
* Extract what is different.
* Share most of the code.

### Things to consider

*Что принимать во внимание в первую очередь при миграции с сервера на клиент.*

* HttpClient (наличие и как используется)
* EF Core (наличие и как используется)
* REST API

#### Изменения в App Project

* `_Host.cshtml` is removed
* Project is now a Razor Class Library
* Code can be fully reused

Для запуска Blazor Server и Blazor Client создаются отдельные проекты, которые используют новую (переделанную) библиотеку Razor Class Library с
Blazor Components.

## Deploying the application

*Демонстрация деплоя приложения на Azure (очень кратко и быстро).*

### Server Requirements

*Требования к серверу (куда будет развертываться приложение).*

Для API:
* ASP.NET Core
* SQL Server

Для Blazor App:
* ASP.NET Core
* Relies on SignalR Connection (дополнительное конфигурирования SignalR (сервис на Azure) для масштабирования и управления соединениями).

### Used Azure Services

Для API:
* Azure App Service (Web Apps)
* Azure SQL Database

Для Blazor App:
* Azure App Service (Web Apps)
* Azure SignalR Service

#### Azure SignalR Service

* Azure implementation of SignalR
* Scalability

### Client-side Blazor deployment

* No specific server requirements
* Static files
* Connect with API
  * CORS

## Рекомендуемые ресурсы

На pluralsight.com:

* Blazor: The Big Picture (Barry Luijbregts)
* Enterprise apps with Blazor (Alex Wolf)
* Creating Blazor Components (Roland Guijt)
* Authentication with Blazor (Kevin Dockx)
* JavaScript interop with Blazor (Scott Allen)

Map component (компонент - карта (на JavaScript и Blazor), используемый в проекте):

* https://aka.ms/blazorworkshop
